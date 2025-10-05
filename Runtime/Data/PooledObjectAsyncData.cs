using Rossoforge.Core.Pool;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rossoforge.Pool.Data
{
    [CreateAssetMenu(fileName = nameof(PooledObjectAsyncData), menuName = "Rossoforge/Pool/Pooled Object Async Data")]
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