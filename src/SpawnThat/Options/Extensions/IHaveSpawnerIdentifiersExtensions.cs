using System.Collections.Generic;
using SpawnThat.Options.Identifiers;
using static Heightmap;

namespace SpawnThat.Spawners;

public static class IHaveSpawnerIdentifiersExtensions
{
    public static T SetIdentifierName<T>(this T builder, params string[] spawnerPrefabNames)
        where T : IHaveSpawnerIdentifiers
    {
        builder.SetIdentifier(new IdentifierName(spawnerPrefabNames));
        return builder;
    }

    public static T SetIdentifierName<T>(this T builder, ICollection<string> spawnerPrefabNames)
    where T : IHaveSpawnerIdentifiers
    {
        builder.SetIdentifier(new IdentifierName(spawnerPrefabNames));
        return builder;
    }

    public static T SetIdentifierBiome<T>(this T builder, params Biome[] biomes)
        where T : IHaveSpawnerIdentifiers
    {
        builder.SetIdentifier(new IdentifierBiome(biomes));
        return builder;
    }

    public static T SetIdentifierBiome<T>(this T builder, IEnumerable<Biome> biomes)
        where T : IHaveSpawnerIdentifiers
    {
        builder.SetIdentifier(new IdentifierBiome(biomes));
        return builder;
    }

    public static T SetIdentifierLocation<T>(this T builder, params string[] locations)
        where T : IHaveSpawnerIdentifiers
    {
        builder.SetIdentifier(new IdentifierLocation(locations));
        return builder;
    }

    public static T SetIdentifierLocation<T>(this T builder, IEnumerable<string> locations)
    where T : IHaveSpawnerIdentifiers
    {
        builder.SetIdentifier(new IdentifierLocation(locations));
        return builder;
    }

    public static T SetIdentifierRoom<T>(this T builder, params string[] roomNames)
        where T : IHaveSpawnerIdentifiers
    {
        builder.SetIdentifier(new IdentifierRoom(roomNames));
        return builder;
    }

    public static T SetIdentifierRoom<T>(this T builder, IEnumerable<string> roomNames)
        where T : IHaveSpawnerIdentifiers
    {
        builder.SetIdentifier(new IdentifierRoom(roomNames));
        return builder;
    }
}
