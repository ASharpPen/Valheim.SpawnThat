using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Locations;
using Valheim.SpawnThat.ServerSide.SpawnConditions;
using Valheim.SpawnThat.ServerSide.SpawnModifiers;
using Valheim.SpawnThat.ServerSide.SpawnPositionConditions;

namespace Valheim.SpawnThat.ServerSide.SpawnerCreatureSpawner.Templates;

public class DefaultLocalTemplate
{
    private int? _prefabHash;

    public bool Enabled { get; set; } = true;

    public List<ISpawnCondition> SpawnConditions { get; set; } = new();

    public List<ISpawnPositionCondition> PositionConditions { get; set; } = new();

    public List<ISpawnModifier> Modifiers { get; set; } = new();

    public int PrefabHash
    {
        get { return _prefabHash ??= PrefabName.GetStableHashCode(); }
        set { _prefabHash = value; }
    }

    public string PrefabName { get; set; }

    public float GroundOffset { get; set; } = 0.5f;

    public TimeSpan SpawnInterval { get; set; } = TimeSpan.FromSeconds(0);
}
