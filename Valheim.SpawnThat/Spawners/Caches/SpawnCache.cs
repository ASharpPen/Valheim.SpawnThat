using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Valheim.SpawnThat.Spawners.Caches
{
    public static class SpawnCache
    {
        private static ConditionalWeakTable<GameObject, Character> SpawnCharacterTable = new ConditionalWeakTable<GameObject, Character>();

        public static Character GetCharacter(GameObject spawn)
        {
            if(!spawn || spawn is null)
            {
                return null;
            }

            if(SpawnCharacterTable.TryGetValue(spawn, out Character existingCharacter))
            {
                return existingCharacter;
            }

            var character = spawn.GetComponent<Character>();

            if(character is not null)
            {
                SpawnCharacterTable.Add(spawn, character);
            }

            return character;
        }
    }
}
