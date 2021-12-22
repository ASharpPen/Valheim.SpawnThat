namespace Valheim.SpawnThat.Spawners.DefaultLocalSpawner;

public static class SpawnerManagerDefaultLocalSpawnExtensions
{
    public static IDefaultLocalSpawnBuilder AddDefaultLocalSpawnTemplate(this SpawnerManager manager)
    {
        return new DefaultLocalSpawnBuilder();
    }

    public static IDefaultLocalSpawnBuilder ConfigureDefaultLocalSpawner(this SpawnerManager manager)
    {
        return new DefaultLocalSpawnBuilder();
    }

    public static IDefaultLocalSpawnBuilder RegisterLocationTemplate(string location, string prefabName)
    {

    }
}
