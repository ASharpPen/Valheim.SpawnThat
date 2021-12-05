using System.Collections.Generic;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

public abstract class SpawnTemplate
{
    public bool Enabled { get; set; } = true;

    public int Index { get; set; }

    public List<ISpawnCondition> SpawnConditions { get; set; } = new();

    public List<ISpawnPositionCondition> PositionConditions { get; set; } = new();

    public List<ISpawnModifier> Modifiers { get; set; } = new();
}
