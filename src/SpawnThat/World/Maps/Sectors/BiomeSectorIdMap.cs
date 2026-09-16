using GameWorld = global::World;

namespace SpawnThat.World.Maps.Sectors;

internal class BiomeSectorIdMap : Map<int>
{
    private int[][] SectorIds { get; }

    public BiomeSectorIdMap(GameWorld world)
    {
        GridWidth = world.m_biomeData.Size;
        SectorIds = new int[GridWidth][];

        var pointSectors = world.m_biomeData.PointSectors;

#if FALSE
        Parallel.For(0, GridWidth, x =>
        {
            var row = SectorIds[x] = new int[GridWidth];

            for (int y = 0; y < GridWidth; y++)
            {
                row[y] = SectorManager.IdBySector[pointSectors[x, y]];
            }
        });
#endif

        for (int x = 0; x < GridWidth; ++x)
        {
            var row = SectorIds[x] = new int[GridWidth];

            for (int y = 0; y < GridWidth; ++y)
            {
                row[y] = SectorManager.IdBySector[pointSectors[x, y]];
            }
        }
    }

    public override int GetCell(int grid_x, int grid_y) =>
        SectorIds[grid_x][grid_y];

    public override int[] GetRow(int grid_x) =>
        SectorIds[grid_x];
}
