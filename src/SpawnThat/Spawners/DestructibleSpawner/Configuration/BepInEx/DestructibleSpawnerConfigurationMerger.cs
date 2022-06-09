using SpawnThat.Core;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Configuration.BepInEx;

internal static class SpawnAreaSpawnerConfigurationMerger
{
    public static void MergeInto(this SpawnAreaSpawnerConfigurationFile source, SpawnAreaSpawnerConfigurationFile target)
    {
        if ((source?.Subsections?.Count ?? 0) == 0)
        {
            return;
        }

        foreach (var sourceSpawner in source.Subsections)
        {
            if (target.Subsections.ContainsKey(sourceSpawner.Key))
            {
                Log.LogWarning($"Overlapping SpawnArea spawner configs for {sourceSpawner.Value.SectionPath}, overriding existing.");
            }

            target.Subsections[sourceSpawner.Key] = sourceSpawner.Value;
        }
    }
}
