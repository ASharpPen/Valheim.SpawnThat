using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.WorldMap
{
    public class AreaMap
    {
        public const int ZoneSize = 64;
        public const int ZoneSizeOffset = 32;

        public int[][] Biomes { get; private set; }

        public int[][] AreaIds { get; private set; }

        public int ZoneOffset { get; private set; }

        private IAreaProvider _areaProvider;

        public int MapWidth { get; private set; }

        public int IndexStartCoordinate { get; private set; }

        private int _mapRadius;

        public AreaMap(IAreaProvider areaProvider, int mapRadius)
        {
            _areaProvider = areaProvider;
            _mapRadius = mapRadius;
        }

        public void Complete()
        {
            Init(_mapRadius);
            FirstScan();
            MergeLabels();
            Build();
        }

        public void Init(int mapRadius)
        {
            ZoneOffset = (int)Math.Ceiling(mapRadius / (double)ZoneSize);

            IndexStartCoordinate = -(ZoneOffset * ZoneSize) + ZoneSizeOffset;

            MapWidth = ZoneOffset * 2;

            Biomes = new int[MapWidth][];
            AreaIds = new int[MapWidth][];
            LabelGrid = new Label[MapWidth][];

            for (int x = 0; x < MapWidth; ++x)
            {
                Biomes[x] = new int[MapWidth];
                AreaIds[x] = new int[MapWidth];
            }
        }

        public class Label
        {
            public int Id { get; set; }
            public int Area { get; set; }
        }

        public Label[][] LabelGrid;

        private Dictionary<int, int> MergeTable = new Dictionary<int, int>();

        public void FirstScan()
        {
            int rollingCount = 0;

            for (int x = 0; x < Biomes.Length; ++x)
            {
                LabelGrid[x] = new Label[MapWidth];

                Label[] lastLabelColumn = x == 0
                    ? null
                    : LabelGrid[x - 1];

                var lastLabel = x == 0
                    ? null
                    : lastLabelColumn[0];

                int[] lastAreaColumn = x == 0
                    ? null
                    : Biomes[x - 1];

                int lastArea = x == 0
                    ? -1
                    : lastAreaColumn[0];

                for (int y = 0; y < Biomes.Length; ++y)
                {
                    var area = GetArea(x, y);
                    Biomes[x][y] = area;

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

        public void MergeLabels()
        {
            // Merge labels
            /*
            for(int x = LabelGrid.Length - 1; x >= 0; --x)
            {
                for (int y = LabelGrid.Length - 1; y >= 0; --y)
            */
            for (int x = 0; x < LabelGrid.Length; ++x)
            {
                for (int y = 0; y < LabelGrid.Length; ++y)
                {
                    var label = LabelGrid[x][y];

                    if (x == 218 && y == 156)
                    {
                        System.Console.Write("");
                    }

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

        public void Build()
        {
            for (int x = 0; x < LabelGrid.Length; ++x)
            {
                for (int y = 0; y < LabelGrid.Length; ++y)
                {
                    var id = MergeTable[LabelGrid[x][y].Id];

                    AreaIds[x][y] = id; // LabelGrid[x][y].Id;
                }
            }

            MergeTable = null;
        }

        public int IndexToCoordinate(int index) => IndexStartCoordinate + index * ZoneSize;

        public int CoordinateToIndex(int coordinate) => (coordinate - ZoneSizeOffset) / ZoneSize + ZoneOffset;

        public int GetArea(int x, int y)
        {
            return _areaProvider.GetArea(IndexToCoordinate(x), IndexToCoordinate(y));
        }
    }
}
