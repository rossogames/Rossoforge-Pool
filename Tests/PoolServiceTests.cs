using NSubstitute;
using NUnit.Framework;
using Rossoforge.Core.Pool;
using Rossoforge.Pool.Components;
using Rossoforge.Pool.Service;
using UnityEngine;

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
        public void Get_CreatesPoolIfNotExistAndReturnsPooledObject()
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
    }
}
