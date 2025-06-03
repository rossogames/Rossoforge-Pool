using RossoForge.Pool.Service;
using RossoForge.Services.Locator;
using UnityEngine;

namespace RossoForge.Pool.Spaceship
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