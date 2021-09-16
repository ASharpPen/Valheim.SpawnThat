using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models
{
    public class SpawnTemplate
    {
        private int? _prefabHash;
        private int? _spawnHash;

        public int PrefabHash => _prefabHash ??= PrefabName.GetStableHashCode();

        public int SpawnHash => _spawnHash ??= ("b_" + PrefabName + Index).GetStableHashCode();

        public string PrefabName { get; set; }

        public int Index { get; set; }

        public List<ISpawnCondition> SpawnConditions { get; set; } = new();
    }
}
