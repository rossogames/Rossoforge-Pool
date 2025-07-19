using UnityEngine;

namespace Rossoforge.Pool.Components
{
    public class PoolRoot : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}