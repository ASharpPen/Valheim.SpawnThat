using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Spawners.Conditions;
using Valheim.SpawnThat.Spawners.Modifiers;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Spawners.DefaultLocalSpawner;

internal class DefaultLocalSpawnBuilder : IDefaultLocalSpawnBuilder
{
    private DefaultLocalSpawnTemplate Template { get; } = new();

    private List<Action<DefaultLocalSpawnTemplate>> PostConfigurations { get; } = new();

    private bool Finalized { get; set; }
 
    public DefaultLocalSpawnBuilder()
    {
    }

    public DefaultLocalSpawnTemplate Build()
    {
        PostConfigurations.ForEach(x => x(Template));

        Finalized = true;

        return Template;
    }

    public IDefaultLocalSpawnBuilder AddCondition(ISpawnCondition condition)
    {
        Template.SpawnConditions.Add(condition);

        return this;
    }

    public IDefaultLocalSpawnBuilder ReplaceCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition
    {
        var conditionType = typeof(TCondition);
        int existingConditionIndex = Template.SpawnConditions.FindIndex(x => x.GetType() == conditionType);

        if (existingConditionIndex < 0)
        {
            Template.SpawnConditions.Add(condition);
        }
        else
        {
            Template.SpawnConditions[existingConditionIndex] = condition;
        }

        return this;
    }

    public IDefaultLocalSpawnBuilder AddModifier(ISpawnModifier modifier)
    {
        throw new NotImplementedException();
    }

    public IDefaultLocalSpawnBuilder AddPostConfiguration(Action<DefaultLocalSpawnTemplate> configure)
    {
        throw new NotImplementedException();
    }

    public IDefaultLocalSpawnBuilder SetEnabled(bool enabled = true)
    {
        Template.Enabled = enabled;
        return this;
    }

    public IDefaultLocalSpawnBuilder SetMaxLevel(int maxLevel = 1)
    {
        Template.MaxLevel = maxLevel;
        return this;
    }

    public IDefaultLocalSpawnBuilder SetMinLevel(int minLevel = 1)
    {
        throw new NotImplementedException();
    }

    public IDefaultLocalSpawnBuilder SetPatrolSpawn(bool patrolSpawn = false)
    {
        throw new NotImplementedException();
    }

    public IDefaultLocalSpawnBuilder SetPrefabName(string prefabName)
    {
        throw new NotImplementedException();
    }

    public IDefaultLocalSpawnBuilder SetSpawnAtDay(bool spawnAtDay = true)
    {
        throw new NotImplementedException();
    }

    public IDefaultLocalSpawnBuilder SetSpawnAtNight(bool spawnAtNight = true)
    {
        throw new NotImplementedException();
    }

    public IDefaultLocalSpawnBuilder SetSpawnInPlayerBase(bool spawnInPlayerBase = false)
    {
        throw new NotImplementedException();
    }

    public IDefaultLocalSpawnBuilder SetSpawnInterval(TimeSpan? frequency = null)
    {
        throw new NotImplementedException();
    }
}
