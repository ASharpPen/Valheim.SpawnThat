using System;
using Valheim.SpawnThat.Spawners.Conditions;
using Valheim.SpawnThat.Spawners.Modifiers;

namespace Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;

public interface ILocalSpawnBuilder
{
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

    ILocalSpawnBuilder AddOrReplaceCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition;

    ILocalSpawnBuilder AddModifier(ISpawnModifier modifier);

    ILocalSpawnBuilder AddOrReplaceModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier;

    ILocalSpawnBuilder AddPostConfiguration(Action<LocalSpawnTemplate> configure);
}
