using System;
using Valheim.SpawnThat.Spawners.Conditions;
using Valheim.SpawnThat.Spawners.Modifiers;

namespace Valheim.SpawnThat.Spawners.DefaultLocalSpawner;

// TODO: How to select the spawner? This doesn't really work unless you are just adding new, or now which one is being built.
public interface IDefaultLocalSpawnBuilder
{
    DefaultLocalSpawnTemplate Build();

    IDefaultLocalSpawnBuilder SetEnabled(bool enabled = true);

    IDefaultLocalSpawnBuilder SetPrefabName(string prefabName);

    IDefaultLocalSpawnBuilder SetSpawnInterval(TimeSpan? frequency = null);

    IDefaultLocalSpawnBuilder SetMinLevel(int minLevel = 1);

    IDefaultLocalSpawnBuilder SetMaxLevel(int maxLevel = 1);

    IDefaultLocalSpawnBuilder SetSpawnAtNight(bool spawnAtNight = true);

    IDefaultLocalSpawnBuilder SetSpawnAtDay(bool spawnAtDay = true);

    IDefaultLocalSpawnBuilder SetSpawnInPlayerBase(bool spawnInPlayerBase = false);

    IDefaultLocalSpawnBuilder SetPatrolSpawn(bool patrolSpawn = false);

    IDefaultLocalSpawnBuilder AddCondition(ISpawnCondition condition);

    IDefaultLocalSpawnBuilder AddModifier(ISpawnModifier modifier);

    IDefaultLocalSpawnBuilder AddPostConfiguration(Action<DefaultLocalSpawnTemplate> configure);
}
