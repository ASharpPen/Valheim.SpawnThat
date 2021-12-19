using System;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General
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

        public void Modify(SpawnContext context)
        {
            if (context.Spawn is null)
            {
                return;
            }

            var character = ComponentCache.Get<Character>(context.Spawn);

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
                ComponentCache.GetZdo(context.Spawn)?.Set("faction", (int)faction);
            }
            else
            {
                Log.LogWarning($"Failed to parse faction '{factionName}'");
            }
        }
    }
}
