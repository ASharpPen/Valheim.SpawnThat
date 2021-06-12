using System.Drawing;
using System.Drawing.Imaging;
//using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Valheim.SpawnThat.Utilities.Images;
using Valheim.SpawnThat.WorldMap;

namespace Valheim.SpawnThat.TestUtils
{
    public class BmpImagePrinter : IImagePrinter
    {
        public void PrintSquareMap(int[][] map, string imageFileName, bool rainbowOn, int valueRangeMin = 0, int valueRangeMax = 1)
        {
            using (var bmp = new Bitmap(map.Length, map.Length, PixelFormat.Format32bppRgb))
            {
                using (LockedBmp lockedBmp = new LockedBmp(bmp))
                {
                    unsafe
                    {
                        for(int x = 0; x < map.Length; ++x)
                        {
                            for(int y = 0; y < map.Length; ++y)
                            {
                                byte* currentLine = lockedBmp.FirstPixel + y * lockedBmp.Stride;
                                int col = x * lockedBmp.BytesPrPixel;

                                var id = map[x][y];
                                if (rainbowOn)
                                {
                                    var (r, g, b) = ColourMapper.Rainbow(id);

                                    currentLine[col] = (byte)(int)(b * 255);
                                    currentLine[col + 1] = (byte)(int)(g * 255);
                                    currentLine[col + 2] = (byte)(int)(r * 255);
                                }
                                else
                                {
                                    var (r, g, b) = ColourMapper.IntegerToColour3Byte(id);
                                    currentLine[col] = (byte)b;
                                    currentLine[col + 1] = (byte)g;
                                    currentLine[col + 2] = (byte)r;
                                }

                                if (map[x][y] == 0)
                                {
                                    currentLine[col] = 255;
                                    currentLine[col + 1] = 255;
                                    currentLine[col + 2] = 255;
                                }
                            }
                        }
                        /*
                        Parallel.For(0, map.Length, y =>
                        {
                            byte* currentLine = lockedBmp.FirstPixel + y * lockedBmp.Stride;

                            for (int x = 0; x < map[y].Length; ++x)
                            {
                                int col = x * lockedBmp.BytesPrPixel;

                                var id = map[y][x];
                                if (rainbowOn)
                                {
                                    var (r, g, b) = ColourMapper.Rainbow(id);

                                    currentLine[col] = (byte)(int)(b * 255);
                                    currentLine[col + 1] = (byte)(int)(g * 255);
                                    currentLine[col + 2] = (byte)(int)(r * 255);
                                }
                                else
                                {
                                    var (r, g, b) = ColourMapper.IntegerToColor255(id);
                                    currentLine[col] = (byte)b;
                                    currentLine[col + 1] = (byte)g;
                                    currentLine[col + 2] = (byte)r;
                                }

                                if (map[y][x] == 0)
                                {
                                    currentLine[col] = 255;
                                    currentLine[col + 1] = 255;
                                    currentLine[col + 2] = 255;
                                }
                            }
                        });
                        */
                    }
                }

                //var filePath = Path.Combine(Paths.PluginPath, $"{imageFileName}.png");
                var filePath = Path.Combine("./", $"{imageFileName}.png");
                bmp.Save(filePath);
            }
        }

        public void PrintBiomes(int[][] map, string imageFileName)
        {
            using (var bmp = new Bitmap(map.Length, map.Length, PixelFormat.Format32bppRgb))
            {
                using (LockedBmp lockedBmp = new LockedBmp(bmp))
                {
                    unsafe
                    {
                        Parallel.For(0, map.Length, y =>
                        {
                            byte* currentLine = lockedBmp.FirstPixel + y * lockedBmp.Stride;

                            for (int x = 0; x < map[y].Length; ++x)
                            {
                                int col = x * lockedBmp.BytesPrPixel;

                                var biome = map[x][y];

                                var (r, g, b) = ColourMapper.ToBiomeColour255(biome);

                                currentLine[col] = (byte)b;
                                currentLine[col + 1] = (byte)g;
                                currentLine[col + 2] = (byte)r;
                            }
                        });
                    }
                }

                //var filePath = Path.Combine(Paths.PluginPath, $"{imageFileName}.png");
                var filePath = Path.Combine("./", $"{imageFileName}.png");
                bmp.Save(filePath);
            }
        }
    }
}
