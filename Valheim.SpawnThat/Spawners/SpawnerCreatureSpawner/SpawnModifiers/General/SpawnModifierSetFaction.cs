using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.General
{
    internal class SpawnModiferSetFaction : ISpawnModifier
    {
        private static SpawnModiferSetFaction _instance;

        public static SpawnModiferSetFaction Instance
        {
            get
            {
                return _instance ??= new SpawnModiferSetFaction();
            }
        }

        public void Modify(GameObject spawn, CreatureSpawnerConfig config)
        {
            if (spawn is null)
            {
                return;
            }

            var character = SpawnCache.GetCharacter(spawn);

            if (character is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(config.SetFaction.Value))
            {
                return;
            }

            var factionName = config.SetFaction.Value;

            if (Enum.TryParse(factionName.Trim(), out Character.Faction faction))
            {
#if DEBUG
                Log.LogDebug($"Setting faction {faction}");
#endif
                character.m_faction = faction;
                SpawnCache.GetZDO(character).Set("faction", (int)faction);
            }
            else
            {
                Log.LogWarning($"Failed to parse faction '{factionName}'");
            }
        }
    }
}
