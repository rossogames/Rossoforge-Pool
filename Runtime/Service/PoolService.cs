using RossoForge.Pool.Data;
using UnityEngine;

namespace RossoForge.Pool.Service
{
    public class PoolService : IPoolService
    {
        public T Get<T>(PooledObjectData data, Transform parent) where T : Component
        {
            throw new System.NotImplementedException();
        }

        public T Get<T>(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo) where T : Component
        {
            throw new System.NotImplementedException();
        }

        public Awaitable<T> GetAsync<T>(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo) where T : Component
        {
            throw new System.NotImplementedException();
        }

        public void Populate(PooledObjectData data)
        {
            throw new System.NotImplementedException();
        }

        public void RecoverPool()
        {
            throw new System.NotImplementedException();
        }

        public void Unload(PooledObjectData data)
        {
            throw new System.NotImplementedException();
        }

        /*
        private readonly Dictionary<PooledObjectData, Components.Pool> _poolGroups;
        private readonly GameObject _root;

        public PoolService()
        {
            _poolGroups = new Dictionary<PooledObjectEntity, Components.Pool>();
            _root = new GameObject("PoolRoot");
            _root.AddComponent<PoolRoot>();
        }
        public void Initialize()
        {
            _eventService = ServiceLocator.Current.Get<IEventService>();
            _addressablesService = ServiceLocator.Current.Get<IAddressablesService>();
        }
        public void Dispose()
        {
            Object.Destroy(_root);
        }

        public PooledObject Get(PooledObjectEntity profile, Transform parent, Vector3 position, Space relativeTo)
        {
            var pool = GetPoolGroup(profile);
            return pool.Get(parent, position, relativeTo);
        }
        public async Awaitable<PooledObject> GetAsync(PooledObjectEntity profile, Transform parent, Vector3 position, Space relativeTo)
        {
            if (!_addressablesService.IsAssetLoaded(profile.AddressableEntity))
                await _addressablesService.LoadAsset(profile.AddressableEntity);

            return Get(profile, parent, position, relativeTo);
        }
        public T Get<T>(PooledObjectEntity profile, Transform parent) where T : Component
        {
            return Get<T>(profile, parent, Vector3.zero, Space.Self);
        }
        public T Get<T>(PooledObjectEntity profile, Transform parent, Vector3 position, Space relativeTo) where T : Component
        {
            var obj = Get(profile, parent, position, relativeTo);
            return obj.GetComponent<T>();
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

        private Components.Pool GetPoolGroup(PooledObjectEntity profile)
        {
            if (!_poolGroups.ContainsKey(profile))
            {
                var newPool = CreatePool(profile, _root.transform);
                _poolGroups.Add(profile, newPool);
            }

            return _poolGroups[profile];
        }
        private Components.Pool CreatePool(PooledObjectEntity profile, Transform parent)
        {
            if (profile.AddressableEntity == null)
            {
                LogHelper.LogMessage($"Missing asset reference: {profile.name}", LogType.Error);
                return null;
            }

            var objReference = _addressablesService.GetAssetReference<GameObject>(profile.AddressableEntity);
            var pool = CreatePool(profile, objReference, profile.MaxSize, parent);

            return pool;
        }
        private Components.Pool CreatePool(PooledObjectEntity profile, GameObject assetTemplate, int maxSize, Transform parent)
        {
            var obj = new GameObject(profile.name);
            obj.transform.parent = parent;

            var pool = obj.AddComponent<Components.Pool>();
            pool.AssetTemplate = assetTemplate;
            pool.MaxSize = maxSize;
            pool.Load();

            return pool;
        }
        */
    }
}