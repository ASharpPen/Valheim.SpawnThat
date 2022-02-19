using System;
using System.Collections.Generic;
using SpawnThat.Options.Identifiers;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration;

internal class DestructibleSpawnerBuilder : IDestructibleSpawnerBuilder
{
    private DestructibleSpawnerTemplate Template { get; } = new();

    private Dictionary<uint, IDestructibleSpawnBuilder> Spawns { get; } = new();

    private Dictionary<Type, ISpawnerIdentifier> Identifiers { get; } = new();

    public IDestructibleSpawnBuilder GetSpawnBuilder(uint id)
    {
        if (Spawns.TryGetValue(id, out var cached))
        {
            return cached;
        }

        return Spawns[id] = new DestructibleSpawnBuilder(id);
    }

    public IDestructibleSpawnerBuilder SetConditionMaxCloseCreatures(int? maxCloseCreatures)
    {
        Template.ConditionMaxCloseCreatures = maxCloseCreatures;
        return this;
    }

    public IDestructibleSpawnerBuilder SetConditionMaxCreatures(int? maxCreatures)
    {
        Template.ConditionMaxCreatures = maxCreatures;
        return this;
    }

    public IDestructibleSpawnerBuilder SetConditionPlayerWithinDistance(float? withinDistance)
    {
        Template.ConditionPlayerWithinDistance = withinDistance;
        return this;
    }

    public IDestructibleSpawnerBuilder SetDistanceConsideredClose(float? distance)
    {
        Template.DistanceConsideredClose = distance;
        return this;
    }

    public IDestructibleSpawnerBuilder SetDistanceConsideredFar(float? distance)
    {
        Template.DistanceConsideredFar = distance;
        return this;
    }

    public void SetIdentifier<T>(T identifier)
         where T : class, ISpawnerIdentifier
    {
        Identifiers[identifier.GetType()] = identifier;
    }

    public IDestructibleSpawnerBuilder SetLevelUpChance(float? levelUpChance)
    {
        Template.LevelUpChance = levelUpChance;
        return this;
    }

    public IDestructibleSpawnerBuilder SetOnGroundOnly(bool? checkGround)
    {
        Template.OnGroundOnly = checkGround;
        return this;
    }

    public IDestructibleSpawnerBuilder SetPatrol(bool? patrolSpawn)
    {
        Template.SetPatrol = patrolSpawn;
        return this;
    }

    public IDestructibleSpawnerBuilder SetSpawnInterval(TimeSpan? interval)
    {
        Template.SpawnInterval = interval;
        return this;
    }
}
