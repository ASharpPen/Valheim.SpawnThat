using SpawnThat.Core;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration.BepInEx;

internal static class DestructibleSpawnerConfigurationMerger
{
    public static void MergeInto(this DestructibleSpawnerConfigurationFile source, DestructibleSpawnerConfigurationFile target)
    {
        if ((source?.Subsections?.Count ?? 0) == 0)
        {
            return;
        }

        foreach (var sourceSpawner in source.Subsections)
        {
            if (target.Subsections.ContainsKey(sourceSpawner.Key))
            {
                Log.LogWarning($"Overlapping destructible spawner configs for {sourceSpawner.Value.SectionKey}, overriding existing.");
            }

            target.Subsections[sourceSpawner.Key] = sourceSpawner.Value;
        }
    }
}
