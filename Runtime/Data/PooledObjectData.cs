using UnityEngine;

namespace RossoForge.Pool.Data
{
    [CreateAssetMenu(fileName = nameof(PooledObjectData), menuName = "RossoForge/Pool/PooledObjectData")]
    public class PooledObjectData : ScriptableObject, IPooledObjectData
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
    public interface IPooledObjectData
    {
        string name { get; }
        int MaxSize { get; }
    }
}