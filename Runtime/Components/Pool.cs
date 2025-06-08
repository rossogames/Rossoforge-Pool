using RossoForge.Core.Pool;
using UnityEngine;
using UnityEngine.Pool;

namespace RossoForge.Pool.Components
{
    public class Pool : MonoBehaviour
    {
        private IObjectPool<IPooledObject> objectPool;
        private int _activeCount;

        public int MaxSize { get; set; }
        public GameObject AssetReference { get; set; }

        public void Load()
        {
            bool collectionCheck = false;
#if UNITY_EDITOR
            collectionCheck = true; // Enable collection check in editor for debugging purposes
#endif

            objectPool = new ObjectPool<IPooledObject>(
                CreateInstance,
                null,
                OnReleaseToPool,
                OnDestroyPooledObject,
                collectionCheck,
                MaxSize,
                MaxSize);
        }
        public IPooledObject Get(Transform parent, Vector3 position, Space relativeTo)
        {
            var pooledObject = objectPool.Get();
            pooledObject.OnReturnedToPool += OnReturnedToPool;

            InitializeObject(pooledObject.gameObject, parent, position, relativeTo);

            _activeCount++;
            if (_activeCount > MaxSize)
                Debug.LogWarning($"[Pool] Pool for '{AssetReference.name}' exceeded MaxSize ({MaxSize}). ActiveCount: {_activeCount}");

            return pooledObject;
        }

        private IPooledObject CreateInstance()
        {
            GameObject obj = Instantiate(AssetReference);
            return obj.AddComponent<PooledObject>();
        }
        private void OnReleaseToPool(IPooledObject pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }
        private void OnDestroyPooledObject(IPooledObject pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
        private void InitializeObject(GameObject obj, Transform parent, Vector3 position, Space relativeTo)
        {
            obj.transform.SetParent(parent);
            if (relativeTo == Space.Self)
                obj.transform.localPosition = position;
            else
                obj.transform.position = position;

            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
        }
        private void OnReturnedToPool(IPooledObject pooledObject)
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