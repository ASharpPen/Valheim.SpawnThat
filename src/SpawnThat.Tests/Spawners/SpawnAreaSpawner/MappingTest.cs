using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Core.Toml;
using SpawnThat.Spawners.SpawnAreaSpawner.Configuration.BepInEx;
using SpawnThat.Spawners.SpawnAreaSpawner.Debug;
using SpawnThat.Utilities;

namespace SpawnThat.Spawners.SpawnAreaSpawner;

[TestClass]
public class MappingTest
{
    [TestMethod]
    public void MapsOneToOne()
    {
        // Prepare configs and data

        // Reset
        SpawnAreaSpawnerTomlCfgManager.Config = null;

        // Load config
        var configFile = TomlLoader.LoadFile<SpawnAreaSpawnerConfigurationFile>(@".\Resources\spawn_that.spawnarea_spawners.test_mapping.cfg");

        var configCollection = new SpawnerConfigurationCollection();

        SpawnAreaSpawnerTomlCfgManager.Config = configFile;

        // Run config building

        SpawnAreaSpawnerConfigApplier.ApplyBepInExConfigs(configCollection);

        configCollection.SpawnerConfigurations.ForEach(x => x.Build());

        // Verify match after converting back to config format.

        var preparedConfig = TemplateWriter.PrepareTomlFile();

        var originalConfig = configFile.GetGenericSubsection(configFile.Subsections.First().Key);
        var comparedConfig = preparedConfig.GetGenericSubsection(configFile.Subsections.First().Key);

        CompareConfig(originalConfig, comparedConfig);

        foreach (var spawnConfig in comparedConfig.Subsections)
        {
            var originalSpawnconfig = originalConfig.Subsections[spawnConfig.Key];

            CompareConfig(originalSpawnconfig, spawnConfig.Value);

            foreach (var modConfig in spawnConfig.Value.Subsections)
            {
                var originalModConfig = originalSpawnconfig.GetSubsection(modConfig.Key);

                CompareConfig(originalModConfig, modConfig.Value);
            }
        }

        void CompareConfig(TomlConfig original, TomlConfig mapped)
        {
            var originalEntries = original
                .GetEntries()
                .ToDictionary(x => x.Key, x => x.Value);

            foreach (var entry in mapped.GetEntries())
            {
                // Skip intentionally not mapped values.
                switch (entry.Key)
                {
                    case "TemplateEnabled":
                    case "DistanceToTriggerPlayerConditions":
                        continue;
                }

                var originalEntry = originalEntries[entry.Key];

                Assert.AreEqual(originalEntry.IsSet, entry.Value.IsSet, $"Mismatching mapped IsSet values for '[{mapped.SectionPath}]:{entry.Key}'");

                var listtype = typeof(List<>);

                if (entry.Value.SettingType.IsGenericType &&
                    entry.Value.SettingType.GetGenericTypeDefinition() == listtype)
                {
                    var entryValue = TomlWriterFactory
                        .Write(entry.Value)
                        .SplitByComma(true);

                    var originalEntryValue = TomlWriterFactory
                        .Write(originalEntry)
                        .SplitByComma(true)
                        .ToHashSet();

                    if (entryValue.Count != originalEntryValue.Count)
                    {
                        Assert.AreEqual(originalEntryValue, entryValue, $"Mismatching mapped Value for '[{mapped.SectionPath}]:{entry.Key}'");
                    }

                    foreach (var value in entryValue)
                    {
                        Assert.IsTrue(originalEntryValue.Contains(value), $"Mismatching mapped Value for '[{mapped.SectionPath}]:{entry.Key}'");
                    }
                }
                else
                {
                    Assert.AreEqual(originalEntry.GetValue(), entry.Value.GetValue(), $"Mismatching mapped Value for '[{mapped.SectionPath}]:{entry.Key}'");
                }
            }
        }
    }
}
