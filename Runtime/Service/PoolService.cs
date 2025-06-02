using RossoForge.Pool.Components;
using RossoForge.Pool.Data;
using RossoForge.Services;
using System.Collections.Generic;
using UnityEngine;

namespace RossoForge.Pool.Service
{
    public class PoolService : IPoolService, IInitializable
    {
        private Dictionary<PooledObjectData, Components.Pool> _poolGroups;
        private GameObject _root;

        public void Initialize()
        {
            _poolGroups = new Dictionary<PooledObjectData, Components.Pool>();
            _root = new GameObject("PoolRoot");
            _root.AddComponent<PoolRoot>();
        }
        public void Dispose()
        {
            Object.Destroy(_root);
        }

        public T Get<T>(PooledObjectData data, Transform parent) where T : Component
        {
            throw new System.NotImplementedException();
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

        //public void Populate(PooledObjectData data)
        //{
        //    throw new System.NotImplementedException();
        //}
        //
        //public void RecoverPool()
        //{
        //    throw new System.NotImplementedException();
        //}
        //
        //public void Unload(PooledObjectData data)
        //{
        //    throw new System.NotImplementedException();
        //}

        /*



        public T Get<T>(PooledObjectEntity profile, Transform parent) where T : Component
        {
            return Get<T>(profile, parent, Vector3.zero, Space.Self);
        }
        public T Get<T>(PooledObjectEntity profile, Transform parent, Vector3 position, Space relativeTo) where T : Component
        {

        }
        public async Awaitable<T> GetAsync<T>(PooledObjectEntity profile, Transform parent, Vector3 position, Space relativeTo) where T : Component
        {
            var obj = await GetAsync(profile, parent, position, relativeTo);
            return obj.GetComponent<T>();
        }
        public void Populate(PooledObjectEntity profile)
        {
            List<PooledObject> pooledObjects = new();

            var pool = GetPoolGroup(profile);
            for (int i = 0; i < profile.MaxSize; i++)
            {
                var obj = pool.Get(pool.gameObject.transform, Vector3.zero, Space.World);
                pooledObjects.Add(obj);
            }

            foreach (var obj in pooledObjects)
                obj.ReturnToPool();
        }
        public void Unload(PooledObjectEntity profile)
        {
            var pool = _poolGroups[profile];
            Object.Destroy(pool.gameObject);
            _poolGroups.Remove(profile);
        }
        */


        private Components.Pool GetPoolGroup(PooledObjectData data)
        {
            if (!_poolGroups.ContainsKey(data))
            {
                var newPool = CreatePool(data, _root.transform);
                _poolGroups.Add(data, newPool);
            }

            return _poolGroups[data];
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