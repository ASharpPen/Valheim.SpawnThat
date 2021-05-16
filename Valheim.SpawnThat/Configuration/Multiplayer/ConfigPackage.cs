using System;
using System.IO;
using System.IO.Compression;
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
                using (var zipStream = new GZipStream(memStream, CompressionLevel.Optimal))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(zipStream, this);
                }

                byte[] serialized = memStream.GetBuffer();

                Log.LogTrace($"Serialized size: {serialized.Length} bytes");

                package.Write(serialized);
            }

            return package;
        }

        public static void Unpack(ZPackage package)
        {
            var serialized = package.ReadByteArray();

            Log.LogTrace($"Deserializing {serialized.Length} bytes of configs");

            using (MemoryStream memStream = new MemoryStream(serialized))
            {
                using (var zipStream = new GZipStream(memStream, CompressionMode.Decompress, true))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    var responseObject = binaryFormatter.Deserialize(zipStream);

                    if (responseObject is ConfigPackage configPackage)
                    {
                        Log.LogDebug("Received and deserialized config package");

                        Log.LogTrace("Unpackaging configs.");

                        ConfigurationManager.GeneralConfig = configPackage.GeneralConfig;
                        ConfigurationManager.SimpleConfig = configPackage.SimpleConfig;
                        ConfigurationManager.SpawnSystemConfig = configPackage.SpawnSystemConfig;
                        ConfigurationManager.CreatureSpawnerConfig = configPackage.CreatureSpawnerConfig;

                        Log.LogTrace("Successfully unpacked configs.");

                        Log.LogDebug($"Unpacked general config");
                        Log.LogDebug($"Unpacked {ConfigurationManager.CreatureSpawnerConfig?.Subsections?.Count ?? 0} local spawner entries");
                        Log.LogDebug($"Unpacked {ConfigurationManager.SpawnSystemConfig?.Subsections?.Values?.FirstOrDefault()?.Subsections?.Count ?? 0} world spawner entries");
                        Log.LogDebug($"Unpacked {ConfigurationManager.SimpleConfig?.Subsections?.Count ?? 0} simple entries");
                    }
                    else
                    {
                        Log.LogWarning("Received bad config package. Unable to load.");
                    }
                }
            }
        }
    }
}
