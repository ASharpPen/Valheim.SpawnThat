using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Configuration.Sync;
using Valheim.SpawnThat.Core.Network;
using YamlDotNet.Serialization;

namespace Valheim.SpawnThat.Tests.Configuration.Sync;

[TestClass]
public class GeneralConfigurationPackageTests
{
    [TestMethod]
    public void CanSync()
    {
        try
        {
            ConfigurationManager.GeneralConfig = new GeneralConfiguration();

            var package = new GeneralConfigPackage();

            var serialized = package.Pack();

            ConfigurationManager.GeneralConfig = null;

            // Reset stream head, to simulate transfer
            serialized.m_stream.Position = 0L;

            CompressedPackage.Unpack<GeneralConfigPackage>(serialized);

            Assert.IsNotNull(ConfigurationManager.GeneralConfig);
        }
        finally
        {
            ConfigurationManager.GeneralConfig = null;
        }
    }
}
