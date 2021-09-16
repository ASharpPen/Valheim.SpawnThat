using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Position;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem
{
    public class SpawnTemplate
    {
        public int? PrefabHash { get; set; }

        public GameObject? Prefab { get; set; }

        public SpawnConfiguration Config { get; set; }

        public List<IConditionOnAwake> WakeupConditions { get; set; } = new();

        public List<IConditionOnSpawn> SpawnConditions { get; set; } = new();

        public List<ISpawnPositionCondition> SpawnPositionConditions { get; set; } = new();

        public List<ISpawnModifier> SpawnModifiers { get; set; } = new();
    }
}
