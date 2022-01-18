using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawn.Conditions;
using Valheim.SpawnThat.Spawn.Modifiers;
using Valheim.SpawnThat.Spawn.PositionConditions;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.Spawners.WorldSpawner;

internal class WorldSpawnBuilder : IWorldSpawnBuilder
{
    public int Id { get; }

    private WorldSpawnTemplate Template { get; }

    private List<Action<WorldSpawnTemplate>> PostConfigurations { get; } = new();

    public bool Finalized { get; private set; } = false;

    public WorldSpawnBuilder(int id)
    {
        Id = id;

        Template = new WorldSpawnTemplate(id);

        // Set all biomes allowed by default.
        Template.BiomeMask = (Heightmap.Biome)1023;
    }

    public WorldSpawnTemplate Build()
    {
#if DEBUG
        Log.LogTrace($"Building WorlSpawnBuilder [{Id}:{Template.PrefabName}]");
#endif


        if (Finalized)
        {
            throw new InvalidOperationException("Builder has already been finalized.");
        }

        Finalized = true;

        foreach (var configure in PostConfigurations)
        {
            try
            {
                configure(Template);
            }
            catch (Exception e)
            {
                Log.LogError($"Error during post configuration of world spawn template '{Template.TemplateName}'. Template may be misconfigured", e);
            }
        }

        return Template;
    }

    public IWorldSpawnBuilder AddCondition(ISpawnCondition condition)
    {
        Template.SpawnConditions.AddNullSafe(condition);
        return this;
    }

    public IWorldSpawnBuilder AddOrReplaceCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition
    {
        Template.SpawnConditions.AddOrReplaceByType(condition);
        return this;
    }

    public IWorldSpawnBuilder AddPositionCondition(ISpawnPositionCondition condition)
    {
        Template.SpawnPositionConditions.AddNullSafe(condition);
        return this;
    }

    public IWorldSpawnBuilder AddOrReplacePositionCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnPositionCondition
    {
        Template.SpawnPositionConditions.AddOrReplaceByType(condition);
        return this;
    }

    public IWorldSpawnBuilder AddModifier(ISpawnModifier modifier)
    {
        Template.SpawnModifiers.AddNullSafe(modifier);
        return this;
    }

    public IWorldSpawnBuilder AddOrReplaceModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier
    {
        Template.SpawnModifiers.AddOrReplaceByType(modifier);
        return this;
    }

    public IWorldSpawnBuilder AddPostConfiguration(Action<WorldSpawnTemplate> configure)
    {
        PostConfigurations.Add(configure);
        return this;
    }

    public IWorldSpawnBuilder SetConditionAllowDuringDay(bool allowSpawnDuringDay)
    {
        Template.ConditionAllowDuringDay = allowSpawnDuringDay;
        return this;
    }

    public IWorldSpawnBuilder SetConditionAllowDuringNight(bool allowSpawnDuringNight)
    {
        Template.ConditionAllowDuringNight = allowSpawnDuringNight;
        return this;
    }

    public IWorldSpawnBuilder SetConditionAllowInForest(bool allowSpawnInForest)
    {
        Template.ConditionAllowInForest = allowSpawnInForest;
        return this;
    }

    public IWorldSpawnBuilder SetConditionAllowOutsideForest(bool allowSpawnOutsideForest)
    {
        Template.ConditionAllowOutsideForest = allowSpawnOutsideForest;
        return this;
    }

    public IWorldSpawnBuilder SetConditionAltitude(float? minAltitude, float? maxAltitude)
    {
        Template.ConditionMinAltitude = minAltitude;
        Template.ConditionMaxAltitude = maxAltitude;
        return this;
    }

    public IWorldSpawnBuilder SetConditionBiomes(IEnumerable<Heightmap.Biome> biomes)
    {
        var biomeMask = Heightmap.Biome.None;

        foreach(var biome in biomes)
        {
            biomeMask |= biome;
        }

        Template.BiomeMask = biomeMask;

        return this;
    }

    public IWorldSpawnBuilder SetConditionBiomes(params Heightmap.Biome[] biomes)
    {
        var biomeMask = Heightmap.Biome.None;

        foreach (var biome in biomes)
        {
            biomeMask |= biome;
        }

        Template.BiomeMask = biomeMask;

        return this;
    }

    public IWorldSpawnBuilder SetConditionBiomes(IEnumerable<string> biomeNames)
    {
        var biomeMask = Heightmap.Biome.None;

        foreach (var biomeName in biomeNames)
        {
            if (Enum.TryParse(biomeName, true, out Heightmap.Biome biome))
            {
                biomeMask |= biome;
            }
            else
            {
                Log.LogWarning($"Unable to parse biome '{biomeName}' during '{nameof(SetConditionBiomes)}' setup of world spanwer {Template.TemplateName}");
            }
        }

        Template.BiomeMask = biomeMask;

        return this;
    }

    public IWorldSpawnBuilder SetConditionBiomes(params string[] biomeNames)
    {
        var biomeMask = Heightmap.Biome.None;

        foreach (var biomeName in biomeNames)
        {
            if (Enum.TryParse(biomeName, true, out Heightmap.Biome biome))
            {
                biomeMask |= biome;
            }
            else
            {
                Log.LogWarning($"Unable to parse biome '{biomeName}' during '{nameof(SetConditionBiomes)}' setup of world spanwer {Template.TemplateName}");
            }
        }

        Template.BiomeMask = biomeMask;

        return this;
    }

    public IWorldSpawnBuilder SetConditionBiomesAll()
    {
        Template.BiomeMask = (Heightmap.Biome)1023;

        return this;
    }

    public IWorldSpawnBuilder SetConditionEnvironments(IEnumerable<string> environmentNames)
    {
        Template.ConditionEnvironments = environmentNames.ToList();
        return this;
    }

    public IWorldSpawnBuilder SetConditionEnvironments(params string[] environmentNames)
    {
        Template.ConditionEnvironments = environmentNames.ToList();
        return this;
    }

    public IWorldSpawnBuilder SetConditionOceanDepth(float? minOceanDepth, float? maxOceanDepth)
    {
        Template.ConditionMinOceanDepth = minOceanDepth;
        Template.ConditionMaxOceanDepth = maxOceanDepth;

        return this;
    }

    public IWorldSpawnBuilder SetConditionRequiredGlobalKey(string globalKey)
    {
        Template.ConditionRequiredGlobalKey = globalKey;
        return this;
    }

    public IWorldSpawnBuilder SetConditionTilt(float? minTilt, float? maxTilt)
    {
        Template.ConditionMinTilt = minTilt;
        Template.ConditionMaxTilt = maxTilt;
        return this;
    }

    public IWorldSpawnBuilder SetEnabled(bool enabled)
    {
        Template.Enabled = enabled;
        return this;
    }

    public IWorldSpawnBuilder SetTemplateEnabled(bool enabled)
    {
        Template.TemplateEnabled = enabled;
        return this;
    }

    public IWorldSpawnBuilder SetLevelUpDistance(float distance)
    {
        Template.LevelUpDistance = distance;
        return this;
    }

    public IWorldSpawnBuilder SetMaxLevel(uint maxLevel)
    {
        Template.MaxLevel = (int)maxLevel;
        return this;
    }

    public IWorldSpawnBuilder SetMaxSpawned(uint maxSpawned)
    {
        Template.MaxSpawned = (int)maxSpawned;
        return this;
    }

    public IWorldSpawnBuilder SetMinDistanceToOther(float distance)
    {
        Template.MinDistanceToOther = distance;
        return this;
    }

    public IWorldSpawnBuilder SetMinLevel(uint minLevel)
    {
        Template.MinLevel = (int)minLevel;
        return this;
    }

    public IWorldSpawnBuilder SetModifierHuntPlayer(bool huntPlayer)
    {
        Template.ModifierHuntPlayer = huntPlayer;
        return this;
    }

    public IWorldSpawnBuilder SetPackSizeMax(uint packSizeMax)
    {
        Template.PackSizeMax = (int)packSizeMax;
        return this;
    }

    public IWorldSpawnBuilder SetPackSizeMin(uint packSizeMin)
    {
        Template.PackSizeMin = (int)packSizeMin;
        return this;
    }

    public IWorldSpawnBuilder SetPackSpawnCircleRadius(float radius)
    {
        Template.PackSpawnCircleRadius = radius;
        return this;
    }

    public IWorldSpawnBuilder SetPrefabName(string prefabName)
    {
        Template.PrefabName = prefabName;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnAtDistanceToGround(float? offset)
    {
        Template.SpawnAtDistanceToGround = offset;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMax(float? distance)
    {
        Template.SpawnAtDistanceToPlayerMax = distance;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMin(float? distance)
    {
        Template.SpawnAtDistanceToPlayerMin = distance;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnChance(float spawnChance)
    {
        Template.SpawnChance = spawnChance;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnInterval(TimeSpan interval)
    {
        Template.SpawnInterval = interval;
        return this;
    }

    public IWorldSpawnBuilder SetTemplateName(string templateName)
    {
        Template.TemplateName = templateName;
        return this;
    }
}