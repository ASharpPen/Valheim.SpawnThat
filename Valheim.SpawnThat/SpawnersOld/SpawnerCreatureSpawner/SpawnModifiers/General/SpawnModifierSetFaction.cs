using System;
using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.General
{
    internal class SpawnModifierSetFaction : ISpawnModifier
    {
        private static SpawnModifierSetFaction _instance;

        public static SpawnModifierSetFaction Instance
        {
            get
            {
                return _instance ??= new SpawnModifierSetFaction();
            }
        }

        public void Modify(GameObject spawn, CreatureSpawnerConfig config)
        {
            if (spawn is null)
            {
                return;
            }

            var character = ComponentCache.Get<Character>(spawn);

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
                ComponentCache.GetZdo(spawn)?.Set("faction", (int)faction);
            }
            else
            {
                Log.LogWarning($"Failed to parse faction '{factionName}'");
            }
        }
    }
}
