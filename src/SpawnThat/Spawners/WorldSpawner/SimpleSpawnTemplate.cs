namespace SpawnThat.Spawners.WorldSpawner;

internal class SimpleSpawnTemplate
{
    public string PrefabName { get; set; }

    public float? SpawnMaxMultiplier { get; set; }

    public float? SpawnFrequencyMultiplier { get; set; }

    public float? GroupSizeMinMultiplier { get; set; }

    public float? GroupSizeMaxMultiplier { get; set; }
}
