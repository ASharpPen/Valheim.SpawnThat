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
                    if (sourceSpawner.Value.TemplateEnabled.IsSet &&
                        (sourceSpawner.Value.TemplateEnabled.Value ?? sourceSpawner.Value.TemplateEnabled.DefaultValue.Value) == false)
                    {
#if DEBUG
                        Log.LogDebug($"Local spawner '{sourceSpawner.Value.SectionPath}' template is disabled. Skipping merge.");
#endif
                        continue;
                    }

                    if (targetSpawner.Subsections.TryGetValue(sourceSpawner.Key, out var targetConfig))
                    {
                        if (targetConfig.TemplateEnabled.Value ?? targetConfig.TemplateEnabled.DefaultValue.Value)
                        {
                            Log.LogWarning($"Overlapping local spawner configs for {sourceSpawner.Value.SectionPath}, overriding existing.");
                        }

#if DEBUG
                        Log.LogWarning("\t Override");
                        Log.LogWarning("\t" + sourceSpawner.Value.PrefabName);
                        Log.LogWarning("\t" + sourceSpawner.Value.Enabled);
                        Log.LogWarning("\t" + sourceSpawner.Value.TemplateEnabled);

                        Log.LogWarning("\t Existing");
                        Log.LogWarning("\t" + targetConfig.PrefabName);
                        Log.LogWarning("\t" + targetConfig.Enabled);
                        Log.LogWarning("\t" + targetConfig.TemplateEnabled);
#endif
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
