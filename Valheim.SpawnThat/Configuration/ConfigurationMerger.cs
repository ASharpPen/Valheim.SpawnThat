using System.Linq;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Configuration
{
    //TODO: Update with another layer for each config. They are too shallow right now.
    public static class ConfigurationMerger
    {
        public static void MergeInto(this SpawnSystemConfigurationFile source, SpawnSystemConfigurationFile target)
        {
            var sourceTemplates = source?.Subsections?.Values?.FirstOrDefault();
            var targetTemplates = source?.Subsections?.Values?.FirstOrDefault();

            if(sourceTemplates is null)
            {
                return;
            }

            foreach(var sourceTemplate in sourceTemplates.Subsections)
            {
                if (targetTemplates.Subsections.ContainsKey(sourceTemplate.Key))
                {
                    Log.LogWarning($"Overlapping world spawner configs for {sourceTemplate.Value.SectionKey}, overriding existing.");
                }

                targetTemplates.Subsections[sourceTemplate.Key] = sourceTemplate.Value;
            }
        }

        public static void MergeInto(this CreatureSpawnerConfigurationFile source, CreatureSpawnerConfigurationFile target)
        {
            if(source.Subsections is null)
            {
                return;
            }

            foreach(var sourceLocation in source.Subsections)
            {
                if(target.Subsections.ContainsKey(sourceLocation.Key))
                {
                    var targetSpawner = target.Subsections[sourceLocation.Key];

                    foreach (var sourceSpawner in sourceLocation.Value.Subsections)
                    {
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
}
