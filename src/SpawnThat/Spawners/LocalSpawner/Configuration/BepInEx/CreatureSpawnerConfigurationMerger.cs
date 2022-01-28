using SpawnThat.Core;

namespace SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;

internal static class CreatureSpawnerConfigurationMerger
{
    public static void MergeInto(this CreatureSpawnerConfigurationFile source, CreatureSpawnerConfigurationFile target)
    {
        if (source.Subsections is null)
        {
            return;
        }

        foreach (var sourceLocation in source.Subsections)
        {
            if (target.Subsections.ContainsKey(sourceLocation.Key))
            {
                var targetSpawner = target.Subsections[sourceLocation.Key];

                foreach (var sourceSpawner in sourceLocation.Value.Subsections)
                {
                    if (!sourceSpawner.Value.Enabled.Value)
                    {
                        continue;
                    }

                    if (targetSpawner.Subsections.ContainsKey(sourceSpawner.Key))
                    {
                        Log.LogWarning($"Overlapping local spawner configs for {sourceSpawner.Value.SectionKey}, overriding existing.");
                    }

                    targetSpawner.Subsections[sourceSpawner.Key] = sourceSpawner.Value;
                }
            }
            else
            {
                target.Subsections[sourceLocation.Key] = sourceLocation.Value;
            }
        }
    }
}
