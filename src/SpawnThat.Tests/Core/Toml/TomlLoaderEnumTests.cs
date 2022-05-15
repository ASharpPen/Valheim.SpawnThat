using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpawnThat.Core.Toml;

[TestClass]
public class TomlLoaderEnumTests
{

    [TestMethod]
    public void CanParseEnums()
    {
        // Arrange
        string file = @"
[SomeHeader]
TestSingle=State1
";

        // Act
        var configFile = TomlLoader.Load<TestEnumTypesConfigFile>(file.Split('\n'), "test.txt");

        // Assert
        Assert.IsNotNull(configFile);

        var config = configFile
            .Subsections.First(x => x.Key.Equals("SomeHeader", StringComparison.OrdinalIgnoreCase)).Value;

        Assert.IsTrue(config.TestSingle.IsSet);
        Assert.AreEqual(TestEnum.State1, config.TestSingle.Value);
    }

    [TestMethod]
    public void CanParseListEnums()
    {        
        // Arrange
        string file = @"
[SomeHeader]
TestMulti=State1, State2
";

        // Act
        var configFile = TomlLoader.Load<TestListEnumTypesConfigFile>(file.Split('\n'), "test.txt");

        // Assert
        Assert.IsNotNull(configFile);

        var config = configFile
            .Subsections.First(x => x.Key.Equals("SomeHeader", StringComparison.OrdinalIgnoreCase)).Value;

        Assert.IsTrue(config.TestMulti.IsSet);
        Assert.AreEqual(2, config.TestMulti.Value.Count);
        Assert.AreEqual(TestEnum.State1, config.TestMulti.Value.First());
        Assert.AreEqual(TestEnum.State2, config.TestMulti.Value.Last());
    }

    [TestMethod]
    public void CanParseNullableEnums()
    {
        // Arrange
        string file = @"
[SomeHeader]
TestSingle=
";

        // Act
        var configFile = TomlLoader.Load<TestNullableEnumTypesConfigFile>(file.Split('\n'), "test.txt");

        // Assert
        Assert.IsNotNull(configFile);

        var config = configFile
            .Subsections.First(x => x.Key.Equals("SomeHeader", StringComparison.OrdinalIgnoreCase)).Value;

        Assert.IsTrue(config.TestSingle.IsSet);
        Assert.IsNull(config.TestSingle.Value);
    }

    internal class TestNullableEnumTypesConfigFile : TomlConfigWithSubsections<TestNullableEnumTypesConfigFile.TestTypesConfig>, ITomlConfigFile
    {
        protected override TestTypesConfig InstantiateSubsection(string subsectionName)
        {
            return new();
        }

        public class TestTypesConfig : TomlConfig
        {
            public TomlConfigEntry<TestEnum?> TestSingle = new("TestSingle", TestEnum.Invalid);
        }
    }

    internal class TestEnumTypesConfigFile : TomlConfigWithSubsections<TestEnumTypesConfigFile.TestTypesConfig>, ITomlConfigFile
    {
        protected override TestTypesConfig InstantiateSubsection(string subsectionName)
        {
            return new();
        }

        public class TestTypesConfig : TomlConfig
        {
            public TomlConfigEntry<TestEnum> TestSingle = new("TestSingle", TestEnum.Invalid);
        }
    }

    internal class TestListEnumTypesConfigFile : TomlConfigWithSubsections<TestListEnumTypesConfigFile.TestTypesConfig>, ITomlConfigFile
    {
        protected override TestTypesConfig InstantiateSubsection(string subsectionName)
        {
            return new();
        }

        public class TestTypesConfig : TomlConfig
        {
            public TomlConfigEntry<List<TestEnum>> TestMulti = new("TestMulti", new());
        }
    }

    internal enum TestEnum
    {
        Invalid = 0,
        State1 = 1,
        State2 = 200,
    }
}
