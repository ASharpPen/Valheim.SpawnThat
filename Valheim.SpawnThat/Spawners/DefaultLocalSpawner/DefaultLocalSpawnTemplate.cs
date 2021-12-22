using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Spawners.Conditions;
using Valheim.SpawnThat.Spawners.Modifiers;

namespace Valheim.SpawnThat.Spawners.DefaultLocalSpawner;

public class DefaultLocalSpawnTemplate
{
    private int? _prefabHash;

    public bool Enabled { get; set; } = true;

    public string PrefabName { get; set; }

    public TimeSpan SpawnInterval { get; set; } = TimeSpan.FromSeconds(0);

    public int MinLevel { get; set; } = 1;

    public int MaxLevel { get; set; } = 1;

    public bool SpawnAtDay { get; set; } = true;

    public bool SpawnAtNight { get; set; } = true;

    public bool AllowSpawnInPlayerBase { get; set; }

    public bool SetPatrolSpawn { get; set; }

    public List<ISpawnCondition> SpawnConditions { get; set; } = new();

    public List<ISpawnModifier> Modifiers { get; set; } = new();

    public int PrefabHash
    {
        get { return _prefabHash ??= PrefabName.GetStableHashCode(); }
        set { _prefabHash = value; }
    }
}
