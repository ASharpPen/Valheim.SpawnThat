using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Configuration
{
    public static class ConfigurationMerger
    {
        public static void MergeInto(this SpawnSystemConfiguration source, SpawnSystemConfiguration target)
        {
            foreach(var sourceTemplate in source.Subsections)
            {
                if (target.Subsections.ContainsKey(sourceTemplate.Key))
                {
                    Log.LogWarning($"Overlapping world spawner configs for {sourceTemplate.Value.SectionKey}, overriding existing.");
                }

                target.Subsections[sourceTemplate.Key] = sourceTemplate.Value;
            }
        }

        public static void MergeInto(this CreatureSpawnerFileConfiguration source, CreatureSpawnerFileConfiguration target)
        {
            foreach(var sourceSpawner in source.Subsections)
            {
                if(target.Subsections.ContainsKey(sourceSpawner.Key))
                {
                    Log.LogWarning($"Overlapping local spawner configs for {sourceSpawner.Value.SectionKey}, overriding existing.");
                }

                target.Subsections[sourceSpawner.Key] = sourceSpawner.Value;
            }
        }
    }
}
