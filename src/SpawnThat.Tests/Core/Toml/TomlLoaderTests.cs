using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static SpawnThat.Core.Toml.TomlLoaderTests.TestTypesConfigFile;

namespace SpawnThat.Core.Toml;

[TestClass]
public class TomlLoaderTests
{
    [TestMethod]
    public void CanParseTomlFile()
    {
        // Arrange
        string configFile = @"
# Test file for toml parsing of TomlConfig.

[Main.Subsection1]
TestSetting=TestValue

[Main.Subsection1.0]
LowerSetting=LowerValue
";

        // Act
        var config = TomlLoader.Load<TestConfig>(configFile.Split('\n'));

        // Assert
        Assert.IsNotNull(config);

        var subsectionConfig = config
            .Subsections.First(x => x.Key.Equals("main", StringComparison.OrdinalIgnoreCase)).Value
            .Subsections.First(x => x.Key.Equals("subsection1", StringComparison.OrdinalIgnoreCase)).Value;

        Assert.AreEqual("TestValue", subsectionConfig.TestSetting.Value);

        var finalConfig = subsectionConfig
            .Subsections.First(x => x.Key.Equals("0", StringComparison.OrdinalIgnoreCase)).Value;

        Assert.AreEqual("LowerValue", finalConfig.LowerEntry.Value);
    }

    [TestMethod]
    public void CanParseAllTypes()
    {
        // Arrange
        string file = @"
# Test file for toml parsing of TomlConfig.

[SomeHeader]
Bool=true
Double=0.5
Float=0.3
Int=2
String=TestTest
ListString=Test,Test
";

        // Act
        var configFile = TomlLoader.Load<TestTypesConfigFile>(file.Split('\n'));

        // Assert
        Assert.IsNotNull(configFile);

        var config = configFile
            .Subsections.First(x => x.Key.Equals("SomeHeader", StringComparison.OrdinalIgnoreCase)).Value;

        Assert.AreEqual(true, config.BoolSetting.Value);
        Assert.AreEqual(0.5, config.DoubleSetting.Value);
        Assert.AreEqual(0.3f, config.FloatSetting.Value);
        Assert.AreEqual(2, config.IntSetting.Value);
        Assert.AreEqual("TestTest", config.StringSetting.Value);

        Assert.AreEqual(2, config.ListStringSetting.Value.Count);
        Assert.AreEqual("Test", config.ListStringSetting.Value[0]);
        Assert.AreEqual("Test", config.ListStringSetting.Value[1]);
    }

    internal class TestTypesConfigFile : TomlConfigWithSubsections<TestTypesConfig>, ITomlConfigFile
    {
        protected override TestTypesConfig InstantiateSubsection(string subsectionName)
        {
            return new();
        }

        public class TestTypesConfig : TomlConfig
        {
            public TomlConfigEntry<bool?> BoolSetting = new("Bool", true);
            public TomlConfigEntry<double?> DoubleSetting = new("Double", 0.1);
            public TomlConfigEntry<float?> FloatSetting = new("Float", 0.1f);
            public TomlConfigEntry<int?> IntSetting = new("Int", 1);
            public TomlConfigEntry<string> StringSetting = new("String", "Test");
            public TomlConfigEntry<List<string>> ListStringSetting = new("ListString", new List<string>());
        }
    }

    internal class TestConfig : TomlConfigWithSubsections<TestConfig.TestConfigMain>, ITomlConfigFile
    {
        protected override TestConfigMain InstantiateSubsection(string subsectionName)
        {
            return new();
        }

        public class TestConfigMain : TomlConfigWithSubsections<TestConfigMain.TestConfigSubsection>
        {
            protected override TestConfigSubsection InstantiateSubsection(string subsectionName)
            {
                return new();
            }

            public class TestConfigSubsection : TomlConfigWithSubsections<TestConfigSubsection.TestConfigSubsectionFinal>
            {
                protected override TestConfigSubsectionFinal InstantiateSubsection(string subsectionName)
                {
                    return new();
                }

                public TomlConfigEntry<string> TestSetting = new("TestSetting", "TestDefault");

                public class TestConfigSubsectionFinal : TomlConfig
                {
                    public TomlConfigEntry<string> LowerEntry = new("LowerSetting", "TestDefault");
                }
            }
        }
    }
}
