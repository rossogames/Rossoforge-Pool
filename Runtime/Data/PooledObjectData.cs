using UnityEngine;

namespace RossoForge.Pool.Data
{
    [CreateAssetMenu(fileName = nameof(PooledObjectData), menuName = "RossoForge/Pool/PooledObjectData")]
    public class PooledObjectData : ScriptableObject
    {
        [field: SerializeField]
        public GameObject PrefabReference { get; private set; }

        [field: SerializeField]
        public int MaxSize { get; private set; } = 1;

        public PooledObjectData()
        {
        }
    }
}