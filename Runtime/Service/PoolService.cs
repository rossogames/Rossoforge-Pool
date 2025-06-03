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

        public T Get<T>(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo) where T : Component
        {
            var obj = Get(data, parent, position, relativeTo);
            return obj.GetComponent<T>();
        }

        public PooledObject Get(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo)
        {
            var pool = GetPoolGroup(data);
            return pool.Get(parent, position, relativeTo);
        }

        public void Populate(PooledObjectData data)
        {
            List<PooledObject> pooledObjects = new();

            var pool = GetPoolGroup(data);
            for (int i = 0; i < data.MaxSize; i++)
            {
                var obj = pool.Get(pool.gameObject.transform, Vector3.zero, Space.World);
                pooledObjects.Add(obj);
            }

            foreach (var obj in pooledObjects)
                obj.ReturnToPool();
        }

        public void Clear(PooledObjectData data)
        {
            if (_poolGroups.TryGetValue(data.name, out Components.Pool pool))
            {
                Object.Destroy(pool.gameObject);
                _poolGroups.Remove(data.name);
            }
        }

        private Components.Pool GetPoolGroup(PooledObjectData data)
        {
            if (!_poolGroups.ContainsKey(data.name))
            {
                var newPool = CreatePool(data, _root.transform);
                _poolGroups.Add(data.name, newPool);
            }

            return _poolGroups[data.name];
        }
        private Components.Pool CreatePool(PooledObjectData data, Transform parent)
        {
            var obj = new GameObject(data.name);
            obj.transform.parent = parent;

            var pool = obj.AddComponent<Components.Pool>();
            pool.AssetTemplate = data.PrefabReference;
            pool.MaxSize = data.MaxSize;
            pool.Load();

            return pool;
        }
    }
}