using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Valheim.SpawnThat.Spawners.Caches
{
    internal static class PrefabCache
    {
        private static ConditionalWeakTable<GameObject, BaseAI> BaseAITable = new();

        public static BaseAI GetBaseAI(GameObject prefab)
        {
            if(!prefab || prefab is null)
            {
                return null;
            }

            if(BaseAITable.TryGetValue(prefab, out BaseAI cached))
            {
                return cached;
            }

            var baseAI = prefab.GetComponent<BaseAI>();
            BaseAITable.Add(prefab, baseAI);

            return baseAI;
        }
    }
}
