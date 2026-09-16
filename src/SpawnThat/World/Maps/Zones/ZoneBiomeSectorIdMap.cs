using SpawnThat.World.Maps.Sectors;
using SpawnThat.World.Zone;
using UnityEngine;

namespace SpawnThat.World.Maps.Zones;

/// <summary>
/// Biome sector id grid, based on how a zone would map it.
/// Because Valheim is a spaghetti nightmare when it comes to its biomes and zones.
/// </summary>
internal class ZoneBiomeSectorIdMap : Map<int>
{
    public int[][] SectorIds { get; }
    public int ZoneOffset { get; private set; }
    public int IndexStartCoordinate { get; private set; }

    public ZoneBiomeSectorIdMap(BiomeSectorIdMap sectorIdMap)
    {
        var worldRadius = AltBiomeWorldData.MapSpaceToWorldSpace(sectorIdMap.GridWidth);
        GridWidth = Mathf.FloorToInt(worldRadius / ZoneUtils.ZoneWidth) * 2;

        SectorIds = new int[GridWidth][];

        IndexStartCoordinate = -(GridWidth / 2) * ZoneUtils.ZoneWidth;

        for (int x = 0; x < GridWidth; x++)
        {
            var row = SectorIds[x] = new int[GridWidth];

            var zoneCoordX = IndexToWorldCoordinate(x);
            var sectorX = AltBiomeWorldData.WorldSpaceToMapSpace(zoneCoordX);

            var sectorRow = sectorIdMap.GetRow(sectorX);

            for (int y = 0; y < GridWidth; y++)
            {
                var zoneCoordY = IndexToWorldCoordinate(y);
                var sectorY = AltBiomeWorldData.WorldSpaceToMapSpace(zoneCoordY);

                row[y] = sectorRow[sectorY];
            }
        }
    }

    public override int GetCell(int grid_x, int grid_y) => SectorIds[grid_x][grid_y];

    public override int[] GetRow(int grid_x) => SectorIds[grid_x];

    private int IndexToWorldCoordinate(int index) => IndexStartCoordinate + index * ZoneUtils.ZoneWidth;
}
