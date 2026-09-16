using static Heightmap;

namespace SpawnThat.World.Maps;

internal abstract class Map<T>
{
    public int GridWidth { get; protected set; }

    public abstract T[] GetRow(int grid_x);

    public abstract T GetCell(int grid_x, int grid_y);
}
