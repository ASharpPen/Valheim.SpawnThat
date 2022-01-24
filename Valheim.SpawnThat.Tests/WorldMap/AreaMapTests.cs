#if FALSE
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.WorldMap
{
    [TestClass]
    public class AreaMapTests
    {
        [TestMethod]
        public void PrintGridIds()
        {
            var map = AreaMapBuilder
                .BiomeMap(128)
                .UseAreaProvider(new AreaProviderMock())
                .CompileMap();

            for(int y = 0; y < map.AreaIds.Length; ++y)
            {
                for(int x = 0; x < map.AreaIds.Length; ++x)
                {
                    System.Console.Write(map.AreaIds[y][x] + ",");
                }
                System.Console.WriteLine();
            }
        }

        [DataTestMethod]
        [DataRow(0, 128, -128)]
        [DataRow(2, 128, 0)]
        [DataRow(4, 128, 128)]
        public void IndexToCoordinateShouldMapXCorrectly(int index, int mapRadius, int expectedCoordinate)
        {
            // Arrange
            var map = AreaMapBuilder
                .BiomeMap(mapRadius)
                .UseAreaProvider(new AreaProviderStub())
                .CompileMap();

            // Act
            int result = map.IndexToCoordinate(index);

            // Assert
            Assert.AreEqual(expectedCoordinate, result);
        }

        [TestMethod]
        public void IndexToCoordinateShouldMapXCorrectlyWhenMapRadiusNotFittingZoneSize()
        {
            // Arrange
            var map = AreaMapBuilder
                .BiomeMap(100)
                .UseAreaProvider(new AreaProviderMock())
                .CompileMap();

            // Act
            int result = map.IndexToCoordinate(0);

            // Assert
            Assert.AreEqual(map.IndexStartCoordinate, result);
        }

        [TestMethod]
        public void IndexToCoordinateCanMapXToMid()
        {
            // Arrange
            var mapRadius = 10000;
            var map = AreaMapBuilder
                .BiomeMap(mapRadius)
                .UseAreaProvider(new AreaProviderStub())
                .CompileMap();

            var index = map.ZoneOffset;

            // Act
            int result = map.IndexToCoordinate(index);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CoordinateToIndex()
        {
            // Arrange
            var mapRadius = 10000;
            var map = AreaMapBuilder
                .BiomeMap(mapRadius)
                .UseAreaProvider(new AreaProviderStub())
                .CompileMap();

            var index = map.ZoneOffset;

            // Act
            int coordinate = map.IndexToCoordinate(index);
            int result = map.CoordinateToIndex(coordinate);

            // Assert
            Assert.AreEqual(index, result);
        }

        [DataTestMethod]
        [DataRow(384, 6)]
        [DataRow(-76, -1)]
        public void IndexToCoordinate(int coordinate, int realZoneId)
        {
            // Arrange
            var mapRadius = 10000;
            var map = AreaMapBuilder
                .BiomeMap(mapRadius)
                .UseAreaProvider(new AreaProviderStub())
                .CompileMap();

            int index = map.CoordinateToIndex(coordinate);

            // Act
            int resolved = map.IndexToCoordinate(index);
            int zoneId = (resolved + 32) / 64;

            // Assert
            var expectedZoneId = (coordinate + 32) / 64;
            Assert.AreEqual(realZoneId, zoneId);
        }

        [TestMethod]
        public void CoordinateToIndexShouldMapTopLeftToTopLeft()
        {
            // Arrange
            var mapRadius = 10000;
            var map = AreaMapBuilder
                .BiomeMap(mapRadius)
                .UseAreaProvider(new AreaProviderStub())
                .CompileMap();

            var index = 0;

            // Act
            int coordinate = map.IndexToCoordinate(index);
            int result = map.CoordinateToIndex(coordinate);

            // Assert
            Assert.AreEqual(index, result);
        }
    }

    public class AreaProviderStub : IAreaProvider
    {
        public int GetArea(int x, int y) => 1;
    }

    public class AreaProviderMock : IAreaProvider
    {
        public int radius = 128;

        public int[][] Grid = new[]
        {
            new int[] { 0, 0, 2, 0 },
            new int[] { 1, 1, 2, 0 },
            new int[] { 0, 1, 2, 2 },
            new int[] { 1, 1, 4, 4 },
        };

        public int GetArea(int x, int y)
        {
            x = (x + radius) / 64;
            y = (y + radius) / 64;
            return Grid[x][y];
        }
    }
}
#endif