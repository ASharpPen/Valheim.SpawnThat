using UnityEngine;

namespace SpawnThat.World.Maps.Images.Colors;

internal static class BiomeGrayscaleColorMapper
{
    public static BiomeGrayscaleColorMapper<int> FromIntMap()
    {
        return new BiomeGrayscaleColorMapper<int>(new IdentitySelect());
    }
}

internal readonly struct BiomeGrayscaleColorMapper<T> : IColorMapper<T>
{
    public ISelect<T, int> CellSelector { get; }

    public BiomeGrayscaleColorMapper(ISelect<T, int> cellSelector)
    {
        CellSelector = cellSelector;
    }

    public Color Map(T cell)
    {
        var biome = CellSelector.Select(cell);

        return (Heightmap.Biome)biome switch
        {
            Heightmap.Biome.None => new Color32(255, 255, 255, 255),
            Heightmap.Biome.Meadows => new Color32(120, 120, 120, 255),
            Heightmap.Biome.Swamp => new Color32(140, 140, 140, 255),
            Heightmap.Biome.Mountain => new Color32(160, 160, 160, 255),
            Heightmap.Biome.BlackForest => new Color32(180, 180, 180, 255),
            Heightmap.Biome.Plains => new Color32(200, 200, 200, 255),
            Heightmap.Biome.AshLands => new Color32(220, 220, 220, 255),
            Heightmap.Biome.DeepNorth => new Color32(240, 240, 240, 255),
            Heightmap.Biome.Mistlands => new Color32(130, 130, 130, 255),
            Heightmap.Biome.Ocean => new Color32(255, 255, 255, 255),
            _ => Color.grey,
        };
    }
}