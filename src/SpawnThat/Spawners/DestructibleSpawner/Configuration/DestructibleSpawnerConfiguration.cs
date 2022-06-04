using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Spawners.DestructibleSpawner.Managers;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration;

internal class DestructibleSpawnerConfiguration : ISpawnerConfiguration
{
    private List<DestructibleSpawnerBuilder> Builders { get; } = new();

    private bool finalized = false;

    public void Build()
    {
        if (finalized)
        {
            Log.LogWarning("Attempting to build world spawner configs that have already been finalized. Ignoring request.");
            return;
        }

        finalized = true;

        var builderMatches = GroupByIdentifiers(Builders);
        var mergedBuilders = MergeMatchingBuilders(builderMatches);

        foreach (var builder in mergedBuilders)
        {
            DestructibleSpawnerManager.AddTemplate(builder.Build());
        }
    }

    public IDestructibleSpawnerBuilder CreateBuilder()
    {
        if (finalized)
        {
            throw new InvalidOperationException("Collection is finalized. Builders cannot be retrieved or modified after build.");
        }

        DestructibleSpawnerBuilder builder = new();

        Builders.Add(builder);

        return builder;
    }

    private static List<DestructibleSpawnerBuilder> MergeMatchingBuilders(List<BuilderIdentifier> builderIdentifiers)
    {
        List<DestructibleSpawnerBuilder> mergedBuilders = new(builderIdentifiers.Count);

        foreach (var builderIdentifier in builderIdentifiers)
        {
            mergedBuilders.Add(builderIdentifier.Builder);

            foreach (var builder in builderIdentifier.ToMerge)
            {
                builderIdentifier.Builder.Merge(builder);
            }
        }

        return mergedBuilders;
    }

    private static List<BuilderIdentifier> GroupByIdentifiers(List<DestructibleSpawnerBuilder> builders)
    {
        List<BuilderIdentifier> uniqueBuilders = new(builders.Count);

        var builderIdentifiers = builders
            .Select(x => new BuilderIdentifier(x))
            .ToList();

        foreach (var builderIdentifier in builderIdentifiers)
        {
            var existingBuilder = uniqueBuilders.Find(x => x.Equals(builderIdentifier));

            if (existingBuilder is null)
            {
                uniqueBuilders.Add(builderIdentifier);
            }
            else
            {
                existingBuilder.ToMerge.Add(builderIdentifier.Builder);
            }
        }

        return uniqueBuilders;
    }

    private class BuilderIdentifier : IEquatable<BuilderIdentifier>
    {
        public List<Type> IdentifierTypes { get; }

        public List<long> ParameterHashes { get; }

        public DestructibleSpawnerBuilder Builder { get; }

        public List<DestructibleSpawnerBuilder> ToMerge { get; } = new();

        public BuilderIdentifier(DestructibleSpawnerBuilder builder)
        {
            Builder = builder;

            var orderedIdentifiers = builder
                .Identifiers
                .OrderBy(x => x.GetType());

            IdentifierTypes = orderedIdentifiers
                .Select(x => x.Key)
                .ToList();

            ParameterHashes = orderedIdentifiers
                .Select(x => x.Value.GetParameterHash())
                .ToList();
        }

        public bool Equals(BuilderIdentifier other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.IdentifierTypes.Count != IdentifierTypes.Count)
            {
                return false;
            }

            for (int i = 0; i < IdentifierTypes.Count; ++i)
            {
                if (IdentifierTypes[i] != other.IdentifierTypes[i])
                {
                    return false;
                }
            }

            for (int i = 0; i < ParameterHashes.Count; ++i)
            {
                if (ParameterHashes[i] != other.ParameterHashes[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
