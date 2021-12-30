using System;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.ModSpecific.MobAI;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.ModSpecific
{
    internal static class SpawnModifierLoaderMobAI
    {
        public static bool InstalledMobAI { get; } = Type.GetType("RagnarsRokare.MobAI.MobAILib, MobAILib") is not null;

        public static SpawnModifierSetAI SetAI
        {
            get
            {
                if (InstalledMobAI) return SpawnModifierSetAI.Instance;

#if DEBUG
                if (!InstalledMobAI) Log.LogDebug("MobAI not found.");
#endif

                return null;
            }
        }
    }
}
