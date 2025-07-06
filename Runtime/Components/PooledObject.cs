using Rossoforge.Core.Pool;
using System;
using UnityEngine;

namespace Rossoforge.Pool.Components
{
    public class PooledObject : MonoBehaviour, IPooledObject
    {
        public event Action<IPooledObject> OnReturnedToPool;
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