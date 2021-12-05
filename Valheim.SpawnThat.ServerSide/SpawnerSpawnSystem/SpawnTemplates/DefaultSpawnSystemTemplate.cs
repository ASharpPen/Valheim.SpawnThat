using System;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem;

public class DefaultSpawnSystemTemplate : SpawnTemplate
{
    private int? _prefabHash;
    private int? _spawnHash;

    public int PrefabHash => _prefabHash ??= PrefabName.GetStableHashCode();

    public string PrefabName { get; set; }

    public int MinSpawned { get; set; } = 1;

    public int MaxSpawned { get; set; } = 1;

    public float Radius { get; set; } = 3f;

    public float GroundOffset { get; set; } = 0.5f;

    public float SpawnChance { get; set; } = 100f;

    public TimeSpan SpawnInterval { get; set; } = TimeSpan.FromSeconds(90);

    public bool IsRaidMob { get; set; } = false;

    public int SpawnHash => _spawnHash ??= (((IsRaidMob) ? "e_" : "b_") + PrefabName + Index).GetStableHashCode();

}
