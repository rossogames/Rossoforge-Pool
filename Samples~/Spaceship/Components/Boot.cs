using Rossoforge.Core.Pool;
using Rossoforge.Pool.Service;
using Rossoforge.Services;
using UnityEngine;

namespace Rossoforge.Pool.Spaceship
{
    public class Boot : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.SetLocator(new DefaultServiceLocator());

            var poolService = new PoolService();

            ServiceLocator.Register<IPoolService>(poolService);

            ServiceLocator.Initialize();
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister<IPoolService>();
        }
    }
}