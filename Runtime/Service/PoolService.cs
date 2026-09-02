using Rossoforge.Core.Addressables;
using Rossoforge.Core.Components;
using Rossoforge.Core.Events;
using Rossoforge.Core.Pool;
using Rossoforge.Core.Services;
using Rossoforge.Pool.Events;
using Rossoforge.Services;
using Rossoforge.Utils.Logger;
using System.Collections.Generic;
using UnityEngine;

namespace Rossoforge.Pool.Service
{
    public class PoolService : IPoolService, IInitializable
    {
        public const string DEFAULT_CATEGORY = "default";

        private Dictionary<string, List<IPooledObjectData>> _categoryGroups;
        private Dictionary<string, Components.Pool> _poolGroups;
        private GameObject _root;

        private IAddressableService _addressableService;
        private IEventService _eventService;

        public void Initialize()
        {
            _poolGroups = new Dictionary<string, Components.Pool>();
            _categoryGroups = new Dictionary<string, List<IPooledObjectData>>();
            _root = new GameObject("PoolRoot");
            _root.AddComponent<DontDestroyRoot>();

            ServiceLocator.TryGet<IAddressableService>(out _addressableService);
            _eventService = ServiceLocator.Get<IEventService>();
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
        public T Get<T>(IPooledGameobjectData data, Transform parent, Vector3 position, Space relativeTo, string category = DEFAULT_CATEGORY) where T : Component
        {
            var obj = Get(data, parent, position, relativeTo, category);
            return obj.gameObject.GetComponent<T>();
        }
        public IPooledObject Get(IPooledGameobjectData data, Transform parent, Vector3 position, Space relativeTo, string category = DEFAULT_CATEGORY)
        {
            RegisterData(category, data);
            var pool = GetPoolGroup(data, data.AssetReference);
            return pool.Get(parent, position, relativeTo);
        }
        public void Populate(IPooledGameobjectData data, string category = DEFAULT_CATEGORY)
        {
            RegisterData(category, data);
            Populate(data, data.AssetReference);
        }
        public void ForceReturnAll()
        {
            _eventService.Raise<ForceReturnToPoolEvent>();
        }

#if HAS_ADDRESSABLES
        public async Awaitable<T> GetAsync<T>(IPooledObjectAsyncData data, Transform parent, Vector3 position, Space relativeTo, string category = DEFAULT_CATEGORY) where T : Component
        {
            var obj = await GetAsync(data, parent, position, relativeTo, category);
            return obj.gameObject.GetComponent<T>();
        }
        public async Awaitable<IPooledObject> GetAsync(IPooledObjectAsyncData data, Transform parent, Vector3 position, Space relativeTo, string category = DEFAULT_CATEGORY)
        {
            CheckAddressableService();
            var assetReference = await _addressableService.LoadAssetAsync<GameObject>(data.AssetReference);
            RegisterData(category, data);
            var pool = GetPoolGroup(data, assetReference);
            return pool.Get(parent, position, relativeTo);
        }
        public async Awaitable PopulateAsync(IPooledObjectAsyncData data, string category = DEFAULT_CATEGORY)
        {
            CheckAddressableService();
            var assetReference = await _addressableService.LoadAssetAsync<GameObject>(data.AssetReference);
            RegisterData(category, data);
            Populate(data, assetReference);
        }
#endif
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

        public bool Clear(string category)
        {
            if (string.IsNullOrEmpty(category))
                category = DEFAULT_CATEGORY;

            if (_categoryGroups.TryGetValue(category, out var list))
            {
                var items = new List<IPooledObjectData>(list);
                bool anyCleared = false;
                foreach (var item in items)
                {
                    if (Clear(item))
                        anyCleared = true;
                }

                _categoryGroups.Remove(category);
                return anyCleared;
            }

            return false;
        }

        private void RegisterData(string category, IPooledObjectData data)
        {
            if (string.IsNullOrEmpty(category))
                category = DEFAULT_CATEGORY;

            if (!_categoryGroups.TryGetValue(category, out var list))
            {
                list = new List<IPooledObjectData>();
                _categoryGroups.Add(category, list);
            }

            if (!list.Contains(data))
                list.Add(data);
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
                RossoLogger.Error(errorMessage);
                throw new System.NullReferenceException();
            }
        }
    }
}