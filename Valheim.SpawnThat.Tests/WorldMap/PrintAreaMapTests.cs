using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Maps;
using Valheim.SpawnThat.TestUtils;

namespace Valheim.SpawnThat.WorldMap
{
    [TestClass]
    public class PrintAreaMapTests
    {
        [TestMethod]
        public void CanPrintMapWithMergedLabels()
        {
            // Arrange
            var mapRadius = 10000;
            var areaProvider = new AreaFromPngProvider(mapRadius);

            var map = AreaMapBuilder
                .BiomeMap(mapRadius)
                .UseAreaProvider(areaProvider)
                .CompileMap();

            var printer = new BmpImagePrinter();

            // Act
            printer.PrintSquareMap(map.AreaIds, "ids_merged", false);
        }

        public class AreaFromPngProvider : IAreaProvider
        {
            public LockedBmp Bmp = new LockedBmp(new Bitmap(@"../../Resources/biome_map.png"), System.Drawing.Imaging.ImageLockMode.ReadOnly);

            public AreaMap Map { get; set; }

            public AreaFromPngProvider(int radius)
            {
                Map = new AreaMap(radius);
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
                if (b == 128 && g == 128 && r == 128)
                {
                    return (int)Heightmap.Biome.Mistlands;
                }
                if (b == 4 && g == 235 && r == 255)
                {
                    return (int)Heightmap.Biome.Plains;
                }
                if (b == 41 && g == 41 && r == 166)
                {
                    return (int)Heightmap.Biome.Swamp;
                }
                if (b == 0 && g == 255 && r == 0)
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
    }
}
