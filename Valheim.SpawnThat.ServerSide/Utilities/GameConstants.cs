using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.ServerSide
{
    internal static class GameConstants
    {

        private static int? spawnSystemPrefab = null;
        private static long? serverID = null;

        public static int SpawnSystemPrefabHash => (spawnSystemPrefab ??= ZoneSystem.instance?.m_zoneCtrlPrefab?.name?.GetStableHashCode()) ?? 0;

        public static long ServerID => (serverID ??= ZDOMan.instance?.GetMyID()) ?? 0;
    }
}
