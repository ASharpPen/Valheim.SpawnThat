using System;
using Valheim.SpawnThat.Spawners.Conditions;
using Valheim.SpawnThat.Spawners.Modifiers;

namespace Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;

public interface ILocalSpawnBuilder
{
    LocalSpawnTemplate Build();

    ILocalSpawnBuilder SetEnabled(bool enabled = true);

    ILocalSpawnBuilder SetPrefabName(string prefabName);

    ILocalSpawnBuilder SetSpawnInterval(TimeSpan? frequency = null);

    ILocalSpawnBuilder SetMinLevel(int minLevel = 1);

    ILocalSpawnBuilder SetMaxLevel(int maxLevel = 1);

    ILocalSpawnBuilder SetSpawnAtNight(bool spawnAtNight = true);

    ILocalSpawnBuilder SetSpawnAtDay(bool spawnAtDay = true);

    ILocalSpawnBuilder SetSpawnInPlayerBase(bool spawnInPlayerBase = false);

    ILocalSpawnBuilder SetPatrolSpawn(bool patrolSpawn = false);

    ILocalSpawnBuilder AddCondition(ISpawnCondition condition);

    ILocalSpawnBuilder AddModifier(ISpawnModifier modifier);

    ILocalSpawnBuilder AddPostConfiguration(Action<LocalSpawnTemplate> configure);
}
