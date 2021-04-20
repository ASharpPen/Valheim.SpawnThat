using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.Locations
{
    [HarmonyPatch(typeof(ZNet))]
    public static class ZoneSystemMultiplayerPatch
    {
		private static FieldInfo _zoneSystemLocations = AccessTools.Field(typeof(ZoneSystem), "m_locationInstances");

		private static bool HaveReceivedLocations = false;

		static ZoneSystemMultiplayerPatch()
		{
			StateResetter.Subscribe(() =>
			{
				HaveReceivedLocations = false;
			});
		}

		[HarmonyPatch("OnNewConnection")]
        [HarmonyPostfix]
        private static void TransferLocationData(ZNet __instance, ZNetPeer peer)
        {
			if (ZNet.instance.IsServer())
			{
				Log.LogDebug("Registering server RPC for sending location data on request from client.");
				peer.m_rpc.Register(nameof(RPC_RequestLocationsSpawnThat), new ZRpc.RpcMethod.Method(RPC_RequestLocationsSpawnThat));
			}
			else
			{
				Log.LogDebug("Registering client RPC for receiving location data from server.");
				peer.m_rpc.Register<ZPackage>(nameof(RPC_ReceiveLocationsSpawnThat), new Action<ZRpc, ZPackage>(RPC_ReceiveLocationsSpawnThat));

				Log.LogDebug("Requesting location data from server.");
				peer.m_rpc.Invoke(nameof(RPC_RequestLocationsSpawnThat));
			}
		}

		private static void RPC_RequestLocationsSpawnThat(ZRpc rpc)
		{
			try
			{
				if(!ZNet.instance.IsServer())
                {
					Log.LogWarning("Non-server instance received request for location data. Ignoring request.");
					return;
				}

				Log.LogInfo($"Sending location data.");

				ZPackage package = new ZPackage();

				var locations = _zoneSystemLocations.GetValue(ZoneSystem.instance) as Dictionary<Vector2i, ZoneSystem.LocationInstance>;

				if(locations is null)
                {
					Log.LogWarning("Unable to get locations from zonesystem to send to client.");
					return;
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
				if(HaveReceivedLocations)
                {
					Log.LogDebug("Already received locations previously. Skipping.");
					return;
                }

				var serialized = pkg.ReadByteArray();

                LoadLocationInfo(serialized);
				HaveReceivedLocations = true;

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
