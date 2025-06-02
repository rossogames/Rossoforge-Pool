using UnityEngine;

namespace RossoForge.Pool.Components
{
    public class PoolRoot : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}