using Rossoforge.Core.Events;
using Rossoforge.Core.Pool;
using Rossoforge.Pool.Events;
using Rossoforge.Services;
using System;
using UnityEngine;

namespace Rossoforge.Pool.Components
{
    public class PooledObject : MonoBehaviour, IPooledObject, 
        IEventListener<ForceReturnToPoolEvent>
    {
        private IEventService _eventService;

        public event Action<IPooledObject> OnReturnedToPool;
        private bool _isPooled;

        private void Awake()
        {
            _eventService = ServiceLocator.Get<IEventService>();
        }

        private void OnEnable()
        {
            _eventService.RegisterListener<ForceReturnToPoolEvent>(this);
            _isPooled = false;
        }
        private void OnDisable()
        {
            _eventService.UnregisterListener<ForceReturnToPoolEvent>(this);

            if (!_isPooled)
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

        public void OnEventInvoked(ForceReturnToPoolEvent eventArg)
        {
            ReturnToPool();
        }
    }
}