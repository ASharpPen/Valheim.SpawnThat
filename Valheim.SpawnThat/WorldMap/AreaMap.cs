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

        public int[][] Grid { get; private set; }

        public int[][] GridIds { get; private set; }

        public int ZoneOffset { get; private set; }

        private IAreaProvider _areaProvider;

        public int MapWidth { get; private set; }

        public int IndexStartCoordinate { get; private set; }

        private int _mapRadius;

        public AreaMap(IAreaProvider areaProvider, int mapRadius)
        {
            _areaProvider = areaProvider;
            _mapRadius = mapRadius;
            //Init(mapRadius);
            //FirstScan();
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

            Grid = new int[MapWidth][];
            GridIds = new int[MapWidth][];
            LabelGrid = new Label[MapWidth][];

            for (int x = 0; x < MapWidth; ++x)
            {
                Grid[x] = new int[MapWidth];
                GridIds[x] = new int[MapWidth];
            }
        }

        public class Label
        {
            public int Id { get; set; }
            public int Area { get; set; }
        }

        public Label[][] LabelGrid;

        public void FirstScan()
        {
            int rollingCount = 0;

            for (int x = 0; x < Grid.Length; ++x)
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
                    : Grid[x - 1];

                int lastArea = x == 0
                    ? -1
                    : lastAreaColumn[0];

                for (int y = 0; y < Grid.Length; ++y)
                {
                    var area = GetArea(x, y);
                    Grid[x][y] = area;

                    if(x == 0 && y == 174)
                    {
                        System.Console.Write(area);
                    }

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
                    }
                    LabelGrid[x][y] = lastLabel;

                    lastArea = area;
                }
            }
        }

        public void MergeLabels()
        {
            // Merge labels
            for(int x = 0; x < LabelGrid.Length; ++x)
            {
                Label[] lastColumn = x == 0
                    ? null
                    : LabelGrid[x - 1];

                Label lastLabel = x == 0
                    ? LabelGrid[0][0]
                    : lastColumn[0];

                for (int y = 0; y < LabelGrid.Length; ++y)
                {
                    var label = LabelGrid[x][y];

                    if(x == 137 && y == 142)
                    {
                        System.Console.WriteLine();
                    }

                    // Look up
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }

                    if (label.Area == lastLabel.Area)
                    {
                        if(label.Id != lastLabel.Id)
                        {
                            // Merge labels.
                            if (label.Id > lastLabel.Id)
                            {
                                label.Id = lastLabel.Id;
                            }
                            else
                            {
                                lastLabel.Id = label.Id;
                            }
                        }
                    }

                    // Look left
                    if (x == 0)
                    {
                        continue;
                    }

                    Label leftLabel = lastColumn[y];

                    if (label.Area == leftLabel.Area)
                    {
                        if (label.Id != leftLabel.Id)
                        {
                            // Merge labels.
                            if (label.Id > leftLabel.Id)
                            {
                                label.Id = leftLabel.Id;
                            }
                            else
                            {
                                leftLabel.Id = label.Id;
                            }
                        }
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
                    GridIds[x][y] = LabelGrid[x][y].Id;
                }
            }
        }

        public int IndexToCoordinate(int index) => IndexStartCoordinate + index * ZoneSize;

        public int CoordinateToIndex(int coordinate) => (coordinate - ZoneSizeOffset) / ZoneSize + ZoneOffset;

        public int GetArea(int x, int y)
        {
            return _areaProvider.GetArea(IndexToCoordinate(x), IndexToCoordinate(y));
        }
    }
}
