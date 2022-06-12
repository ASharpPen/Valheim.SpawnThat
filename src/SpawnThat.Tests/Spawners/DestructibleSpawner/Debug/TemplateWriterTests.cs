using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Integrations.CLLC.Models;
using SpawnThat.Spawners.SpawnAreaSpawner.Configuration;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Debug;

[TestClass]
public class TemplateWriterTests
{
    [TestMethod]
    public void CanPrepareTomlFileWithIntegrations()
    {
        var config = new SpawnAreaSpawnerConfiguration();

        var spawner = config.CreateBuilder();

        spawner
            .SetIdentifierBiome(Heightmap.Biome.Meadows)
            .SetSpawnInterval(TimeSpan.FromSeconds(5))
            ;

        spawner.GetSpawnBuilder(0)
            .SetCllcModifierInfusion(CllcCreatureInfusion.Poison)
            ;

        config.Build();

        var tomlFile = TemplateWriter.PrepareTomlFile();

        Assert.AreEqual(CllcCreatureInfusion.Poison, tomlFile.Subsections.First().Value.Subsections.First().Value.CllcConfig.SetInfusion.Value);
    }
}
