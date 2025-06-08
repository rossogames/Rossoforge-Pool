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
* https://github.com/rossogames/Rossoforge-Addressables.git

Watch the tutorial on [Pending]

#
```csharp
// Initialize Service default
private void Awake()
{
    ServiceLocator.SetLocator(new DefaultServiceLocator());
    ServiceLocator.Register<IPoolService>(new PoolService());
    ServiceLocator.Initialize();
}

// Initialize Service with addressables
private void Awake()
{
    var addressableService = new AddressableService();
    ServiceLocator.SetLocator(new DefaultServiceLocator());
    ServiceLocator.Register<IAddressableService>(addressableService);
    ServiceLocator.Register<IPoolService>(new PoolService(addressableService));
    ServiceLocator.Initialize();
}

// Get the gameobject from the pool 
_poolService.Get(_missilePoolData, transform.parent, transform.position, Space.World);
_poolService.GetAsync(_missilePoolData, transform.parent, transform.position, Space.World);

// Get the gameobject's component from the pool
_poolService.Get<Missile>(_missilePoolData, transform.parent, transform.position, Space.World);
_poolService.GetAsync<Missile>(_missilePoolData, transform.parent, transform.position, Space.World);

// Preload pool
_poolService.Populate(_missilePoolData);
_poolService.PopulateAsync(_missilePoolData);

```
#
This package is part of the **RossoForge** suite, designed to streamline and enhance Unity development workflows.

Developed by Agustin Rosso
https://www.linkedin.com/in/rossoagustin/
