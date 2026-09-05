using Rossoforge.Addressables.Service;
using Rossoforge.Events.Service;
using Rossoforge.Pool.Service;
using Rossoforge.Services.Locator;
using UnityEngine;

namespace Rossoforge.Pool.Samples.SpaceshipAsync
{
    public class Boot : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.SetLocator(new DefaultServiceLocator());

            var eventService = new EventService();
            var addressableService = new AddressableService();
            var poolService = new PoolService();

            ServiceLocator.Register<IEventService>(eventService);
            ServiceLocator.Register<IAddressableService>(addressableService);
            ServiceLocator.Register<IPoolService>(poolService);

            ServiceLocator.Initialize();
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister<IPoolService>();
        }
    }
}