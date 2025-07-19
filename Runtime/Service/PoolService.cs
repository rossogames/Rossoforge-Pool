using Rossoforge.Core.Addressables;
using Rossoforge.Core.Pool;
using Rossoforge.Core.Services;
using Rossoforge.Pool.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Rossoforge.Pool.Service
{
    public class PoolService : IPoolService, IInitializable
    {
        private Dictionary<string, Components.Pool> _poolGroups;
        private GameObject _root;

        private IAddressableService _addressableService;

        public PoolService()
        {
        }
        public PoolService(IAddressableService addressableService)
        {
            _addressableService = addressableService;
        }

        public void Initialize()
        {
            _poolGroups = new Dictionary<string, Components.Pool>();
            _root = new GameObject("PoolRoot");
            _root.AddComponent<PoolRoot>();
        }
        public void Dispose()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Object.DestroyImmediate(_root);
            else
                Object.Destroy(_root);
#else
            Object.Destroy(_root);
#endif
        }

        // Default
        public T Get<T>(IPooledGameobjectData data, Transform parent, Vector3 position, Space relativeTo) where T : Component
        {
            var obj = Get(data, parent, position, relativeTo);
            return obj.gameObject.GetComponent<T>();
        }
        public IPooledObject Get(IPooledGameobjectData data, Transform parent, Vector3 position, Space relativeTo)
        {
            var pool = GetPoolGroup(data, data.AssetReference);
            return pool.Get(parent, position, relativeTo);
        }
        public void Populate(IPooledGameobjectData data)
        {
            Populate(data, data.AssetReference);
        }

        //Async
        public async Awaitable<T> GetAsync<T>(IPooledObjectAsyncData data, Transform parent, Vector3 position, Space relativeTo) where T : Component
        {
            var obj = await GetAsync(data, parent, position, relativeTo);
            return obj.gameObject.GetComponent<T>();
        }
        public async Awaitable<IPooledObject> GetAsync(IPooledObjectAsyncData data, Transform parent, Vector3 position, Space relativeTo)
        {
            CheckAddressableService();
            var assetReference = await _addressableService.LoadAssetAsync<GameObject>(data.AssetReference);
            var pool = GetPoolGroup(data, assetReference);
            return pool.Get(parent, position, relativeTo);
        }
        public async Awaitable PopulateAsync(IPooledObjectAsyncData data)
        {
            CheckAddressableService();
            var assetReference = await _addressableService.LoadAssetAsync<GameObject>(data.AssetReference);
            Populate(data, assetReference);
        }

        public bool Clear(IPooledObjectData data)
        {
            if (_poolGroups.TryGetValue(data.name, out Components.Pool pool))
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    Object.DestroyImmediate(pool.gameObject);
                else
                    Object.Destroy(pool.gameObject);
#else
                Object.Destroy(pool.gameObject);
#endif
                _poolGroups.Remove(data.name);
                return true;
            }
            return false;
        }

        private Components.Pool GetPoolGroup(IPooledObjectData data, GameObject assetReference)
        {
            if (_poolGroups.TryGetValue(data.name, out Components.Pool pool))
            {
                return pool;
            }

            var newPool = CreatePool(data, assetReference, _root.transform);
            _poolGroups.Add(data.name, newPool);
            return newPool;
        }
        private Components.Pool CreatePool(IPooledObjectData data, GameObject assetReference, Transform parent)
        {
            var obj = new GameObject(data.name);
            obj.transform.parent = parent;

            var pool = obj.AddComponent<Components.Pool>();
            pool.AssetReference = assetReference;
            pool.MaxSize = data.MaxSize;
            pool.Load();

            return pool;
        }
        private void Populate(IPooledObjectData data, GameObject assetReference)
        {
            List<IPooledObject> pooledObjects = new();

            var pool = GetPoolGroup(data, assetReference);
            for (int i = 0; i < data.MaxSize; i++)
            {
                var obj = pool.Get(pool.gameObject.transform, Vector3.zero, Space.World);
                pooledObjects.Add(obj);
            }

            foreach (var obj in pooledObjects)
                obj.ReturnToPool();
        }
        private void CheckAddressableService()
        {
            if (_addressableService == null)
            {
                string errorMessage = "Failed to load asset: AddressableService is null. Ensure it is properly registered in the service container.";
                Debug.LogError(errorMessage);
                throw new System.NullReferenceException();
            }
        }
    }
}