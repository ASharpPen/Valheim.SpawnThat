using System;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.ModSpecific.CLLC;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.ModSpecific
{
    internal static class SpawnModifierLoaderCLLC
    {
        public static bool InstalledCLLC { get; } = Type.GetType("CreatureLevelControl.API, CreatureLevelControl") is not null;

        public static SpawnModifierBossAffix BossAffix
        {
            get
            {
                if (InstalledCLLC) return SpawnModifierBossAffix.Instance;

#if DEBUG
                if (!InstalledCLLC) Log.LogDebug("CLLC not found.");
#endif

                return null;
            }
        }

        public static SpawnModifierExtraEffect ExtraEffect
        {
            get
            {
                if (InstalledCLLC) return SpawnModifierExtraEffect.Instance;

#if DEBUG
                if (!InstalledCLLC) Log.LogDebug("CLLC not found.");
#endif

                return null;

            }
        }

        public static SpawnModifierInfusion Infusion
        {
            get
            {
                if (InstalledCLLC) return SpawnModifierInfusion.Instance;

#if DEBUG
                if (!InstalledCLLC) Log.LogDebug("CLLC not found.");
#endif

                return null;

            }
        }

        public static SpawnModifierSetLevel SetLevel
        {
            get
            {
                if (InstalledCLLC) return SpawnModifierSetLevel.Instance;

                return null;
            }
        }
    }
}
