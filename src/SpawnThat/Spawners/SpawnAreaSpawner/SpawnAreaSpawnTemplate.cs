using System.Collections.Generic;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;

namespace SpawnThat.Spawners.SpawnAreaSpawner;

/// <summary>
/// Configurations for spawn in spawner.
/// </summary>
internal class SpawnAreaSpawnTemplate
{
    /// <summary>
    /// Id of spawn entry.
    /// 
    /// If id matches the index of an existing entry, the existing entry will be
    /// overridden by the assigned settings of this configuration.
    /// </summary>
    public uint Id { get; set; }

    public bool Enabled { get; set; } = true;

    public bool TemplateEnabled { get; set; } = true;

    public string PrefabName { get; set; }

    public float? SpawnWeight { get; set; }

    public int? LevelMin { get; set; }

    public int? LevelMax { get; set; }

    public ICollection<ISpawnCondition> Conditions { get; set; } = new List<ISpawnCondition>();

    public ICollection<ISpawnPositionCondition> PositionConditions { get; set; } = new List<ISpawnPositionCondition>();

    public ICollection<ISpawnModifier> Modifiers { get; set; } = new List<ISpawnModifier>();
}
