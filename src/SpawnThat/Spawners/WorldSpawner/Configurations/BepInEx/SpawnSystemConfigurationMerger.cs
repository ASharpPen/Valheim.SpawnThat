using System.Linq;
using SpawnThat.Core;

namespace SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

internal static class SpawnSystemConfigurationMerger
{
    public static void MergeInto(this SpawnSystemConfigurationFile source, SpawnSystemConfigurationFile target)
    {
        var sourceTemplates = source?.Subsections?.Values?.FirstOrDefault();
        var targetTemplates = target?.Subsections?.Values?.FirstOrDefault();

        if (sourceTemplates is null)
        {
            return;
        }

        if (targetTemplates is null)
        {
            var sourceEntry = source.Subsections.First();
            target.Subsections[sourceEntry.Key] = sourceEntry.Value;
            return;
        }

        foreach (var sourceTemplate in sourceTemplates.Subsections)
        {
            if (sourceTemplate.Value.TemplateEnabled.IsSet &&
                (sourceTemplate.Value.TemplateEnabled.Value ?? sourceTemplate.Value.TemplateEnabled.DefaultValue.Value) == false)
            {
                continue;
            }

            if (targetTemplates.Subsections.TryGetValue(sourceTemplate.Key, out var targetConfig))
            {
                if (targetConfig.TemplateEnabled.Value ?? targetConfig.TemplateEnabled.DefaultValue.Value)
                {
                    Log.LogWarning($"Overlapping world spawner configs for {sourceTemplate.Value.SectionPath}, overriding existing.");
                }
            }

            targetTemplates.Subsections[sourceTemplate.Key] = sourceTemplate.Value;
        }
    }
}
