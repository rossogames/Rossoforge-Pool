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
            ReturnToPoolAsync();
        }

        public void ReturnToPool()
        {
            _isPooled = true;
            OnReturnedToPool.Invoke(this);
        }

        private async void ReturnToPoolAsync()
        {
            await Awaitable.NextFrameAsync();
            if (!_isPooled)
            {
                _isPooled = true;
                OnReturnedToPool?.Invoke(this);
            }
        }
    }
}