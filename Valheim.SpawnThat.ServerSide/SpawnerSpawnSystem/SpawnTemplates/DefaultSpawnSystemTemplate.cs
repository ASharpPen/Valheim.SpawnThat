using System;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem;

public class DefaultSpawnSystemTemplate : SpawnTemplate
{
    public int MinSpawned { get; set; } = 1;

    public int MaxSpawned { get; set; } = 1;

    public float Radius { get; set; } = 3f;

    public float GroundOffset { get; set; } = 0.5f;

    public float SpawnChance { get; set; } = 100f;

    public TimeSpan SpawnInterval { get; set; } = TimeSpan.FromSeconds(90);
}
