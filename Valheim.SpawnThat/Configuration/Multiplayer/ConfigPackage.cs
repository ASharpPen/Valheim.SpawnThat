using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Configuration.Multiplayer
{
    [Serializable]
    internal class ConfigPackage
    {
        public GeneralConfiguration GeneralConfig;

        public SimpleConfigurationFile SimpleConfig;

        public SpawnSystemConfigurationFile SpawnSystemConfig;

        public CreatureSpawnerConfigurationFile CreatureSpawnerConfig;

        public ZPackage Pack()
        {
            ZPackage package = new ZPackage();

            GeneralConfig = ConfigurationManager.GeneralConfig;
            SimpleConfig = ConfigurationManager.SimpleConfig;
            SpawnSystemConfig = ConfigurationManager.SpawnSystemConfig;
            CreatureSpawnerConfig = ConfigurationManager.CreatureSpawnerConfig;

            Log.LogTrace("Serializing configs.");

            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memStream, this);

                byte[] serialized = memStream.ToArray();

                package.Write(serialized);
            }

            return package;
        }

        public static void Unpack(ZPackage package)
        {
            var serialized = package.ReadByteArray();

            Log.LogTrace("Deserializing package.");

            using (MemoryStream memStream = new MemoryStream(serialized))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                var responseObject = binaryFormatter.Deserialize(memStream);

                if (responseObject is ConfigPackage configPackage)
                {
                    Log.LogDebug("Received and deserialized config package");

                    Log.LogTrace("Unpackaging configs.");

                    ConfigurationManager.GeneralConfig = configPackage.GeneralConfig;
                    ConfigurationManager.SimpleConfig = configPackage.SimpleConfig;
                    ConfigurationManager.SpawnSystemConfig = configPackage.SpawnSystemConfig;
                    ConfigurationManager.CreatureSpawnerConfig = configPackage.CreatureSpawnerConfig;

                    Log.LogTrace("Successfully unpacked configs.");

                    Log.LogTrace($"Unpacked {ConfigurationManager.CreatureSpawnerConfig?.Subsections?.Count ?? 0} creature spawner entries");
                    Log.LogTrace($"Unpacked {ConfigurationManager.SpawnSystemConfig?.Subsections?.Values?.FirstOrDefault()?.Subsections?.Count ?? 0} spawn system entries");
                    Log.LogTrace($"Unpacked general config: {ConfigurationManager.GeneralConfig is not null}");
                    Log.LogTrace($"Unpacked {ConfigurationManager.SimpleConfig?.Subsections?.Count ?? 0} simple entries");
                }
                else
                {
                    Log.LogWarning("Received bad config package. Unable to load.");
                }
            }
        }
    }
}
