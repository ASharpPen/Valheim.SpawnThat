using UnityEngine;
using Valheim.SpawnThat.Debugging;

namespace Valheim.SpawnThat.World.Maps.Area;

internal class ImageBuilder
{
    private AreaMap areaMap;
    private Texture2D image;

    private ImageBuilder(AreaMap areaMap)
    {
        this.areaMap = areaMap;
        image = new Texture2D(this.areaMap.MapWidth, this.areaMap.MapWidth, TextureFormat.RGBA32, true);
    }

    public static ImageBuilder Init(AreaMap areaMap)
    {
        return new ImageBuilder(areaMap);
    }

    public static ImageBuilder SetBiomes(AreaMap areaMap)
    {
        var builder = new ImageBuilder(areaMap);

        for (int x = 0; x < areaMap.MapWidth; ++x)
        {
            for (int y = 0; y < areaMap.MapWidth; ++y)
            {
                var (r, g, b) = ColourMapper.ToBiomeColour((Heightmap.Biome)areaMap.Biomes[x][y]);

                builder.image.SetPixel(x, y, new Color(r, g, b));
            }
        }

        return builder;
    }

    public static ImageBuilder SetGrayscaleBiomes(AreaMap areaMap)
    {
        var builder = new ImageBuilder(areaMap);

        for (int x = 0; x < areaMap.MapWidth; ++x)
        {
            for (int y = 0; y < areaMap.MapWidth; ++y)
            {
                var value = areaMap.Biomes[x][y];

                var (r, g, b) = ColourMapper.ToBiomeColourGrayscale(value);

                builder.image.SetPixel(x, y, new Color32((byte)r, (byte)g, (byte)b, 255));
            }
        }

        return builder;
    }

    public static ImageBuilder SetIds(AreaMap areaMap)
    {
        var builder = new ImageBuilder(areaMap);

        for (int x = 0; x < areaMap.MapWidth; ++x)
        {
            for (int y = 0; y < areaMap.MapWidth; ++y)
            {
                var value = areaMap.AreaIds[x][y];

                var (r, g, b) = ColourMapper.IntegerToColour3Byte(value);
                Color32 color = new Color32((byte)r, (byte)g, (byte)b, 255);

                builder.image.SetPixel(x, y, color);
            }
        }

        return builder;
    }

    public static ImageBuilder SetRainbowIds(AreaMap areaMap)
    {
        var builder = new ImageBuilder(areaMap);

        for (int x = 0; x < areaMap.MapWidth; ++x)
        {
            for (int y = 0; y < areaMap.MapWidth; ++y)
            {
                var value = areaMap.AreaIds[x][y];

                var (r, g, b) = ColourMapper.Rainbow255(value);
                Color32 color = new Color32((byte)r, (byte)g, (byte)b, 255);

                builder.image.SetPixel(x, y, color);
            }
        }

        return builder;
    }

    public ImageBuilder AddHeatZones(int[][] heatZones, bool ignoreZero = true)
    {
        for (int x = 0; x < heatZones.Length; ++x)
        {
            for (int y = 0; y < heatZones.Length; ++y)
            {
                var value = heatZones[x][y];

                if (!ignoreZero || value != 0)
                {
                    image.SetPixel(x, y, new Color32(255, (byte)(255 - value), (byte)(255 - value), 255));
                }
            }
        }

        return this;
    }

    public void Print(string filename, string description = null)
    {
        image.Apply();

        DebugFileWriter.WriteFile(image.EncodeToPNG(), filename + ".png", description ?? "map");
    }
}
