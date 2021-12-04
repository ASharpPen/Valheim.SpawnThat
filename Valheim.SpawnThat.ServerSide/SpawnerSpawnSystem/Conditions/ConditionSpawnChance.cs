using UnityEngine;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionSpawnChance : ISpawnCondition
    {
        private double Chance { get; }

        public ConditionSpawnChance(double chance)
        {
            // Clamp to range 0-100.
            Chance = chance;
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (Chance <= 0)
            {
#if DEBUG
                Log.LogTrace($"Template {template.Index} has 0 or less SpawnChance.");
#endif
                return false;
            }
            else if(Chance >= 100)
            {
                return true;
            }

#if false && DEBUG
            var random = Random.Range(0, 100f);
            Log.LogTrace($"SpawnChance: {Chance}, Rolled: {random}");

            return random <= Chance;
#else
            return Random.Range(0, 100f) <= Chance;
#endif
        }
    }
}
