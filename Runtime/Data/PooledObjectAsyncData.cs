using RossoForge.Core.Pool;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RossoForge.Pool.Data
{
    [CreateAssetMenu(fileName = nameof(PooledObjectAsyncData), menuName = "RossoForge/Pool/PooledObjectAsyncData")]
    public class PooledObjectAsyncData : ScriptableObject, IPooledObjectAsyncData
    {
        [field: SerializeField]
        public AssetReferenceGameObject AssetReference { get; private set; }

        [field: SerializeField]
        public int MaxSize { get; private set; } = 1;

        private void OnValidate()
        {
            MaxSize = Mathf.Max(1, MaxSize);
        }
    }
}