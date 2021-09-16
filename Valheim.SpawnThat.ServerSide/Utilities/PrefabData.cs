using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Valheim.SpawnThat.ServerSide.Utilities
{
    internal static class PrefabData
    {
        private Dictionary<int, bool> prefabIsBlocking { get; } = new();

        public static bool IsBlocking(ZDO obj, Vector3 point)
        {
            var prefabHash = obj.GetPrefab();

            var prefab = ZNetScene.instance.GetPrefab(prefabHash);

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

        }
    }
}
