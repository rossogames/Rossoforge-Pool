using RossoForge.Addressables;
using RossoForge.Core.Addressables;
using RossoForge.Core.Pool;
using RossoForge.Pool.Service;
using RossoForge.Services;
using UnityEngine;

namespace RossoForge.Pool.SpaceshipAsync
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