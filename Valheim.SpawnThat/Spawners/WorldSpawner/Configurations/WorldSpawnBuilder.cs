namespace Valheim.SpawnThat.Spawners.WorldSpawner;

public interface IWorldSpawnBuilder 
{
    WorldSpawnTemplate Build();
}

internal class WorldSpawnBuilder : IWorldSpawnBuilder
{
    public WorldSpawnTemplate Build()
    {
        throw new System.NotImplementedException();
    }
}