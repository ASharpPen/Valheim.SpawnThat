namespace SpawnThat.Spawners.LocalSpawner.Models;

public record LocationIdentifier
{
    public string Location { get; set; }
    public string PrefabName { get; set; }

    public LocationIdentifier()
    { }

    public LocationIdentifier(string location, string prefabName)
    {
        Location = location;
        PrefabName = prefabName;
    }
}
