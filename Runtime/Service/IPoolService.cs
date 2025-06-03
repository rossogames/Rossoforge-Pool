using RossoForge.Pool.Components;
using RossoForge.Pool.Data;
using RossoForge.Services;
using UnityEngine;

namespace RossoForge.Pool.Service
{
    public interface IPoolService : IService
    {
        T Get<T>(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo) where T : Component;
        PooledObject Get(PooledObjectData data, Transform parent, Vector3 position, Space relativeTo);
        void Populate(PooledObjectData data);
        //void RecoverPool();
        void Clear(PooledObjectData data);
    }
}
