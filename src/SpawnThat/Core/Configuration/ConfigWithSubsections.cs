using System;
using System.Collections.Generic;

namespace SpawnThat.Core.Configuration
{
    [Serializable]
    public abstract class ConfigWithSubsections<T> : Config, IHaveSubsections where T : Config
    {
        public Dictionary<string, T> Subsections { get; set; } = new Dictionary<string, T>();

        public Config GetSubsection(string subsectionName)
        {
            var cleanedName = subsectionName.Trim().ToUpperInvariant();

            if (Subsections.TryGetValue(cleanedName, out T existingItem))
            {
                return existingItem;
            }
            else
            {
                var newItem = InstantiateSubsection(cleanedName);
                Subsections[cleanedName] = newItem;

                return newItem;
            }
        }

        public bool TryGet(string key, out T value)
        {
            return Subsections.TryGetValue(key.Trim().ToUpperInvariant(), out value);
        }

        protected abstract T InstantiateSubsection(string subsectionName);
    }
}
