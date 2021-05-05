using System;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General
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

        public void Modify(SpawnContext context)
        {
            if (context.Spawn is null)
            {
                return;
            }

            var character = SpawnCache.GetCharacter(context.Spawn);

            if(character is null)
            {
                return;
            }

            if(string.IsNullOrWhiteSpace(context.Config.SetFaction.Value))
            {
                return;
            }

            var factionName = context.Config.SetFaction.Value;

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
