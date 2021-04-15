using System;
using System.Collections.Generic;

namespace Valheim.SpawnThat.Core.Configuration
{
    [Serializable]
    public abstract class ConfigWithSubsections<T> : Config, IHaveSubsections where T : Config
    {
        public Dictionary<string, T> Subsections { get; set; } = new Dictionary<string, T>();

        public Config GetSubsection(string subsectionName)
        {
            if (Subsections.TryGetValue(subsectionName, out T existingItem))
            {
                return existingItem;
            }
            else
            {
                var newItem = InstantiateSubsection(subsectionName);
                Subsections[subsectionName] = newItem;

                return newItem;
            }
        }

        protected abstract T InstantiateSubsection(string subsectionName);
    }
}
