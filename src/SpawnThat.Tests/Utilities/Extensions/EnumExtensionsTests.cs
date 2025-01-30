using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpawnThat.Utilities.Extensions;

[TestClass]
public class EnumExtensionsTests
{
    [TestMethod]
    public void SplitBiomeShouldNotIncludeMax()
    {
        // Arrange
        var biome = Heightmap.Biome.All;

        // Act
        var split = EnumExtensions.Split(biome);

        // Assert
        Assert.IsNotNull(split);
        Assert.AreEqual(split.Count, 2);
        Assert.AreEqual(split[0], Heightmap.Biome.Meadows);
        Assert.AreEqual(split[1], Heightmap.Biome.Mistlands);
    }

    [TestMethod]
    public void SplitBiomeShouldOutputAllComponents()
    {
        // Arrange
        var biome = Heightmap.Biome.Meadows | Heightmap.Biome.BlackForest | Heightmap.Biome.DeepNorth;

        // Act
        var split = EnumExtensions.Split(biome);

        // Assert
        Assert.IsNotNull(split);
        Assert.AreEqual(split.Count, 3);
        Assert.AreEqual(split[0], Heightmap.Biome.Meadows);
        Assert.AreEqual(split[1], Heightmap.Biome.BlackForest);
        Assert.AreEqual(split[2], Heightmap.Biome.DeepNorth);
    }
}
