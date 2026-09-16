using UnityEngine;

namespace SpawnThat.World.Maps.Images.Colors;

internal static class BiomeColorMapper
{
    public static BiomeColorMapper<int> FromIntMap() =>
        new BiomeColorMapper<int>(new IdentitySelect());

    public static BiomeColorMapper<Heightmap.Biome> FromBiomeMap() =>
        new BiomeColorMapper<Heightmap.Biome>(new BiomeSelector());
}

internal readonly struct BiomeColorMapper<T>(
    ISelect<T, int> selector)
    : IColorMapper<T>
{
    public Color Map(T cell)
    {
        var biome = selector.Select(cell);

        return (Heightmap.Biome)biome switch
        {
            Heightmap.Biome.None => new Color(0, 0, 0),
            Heightmap.Biome.Meadows => new Color(0, 1, 0),
            Heightmap.Biome.Swamp => new Color(0.5f, 0, 0),
            Heightmap.Biome.Mountain => new Color(1, 1, 1),
            Heightmap.Biome.BlackForest => new Color(0, 0.5f, 0),
            Heightmap.Biome.Plains => new Color(1, 1, 0),
            Heightmap.Biome.AshLands => new Color(1, 0, 0),
            Heightmap.Biome.DeepNorth => new Color(0, 1, 1),
            Heightmap.Biome.Ocean => new Color(0, 0, 1),
            Heightmap.Biome.Mistlands => new Color(0.5f, 0.5f, 0.5f),
            Heightmap.Biome.Land => new Color(200/255f, 100/255f, 20/255f),
            Heightmap.Biome.All => new Color(0, 0, 0),
            // If this is an expanded biome. Just try to find some color for it.
            _ => new RainbowColorMapper().Map(biome),
        };
    }
}