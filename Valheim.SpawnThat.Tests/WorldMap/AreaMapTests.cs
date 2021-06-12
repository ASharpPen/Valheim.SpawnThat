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
            var map = new AreaMap(new AreaProviderMock(), 128);
            map.Complete();

            for(int y = 0; y < map.GridIds.Length; ++y)
            {
                for(int x = 0; x < map.GridIds.Length; ++x)
                {
                    System.Console.Write(map.GridIds[y][x] + ",");
                }
                System.Console.WriteLine();
            }
        }

        [DataTestMethod]
        [DataRow(0, 128, -128+AreaMap.ZoneSizeOffset)]
        [DataRow(2, 128, AreaMap.ZoneSizeOffset)]
        [DataRow(4, 128, 128+AreaMap.ZoneSizeOffset)]
        public void IndexToCoordinateShouldMapXCorrectly(int index, int mapRadius, int expectedCoordinate)
        {
            // Arrange
            var map = new AreaMap(new AreaProviderStub(), mapRadius);
            map.Complete();

            // Act
            int result = map.IndexToCoordinate(index);

            // Assert
            Assert.AreEqual(expectedCoordinate, result);
        }

        [TestMethod]
        public void IndexToCoordinateShouldMapXCorrectlyWhenMapRadiusNotFittingZoneSize()
        {
            // Arrange
            var mapRadius = 100;
            var map = new AreaMap(new AreaProviderMock(), mapRadius);
            map.Complete();

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
            var map = new AreaMap(new AreaProviderStub(), mapRadius);
            map.Complete();

            var index = map.ZoneOffset;

            // Act
            int result = map.IndexToCoordinate(index);

            // Assert
            Assert.AreEqual(AreaMap.ZoneSizeOffset, result);
        }

        [TestMethod]
        public void CoordinateToIndex()
        {
            // Arrange
            var mapRadius = 10000;
            var map = new AreaMap(new AreaProviderStub(), mapRadius);
            map.Complete();

            var index = map.ZoneOffset;

            // Act
            int coordinate = map.IndexToCoordinate(index);
            int result = map.CoordinateToIndex(coordinate);

            // Assert
            Assert.AreEqual(index, result);
        }

        [TestMethod]
        public void CoordinateToIndexShouldMapTopLeftToTopLeft()
        {
            // Arrange
            var mapRadius = 10000;
            var map = new AreaMap(new AreaProviderStub(), mapRadius);
            map.Complete();

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
