using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Core.Network;
using YamlDotNet.Serialization;

namespace SpawnThat.Configuration.Sync;

public class GeneralConfigPackage : CompressedPackage
{
    public GeneralConfiguration GeneralConfig;

    protected override void BeforePack(SerializerBuilder builder)
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
