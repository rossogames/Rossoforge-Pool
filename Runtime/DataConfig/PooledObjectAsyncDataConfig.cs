#if HAS_ADDRESSABLES
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rossoforge.Pool.DataConfig
{
    [CreateAssetMenu(fileName = nameof(PooledObjectAsyncDataConfig), menuName = "Rossoforge/Data Config/Pool/Pooled Async Gameobject")]
    public class PooledObjectAsyncDataConfig : ScriptableObject, IPooledObjectAsyncDataConfig
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
#endif