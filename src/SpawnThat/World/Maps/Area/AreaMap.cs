using System;

namespace SpawnThat.World.Maps.Area;

internal class AreaMap
{
    public const int ZoneSize = 64;
    public const int ZoneSizeOffset = 32;

    public int[][] Biomes { get; private set; }

    public int[][] AreaIds { get; private set; }

    public int ZoneOffset { get; private set; }

    public int MapWidth { get; private set; }

    public int IndexStartCoordinate { get; private set; }

    public AreaMap(int mapRadius)
    {
        ZoneOffset = (int)Math.Ceiling(mapRadius / (double)ZoneSize);

        IndexStartCoordinate = -(ZoneOffset * ZoneSize);

        MapWidth = ZoneOffset * 2;

        Biomes = new int[MapWidth][];
        AreaIds = new int[MapWidth][];

        for (int x = 0; x < MapWidth; ++x)
        {
            Biomes[x] = new int[MapWidth];
            AreaIds[x] = new int[MapWidth];
        }
    }

    public int IndexToCoordinate(int index) => IndexStartCoordinate + index * ZoneSize;

    public int CoordinateToIndex(int coordinate) => coordinate / ZoneSize + ZoneOffset;
}
