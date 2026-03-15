using Rossoforge.Addressables;
using Rossoforge.Core.Addressables;
using Rossoforge.Core.Pool;
using Rossoforge.Pool.Service;
using Rossoforge.Services;
using UnityEngine;

namespace Rossoforge.Pool.SpaceshipAsync
{
    public class Boot : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.SetLocator(new DefaultServiceLocator());

            var addressableService = new AddressableService();
            var poolService = new PoolService();

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