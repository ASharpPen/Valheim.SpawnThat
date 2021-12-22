using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Spawners;

public class SpawnerManager
{
    private static SpawnerManager _instance;

    static SpawnerManager()
    {
        StateResetter.Subscribe(() =>
        {
            _instance = null;
        });
    }

    private SpawnerManager()
    {
    }

    public static SpawnerManager Instance => _instance ??= new SpawnerManager();
}
