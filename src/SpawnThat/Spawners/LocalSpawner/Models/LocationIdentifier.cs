using SpawnThat.Core;

namespace SpawnThat.Spawners.LocalSpawner.Models;

internal record LocationIdentifier
{
    public string Location { get; internal set; }
    public string PrefabName { get; internal set; }

    public LocationIdentifier()
    { }

    public LocationIdentifier(string location, string prefabName)
    {
#if DEBUG
        if (string.IsNullOrWhiteSpace(location))
        {
            Log.LogWarning("LocalSpawner builder with empty location for LocationIdentifier detected.");
        }
        if (string.IsNullOrWhiteSpace(prefabName))
        {
            Log.LogWarning("LocalSpawner builder with empty prefabName for LocationIdentifier detected.");
        }
#endif

        Location = location.Trim();
        PrefabName = prefabName.Trim();
    }
}
