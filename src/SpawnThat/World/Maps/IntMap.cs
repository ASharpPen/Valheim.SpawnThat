namespace SpawnThat.World.Maps;

internal class IntMap : Map<int>
{
    public int[][] Grid { get; protected set; }

    public IntMap()
    {
    }

    public IntMap(int size)
    {
        GridWidth = size;
        Grid = new int[GridWidth][];

        for (int x = 0; x < GridWidth; ++x)
        {
            Grid[x] = new int[GridWidth];
        }
    }

    public override int[] GetRow(int grid_x) =>
        Grid[grid_x];

    public override int GetCell(int grid_x, int grid_y) => 
        Grid[grid_x][grid_y];

    public static IntMap FromGrid(int[][] grid)
    {
        return new IntMap()
        {
            Grid = grid,
            GridWidth = grid.Length,
        };
    }
}