using System.Collections.Generic;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;

namespace SpawnThat.Spawners.DestructibleSpawner;

/// <summary>
/// <para>
///     Destructible spawn builder settings.
///     Any property not set will be ignored.
/// </para>
/// <para>
///     Any setting not set for a builder will mean Spawn That will use the existing setting
///     when overriding a template, or set a default value if creating a new.
/// </para>
/// <para>
///     Intended as an optional way to configure IDestructibleSpawnBuilder.
/// </para>
/// </summary>
public class DestructibleSpawnSettings
{    /// <summary>
     /// Id of spawn entry.
     /// 
     /// If id matches the index of an existing entry, the existing entry will be
     /// overridden by the assigned settings of this configuration.
     /// </summary>
    public uint Id { get; set; }

    public bool Enabled { get; set; }

    public bool TemplateEnabled { get; set; }

    public string? PrefabName { get; set; }

    public float? SpawnWeight { get; set; }

    public int? LevelMin { get; set; }

    public int? LevelMax { get; set; }

    public ICollection<ISpawnCondition> Conditions { get; set; } = new List<ISpawnCondition>();

    public ICollection<ISpawnPositionCondition> PositionConditions { get; set; } = new List<ISpawnPositionCondition>();

    public ICollection<ISpawnModifier> Modifiers { get; set; } = new List<ISpawnModifier>();
}
