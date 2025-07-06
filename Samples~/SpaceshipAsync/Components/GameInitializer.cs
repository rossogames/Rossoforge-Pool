using Rossoforge.Addressables;
using Rossoforge.Core.Addressables;
using Rossoforge.Core.Pool;
using Rossoforge.Pool.Service;
using Rossoforge.Services;
using UnityEngine;

namespace Rossoforge.Pool.SpaceshipAsync
{
    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            var addressableService = new AddressableService();

            ServiceLocator.SetLocator(new DefaultServiceLocator());
            ServiceLocator.Register<IAddressableService>(addressableService);
            ServiceLocator.Register<IPoolService>(new PoolService(addressableService));
            ServiceLocator.Initialize();
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister<IPoolService>();
        }
    }
}