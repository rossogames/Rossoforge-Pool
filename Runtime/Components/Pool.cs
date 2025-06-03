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
        private int _activeCount;

        public int MaxSize { get; set; }
        public GameObject AssetTemplate { get; set; }

        public void Load()
        {
            bool collectionCheck = false;
#if UNITY_EDITOR
            collectionCheck = true; // Enable collection check in editor for debugging purposes
#endif

            objectPool = new ObjectPool<PooledObject>(
                CreateInstance,
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPooledObject,
                collectionCheck,
                MaxSize,
                MaxSize);
        }
        public PooledObject Get(Transform parent, Vector3 position, Space relativeTo)
        {
            _activeCount++;
            if (_activeCount > MaxSize)
                Debug.LogWarning($"[Pool] Pool for '{AssetTemplate.name}' exceeded MaxSize ({MaxSize}). ActiveCount: {_activeCount}");

            _nextObjectParent = parent;
            _nextObjectPosition = position;
            _nextObjectSpace = relativeTo;

            var pooledObject = objectPool.Get();
            pooledObject.OnReturnedToPool += OnReturnedToPool;
            return pooledObject;
        }

        private PooledObject CreateInstance()
        {
            GameObject obj = Instantiate(AssetTemplate);
            InitializeObject(obj);
            return obj.AddComponent<PooledObject>();
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
        private void OnReturnedToPool(PooledObject pooledObject)
        {
            _activeCount = Mathf.Max(0, _activeCount - 1);

            if (this != null && pooledObject != null)
            {
                pooledObject.OnReturnedToPool -= OnReturnedToPool;
                pooledObject.transform.SetParent(transform);
                objectPool.Release(pooledObject);
            }
        }
    }
}