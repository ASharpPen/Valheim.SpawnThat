using System.Collections.Generic;
using static Version;

namespace SpawnThat.World.Maps.Zones;

internal class ZoneAreaMapBuilder
{
    private class Label
    {
        public int Id { get; set; }
        public int Area { get; set; }
    }

    private AreaIdMap AreaIdMap { get; }
    private AreaBiomeMap AreaBiomeMap { get; }
    private Label[][] LabelGrid { get; }

    private Dictionary<int, int> MergeTable { get; } = new();

    private int Size => AreaIdMap.MapWidth;

    public ZoneAreaMapBuilder(int mapRadius)
    {
        AreaIdMap = new AreaIdMap(mapRadius);
        AreaBiomeMap = new AreaBiomeMap(mapRadius);

        LabelGrid = new Label[Size][];

        for (int x = 0; x < Size; ++x)
        {
            LabelGrid[x] = new Label[Size];
        }
    }

    public (AreaIdMap, AreaBiomeMap) CompileMap()
    {
        ScanAreas();
        MergeLabels();
        Build();

        return (AreaIdMap, AreaBiomeMap);
    }

    private void ScanAreas()
    {
        int rollingCount = 0;

        for (int x = 0; x < AreaIdMap.AreaIds.Length; ++x)
        {
            Label[] lastLabelColumn = x == 0
                ? null
                : LabelGrid[x - 1];

            var lastLabel = x == 0
                ? null
                : lastLabelColumn[0];

            int[] lastAreaColumn = x == 0
                ? null
                : AreaBiomeMap.Biomes[x - 1];

            int lastArea = x == 0
                ? -1
                : lastAreaColumn[0];

            for (int y = 0; y < AreaBiomeMap.Biomes.Length; ++y)
            {
                var area = GetBiome(x, y);
                AreaBiomeMap.Biomes[x][y] = area;

                // Check up
                if (lastArea == area)
                {
                }
                // Check left
                else if (x > 0 && lastAreaColumn[y] == area)
                {
                    lastLabel = lastLabelColumn[y];
                }
                // New id
                else
                {
                    lastLabel = new Label { Id = rollingCount++, Area = area };
                    MergeTable[lastLabel.Id] = lastLabel.Id;
                }
                LabelGrid[x][y] = lastLabel;

                lastArea = area;
            }
        }
    }

    private int GetBiome(int x, int y) =>
        (int)WorldGenerator.instance.GetBiome(
            AreaIdMap.IndexToCoordinate(x), 
            AreaIdMap.IndexToCoordinate(y));

    private void MergeLabels()
    {
        for (int x = 0; x < LabelGrid.Length; ++x)
        {
            for (int y = 0; y < LabelGrid.Length; ++y)
            {
                var label = LabelGrid[x][y];

                // Look up
                if (y > 0)
                {
                    Label labelUp = LabelGrid[x][y - 1];

                    if (label.Area == labelUp.Area)
                    {
                    }

                    if (MergeLabels(labelUp, label, out var result))
                    {
                        LabelGrid[x][y] = result;
                        continue;
                    }
                }

                // Look left
                if (x > 0)
                {
                    Label labelLeft = LabelGrid[x - 1][y];

                    if (MergeLabels(labelLeft, label, out var result))
                    {
                        LabelGrid[x][y] = result;
                        continue;
                    }
                }

                // Look down
                if (y + 1 < LabelGrid.Length)
                {
                    Label labelDown = LabelGrid[x][y + 1];

                    if (MergeLabels(labelDown, label, out var result))
                    {
                        LabelGrid[x][y] = result;
                        continue;
                    }
                }

                // Look right
                if (x + 1 < LabelGrid.Length)
                {
                    var labelRight = LabelGrid[x + 1][y];

                    if (MergeLabels(labelRight, label, out var result))
                    {
                        LabelGrid[x][y] = result;
                        continue;
                    }
                }

                bool MergeLabels(Label label1, Label label2, out Label result)
                {
                    result = null;

                    if (label1.Area == label2.Area &&
                        label1.Id != label2.Id)
                    {
                        // Merge labels.
                        if (label1.Id > label2.Id)
                        {
                            if (MergeTable[label1.Id] > label2.Id)
                            {
                                MergeTable[label1.Id] = label2.Id;
                            }
                            result = label2;
                            label1.Id = label2.Id;
                        }
                        else
                        {
                            if (MergeTable[label2.Id] > label1.Id)
                            {
                                MergeTable[label2.Id] = label1.Id;
                            }

                            result = label1;
                            label2.Id = label1.Id;
                        }

                        return true;
                    }

                    return false;
                }
            }
        }
    }

    private void Build()
    {
        for (int x = 0; x < LabelGrid.Length; ++x)
        {
            for (int y = 0; y < LabelGrid.Length; ++y)
            {
                var areaId = LabelGrid[x][y].Id;
                var id = MergeTable[areaId];

                while (MergeTable[id] < id)
                {
                    id = MergeTable[id];
                    MergeTable[areaId] = id;
                }

                AreaIdMap.AreaIds[x][y] = id;
            }
        }
    }
}
