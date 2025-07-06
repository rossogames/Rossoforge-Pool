using NSubstitute;
using NUnit.Framework;
using Rossoforge.Core.Addressables;
using Rossoforge.Core.Pool;
using Rossoforge.Pool.Components;
using Rossoforge.Pool.Service;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rossoforge.Pool.Tests
{
    public class PoolServiceTests
    {
        [Test]
        public void Initialize_CreatesRootAndEmptyPoolGroups()
        {
            var service = new PoolService();

            service.Initialize();

            var rootObject = GameObject.Find("PoolRoot");
            Assert.IsNotNull(rootObject, "Root GameObject should be created");
            Assert.IsNotNull(rootObject.GetComponent<PoolRoot>(), "PoolRoot component should be added");
        }

        [Test]
        public void Get_ReturnsPooledObject()
        {
            // Arrange
            var service = new PoolService();
            service.Initialize();

            var data = Substitute.For<IPooledGameobjectData>();
            data.name.Returns("MyPrefab");
            data.MaxSize.Returns(5);
            data.AssetReference.Returns(new GameObject("AssetRef"));

            // Act
            var pooled = service.Get(data, null, Vector3.zero, Space.World);

            // Assert
            Assert.IsNotNull(pooled);
            var poolObject = GameObject.Find("MyPrefab");
            Assert.IsNotNull(poolObject);
        }

        [Test]
        public void Get_ReturnsPooledObjectComponent()
        {
            // Arrange
            var service = new PoolService();
            service.Initialize();

            var data = Substitute.For<IPooledGameobjectData>();
            data.name.Returns("MyPrefab");
            data.MaxSize.Returns(5);
            data.AssetReference.Returns(new GameObject("AssetRef"));

            // Act
            var pooled = service.Get<PooledObject>(data, null, Vector3.zero, Space.World);

            // Assert
            Assert.IsNotNull(pooled);
            var poolObject = GameObject.Find("MyPrefab");
            Assert.IsNotNull(poolObject);
        }

        [Test]
        public void Populate_CreatesPoolAndPrepopulatesObjects()
        {
            var service = new PoolService();
            service.Initialize();

            var data = Substitute.For<IPooledGameobjectData>();
            data.name.Returns("PreloadPrefab");
            data.MaxSize.Returns(3);
            data.AssetReference.Returns(new GameObject("AssetRef"));

            // Act
            service.Populate(data);

            var poolObject = GameObject.Find("PreloadPrefab");
            Assert.IsNotNull(poolObject);
        }

        [Test]
        public void Clear_ExistingPool()
        {
            var service = new PoolService();
            service.Initialize();

            var data = Substitute.For<IPooledGameobjectData>();
            data.name.Returns("ToClear");
            data.MaxSize.Returns(1);
            data.AssetReference.Returns(new GameObject("AssetRef"));

            var pooled = service.Get(data, null, Vector3.zero, Space.World);
            Assert.IsNotNull(pooled);

            bool isCleared = service.Clear(data);
            Assert.IsTrue(isCleared);
        }

        [Test]
        public void Clear_NonExistingPool()
        {
            var service = new PoolService();
            service.Initialize();

            var data = Substitute.For<IPooledGameobjectData>();
            data.name.Returns("ToClear");
            data.MaxSize.Returns(1);
            data.AssetReference.Returns(new GameObject("AssetRef"));

            bool isCleared = service.Clear(data);
            Assert.IsFalse(isCleared);
        }

        [Test]
        public void Dispose_DestroysRootObject()
        {
            var service = new PoolService();
            service.Initialize();

            var root = GameObject.Find("PoolRoot");
            Assert.IsNotNull(root);

            service.Dispose();

            Assert.IsTrue(root == null || root.Equals(null), "Root should be destroyed");
        }

        [Test]
        public async Task GetAsync_ReturnsPooledObject()
        {
            var addressableService = Substitute.For<IAddressableService>();
            var service = new PoolService(addressableService);
            service.Initialize();

            var assetReference = new AssetReferenceGameObject(Guid.NewGuid().ToString());

            var data = Substitute.For<IPooledObjectAsyncData>();
            data.MaxSize.Returns(1);
            data.name.Returns("async_key");
            data.AssetReference.Returns(assetReference);

            Func<NSubstitute.Core.CallInfo, Awaitable<GameObject>> value = async call =>
                {
                    await Awaitable.WaitForSecondsAsync(0.1f); // Simulate async loading delay
                    return new GameObject("AsyncPrefab");
                };

            addressableService
                .LoadAssetAsync<GameObject>(data.AssetReference)
                .Returns(value);

            // Act
            var result = await service.GetAsync(data, null, Vector3.zero, Space.World);

            Assert.IsNotNull(result);
            Assert.IsNotNull(GameObject.Find("AsyncPrefab"));
        }

        /*


            [Test]
            public void GetAsync_ThrowsIfAddressableServiceIsNull()
            {
                var service = new PoolService();
                service.Initialize();

                var data = Substitute.For<IPooledObjectAsyncData>();
                data.AssetReference.Returns("async_key");

                Assert.ThrowsAsync<NullReferenceException>(async () =>
                {
                    await service.GetAsync(data, null, Vector3.zero, Space.World);
                });
            }
            */
    }
}
