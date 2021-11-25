using System.Collections.Generic;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions;

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

        public List<ISpawnPositionCondition> PositionConditions { get; set; } = new();
    }
}
