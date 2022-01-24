using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Network;
using Valheim.SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;
using YamlDotNet.Serialization;

namespace Valheim.SpawnThat.Spawners.WorldSpawner.Sync;

public class WorldSpawnerConfigPackage : CompressedPackage
{
    public Dictionary<int, WorldSpawnTemplate> TemplatesById;
    public SimpleConfigurationFile SimpleConfig = SpawnSystemConfigurationManager.SimpleConfig;

    protected override void BeforePack(SerializerBuilder builder)
    {
        TemplatesById = new(WorldSpawnTemplateManager.TemplatesById);

        SimpleConfig = SpawnSystemConfigurationManager.SimpleConfig;

        Log.LogDebug($"Packaged world spawner configurations: {TemplatesById?.Count ?? 0}");
        Log.LogDebug($"Packaged simple world spawner configurations: {SimpleConfig?.Subsections?.Count ?? 0}");

        RegisterType(builder, TemplatesById.Values.SelectMany(x => x.SpawnConditions));
        RegisterType(builder, TemplatesById.Values.SelectMany(x => x.SpawnPositionConditions));
        RegisterType(builder, TemplatesById.Values.SelectMany(x => x.SpawnModifiers));
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is WorldSpawnerConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized world spawner config package");

            WorldSpawnTemplateManager.TemplatesById = configPackage.TemplatesById;
            SpawnSystemConfigurationManager.SimpleConfig = configPackage.SimpleConfig;

            Log.LogDebug($"Unpacked world spawner configurations: {WorldSpawnTemplateManager.TemplatesById?.Count ?? 0}");
            Log.LogDebug($"Unpacked simple world spawner configurations: {SpawnSystemConfigurationManager.SimpleConfig?.Subsections?.Count ?? 0}");

            Log.LogInfo("Successfully unpacked world spawner configs.");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}
