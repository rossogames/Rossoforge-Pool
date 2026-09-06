# Rossoforge - Pool

<table>
  <tr>
    <td><img src="https://github.com/rossogames/Rossoforge-Pool/blob/master/logo.png?raw=true" alt="Rossoforge" width="64"/></td>
    <td><h2>Rossoforge - Pool</h2></td>
  </tr>
</table>

Rossoforge Pool is a lightweight, high-performance object pooling system for Unity. It is designed for performance-critical projects and supports data-driven configuration via ScriptableObjects. The package is modular and integrates well with Addressables and other Rossoforge libraries.

## Key features

- Fast, allocation-minimizing pooling of GameObjects and components
- Data-driven configuration using ScriptableObjects (PoolData assets)
- Synchronous and asynchronous APIs
- Optional Addressables integration for loading and caching pooled prefabs
- Simple service registration via the Rossoforge ServiceLocator

## Requirements

- Unity 6 or later
- Optional: Unity Addressables (com.unity.addressables) if you use Addressables

## Dependencies

- com.unity.addressables (optional)
- [Rossoforge-Core](https://github.com/rossogames/Rossoforge-Core.git)
- [Rossoforge-Utils](https://github.com/rossogames/Rossoforge-Utils.git)
- [Rossoforge-Services](https://github.com/rossogames/Rossoforge-Services.git)
- [Rossoforge-Addressables](https://github.com/rossogames/Rossoforge-Addressables.git) (optional)

## Installation

Add this package to your project using the Unity Package Manager (Add package from Git URL) or include it in your package registry. Example Git URL:

```
https://github.com/rossogames/Rossoforge-Pool.git
```

Make sure required Rossoforge packages are also present in your project.

## Quick start

Register the required services with the ServiceLocator in an initialization script (for example, a Bootstrapper MonoBehaviour):

```csharp
// Initialize Service (default)
private void Awake()
{
    ServiceLocator.SetLocator(new DefaultServiceLocator());
    
    var poolService = new PoolService();
    ServiceLocator.Register<IPoolService>(poolService);

    ServiceLocator.Initialize();
}

// Initialize Service with Addressables
private void Awake()
{
    ServiceLocator.SetLocator(new DefaultServiceLocator());

    var addressableService = new AddressableService();
    var poolService = new PoolService();

    ServiceLocator.Register<IAddressableService>(addressableService);
    ServiceLocator.Register<IPoolService>(poolService);

    ServiceLocator.Initialize();
}
```

Getting objects from the pool (synchronous and asynchronous examples):

```csharp
// Get a GameObject from the pool (synchronous)
var go = _poolService.Get(_missilePoolData, transform.parent, transform.position, Space.World);

// Get a GameObject asynchronously
var goAsync = await _poolService.GetAsync(_missilePoolData, transform.parent, transform.position, Space.World);

// Get a component from a pooled GameObject (synchronous)
var missile = _poolService.Get<Missile>(_missilePoolData, transform.parent, transform.position, Space.World);

// Async component retrieval
var missileAsync = await _poolService.GetAsync<Missile>(_missilePoolData, transform.parent, transform.position, Space.World);
```

Preloading a pool (warm-up):

```csharp
_poolService.Populate(_missilePoolData);
await _poolService.PopulateAsync(_missilePoolData);
```

Replace `_missilePoolData` with your PoolData ScriptableObject reference. Generic overloads return the requested component type directly from the pooled GameObject.

## Addressables

To use Addressables, register an `IAddressableService` implementation (for example, `AddressableService`) before initializing the `PoolService`. Pooled prefabs can then be loaded via Addressables and cached in the pool.

## Tutorial

A walkthrough (in Spanish) is available here:

https://www.youtube.com/watch?v=0S3NYG8uiQ0

## Contributing

Contributions, bug reports and feature requests are welcome. Please open an issue or a pull request and follow the repository contribution guidelines.

## Author

Developed by Agustin Rosso
https://www.linkedin.com/in/rossoagustin/

## License

Add a license (for example, MIT) to clarify usage rights.
