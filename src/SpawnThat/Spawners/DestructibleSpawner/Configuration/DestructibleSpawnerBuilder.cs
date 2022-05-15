using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Options.Identifiers;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration;

internal class DestructibleSpawnerBuilder : IDestructibleSpawnerBuilder
{
    private DestructibleSpawnerTemplate Template { get; } = new();

    internal Dictionary<uint, DestructibleSpawnBuilder> Spawns { get; } = new();

    internal Dictionary<Type, ISpawnerIdentifier> Identifiers { get; } = new();

    private BuilderOption<string> TemplateName;
    private BuilderOption<int?> ConditionMaxCloseCreatures;
    private BuilderOption<int?> ConditionMaxCreatures;
    private BuilderOption<float?> ConditionPlayerWithinDistance;
    private BuilderOption<float?> DistanceConsideredClose;
    private BuilderOption<float?> DistanceConsideredFar;
    private BuilderOption<float?> LevelUpChance;
    private BuilderOption<bool?> OnGroundOnly;
    private BuilderOption<bool?> Patrol;
    private BuilderOption<TimeSpan?> SpawnInterval;
    private BuilderOption<bool> RemoveNotConfiguredSpawns;

    public IDestructibleSpawnBuilder GetSpawnBuilder(uint id)
    {
        if (Spawns.TryGetValue(id, out var cached))
        {
            return cached;
        }

        return Spawns[id] = new DestructibleSpawnBuilder(id);
    }

    internal DestructibleSpawnerTemplate Build()
    {
        TemplateName.DoIfSet(x => Template.TemplateName = x);
        ConditionMaxCloseCreatures.DoIfSet(x => Template.ConditionMaxCloseCreatures = x);
        ConditionMaxCreatures.DoIfSet(x => Template.ConditionMaxCreatures = x);
        ConditionPlayerWithinDistance.DoIfSet(x => Template.ConditionPlayerWithinDistance = x);
        DistanceConsideredClose.DoIfSet(x => Template.DistanceConsideredClose = x);
        DistanceConsideredFar.DoIfSet(x => Template.DistanceConsideredFar = x);
        LevelUpChance.DoIfSet(x => Template.LevelUpChance = x);
        OnGroundOnly.DoIfSet(x => Template.OnGroundOnly = x);
        Patrol.DoIfSet(x => Template.SetPatrol = x);
        SpawnInterval.DoIfSet(x => Template.SpawnInterval = x);
        RemoveNotConfiguredSpawns.DoIfSet(x => Template.RemoveNotConfiguredSpawns = x);

        foreach (var spawnBuilder in Spawns)
        {
            Template.Spawns[spawnBuilder.Key] = spawnBuilder.Value.Build();
        }

        Template.Identifiers = Identifiers.Values.ToList();

        return Template;
    }

    internal void Merge(DestructibleSpawnerBuilder builder)
    {
        builder.TemplateName.AssignIfSet(ref TemplateName);
        builder.ConditionMaxCloseCreatures.AssignIfSet(ref ConditionMaxCloseCreatures);
        builder.ConditionMaxCreatures.AssignIfSet(ref ConditionMaxCreatures);
        builder.ConditionPlayerWithinDistance.AssignIfSet(ref ConditionPlayerWithinDistance);
        builder.DistanceConsideredClose.AssignIfSet(ref DistanceConsideredClose);
        builder.DistanceConsideredFar.AssignIfSet(ref DistanceConsideredFar);
        builder.LevelUpChance.AssignIfSet(ref LevelUpChance);
        builder.OnGroundOnly.AssignIfSet(ref OnGroundOnly);
        builder.Patrol.AssignIfSet(ref Patrol);
        builder.SpawnInterval.AssignIfSet(ref SpawnInterval);
        builder.RemoveNotConfiguredSpawns.AssignIfSet(ref RemoveNotConfiguredSpawns);

        foreach (var spawn in builder.Spawns)
        {
            if (Spawns.TryGetValue(spawn.Key, out var existing))
            {
                existing.Merge(spawn.Value);
            }
            else
            {
                Spawns[spawn.Key] = spawn.Value;
            }
        }
    }

    public IDestructibleSpawnerBuilder SetTemplateName(string templateName)
    {
        TemplateName = new(templateName);
        return this;
    }

    public IDestructibleSpawnerBuilder SetConditionMaxCloseCreatures(int? maxCloseCreatures)
    {
        ConditionMaxCloseCreatures = new(maxCloseCreatures);
        return this;
    }

    public IDestructibleSpawnerBuilder SetConditionMaxCreatures(int? maxCreatures)
    {
        ConditionMaxCreatures = new(maxCreatures);
        return this;
    }

    public IDestructibleSpawnerBuilder SetConditionPlayerWithinDistance(float? withinDistance)
    {
        ConditionPlayerWithinDistance = new(withinDistance);
        return this;
    }

    public IDestructibleSpawnerBuilder SetDistanceConsideredClose(float? distance)
    {
        DistanceConsideredClose= new(distance);
        return this;
    }

    public IDestructibleSpawnerBuilder SetDistanceConsideredFar(float? distance)
    {
        DistanceConsideredFar = new(distance);
        return this;
    }

    public void SetIdentifier<T>(T identifier)
         where T : class, ISpawnerIdentifier
    {
        Identifiers[identifier.GetType()] = identifier;
    }

    public IDestructibleSpawnerBuilder SetLevelUpChance(float? levelUpChance)
    {
        LevelUpChance = new(levelUpChance);
        return this;
    }

    public IDestructibleSpawnerBuilder SetOnGroundOnly(bool? checkGround)
    {
        OnGroundOnly = new(checkGround);
        return this;
    }

    public IDestructibleSpawnerBuilder SetPatrol(bool? patrolSpawn)
    {
        Patrol = new(patrolSpawn);
        return this;
    }

    public IDestructibleSpawnerBuilder SetSpawnInterval(TimeSpan? interval)
    {
        SpawnInterval = new(interval);
        return this;
    }

    public IDestructibleSpawnerBuilder SetRemoveNotConfiguredSpawns(bool removeNotConfigured)
    {
        RemoveNotConfiguredSpawns = new(removeNotConfigured);
        return this;
    }
}
