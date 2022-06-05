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

    public BuilderOption<string> PrefabName;
    public BuilderOption<bool> Enabled;
    public BuilderOption<bool> TemplateEnabled;
    public BuilderOption<float?> SpawnWeight;
    public BuilderOption<int?> LevelMin;
    public BuilderOption<int?> LevelMax;

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

        PrefabName.DoIfSet(x => Template.PrefabName = x);
        Enabled.DoIfSet(x => Template.Enabled = x);
        TemplateEnabled.DoIfSet(x => Template.TemplateEnabled = x);
        SpawnWeight.DoIfSet(x => Template.SpawnWeight = x);
        LevelMin.DoIfSet(x => Template.LevelMin = x);
        LevelMax.DoIfSet(x => Template.LevelMax = x);

        return Template;
    }

    internal void Merge(DestructibleSpawnBuilder builder)
    {
        builder.PrefabName.AssignIfSet(ref PrefabName);
        builder.Enabled.AssignIfSet(ref Enabled);
        builder.TemplateEnabled.AssignIfSet(ref TemplateEnabled);
        builder.SpawnWeight.AssignIfSet(ref SpawnWeight);
        builder.LevelMin.AssignIfSet(ref LevelMin);
        builder.LevelMax.AssignIfSet(ref LevelMax);

        foreach (var condition in builder.SpawnConditions)
        {
            SpawnConditions.AddOrReplaceByType(condition);
        }

        foreach (var positionCondition in builder.SpawnPositionConditions)
        {
            SpawnPositionConditions.AddOrReplaceByType(positionCondition);
        }

        foreach (var modifier in builder.SpawnModifiers)
        {
            SpawnModifiers.AddOrReplaceByType(modifier);
        }
    }

    public IDestructibleSpawnBuilder SetPrefabName(string prefabName)
    {
        PrefabName = new(prefabName);
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
        Enabled = new(enabled);
        return this;
    }

    public IDestructibleSpawnBuilder SetTemplateEnabled(bool enabled)
    {
        TemplateEnabled = new(enabled);
        return this;
    }

    public IDestructibleSpawnBuilder SetSpawnWeight(float? spawnWeight)
    {
        SpawnWeight = new(spawnWeight);
        return this;
    }

    public IDestructibleSpawnBuilder SetLevelMin(int? minLevel)
    {
        LevelMin = new(minLevel);
        return this;
    }

    public IDestructibleSpawnBuilder SetLevelMax(int? maxLevel)
    {
        LevelMax = new(maxLevel);
        return this;
    }
}
