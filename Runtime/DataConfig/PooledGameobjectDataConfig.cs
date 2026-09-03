using UnityEngine;

namespace Rossoforge.Pool.DataConfig
{
    [CreateAssetMenu(fileName = nameof(PooledGameobjectDataConfig), menuName = "Rossoforge/Data Config/Pool/Pooled Gameobject")]
    public class PooledGameobjectDataConfig : ScriptableObject, IPooledGameobjectDataConfig
    {
        [field: SerializeField]
        public GameObject AssetReference { get; private set; }

        [field: SerializeField]
        public int MaxSize { get; private set; } = 1;

        private void OnValidate()
        {
            MaxSize = Mathf.Max(1, MaxSize);
        }
    }
}