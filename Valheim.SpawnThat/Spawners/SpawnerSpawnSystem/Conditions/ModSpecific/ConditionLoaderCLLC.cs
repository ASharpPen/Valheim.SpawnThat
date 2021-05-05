using System;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions.ModSpecific.CLLC;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions.ModSpecific
{
    internal static class ConditionLoaderCLLC
    {
        public static bool InstalledCLLC { get; } = Type.GetType("CreatureLevelControl.API, CreatureLevelControl") is not null;

        public static ConditionWorldLevel ConditionWorldLevel
        {
            get
            {
                if (InstalledCLLC) return ConditionWorldLevel.Instance;

#if DEBUG
                if (!InstalledCLLC) Log.LogDebug("CLLC not found.");
#endif

                return null;
            }
        }
    }
}
