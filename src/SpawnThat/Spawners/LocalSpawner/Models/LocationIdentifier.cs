namespace SpawnThat.Spawners.LocalSpawner.Models;

internal record LocationIdentifier
{
    public string Location { get; internal set; }
    public string PrefabName { get; internal set; }

    internal LocationIdentifier()
    { }

    public LocationIdentifier(string location, string prefabName)
    {
        Location = location;
        PrefabName = prefabName;
    }
}
