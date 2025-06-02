using RossoForge.Pool.Data;
using RossoForge.Services;
using UnityEngine;

namespace RossoForge.Pool.Service
{
    public interface IPoolService : IService
    {
        //PooledObject Get(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo);
        //Awaitable<PooledObject> GetAsync(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo);
        T Get<T>(PooledObjectData data, Transform parent) where T : Component;
        T Get<T>(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo) where T : Component;
        Awaitable<T> GetAsync<T>(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo) where T : Component;
        void Populate(PooledObjectData data);
        void RecoverPool();
        void Unload(PooledObjectData data);
    }
}
