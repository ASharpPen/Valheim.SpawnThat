using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public interface IConditionOnAwake
    {
        bool ShouldFilter(SpawnSystem spawner, SpawnConfiguration config);
    }
}
