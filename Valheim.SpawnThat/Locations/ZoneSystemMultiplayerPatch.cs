using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.SpawnThat.Locations
{
    [HarmonyPatch(typeof(ZNet))]
    public static class ZoneSystemMultiplayerPatch
    {
		private static FieldInfo _zoneSystemLocations = AccessTools.Field(typeof(ZoneSystem), "m_locationInstances");

		[HarmonyPatch("OnNewConnection")]
        [HarmonyPostfix]
        private static void TransferLocationData(ZNet __instance, ZNetPeer peer)
        {
			if (ZNet.instance.IsServer())
			{
				Log.LogDebug("Sending locations to " + peer.m_playerName);
				SendPackage(peer.m_rpc);
			}
			else
			{
				Log.LogDebug("Registering client RPC for receiving location data from server.");
				peer.m_rpc.Register<ZPackage>(nameof(RPC_ReceiveLocationsSpawnThat), new Action<ZRpc, ZPackage>(RPC_ReceiveLocationsSpawnThat));
			}
		}

		private static void SendPackage(ZRpc rpc)
		{
			try
			{
				ZPackage package = new ZPackage();

				var locations = _zoneSystemLocations.GetValue(ZoneSystem.instance) as Dictionary<Vector2i, ZoneSystem.LocationInstance>;

				if(locations is null)
                {
					Log.LogWarning("Unable to get locations from zonesystem to send to client.");
                }

				package.Write(SerializeLocationInfo(locations));

				Log.LogDebug("Sending locations package.");

				rpc.Invoke(nameof(RPC_ReceiveLocationsSpawnThat), new object[] { package });

				Log.LogDebug("Finished sending locations package.");
			}
			catch (Exception e)
			{
				Log.LogError("Unexpected error while attempting to create and send locations package from server to client.", e);
			}
		}

		private static void RPC_ReceiveLocationsSpawnThat(ZRpc rpc, ZPackage pkg)
		{
			Log.LogDebug("Received locations package.");
			try
			{
				var serialized = pkg.ReadByteArray();

                LoadLocationInfo(serialized);

                Log.LogDebug("Successfully received locations package.");
            }
			catch (Exception e)
			{
				Log.LogError("Error while attempting to read received locations package.", e);
			}
		}

		private static void LoadLocationInfo(byte[] serialized)
        {
            using (MemoryStream memStream = new MemoryStream(serialized))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                var responseObject = binaryFormatter.Deserialize(memStream);

                if (responseObject is List<SimpleLocationDTO> locations)
                {
#if DEBUG
                    Log.LogDebug($"Deserialized {locations.Count} locations.");
#endif
					IEnumerable<SimpleLocation> simpleLocations = locations.Select(x => x.ToSimpleLocation());

                    LocationHelper.SetLocations(simpleLocations);
#if DEBUG
                    Log.LogDebug($"Assigning locations.");
#endif
                }
            }
        }

        private static byte[] SerializeLocationInfo(Dictionary<Vector2i, ZoneSystem.LocationInstance> locationInstances)
        {
#if DEBUG
            Log.LogDebug($"Serializing {locationInstances.Count} location instances.");
#endif

            List<SimpleLocationDTO> simpleLocations = new List<SimpleLocationDTO>();

            foreach (var location in locationInstances)
            {
				simpleLocations.Add(new SimpleLocationDTO(location.Key, location.Value.m_position, location.Value.m_location.m_prefabName));
            }

            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memStream, simpleLocations);

                byte[] serializedLocations = memStream.ToArray();

#if DEBUG
                Log.LogDebug($"Serialized {serializedLocations.Length} bytes of locations.");
#endif

                return serializedLocations;
            }
        }
    }
}
