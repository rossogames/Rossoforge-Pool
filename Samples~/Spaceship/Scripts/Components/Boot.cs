using Rossoforge.Core.Events;
using Rossoforge.Events.Service;
using Rossoforge.Pool.Service;
using Rossoforge.Services;
using UnityEngine;

namespace Rossoforge.Pool.Samples.Spaceship
{
    public class Boot : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.SetLocator(new DefaultServiceLocator());

            var eventService = new EventService();
            var poolService = new PoolService();

            ServiceLocator.Register<IEventService>(eventService);
            ServiceLocator.Register<IPoolService>(poolService);

            ServiceLocator.Initialize();
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister<IPoolService>();
        }
    }
}