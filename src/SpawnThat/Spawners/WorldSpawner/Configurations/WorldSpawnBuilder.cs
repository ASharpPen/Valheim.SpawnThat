using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;
using SpawnThat.Utilities.Enums;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners.WorldSpawner.Configurations;

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

    public IWorldSpawnBuilder SetCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition
    {
        Template.SpawnConditions.AddOrReplaceByType(condition);
        return this;
    }

    public IWorldSpawnBuilder SetPositionCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnPositionCondition
    {
        Template.SpawnPositionConditions.AddOrReplaceByType(condition);
        return this;
    }

    public IWorldSpawnBuilder SetModifier<TModifier>(TModifier modifier)
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

    public IWorldSpawnBuilder SetSpawnDuringDay(bool allowSpawnDuringDay)
    {
        Template.ConditionAllowDuringDay = allowSpawnDuringDay;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnDuringDay(bool? allowSpawnDuringDay)
    {
        Template.ConditionAllowDuringDay = allowSpawnDuringDay;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnDuringNight(bool allowSpawnDuringNight)
    {
        Template.ConditionAllowDuringNight = allowSpawnDuringNight;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnDuringNight(bool? allowSpawnDuringNight)
    {
        Template.ConditionAllowDuringNight = allowSpawnDuringNight;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnInForest(bool allowSpawnInForest)
    {
        Template.ConditionAllowInForest = allowSpawnInForest;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnInForest(bool? allowSpawnInForest)
    {
        Template.ConditionAllowInForest = allowSpawnInForest;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnOutsideForest(bool allowSpawnOutsideForest)
    {
        Template.ConditionAllowOutsideForest = allowSpawnOutsideForest;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnOutsideForest(bool? allowSpawnOutsideForest)
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

    public IWorldSpawnBuilder SetConditionAltitudeMin(float minAltitude)
    {
        Template.ConditionMinAltitude = minAltitude;
        return this;
    }

    public IWorldSpawnBuilder SetConditionAltitudeMin(float? minAltitude)
    {
        Template.ConditionMinAltitude = minAltitude;
        return this;
    }

    public IWorldSpawnBuilder SetConditionAltitudeMax(float maxAltitude)
    {
        Template.ConditionMaxAltitude = maxAltitude;
        return this;
    }

    public IWorldSpawnBuilder SetConditionAltitudeMax(float? maxAltitude)
    {
        Template.ConditionMaxAltitude = maxAltitude;
        return this;
    }

    public IWorldSpawnBuilder SetConditionBiomes(IEnumerable<Heightmap.Biome> biomes)
    {
        if (biomes is null)
        {
            Template.BiomeMask = null;
            return this;
        }

        var biomeMask = Heightmap.Biome.None;

        if (biomes is not null)
        {
            foreach (var biome in biomes)
            {
                biomeMask |= biome;
            }
        }

        Template.BiomeMask = biomeMask;

        return this;
    }

    public IWorldSpawnBuilder SetConditionBiomes(params Heightmap.Biome[] biomes)
    {
        if (biomes is null)
        {
            Template.BiomeMask = null;
            return this;
        }

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
        if (biomeNames is null)
        {
            Template.BiomeMask = null;
            return this;
        }

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
        if (biomeNames is null)
        {
            Template.BiomeMask = null;
            return this;
        }

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
        Template.ConditionEnvironments = environmentNames?.ToList() ?? new(); ;
        return this;
    }

    public IWorldSpawnBuilder SetConditionEnvironments(params string[] environmentNames)
    {
        Template.ConditionEnvironments = environmentNames?.ToList() ?? new();
        return this;
    }

    public IWorldSpawnBuilder SetConditionEnvironments(params EnvironmentName[] environmentNames)
    {
        Template.ConditionEnvironments = environmentNames?.Select(x => x.GetName()).ToList() ?? new();
        return this;
    }

    public IWorldSpawnBuilder SetConditionOceanDepth(float? minOceanDepth, float? maxOceanDepth)
    {
        Template.ConditionMinOceanDepth = minOceanDepth;
        Template.ConditionMaxOceanDepth = maxOceanDepth;
        return this;
    }

    public IWorldSpawnBuilder SetConditionOceanDepthMin(float minOceanDepth)
    {
        Template.ConditionMinOceanDepth = minOceanDepth;
        return this;
    }

    public IWorldSpawnBuilder SetConditionOceanDepthMin(float? minOceanDepth)
    {
        Template.ConditionMinOceanDepth = minOceanDepth;
        return this;
    }

    public IWorldSpawnBuilder SetConditionOceanDepthMax(float maxOceanDepth)
    {
        Template.ConditionMaxOceanDepth = maxOceanDepth;
        return this;
    }

    public IWorldSpawnBuilder SetConditionOceanDepthMax(float? maxOceanDepth)
    {
        Template.ConditionMaxOceanDepth = maxOceanDepth;
        return this;
    }

    public IWorldSpawnBuilder SetConditionRequiredGlobalKey(string globalKey)
    {
        Template.ConditionRequiredGlobalKey = globalKey;
        return this;
    }

    public IWorldSpawnBuilder SetConditionRequiredGlobalKey(GlobalKey globalKey)
    {
        Template.ConditionRequiredGlobalKey = globalKey.GetName();
        return this;
    }

    public IWorldSpawnBuilder SetConditionRequiredGlobalKey(GlobalKey? globalKey)
    {
        Template.ConditionRequiredGlobalKey = globalKey is null ? string.Empty : globalKey.Value.GetName();
        return this;
    }

    public IWorldSpawnBuilder SetConditionTilt(float? minTilt, float? maxTilt)
    {
        Template.ConditionMinTilt = minTilt;
        Template.ConditionMaxTilt = maxTilt;
        return this;
    }

    public IWorldSpawnBuilder SetConditionTiltMin(float minTilt)
    {
        Template.ConditionMinTilt = minTilt;
        return this;
    }

    public IWorldSpawnBuilder SetConditionTiltMin(float? minTilt)
    {
        Template.ConditionMinTilt = minTilt;
        return this;
    }

    public IWorldSpawnBuilder SetConditionTiltMax(float maxTilt)
    {
        Template.ConditionMaxTilt = maxTilt;
        return this;
    }

    public IWorldSpawnBuilder SetConditionTiltMax(float? maxTilt)
    {
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

    public IWorldSpawnBuilder SetDistanceToCenterForLevelUp(float distance)
    {
        Template.DistanceToCenterForLevelUp = distance;
        return this;
    }

    public IWorldSpawnBuilder SetDistanceToCenterForLevelUp(float? distance)
    {
        Template.DistanceToCenterForLevelUp = distance;
        return this;
    }

    public IWorldSpawnBuilder SetMaxLevel(uint maxLevel)
    {
        Template.MaxLevel = (int?)maxLevel;
        return this;
    }

    public IWorldSpawnBuilder SetMaxLevel(uint? maxLevel)
    {
        Template.MaxLevel = (int?)maxLevel;
        return this;
    }

    public IWorldSpawnBuilder SetLevelUpChance(float? chance)
    {
        Template.LevelUpChance = chance;
        return this;
    }

    public IWorldSpawnBuilder SetMaxSpawned(uint maxSpawned)
    {
        Template.MaxSpawned = (int?)maxSpawned;
        return this;
    }

    public IWorldSpawnBuilder SetMaxSpawned(uint? maxSpawned)
    {
        Template.MaxSpawned = (int?)maxSpawned;
        return this;
    }

    public IWorldSpawnBuilder SetMinDistanceToOther(float distance)
    {
        Template.MinDistanceToOther = distance;
        return this;
    }

    public IWorldSpawnBuilder SetMinDistanceToOther(float? distance)
    {
        Template.MinDistanceToOther = distance;
        return this;
    }

    public IWorldSpawnBuilder SetMinLevel(uint minLevel)
    {
        Template.MinLevel = (int)minLevel;
        return this;
    }

    public IWorldSpawnBuilder SetMinLevel(uint? minLevel)
    {
        Template.MinLevel = (int?)minLevel;
        return this;
    }

    public IWorldSpawnBuilder SetModifierHuntPlayer(bool huntPlayer)
    {
        Template.ModifierHuntPlayer = huntPlayer;
        return this;
    }

    public IWorldSpawnBuilder SetModifierHuntPlayer(bool? huntPlayer)
    {
        Template.ModifierHuntPlayer = huntPlayer;
        return this;
    }

    public IWorldSpawnBuilder SetPackSizeMax(uint packSizeMax)
    {
        Template.PackSizeMax = (int)packSizeMax;
        return this;
    }

    public IWorldSpawnBuilder SetPackSizeMax(uint? packSizeMax)
    {
        Template.PackSizeMax = (int?)packSizeMax;
        return this;
    }

    public IWorldSpawnBuilder SetPackSizeMin(uint packSizeMin)
    {
        Template.PackSizeMin = (int?)packSizeMin;
        return this;
    }

    public IWorldSpawnBuilder SetPackSizeMin(uint? packSizeMin)
    {
        Template.PackSizeMin = (int?)packSizeMin;
        return this;
    }

    public IWorldSpawnBuilder SetPackSpawnCircleRadius(float radius)
    {
        Template.PackSpawnCircleRadius = radius;
        return this;
    }

    public IWorldSpawnBuilder SetPackSpawnCircleRadius(float? radius)
    {
        Template.PackSpawnCircleRadius = radius;
        return this;
    }

    public IWorldSpawnBuilder SetPrefabName(string prefabName)
    {
        Template.PrefabName = prefabName;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnAtDistanceToGround(float offset)
    {
        Template.SpawnAtDistanceToGround = offset;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnAtDistanceToGround(float? offset)
    {
        Template.SpawnAtDistanceToGround = offset;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMax(float distance)
    {
        Template.SpawnAtDistanceToPlayerMax = distance;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMax(float? distance)
    {
        Template.SpawnAtDistanceToPlayerMax = distance;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMin(float distance)
    {
        Template.SpawnAtDistanceToPlayerMin = distance;
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

    public IWorldSpawnBuilder SetSpawnChance(float? spawnChance)
    {
        Template.SpawnChance = spawnChance;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnInterval(TimeSpan interval)
    {
        Template.SpawnInterval = interval;
        return this;
    }

    public IWorldSpawnBuilder SetSpawnInterval(TimeSpan? interval)
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