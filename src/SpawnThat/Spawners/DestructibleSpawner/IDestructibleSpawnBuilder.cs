using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;

namespace SpawnThat.Spawners.DestructibleSpawner;

/// <summary>
/// Builder for the individual spawn entries of a destructible spawner.
/// 
/// Multiple entries can exist pr spawner, and will be selected based on weight,
/// after filtering for conditions.
/// </summary>
public interface IDestructibleSpawnBuilder
{
    /// <summary>
    /// Id of spawn entry.
    /// 
    /// If id matches the index of an existing entry, the existing entry will be
    /// overridden by the assigned settings of this configuration.
    /// </summary>
    uint Id { get; }

    IDestructibleSpawnBuilder SetCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition;

    IDestructibleSpawnBuilder SetModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier;

    void SetPrefabName(string prefabName);

    string? PrefabName { get; set; }

    public float? SpawnWeight { get; set; }

    public int? LevelMin { get; set; }

    public int? LevelMax { get; set; }
}
