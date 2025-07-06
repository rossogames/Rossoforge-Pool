using Rossoforge.Core.Pool;
using Rossoforge.Pool.Service;
using Rossoforge.Services;
using UnityEngine;

namespace Rossoforge.Pool.Spaceship
{
    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.SetLocator(new DefaultServiceLocator());
            ServiceLocator.Register<IPoolService>(new PoolService());
            ServiceLocator.Initialize();
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister<IPoolService>();
        }
    }
}