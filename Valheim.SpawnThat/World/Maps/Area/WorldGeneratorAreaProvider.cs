namespace Valheim.SpawnThat.World.Maps.Area;

internal class WorldGeneratorAreaProvider : IAreaProvider
{
    private WorldGenerator _instance = null;

    private WorldGenerator World => _instance ??= WorldGenerator.instance;

    public int GetArea(int x, int y)
    {
        return (int)World.GetBiome(x, y);
    }
}
