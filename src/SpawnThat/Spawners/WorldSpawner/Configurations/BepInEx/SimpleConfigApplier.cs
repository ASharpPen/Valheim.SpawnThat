using System.Linq;
using SpawnThat.Spawners.WorldSpawner.Managers;

namespace SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

internal class SimpleConfigApplier
{
    internal static void ApplyBepInExConfigs()
    {
        var configs = SpawnSystemConfigurationManager
            .SimpleConfig?
            .Subsections?
            .Select(x => x.Value)?
            .ToList() ?? new();

        foreach (var config in configs)
        {
            if (!config.Enable)
            {
                continue;
            }

            WorldSpawnTemplateManager.SetTemplate(new()
            {
                PrefabName = config.PrefabName,
                SpawnMaxMultiplier = config.SpawnMaxMultiplier.IsSet ? config.SpawnMaxMultiplier : null,
                SpawnFrequencyMultiplier = config.SpawnFrequencyMultiplier.IsSet ? config.SpawnFrequencyMultiplier : null,
                GroupSizeMinMultiplier = config.GroupSizeMinMultiplier.IsSet ? config.GroupSizeMinMultiplier : null,
                GroupSizeMaxMultiplier = config.GroupSizeMaxMultiplier.IsSet ? config.GroupSizeMaxMultiplier : null,
            });
        }
    }
}
