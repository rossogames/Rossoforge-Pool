using RossoForge.Core.Pool;
using UnityEngine;

namespace RossoForge.Pool.Data
{
    [CreateAssetMenu(fileName = nameof(PooledGameobjectData), menuName = "RossoForge/Pool/Pooled Gameobject Data")]
    public class PooledGameobjectData : ScriptableObject, IPooledGameobjectData
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