using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Core.Toml;
using SpawnThat.Integrations.CLLC.Models;
using SpawnThat.Integrations.CLLC.Modifiers;
using SpawnThat.Integrations.MobAi.Modifiers;
using SpawnThat.Options.Modifiers;
using SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;
using SpawnThat.Spawners.LocalSpawner.Managers;
using SpawnThat.Spawners.LocalSpawner.Models;
using SpawnThat.Utilities;

namespace SpawnThat.Spawners.LocalSpawner;

[TestClass]
public class MappingTest
{
    [TestMethod]
    public void MapsOneToOne()
    {
        // Prepare configs and data

        // Reset
        CreatureSpawnerConfigurationManager.CreatureSpawnerConfig = null;

        // Load config
        var configFile = TomlLoader.LoadFile<CreatureSpawnerConfigurationFile>(@".\Resources\spawn_that.local_spawners.test_mapping.cfg");

        var configCollection = new SpawnerConfigurationCollection();

        CreatureSpawnerConfigurationManager.CreatureSpawnerConfig = configFile;

        // Run config building

        CreatureSpawnerConfigApplier.ApplyBepInExConfigs(configCollection);

        configCollection.SpawnerConfigurations.ForEach(x => x.Build());

        // Verify matches

        var template = LocalSpawnTemplateManager.GetTemplate(new LocationIdentifier("Runestone_Boars", "Boar"));
        var original = configFile.GetGenericSubsection("Runestone_Boars").GetGenericSubsection("Boar");

        Assert.IsNotNull(template);

        Assert.AreEqual(original.PrefabName.Value, template.PrefabName);
        Assert.AreEqual(original.Enabled.Value, template.Enabled);
        Assert.AreEqual(original.TemplateEnabled.Value, template.TemplateEnabled);
        Assert.AreEqual(original.SpawnAtDay.Value, template.SpawnAtDay);
        Assert.AreEqual(original.SpawnAtNight.Value, template.SpawnAtNight);
        Assert.AreEqual(original.LevelMin.Value, template.MinLevel);
        Assert.AreEqual(original.LevelMax.Value, template.MaxLevel);
        Assert.AreEqual(original.LevelUpChance.Value, template.LevelUpChance);
        Assert.AreEqual((int)original.RespawnTime.Value, (int)template.SpawnInterval.Value.TotalMinutes);
        Assert.AreEqual(original.TriggerDistance.Value, template.ConditionPlayerDistance);
        Assert.AreEqual(original.TriggerNoise.Value, template.ConditionPlayerNoise);
        Assert.AreEqual(original.SpawnInPlayerBase.Value, template.AllowSpawnInPlayerBase);
        Assert.AreEqual(original.SetPatrolPoint.Value, template.SetPatrolSpawn);
        Assert.AreEqual(original.SetFaction.Value, template.Modifiers.OfType<ModifierSetFaction>().First().Faction);
        Assert.AreEqual(original.SetTamed.Value, template.Modifiers.OfType<ModifierSetTamed>().First().Tamed);
        Assert.AreEqual(original.SetTamedCommandable.Value, template.Modifiers.OfType<ModifierSetTamedCommandable>().First().Commandable);

        var originalCllc = original.GetGenericSubsection(CreatureSpawnerConfigCLLC.ModName) as CreatureSpawnerConfigCLLC;

        Assert.AreEqual(originalCllc.SetInfusion.Value, template.Modifiers.OfType<ModifierCllcInfusion>().First().Infusion);
        Assert.AreEqual(originalCllc.SetExtraEffect.Value, template.Modifiers.OfType<ModifierCllcExtraEffect>().First().ExtraEffect);
        Assert.AreEqual(originalCllc.SetBossAffix.Value, template.Modifiers.OfType<ModifierCllcBossAffix>().First().Affix);

        var originalMobAI = original.GetGenericSubsection(CreatureSpawnerConfigMobAI.ModName) as CreatureSpawnerConfigMobAI;

        Assert.AreEqual(originalMobAI.SetAI.Value, template.Modifiers.OfType<ModifierSetAI>().First().AiName);
    }
}
