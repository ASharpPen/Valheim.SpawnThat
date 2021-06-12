using BepInEx;
using System.IO;
using UnityEngine;
using Valheim.SpawnThat.Utilities.Images;

namespace Valheim.SpawnThat.WorldMap
{

    public class ImagePrinter : IImagePrinter
    {
        public void PrintSquareMap(int[][] map, string imageFileName, bool rainbowOn, int valueRangeMin = 0, int valueRangeMax = 1)
        {
            double scale = 1.0 / (valueRangeMax - valueRangeMin);

            Texture2D image = new Texture2D(map.Length, map.Length);

            for (int x = 0; x < map.Length; ++x)
            {
                for (int y = 0; y < map.Length; ++y)
                {
                    var value = map[x][y];

                    Color color;

                    if (rainbowOn)
                    {
                        var (r, g, b) = ColourMapper.Rainbow(value);
                        color = new Color(r, g, b);
                    }
                    else
                    {
                        var (r, g, b) = ColourMapper.IntegerToColor(value);
                        color = new Color(r, g, b);
                    }

                    image.SetPixel(x, y, color);
                }
            }

            var filePath = Path.Combine(Paths.BepInExRootPath, $"{imageFileName}.png");

            image.Apply();

            File.WriteAllBytes(filePath, image.EncodeToPNG());
        }
        public void PrintBiomes(int[][] map, string imageFileName)
        {
            Texture2D image = new Texture2D(map.Length, map.Length);

            for (int x = 0; x < map.Length; ++x)
            {
                for (int y = 0; y < map.Length; ++y)
                {
                    var (r, g, b) = ColourMapper.ToBiomeColour((Heightmap.Biome)map[x][y]);

                    image.SetPixel(x, y, new Color(r, g, b));
                }
            }

            var filePath = Path.Combine(Paths.BepInExRootPath, $"{imageFileName}.png");
            //var filePath = Path.Combine("./", $"{imageFileName}.png");

            image.Apply();

            File.WriteAllBytes(filePath, image.EncodeToPNG());
        }
    }
}
