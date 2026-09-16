namespace SpawnThat.World.Maps.Images;

internal interface ISelect<T, K>
{
    K Select(T input);
}

internal readonly struct BiomeSectorBiomeSelector : ISelect<BiomeSector, int>
{
    public int Select(BiomeSector input) => (int)input.Biome;
}

internal readonly struct BiomeSelector : ISelect<Heightmap.Biome, int>
{
    public int Select(Heightmap.Biome input) => (int)input;
}

internal readonly struct IdentitySelect : ISelect<int, int>
{
    public int Select(int input) => input;
}