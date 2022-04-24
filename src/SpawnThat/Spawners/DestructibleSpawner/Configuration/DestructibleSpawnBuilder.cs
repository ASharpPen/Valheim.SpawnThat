using System.Collections.Generic;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration;

internal class DestructibleSpawnBuilder : IDestructibleSpawnBuilder
{
    private DestructibleSpawnTemplate Template { get; } = new();

    public uint Id { get; }

    /// <summary>
    /// Conditions required fullfilled for spawning.
    /// </summary>
    public IList<ISpawnCondition> SpawnConditions { get; set; } = new List<ISpawnCondition>();

    /// <summary>
    /// Positional conditions required fullfilled for spawning.
    /// </summary>
    public IList<ISpawnPositionCondition> SpawnPositionConditions { get; set; } = new List<ISpawnPositionCondition>();

    /// <summary>
    /// Modifications applied to entity after spawning.
    /// </summary>
    public IList<ISpawnModifier> SpawnModifiers { get; set; } = new List<ISpawnModifier>();

    public DestructibleSpawnBuilder(uint id)
    {
        Id = id;
    }

    internal DestructibleSpawnTemplate Build()
    {
        Template.Id = Id;

        Template.Conditions = SpawnConditions;
        Template.PositionConditions = SpawnPositionConditions;
        Template.Modifiers = SpawnModifiers;

        return Template;
    }

    public IDestructibleSpawnBuilder SetPrefabName(string prefabName)
    {
        Template.PrefabName = prefabName;
        return this;
    }

    public void SetCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition
    {
        SpawnConditions.AddOrReplaceByType(condition);
    }

    public void SetPositionCondition<TCondition>(TCondition positionCondition)
        where TCondition : class, ISpawnPositionCondition
    {
        SpawnPositionConditions.AddOrReplaceByType(positionCondition);
    }

    public void SetModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier
    {
        SpawnModifiers.AddOrReplaceByType(modifier);
    }

    public IDestructibleSpawnBuilder SetEnabled(bool enabled)
    {
        Template.Enabled = enabled;
        return this;
    }

    public IDestructibleSpawnBuilder SetTemplateEnabled(bool enabled)
    {
        Template.TemplateEnabled = enabled;
        return this;
    }

    public IDestructibleSpawnBuilder SetSpawnWeight(float? spawnWeight)
    {
        Template.SpawnWeight = spawnWeight;
        return this;
    }

    public IDestructibleSpawnBuilder SetLevelMin(int? minLevel)
    {
        Template.LevelMin = minLevel;
        return this;
    }

    public IDestructibleSpawnBuilder SetLevelMax(int? maxLevel)
    {
        Template.LevelMax = maxLevel;
        return this;
    }
}
