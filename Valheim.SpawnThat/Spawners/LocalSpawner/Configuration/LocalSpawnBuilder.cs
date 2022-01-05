using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Spawn.Conditions;
using Valheim.SpawnThat.Spawn.Modifiers;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;

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

    public ILocalSpawnBuilder AddCondition(ISpawnCondition condition)
    {
        Template.SpawnConditions.Add(condition);
        return this;
    }

    public ILocalSpawnBuilder AddOrReplaceCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition
    {
        Template.SpawnConditions.AddOrReplaceByType(condition);
        return this;
    }

    public ILocalSpawnBuilder AddModifier(ISpawnModifier modifier)
    {
        Template.Modifiers.Add(modifier);
        return this;
    }

    public ILocalSpawnBuilder AddOrReplaceModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier
    {
        Template.Modifiers.AddOrReplaceByType(modifier);
        return this;
    }

    public ILocalSpawnBuilder AddPostConfiguration(Action<LocalSpawnTemplate> configure)
    {
        PostConfigurations.Add(configure);
        return this;
    }

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

    public ILocalSpawnBuilder SetSpawnAtDay(bool spawnAtDay = true)
    {
        Template.SpawnAtDay = spawnAtDay;
        return this;
    }

    public ILocalSpawnBuilder SetSpawnAtNight(bool spawnAtNight = true)
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
}
