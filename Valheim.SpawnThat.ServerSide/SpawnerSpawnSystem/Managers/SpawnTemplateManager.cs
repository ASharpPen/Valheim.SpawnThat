using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Managers
{
    public static class SpawnTemplateManager
    {
        private static Dictionary<int, SpawnTemplate> RegisteredTemplates { get; set; } = new();

        static SpawnTemplateManager()
        {
            StateResetter.Subscribe(() =>
            {
                RegisteredTemplates = new();
            });
        }

        /// <summary>
        /// Register template.
        /// If index is already used, exising template is overriden.
        /// </summary>
        /// <param name="warnOnOverride">If true, will print warning when overriding existing template.</param>
        public static void RegisterTemplate(SpawnTemplate template, bool warnOnOverride = false)
        {
            if (warnOnOverride)
            {
                if (RegisteredTemplates.TryGetValue(template.Index, out SpawnTemplate existing))
                {
                    Log.LogWarning($"Overlapping world spawner configs for index '{template.Index}', overriding existing template for entity '{existing.PrefabName}' with entity '{template.PrefabName}'.");
                }
            }

            RegisteredTemplates[template.Index] = template;
        }

        /// <summary>
        /// Try register template. 
        /// If index is already used, template is not registered.
        /// </summary>
        /// <returns>True if template was registered.</returns>
        public static bool TryRegisterTemplate(SpawnTemplate template)
        {
            if (RegisteredTemplates.ContainsKey(template.Index))
            {
                return false;
            }

            RegisteredTemplates[template.Index] = template;

            return true;
        }

        public static List<SpawnTemplate> GetTemplates()
        {
            return RegisteredTemplates.Values.ToList();
        }
    }
}
