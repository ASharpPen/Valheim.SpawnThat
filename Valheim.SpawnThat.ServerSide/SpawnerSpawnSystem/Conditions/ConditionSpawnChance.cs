using System;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionSpawnChance : ISpawnCondition
    {
        private double Chance { get; }
        private long? Seed { get; }
        private Random Random { get; }

        public ConditionSpawnChance(double chance, int? seed = 0)
        {
            // Clamp to range 0-100.
            Chance = chance;
            Seed = seed;

            Random = Seed is null
                ? new Random()
                : new Random(seed.Value);
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (Chance <= 0)
            {
                return false;
            }
            else if(Chance >= 100)
            {
                return true;
            }

            return Random.NextDouble() * 100 > (100 - Chance);
        }
    }
}
