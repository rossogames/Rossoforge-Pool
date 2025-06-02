using System;
using UnityEngine;

namespace RossoForge.Pool.Components
{
    public class PooledObject : MonoBehaviour
    {
        public event Action<PooledObject> OnReturnedToPool;
        private bool _isPooled;

        private void OnEnable()
        {
            _isPooled = false;
        }
        private void OnDisable()
        {
            if (!_isPooled)
            {
                ReturnToPoolAsync();
            }
        }

        public void ReturnToPool()
        {
            _isPooled = true;
            OnReturnedToPool.Invoke(this);
        }

        private async void ReturnToPoolAsync()
        {
            _isPooled = true;
            await Awaitable.NextFrameAsync();
            OnReturnedToPool.Invoke(this);
        }
    }
}