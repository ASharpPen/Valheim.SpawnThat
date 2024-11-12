using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Options.Identifiers;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Configuration;

internal class SpawnAreaSpawnerBuilder : ISpawnAreaSpawnerBuilder
{
    private SpawnAreaSpawnerTemplate Template { get; } = new();

    internal Dictionary<uint, SpawnAreaSpawnBuilder> Spawns { get; } = new();

    internal Dictionary<Type, ISpawnerIdentifier> Identifiers { get; } = new();

    private BuilderOption<string> TemplateName;
    private BuilderOption<float?> SpawnRadius;
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

    public ISpawnAreaSpawnBuilder GetSpawnBuilder(uint id)
    {
        if (Spawns.TryGetValue(id, out var cached))
        {
            return cached;
        }

        return Spawns[id] = new SpawnAreaSpawnBuilder(id);
    }

    internal SpawnAreaSpawnerTemplate Build()
    {
        TemplateName.DoIfSet(x => Template.TemplateName = x);
        SpawnRadius.DoIfSet(x => Template.SpawnRadius = x);
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

    internal void Merge(SpawnAreaSpawnerBuilder builder)
    {
        builder.TemplateName.AssignIfSet(ref TemplateName);
        builder.ConditionMaxCloseCreatures.AssignIfSet(ref ConditionMaxCloseCreatures);
        builder.SpawnRadius.AssignIfSet(ref SpawnRadius);
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

    public ISpawnAreaSpawnerBuilder SetTemplateName(string templateName)
    {
        TemplateName = new(templateName);
        return this;
    }

    public ISpawnAreaSpawnerBuilder SetConditionMaxCloseCreatures(int? maxCloseCreatures)
    {
        ConditionMaxCloseCreatures = new(maxCloseCreatures);
        return this;
    }

    public ISpawnAreaSpawnerBuilder SetConditionMaxCreatures(int? maxCreatures)
    {
        ConditionMaxCreatures = new(maxCreatures);
        return this;
    }

    public ISpawnAreaSpawnerBuilder SetConditionPlayerWithinDistance(float? withinDistance)
    {
        ConditionPlayerWithinDistance = new(withinDistance);
        return this;
    }

    public ISpawnAreaSpawnerBuilder SetSpawnRadius(float? spawnRadius)
    {
        SpawnRadius = new(spawnRadius);
        return this;
    }

    public ISpawnAreaSpawnerBuilder SetDistanceConsideredClose(float? distance)
    {
        DistanceConsideredClose= new(distance);
        return this;
    }

    public ISpawnAreaSpawnerBuilder SetDistanceConsideredFar(float? distance)
    {
        DistanceConsideredFar = new(distance);
        return this;
    }

    public void SetIdentifier<T>(T identifier)
         where T : class, ISpawnerIdentifier
    {
        Identifiers[identifier.GetType()] = identifier;
    }

    public ISpawnAreaSpawnerBuilder SetLevelUpChance(float? levelUpChance)
    {
        LevelUpChance = new(levelUpChance);
        return this;
    }

    public ISpawnAreaSpawnerBuilder SetOnGroundOnly(bool? checkGround)
    {
        OnGroundOnly = new(checkGround);
        return this;
    }

    public ISpawnAreaSpawnerBuilder SetPatrol(bool? patrolSpawn)
    {
        Patrol = new(patrolSpawn);
        return this;
    }

    public ISpawnAreaSpawnerBuilder SetSpawnInterval(TimeSpan? interval)
    {
        SpawnInterval = new(interval);
        return this;
    }

    public ISpawnAreaSpawnerBuilder SetRemoveNotConfiguredSpawns(bool removeNotConfigured)
    {
        RemoveNotConfiguredSpawns = new(removeNotConfigured);
        return this;
    }
}
