using RossoForge.Addressables;
using RossoForge.Pool.Components;
using RossoForge.Pool.Data;
using RossoForge.Services;
using System.Collections.Generic;
using UnityEngine;

namespace RossoForge.Pool.Service
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
            Object.Destroy(_root);
        }

        // Default
        public T Get<T>(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo) where T : Component
        {
            var obj = Get(data, parent, position, relativeTo);
            return obj.GetComponent<T>();
        }
        public PooledObject Get(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo)
        {
            var pool = GetPoolGroup(data, data.AssetReference);
            return pool.Get(parent, position, relativeTo);
        }
        public void Populate(PooledObjectData data)
        {
            Populate(data, data.AssetReference);
        }

        //Async
        public async Awaitable<T> GetAsync<T>(PooledObjectAsyncData data, Transform parent, Vector3 position, Space relativeTo) where T : Component
        {
            var obj = await GetAsync(data, parent, position, relativeTo);
            return obj.GetComponent<T>();
        }
        public async Awaitable<PooledObject> GetAsync(PooledObjectAsyncData data, Transform parent, Vector3 position, Space relativeTo)
        {
            var assetReference = await _addressableService.LoadAsync<GameObject>(data.AssetReference);
            var pool = GetPoolGroup(data, assetReference);
            return pool.Get(parent, position, relativeTo);
        }
        public async Awaitable PopulateAsync(PooledObjectAsyncData data)
        {
            var assetReference = await _addressableService.LoadAsync<GameObject>(data.AssetReference);
            Populate(data, assetReference);
        }

        public void Clear(IPooledObjectData data)
        {
            if (_poolGroups.TryGetValue(data.name, out Components.Pool pool))
            {
                Object.Destroy(pool.gameObject);
                _poolGroups.Remove(data.name);
            }
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
            List<PooledObject> pooledObjects = new();

            var pool = GetPoolGroup(data, assetReference);
            for (int i = 0; i < data.MaxSize; i++)
            {
                var obj = pool.Get(pool.gameObject.transform, Vector3.zero, Space.World);
                pooledObjects.Add(obj);
            }

            foreach (var obj in pooledObjects)
                obj.ReturnToPool();
        }
    }
}