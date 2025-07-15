using NSubstitute;
using NUnit.Framework;
using Rossoforge.Core.Addressables;
using Rossoforge.Core.Pool;
using Rossoforge.Pool.Components;
using Rossoforge.Pool.Service;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Rossoforge.Pool.Tests
{
    [TestFixture]
    public class PoolServiceTests
    {
        [TearDown]
        public void TearDown()
        {
            var root = GameObject.Find("PoolRoot");
            if (root != null)
                GameObject.DestroyImmediate(root);
        }

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
            data.name.Returns("MyPrefab1");
            data.MaxSize.Returns(5);
            data.AssetReference.Returns(new GameObject("MyPrefab1"));

            // Act
            var pooled = service.Get(data, null, new Vector3(1,2,3), Space.World);

            // Assert
            Assert.IsNotNull(pooled);
            var poolObject = GameObject.Find("MyPrefab1(Clone)");
            Assert.IsNotNull(poolObject);

            Assert.AreEqual(new Vector3(1, 2, 3), poolObject.transform.position, "Pooled object should be at the specified position");
        }

        [Test]
        public void Get_ReturnsPooledObjectComponent()
        {
            // Arrange
            var service = new PoolService();
            service.Initialize();

            var data = Substitute.For<IPooledGameobjectData>();
            data.name.Returns("MyPrefab2");
            data.MaxSize.Returns(5);
            data.AssetReference.Returns(new GameObject("MyPrefab2"));

            // Act
            var pooled = service.Get<PooledObject>(data, null, Vector3.zero, Space.World);

            // Assert
            Assert.IsNotNull(pooled);
            var poolObject = GameObject.Find("MyPrefab2(Clone)");
            Assert.IsNotNull(poolObject);
        }

        [Test]
        public void Populate()
        {
            var service = new PoolService();
            service.Initialize();

            var data = Substitute.For<IPooledGameobjectData>();
            data.name.Returns("MyPrefab3");
            data.MaxSize.Returns(3);
            data.AssetReference.Returns(new GameObject("MyPrefab3"));

            // Act
            service.Populate(data);

            var poolObject = GameObject.Find("MyPrefab3");
            Assert.IsNotNull(poolObject);
        }

        [Test]
        public void Clear_ExistingPool()
        {
            var service = new PoolService();
            service.Initialize();

            var data = Substitute.For<IPooledGameobjectData>();
            data.name.Returns("MyPrefab4");
            data.MaxSize.Returns(1);
            data.AssetReference.Returns(new GameObject("MyPrefab4"));

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
            data.name.Returns("MyPrefab5");
            data.MaxSize.Returns(1);
            data.AssetReference.Returns(new GameObject("MyPrefab5"));

            // Act
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

            // Act
            service.Dispose();

            Assert.IsTrue(root == null || root.Equals(null), "Root should be destroyed");
        }

        [UnityTest]
        public IEnumerator GetAsync_ReturnsPooledObject()
        {
            yield return Run().AsCoroutine();

            async Task Run()
            {
                var addressableService = Substitute.For<IAddressableService>();
                var service = new PoolService(addressableService);
                service.Initialize();

                var assetReference = new AssetReferenceGameObject(Guid.NewGuid().ToString());

                var data = Substitute.For<IPooledObjectAsyncData>();
                data.MaxSize.Returns(1);
                data.name.Returns("async_key");
                data.AssetReference.Returns(assetReference);

                WhenAddressableLoadAssetAsyncGameObject(addressableService, data);

                // Act
                var result = await service.GetAsync(data, null, Vector3.zero, Space.World);
                await Awaitable.NextFrameAsync();

                Assert.IsNotNull(result);
                Assert.IsNotNull(GameObject.Find("AsyncPrefab"));
            }
        }

        [UnityTest]
        public IEnumerator GetAsync_ReturnsPooledComponent()
        {
            yield return Run().AsCoroutine();

            async Task Run()
            {
                var addressableService = Substitute.For<IAddressableService>();
                var service = new PoolService(addressableService);
                service.Initialize();

                var assetReference = new AssetReferenceGameObject(Guid.NewGuid().ToString());

                var data = Substitute.For<IPooledObjectAsyncData>();
                data.MaxSize.Returns(1);
                data.name.Returns("async_key");
                data.AssetReference.Returns(assetReference);

                WhenAddressableLoadAssetAsyncGameObject(addressableService, data);

                // Act
                var result = await service.GetAsync<PooledObject>(data, null, Vector3.zero, Space.World);

                Assert.IsNotNull(result);
                Assert.IsNotNull(GameObject.Find("AsyncPrefab"));
            }
        }

        [UnityTest]
        public IEnumerator PupulateAsync()
        {
            yield return Run().AsCoroutine();

            async Task Run()
            {
                var addressableService = Substitute.For<IAddressableService>();
                var service = new PoolService(addressableService);
                service.Initialize();

                var assetReference = new AssetReferenceGameObject(Guid.NewGuid().ToString());

                var data = Substitute.For<IPooledObjectAsyncData>();
                data.MaxSize.Returns(1);
                data.name.Returns("async_key");
                data.AssetReference.Returns(assetReference);

                WhenAddressableLoadAssetAsyncGameObject(addressableService, data);

                // Act
                await service.PopulateAsync(data);

                var poolObject = GameObject.Find("AsyncPrefab");
                Assert.IsNotNull(poolObject);
            }
        }

        [Test]
        public void GetAsync_ThrowsIfAddressableServiceIsNull()
        {
            var service = new PoolService();
            service.Initialize();

            var data = Substitute.For<IPooledObjectAsyncData>();

            LogAssert.Expect(LogType.Error, "Failed to load asset: AddressableService is null. Ensure it is properly registered in the service container.");

            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await service.GetAsync(data, null, Vector3.zero, Space.World);
            });
        }

        private void WhenAddressableLoadAssetAsyncGameObject(IAddressableService addressableService, IPooledObjectAsyncData data)
        {
            Func<NSubstitute.Core.CallInfo, Awaitable<GameObject>> value = async call =>
            {
                await Awaitable.NextFrameAsync(); // Simulate async loading delay
                return new GameObject("AsyncPrefab");
            };

            addressableService
                .LoadAssetAsync<GameObject>(data.AssetReference)
                .Returns(value);
        }
    }
}
