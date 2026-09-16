namespace SpawnThat.World.Maps.Sectors;

internal sealed class BiomeSectorBiomeMap : Map<Heightmap.Biome>
{
    private Heightmap.Biome[][] Biomes { get; }

    public BiomeSectorBiomeMap(global::World world)
    {
        GridWidth = world.m_biomeData.Size;
        Biomes = new Heightmap.Biome[GridWidth][];

        var pointSectors = world.m_biomeData.PointSectors;

#if FALSE
        Parallel.For(0, GridWidth, x =>
        {
            var row = Biomes[x] = new Heightmap.Biome[GridWidth];

            for (int y = 0; y < GridWidth; y++)
            {
                row[y] = pointSectors[x, y].Biome;
            }
        });
#endif

        for (int x = 0; x < GridWidth; ++x)
        {
            var row = Biomes[x] = new Heightmap.Biome[GridWidth];

            for (int y = 0; y < GridWidth; ++y)
            {
                row[y] = pointSectors[x, y].Biome;
            }
        }
    }

    public override Heightmap.Biome GetCell(int grid_x, int grid_y) =>
        Biomes[grid_x][grid_y];

    public override Heightmap.Biome[] GetRow(int grid_x) =>
        Biomes[grid_x];
}
