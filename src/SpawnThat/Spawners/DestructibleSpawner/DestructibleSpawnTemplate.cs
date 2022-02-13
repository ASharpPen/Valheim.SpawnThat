using System.Collections.Generic;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;

namespace SpawnThat.Spawners.DestructibleSpawner;

/// <summary>
/// Configurations for spawn in spawner.
/// </summary>
internal class DestructibleSpawnTemplate
{
    public uint Id { get; set; }

    public string? PrefabName { get; set; }

    public float? SpawnWeight { get; set; }

    public int? LevelMin { get; set; }

    public int? LevelMax { get; set; }

    public ICollection<ISpawnCondition> Conditions { get; set; } = new List<ISpawnCondition>();

    public ICollection<ISpawnPositionCondition> PositionConditions { get; set; } = new List<ISpawnPositionCondition>();

    public ICollection<ISpawnModifier> Modifiers { get; set; } = new List<ISpawnModifier>();
}
