# Rosso Games

<table>
  <tr>
    <td><img src="https://github.com/rossogames/Rossoforge-Events/blob/master/logo.png?raw=true" alt="RossoForge" width="64"/></td>
    <td><h2>RossoForge - Toolbar</h2></td>
  </tr>
</table>

**RossoForge - Pool** A lightweight object pooling system for Unity, designed for performance-critical applications. Includes support for data-driven configuration via ScriptableObjects, runtime instantiation, and integration with a global service locator. Ideal for reducing GC allocations and improving scene performance in games with frequent object spawning.

The following dependencies must be installed
* https://github.com/rossogames/Rossoforge-Services.git

Watch the tutorial on [Pending]

#
```csharp
// Initialize Service
public class GameInitializer : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.SetLocator(new DefaultServiceLocator());
        ServiceLocator.Register<IPoolService>(new PoolService());
        ServiceLocator.Initialize();
    }
}

// Get the game object from the pool 
public class Spaceship : MonoBehaviour
{
    [SerializeField]
    private PooledObjectData _missilePoolData;

    private IPoolService _poolService;

    void Start()
    {
        _poolService = ServiceLocator.Get<IPoolService>();
    }

    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _poolService.Get(_missilePoolData, transform.parent, transform.position, Space.World);
    }
}
```
#
This package is part of the **RossoForge** suite, designed to streamline and enhance Unity development workflows.

Developed by Agustin Rosso
https://www.linkedin.com/in/rossoagustin/
