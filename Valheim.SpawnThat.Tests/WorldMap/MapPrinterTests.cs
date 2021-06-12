using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.TestUtils;
using Valheim.SpawnThat.Utilities.Images;

namespace Valheim.SpawnThat.WorldMap
{
    [TestClass]
    public class MapPrinterTests
    {
        [TestMethod]
        public void CanPrintMap()
        {
            // Arrange
            var mapRadius = 10000;
            var areaProvider = new AreaFromPngProvider(mapRadius);
            var map = new AreaMap(areaProvider, mapRadius);
            areaProvider.Map = map;

            MapPrinter.Printer = new BmpImagePrinter();

            // Act
            map.Init(mapRadius);
            map.FirstScan();
            map.Build();

            MapPrinter.PrintAreaMap(map);
        }

        [TestMethod]
        public void CanPrintMapWithMergedLabels()
        {
            // Arrange
            var mapRadius = 10000;
            var areaProvider = new AreaFromPngProvider(mapRadius);
            var map = new AreaMap(areaProvider, mapRadius);
            areaProvider.Map = map;

            MapPrinter.Printer = new BmpImagePrinter();

            // Act
            map.Complete();

            MapPrinter.PrintAreaMap(map);
        }

        [TestMethod]
        public void TestAreaFromPngProvider()
        {
            // Arrange
            var mapRadius = 10000;
            var provider = new AreaFromPngProvider(mapRadius);
            var map = new AreaMap(provider, mapRadius);

            var coordinateX = map.IndexToCoordinate(0);
            var coordinateY = map.IndexToCoordinate(0);

            // Act
            var resultPixel = provider.GetArea(coordinateX, coordinateY);

            // Assert
            Assert.AreEqual((byte)10, resultPixel);
        }

        public class RandomAreaProvider : IAreaProvider
        {
            private Random Random { get; } = new Random();

            public int GetArea(int x, int y)
            {
                return Random.Next(0, 9);
            }
        }

        public class AreaFromPngProvider : IAreaProvider
        {
            public LockedBmp Bmp = new LockedBmp(new Bitmap(@"../../Resources/biome_map.png"), System.Drawing.Imaging.ImageLockMode.ReadOnly);

            public AreaMap Map { get; set; }

            private int _mapSize;
            private int _radius;

            public AreaFromPngProvider(int radius)
            {
                _mapSize = radius*2;
                _radius = radius;
            }

            public int GetArea(int x, int y)
            {
                var indexX = Map.CoordinateToIndex(x);
                var indexY = Map.CoordinateToIndex(y);

                unsafe
                {
                    byte* currentLine = Bmp.FirstPixel + indexY * Bmp.Stride;

                    int col = indexX * Bmp.BytesPrPixel;

                    var b = currentLine[col];
                    var g = currentLine[col + 1];
                    var r = currentLine[col + 2];

                    return BiomeColourToBiomeId(r, g, b);
                }
            }

            private int BiomeColourToBiomeId(int r, int g, int b)
            {
                if (b == 255 && g == 0 && r == 0)
                {
                    return (int)Heightmap.Biome.Ocean;
                }
                if (b == 255 && g == 255 && r == 0)
                {
                    return (int)Heightmap.Biome.DeepNorth;
                }
                if (b == 0 && g == 128 && r == 0)
                {
                    return (int)Heightmap.Biome.BlackForest;
                }
                if (b == 255 && g == 255 && r == 255)
                {
                    return (int)Heightmap.Biome.Mountain;
                }
                if(b == 128 && g == 128 && r == 128)
                {
                    return (int)Heightmap.Biome.Mistlands;
                }
                if(b == 4 && g == 235 && r == 255)
                {
                    return (int)Heightmap.Biome.Plains;
                }
                if(b == 41 && g == 41 && r == 166)
                {
                    return (int)Heightmap.Biome.Swamp;
                }
                if(b == 0 && g == 255 && r == 0)
                {
                    return (int)Heightmap.Biome.Meadows;
                }
                if (b == 0 && g == 0 && r == 255)
                {
                    return (int)Heightmap.Biome.AshLands;
                }
                return (int)Heightmap.Biome.None;
            }
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
}
