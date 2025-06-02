using UnityEngine;
using UnityEngine.Pool;

namespace RossoForge.Pool.Components
{
    public class Pool : MonoBehaviour
    {
        private IObjectPool<PooledObject> objectPool;

        private Transform _nextObjectParent;
        private Vector3 _nextObjectPosition;
        private Space _nextObjectSpace;

        public int MaxSize { get; set; }
        public GameObject AssetTemplate { get; set; }

        public void Load()
        {
            objectPool = new ObjectPool<PooledObject>(
                CreateInstance,
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPooledObject,
                true,
                MaxSize,
                MaxSize);
        }
        public PooledObject Get(Transform parent, Vector3 position, Space relativeTo)
        {
            _nextObjectParent = parent;
            _nextObjectPosition = position;
            _nextObjectSpace = relativeTo;
            return objectPool.Get();
        }

        private PooledObject CreateInstance()
        {
            GameObject obj = Instantiate(AssetTemplate);
            InitializeObject(obj);

            PooledObject pooledObject = obj.AddComponent<PooledObject>();
            pooledObject.OnReturnedToPool += obj =>
            {
                if (this != null && obj != null)
                {
                    obj.transform.SetParent(transform);
                    objectPool.Release(obj);
                }
            };

            return pooledObject;
        }
        private void OnReleaseToPool(PooledObject pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }
        private void OnGetFromPool(PooledObject pooledObject)
        {
            InitializeObject(pooledObject.gameObject);
        }
        private void OnDestroyPooledObject(PooledObject pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
        private void InitializeObject(GameObject obj)
        {
            obj.transform.SetParent(_nextObjectParent);
            if (_nextObjectSpace == Space.Self)
                obj.transform.localPosition = _nextObjectPosition;
            else
                obj.transform.position = _nextObjectPosition;

            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
        }
    }
}