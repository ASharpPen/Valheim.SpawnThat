using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.Utilities
{
    internal static class PrefabData
    {
        private static Dictionary<int, bool> prefabIsBlocking { get; } = new();
        private static Dictionary<int, bool> prefabIsPlayerBase { get; } = new();

        // TODO: Clean up. This just seems pointless compared to just doing the work where needed.
        public static bool IsBlocking(ZDO obj, out GameObject prefab)
        {
            var prefabHash = obj.GetPrefab();

            prefab = ZNetScene.instance.GetPrefab(prefabHash);

            if (prefab is null)
            {
                return false;
            }

            var blockingLayer = ZoneSystem.instance.m_blockRayMask;

            // Check if prefab can block
            if ((prefab.layer & blockingLayer) == 0)
            {
                return false;
            }

            return true;
        }

        public static bool IsPlayerBase(ZDO zdo) => IsPlayerBase(zdo.GetPrefab());

        public static bool IsPlayerBase(int prefabHash)

        {
            if (prefabIsPlayerBase.TryGetValue(prefabHash, out var cached))
            {
                return cached;
            }

            var prefab = ZNetScene.instance.GetPrefab(prefabHash);

            if (prefab is null)
            {
                return false;
            }

            var effectArea = ComponentCache.GetComponentInChildren<EffectArea>(prefab, true);

            if (effectArea is null)
            {
                return prefabIsPlayerBase[prefabHash] = false;
            }
            else
            {
                return prefabIsPlayerBase[prefabHash] = (effectArea.m_type & EffectArea.Type.PlayerBase) > 0;
            }
        }
    }
}
