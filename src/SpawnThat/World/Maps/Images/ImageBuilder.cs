using System;
using SpawnThat.Debugging;
using SpawnThat.World.Maps.Images.Colors;
using UnityEngine;

namespace SpawnThat.World.Maps.Images;

internal class ImageBuilder
{
    private int Width { get; }

    private Texture2D Image { get; }

    public ImageBuilder(int width)
    {
        Width = width;
        Image = new Texture2D(width, width, TextureFormat.RGBA32, true);
    }

    public static ImageBuilder Init(int width) => new(width);

    public ImageBuilder Apply<T, K>(
        Map<T> map,
        Predicate<T> condition,
        in K colorMapper)
        where K : struct, IColorMapper<T>
    {
        if (map.GridWidth != Width)
        {
            return this;
        }

        for (int x = 0; x < Width; ++x)
        {
            var row = map.GetRow(x);

            for (int y = 0; y < Width; ++y)
            {
                var value = row[y];

                if (condition(value))
                {
                    var color = colorMapper.Map(value);
                    Image.SetPixel(x, y, color);
                }
            }
        }

        return this;
    }

    public ImageBuilder Apply<T, K>(
        Map<T> map,
        in K colorMapper)
        where K : struct, IColorMapper<T>
    {
        if (map.GridWidth != Width)
        {
            return this;
        }

        for (int x = 0; x < Width; ++x)
        {
            var row = map.GetRow(x);

            for (int y = 0; y < Width; ++y)
            {
                var value = row[y];
                var color = colorMapper.Map(value);
                Image.SetPixel(x, y, color);
            }
        }

        return this;
    }

    public ImageBuilder Apply<T>(int gridx, int gridy, T value, IColorMapper<T> colorMapper)
    {
        Image.SetPixel(gridx, gridy, colorMapper.Map(value));

        return this;
    }

    public void Print(string filename, string description = null)
    {
        Image.Apply();

        DebugFileWriter.WriteFile(Image.EncodeToPNG(), filename + ".png", description ?? "map");
    }
}