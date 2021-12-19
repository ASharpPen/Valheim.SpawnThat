using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Network;

namespace Valheim.SpawnThat.Configuration.Multiplayer;

[Serializable]
internal class SpawnSystemConfigPackage : CompressedPackage
{
    public SpawnSystemConfigurationFile SpawnSystemConfig;
    public SimpleConfigurationFile SimpleConfig = ConfigurationManager.SimpleConfig;

    protected override void BeforePack()
    {
        SpawnSystemConfig = ConfigurationManager.SpawnSystemConfig;
        SimpleConfig = ConfigurationManager.SimpleConfig;

        Log.LogDebug($"Packaged world spawner configurations: {SpawnSystemConfig?.Subsections?.Count ?? 0}");
        Log.LogDebug($"Packaged simple world spawner configurations: {SimpleConfig?.Subsections?.Count ?? 0}");
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is SpawnSystemConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized world spawner config package");

            ConfigurationManager.SpawnSystemConfig = configPackage.SpawnSystemConfig;
            ConfigurationManager.SimpleConfig = configPackage.SimpleConfig;

            Log.LogDebug($"Unpacked world spawner configurations: {ConfigurationManager.SpawnSystemConfig?.Subsections?.Count ?? 0}");
            Log.LogDebug($"Unpacked simple world spawner configurations: {ConfigurationManager.SimpleConfig?.Subsections?.Count ?? 0}");

            Log.LogInfo("Successfully unpacked world spawner configs.");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}
