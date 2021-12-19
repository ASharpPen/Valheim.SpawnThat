using System;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Network;

namespace Valheim.SpawnThat.Configuration.Multiplayer;

[Serializable]
internal class GeneralConfigPackage : CompressedPackage
{
    public GeneralConfiguration GeneralConfig;

    protected override void BeforePack()
    {
        GeneralConfig = ConfigurationManager.GeneralConfig;
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is GeneralConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized config package");

            ConfigurationManager.GeneralConfig = configPackage.GeneralConfig;

            Log.LogInfo("Successfully general configs.");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}
