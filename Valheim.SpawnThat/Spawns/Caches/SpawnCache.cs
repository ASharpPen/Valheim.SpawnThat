using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.Spawns.Caches
{
    public static class SpawnCache
    {
        public static Character GetCharacter(GameObject spawn) => ComponentCache.GetComponent<Character>(spawn);

        public static ZDO GetZDO(Character character)
        {
            var m_nview = ComponentCache.GetComponent<ZNetView>(character.gameObject);

            if (m_nview is not null && m_nview.IsValid())
            {
                return m_nview.GetZDO();
            }

            return null;
        }

        public static ZDO GetZDO(GameObject gameObject)
        {
            var znetView = ComponentCache.GetComponent<ZNetView>(gameObject);

            if (!znetView || znetView is null)
            {
                return null;
            }

            return znetView.GetZDO();
        }

        public static Tameable GetTameable(GameObject gameObject) => ComponentCache.GetComponent<Tameable>(gameObject); 
    }
}
