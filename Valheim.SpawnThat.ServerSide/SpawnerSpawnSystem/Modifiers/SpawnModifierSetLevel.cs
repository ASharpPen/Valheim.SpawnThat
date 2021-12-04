using UnityEngine;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers
{
    public class SpawnModifierSetLevel : ISpawnModifier
    {
        public int Level { get; }

        public SpawnModifierSetLevel(int level)
        {
            Level = level;
        }

        public void Modify(SpawnContext context, GameObject entity, ZDO entityZdo)
        {
            if (Level <= 0)
            {
                return;
            }

            var character = ComponentCache.GetComponent<Character>(entity);

            if (!character || character is null)
            {
                return;
            }

            character.SetLevel(Level);
        }
    }
}
