using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Multiplayer
{
	[HarmonyPatch(typeof(ZNet))]
	internal static class ConfigMultiplayerPatch
	{
		[HarmonyPatch("OnNewConnection")]
		[HarmonyPostfix]
		private static void SyncConfigs(ZNet __instance, ZNetPeer peer)
		{
			if (ZNet.instance.IsServer())
			{
				Log.LogDebug("Registering server RPC for sending configs on request from client.");
				peer.m_rpc.Register(nameof(RPC_RequestConfigsSpawnThat), new ZRpc.RpcMethod.Method(RPC_RequestConfigsSpawnThat));
			}
			else
			{
				Log.LogDebug("Registering client RPC for receiving configs from server.");
				peer.m_rpc.Register<ZPackage>(nameof(RPC_ReceiveConfigsSpawnThat), new Action<ZRpc, ZPackage>(RPC_ReceiveConfigsSpawnThat));

				Log.LogDebug("Requesting configs from server.");
				peer.m_rpc.Invoke(nameof(RPC_RequestConfigsSpawnThat));
			}
		}

		private static void RPC_RequestConfigsSpawnThat(ZRpc rpc)
		{
			try
			{
				if (!ZNet.instance.IsServer())
				{
					Log.LogWarning("Non-server instance received request for configs. Ignoring request.");
				}

				Log.LogInfo("Received request for configs.");

				ZPackage configPackage = new ZPackage();

				var package = new ConfigurationPackage(
					ConfigurationManager.GeneralConfig, 
					ConfigurationManager.SimpleConfig,
					ConfigurationManager.SpawnSystemConfig,
					ConfigurationManager.CreatureSpawnerConfig);

				Log.LogTrace("Serializing configs.");

				using (MemoryStream memStream = new MemoryStream())
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					binaryFormatter.Serialize(memStream, package);

					byte[] serialized = memStream.ToArray();

					configPackage.Write(serialized);
				}

				Log.LogTrace("Sending config package.");

				rpc.Invoke(nameof(RPC_ReceiveConfigsSpawnThat), new object[] { configPackage });

				Log.LogTrace("Finished sending config package.");
			}
			catch (Exception e)
            {
				Log.LogError("Unexpected error while attempting to create and send config package from server to client.", e);
            }
		}

		private static void RPC_ReceiveConfigsSpawnThat(ZRpc rpc, ZPackage pkg)
		{
			Log.LogTrace("Received package.");
			try
			{
				var serialized = pkg.ReadByteArray();

				Log.LogTrace("Deserializing package.");

				using (MemoryStream memStream = new MemoryStream(serialized))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					var responseObject = binaryFormatter.Deserialize(memStream);

					if (responseObject is ConfigurationPackage configPackage)
					{
						Log.LogDebug("Received and deserialized config package");

						Log.LogTrace("Unpackaging general config.");

						ConfigurationManager.GeneralConfig = (GeneralConfig)configPackage.GeneralConfig;

						Log.LogTrace("Successfully set general config.");
						Log.LogTrace("Unpackaging simple configs.");

						ConfigurationManager.SimpleConfig = (List<SimpleConfig>)configPackage.SimpleConfig;

						Log.LogTrace("Successfully set simple configs.");
						Log.LogTrace("Unpackaging world spawner configs.");

						ConfigurationManager.SpawnSystemConfig = (SpawnSystemConfigurationAdvanced)configPackage.SpawnSystemConfig;

						Log.LogTrace("Successfully set world spawner configs.");
						Log.LogTrace("Unpackaging local spawner configs.");

						ConfigurationManager.CreatureSpawnerConfig = (Dictionary<string, CreatureSpawnerConfigurationAdvanced>)configPackage.CreatureSpawnerConfig;

						Log.LogTrace("Successfully set local spawner configs.");
					}
					else
					{
						Log.LogWarning("Received bad config package. Unable to load.");
					}
				}
			}
			catch(Exception e)
            {
				Log.LogError("Error while attempting to read received config package.", e);
            }
		}
	}
}
