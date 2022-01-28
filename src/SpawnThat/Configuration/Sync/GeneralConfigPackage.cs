using SpawnThat.Core;
using SpawnThat.Core.Network;

namespace SpawnThat.Configuration.Sync;

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
