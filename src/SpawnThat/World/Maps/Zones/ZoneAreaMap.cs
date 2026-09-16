using System;
using SpawnThat.World.Zone;

namespace SpawnThat.World.Maps.Zones;

internal class AreaIdMap : Map<int>
{
    public int[][] AreaIds { get; private set; }
    public int ZoneOffset { get; private set; }
    public int MapWidth { get; private set; }
    public int IndexStartCoordinate { get; private set; }
    public int ZoneSize { get; private set; }

    public AreaIdMap(int mapRadius)
    {
        ZoneSize = ZoneUtils.ZoneWidth;

        ZoneOffset = (int)Math.Ceiling(mapRadius / (double)ZoneSize);
        IndexStartCoordinate = -(ZoneOffset * ZoneSize);

        MapWidth = ZoneOffset * 2;
        GridWidth = MapWidth;

        AreaIds = new int[MapWidth][];

        for (int x = 0; x < MapWidth; ++x)
        {
            AreaIds[x] = new int[MapWidth];
        }
    }

    public override int GetCell(int grid_x, int grid_y) =>
        AreaIds[grid_x][grid_y];

    public int IndexToCoordinate(int index) => IndexStartCoordinate + index * ZoneUtils.ZoneWidth;

    public int CoordinateToIndex(int coordinate) => ZoneUtils.Zonify(coordinate);

    public override int[] GetRow(int grid_x) =>
        AreaIds[grid_x];
}

internal class AreaBiomeMap : Map<int>
{
    public int[][] Biomes { get; private set; }
    public int ZoneOffset { get; private set; }
    public int MapWidth { get; private set; }
    public int IndexStartCoordinate { get; private set; }
    public int ZoneSize { get; private set; }

    public AreaBiomeMap(int mapRadius)
    {
        ZoneSize = ZoneUtils.ZoneWidth;
        
        ZoneOffset = (int)Math.Ceiling(mapRadius / (double)ZoneSize);
        IndexStartCoordinate = -(ZoneOffset * ZoneSize);

        MapWidth = ZoneOffset * 2;
        GridWidth = MapWidth;

        Biomes = new int[MapWidth][];

        for (int x = 0; x < MapWidth; ++x)
        {
            Biomes[x] = new int[MapWidth];
        }
    }

    public override int GetCell(int grid_x, int grid_y) =>
        Biomes[grid_x][grid_y];

    public override int[] GetRow(int grid_x) =>
        Biomes[grid_x];

    public int IndexToCoordinate(int index) => IndexStartCoordinate + index * ZoneUtils.ZoneWidth;

    public int CoordinateToIndex(int coordinate) => ZoneUtils.Zonify(coordinate);
}