using System;
using System.Collections.Generic;
using System.ComponentModel;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;
using SpawnThat.Utilities.Extensions;
using YamlDotNet.Serialization;
using static Heightmap;

namespace SpawnThat.Spawners.WorldSpawner;

internal class WorldSpawnTemplate
{
    private string _prefabName;
    private int? _prefabHash;

    public WorldSpawnTemplate()
    {
    }

    public WorldSpawnTemplate(int index) : this()
    {
        Index = index;
    }

    public int Index { get; internal set; }

    public List<ISpawnCondition> SpawnConditions { get; set; } = new();

    public List<ISpawnPositionCondition> SpawnPositionConditions { get; set; } = new();

    public List<ISpawnModifier> SpawnModifiers { get; set; } = new();

    /// <summary>   
    /// <para>Prefab to override existing with.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    public string PrefabName
    {
        get => _prefabName;
        set
        {
            _prefabName = value;
            _prefabHash = null;
        }
    }

    [YamlIgnore]
    public int PrefabHash
    {
        get
        {
            if (_prefabHash is null && PrefabName is not null)
            {
                int hash = PrefabName.HashInteger();
                _prefabHash = hash;
                return hash;
            }

            return _prefabHash ?? -1;
        }
    }

    /// <summary>
    /// Toggles this spawner.
    /// If disabled, this spawn entry will not run.
    /// Can be used to disable existing spawn templates.
    /// </summary>
    [DefaultValue(true)]
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Toggles this configuration on / off.
    /// If disabled, template will be ignored.
    /// Cannot be used to disable existing spawn templates.
    /// </summary>
    [DefaultValue(true)]
    public bool TemplateEnabled { get; set; } = true;

    /// <summary>
    /// <para>Just a field for setting a custom name for the template. This is just for debugging.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// </summary>
    public string TemplateName { get; set; }

    /// <summary>
    /// <para>Bitmasked Biome in which template is active.</para>
    /// <para>Condition for biomes in which entity can spawn.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// </summary>
    public Biome? BiomeMask { get; set; }

    /// <summary>
    /// <para>Sets AI to hunt a player target.</para>
    /// <para>Vanilla modifier.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// </summary>
    public bool? ModifierHuntPlayer { get; set; }

    /// <summary>
    /// <para>Maximum number of prefab spawned in local surroundings.</para>
    /// <para>Vanilla condition.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// </summary>
    public int? MaxSpawned { get; set; }

    /// <summary>
    /// <para>Time between new spawn checks.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// </summary>
    public TimeSpan? SpawnInterval { get; set; }

    /// <summary>
    /// <para>Radius of circle, in which to spawn a pack of entities.</para>
    /// <para>Eg., when pack size is 3, all 3 spawns will happen inside a circle indicated by this radius.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// </summary>
    public float? PackSpawnCircleRadius { get; set; }

    public int? PackSizeMin { get; set; }

    public int? PackSizeMax { get; set; }

    public bool? ConditionAllowInForest { get; set; }

    public bool? ConditionAllowOutsideForest { get; set; }

    public float? DistanceToCenterForLevelUp { get; set; }

    public int? MinLevel { get; set; }

    public int? MaxLevel { get; set; }

    public float? LevelUpChance { get; set; }

    public float? ConditionMinAltitude { get; set; }

    public float? ConditionMaxAltitude { get; set; }

    public float? ConditionMinOceanDepth { get; set; }

    public float? ConditionMaxOceanDepth { get; set; }

    public float? ConditionMinTilt { get; set; }

    public float? ConditionMaxTilt { get; set; }

    public List<string> ConditionEnvironments { get; set; }

    public string ConditionRequiredGlobalKey { get; set; }

    public bool? ConditionAllowDuringDay { get; set; }

    public bool? ConditionAllowDuringNight { get; set; }

    public float? SpawnChance { get; set; }

    public float? MinDistanceToOther { get; set; }

    public float? SpawnAtDistanceToPlayerMin { get; set; }

    public float? SpawnAtDistanceToPlayerMax { get; set; }

    public float? SpawnAtDistanceToGround { get; set; }
}
