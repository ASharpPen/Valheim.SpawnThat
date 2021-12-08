using System.Collections.Generic;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.Utilities
{
    internal static class PrefabData
    {
        private static Dictionary<int, bool> prefabIsPlayerBase { get; } = new();
        private static Dictionary<int, bool> prefabIsSolid { get; } = new();

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

        public static bool IsSolid(ZDO zdo) => IsSolid(zdo.GetPrefab());

        public static bool IsSolid(int prefabHash)
        {
            if (prefabIsSolid.TryGetValue(prefabHash, out var cached))
            {
                return cached;
            }

            var prefab = ZNetScene.instance.GetPrefab(prefabHash);

            // Check if prefab layer is in solid
            if ((prefab.layer & ZoneSystem.instance.m_solidRayMask) == 0)
            {
                return false;
            }

            return true;

        }
    }
}
