using System;
using System.Collections.Generic;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Spawners.LocalSpawner.Models;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners.LocalSpawner.Configuration;

internal class LocalSpawnBuilder : ILocalSpawnBuilder
{
    private LocalSpawnTemplate Template { get; } = new();

    private List<Action<LocalSpawnTemplate>> PostConfigurations { get; } = new();

    public LocalSpawnBuilder()
    {
    }

    public LocalSpawnTemplate Build()
    {
        PostConfigurations.ForEach(x => x(Template));
        return Template;
    }

    public ILocalSpawnBuilder SetCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition
    {
        Template.SpawnConditions.AddOrReplaceByType(condition);
        return this;
    }

    public ILocalSpawnBuilder SetModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier
    {
        Template.Modifiers.AddOrReplaceByType(modifier);
        return this;
    }

    /*
    public ILocalSpawnBuilder AddPostConfiguration(Action<LocalSpawnTemplate> configure)
    {
        PostConfigurations.Add(configure);
        return this;
    }
    */

    public ILocalSpawnBuilder SetEnabled(bool enabled = true)
    {
        Template.Enabled = enabled;
        return this;
    }

    public ILocalSpawnBuilder SetMaxLevel(int maxLevel = 1)
    {
        Template.MaxLevel = maxLevel;
        return this;
    }

    public ILocalSpawnBuilder SetMinLevel(int minLevel = 1)
    {
        Template.MinLevel = minLevel;
        return this;
    }

    public ILocalSpawnBuilder SetPatrolSpawn(bool patrolSpawn = false)
    {
        Template.SetPatrolSpawn = patrolSpawn;
        return this;
    }

    public ILocalSpawnBuilder SetPrefabName(string prefabName)
    {
        Template.PrefabName = prefabName;
        return this;
    }

    public ILocalSpawnBuilder SetSpawnDuringNight(bool spawnAtDay = true)
    {
        Template.SpawnAtDay = spawnAtDay;
        return this;
    }

    public ILocalSpawnBuilder SetSpawnDuringDay(bool spawnAtNight = true)
    {
        Template.SpawnAtNight = spawnAtNight;
        return this;
    }

    public ILocalSpawnBuilder SetSpawnInPlayerBase(bool spawnInPlayerBase = false)
    {
        Template.AllowSpawnInPlayerBase = spawnInPlayerBase;
        return this;
    }

    public ILocalSpawnBuilder SetSpawnInterval(TimeSpan? frequency = null)
    {
        Template.SpawnInterval = frequency ?? TimeSpan.Zero;
        return this;
    }

    public ILocalSpawnBuilder SetLevelUpChance(float chance)
    {
        Template.LevelUpChance = chance;
        return this;
    }

    public ILocalSpawnBuilder SetConditionPlayerWithinDistance(float withinDistance)
    {
        Template.ConditionPlayerDistance = withinDistance;
        return this;
    }

    public ILocalSpawnBuilder SetConditionPlayerNoise(float noise)
    {
        Template.ConditionPlayerNoise = noise;
        return this;
    }
}
