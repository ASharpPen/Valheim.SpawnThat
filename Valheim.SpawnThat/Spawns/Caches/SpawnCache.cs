using HarmonyLib;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Valheim.SpawnThat.Spawns.Caches
{
    public static class SpawnCache
    {
        private static ConditionalWeakTable<GameObject, Character> SpawnCharacterTable = new();
        private static ConditionalWeakTable<Character, ZNetView> CharacterZnetTable = new();
        private static ConditionalWeakTable<GameObject, ZDO> SpawnZdoTable = new ();
        private static ConditionalWeakTable<GameObject, MonsterAI> SpawnMonsterAITable = new();
        private static ConditionalWeakTable<GameObject, Tameable> SpawnTameableTable = new();

        private static FieldInfo CharacterZNetView = AccessTools.Field(typeof(Character), "m_nview");

        public static Character GetCharacter(GameObject spawn)
        {
            if (!spawn || spawn is null)
            {
                return null;
            }

            if (SpawnCharacterTable.TryGetValue(spawn, out Character existingCharacter))
            {
                return existingCharacter;
            }

            var character = spawn.GetComponent<Character>();

            if (character is not null)
            {
                SpawnCharacterTable.Add(spawn, character);
            }

            return character;
        }

        public static ZDO GetZDO(Character character)
        {
            if (character is null)
            {
                return null;
            }

            if (CharacterZnetTable.TryGetValue(character, out ZNetView existing))
            {
                return existing.GetZDO();
            }

            var m_nview = CharacterZNetView.GetValue(character) as ZNetView;

            if (m_nview is not null && m_nview.IsValid())
            {
                CharacterZnetTable.Add(character, m_nview);
                return m_nview.GetZDO();
            }

            return null;
        }

        public static ZDO GetZDO(GameObject gameObject)
        {
            if(SpawnZdoTable.TryGetValue(gameObject, out ZDO existing))
            {
                return existing;
            }

            var znetView = gameObject.GetComponent<ZNetView>();
            if (!znetView || znetView is null)
            {
                return null;
            }

            var zdo = znetView.GetZDO();
            SpawnZdoTable.Add(gameObject, zdo);
            return zdo;
        }

        public static MonsterAI GetMonsterAI(GameObject gameObject)
        {
            if (SpawnMonsterAITable.TryGetValue(gameObject, out MonsterAI existing))
            {
                return existing;
            }

            var component = gameObject.GetComponent<MonsterAI>();
            if (!component || component is null)
            {
                return null;
            }

            SpawnMonsterAITable.Add(gameObject, component);
            return component;
        }

        public static Tameable GetTameable(GameObject gameObject)
        {
            if (SpawnTameableTable.TryGetValue(gameObject, out Tameable existing))
            {
                return existing;
            }

            var component = gameObject.GetComponent<Tameable>();
            if (!component || component is null)
            {
                return null;
            }

            SpawnTameableTable.Add(gameObject, component);
            return component;
        }
    }
}
