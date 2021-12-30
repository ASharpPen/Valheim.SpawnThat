using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Heightmap;

namespace Valheim.SpawnThat.Spawners.WorldSpawner;

public class WorldSpawnTemplate
{
    private string _prefabName;
    private int? _prefabHash;

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

    public int PrefabHash
    {
        get => _prefabHash ?? _prefabName.GetStableHashCode();
    }

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

    public float? PackSizeMin { get; set; }

    public float? PackSizeMax { get; set; }

    public bool? ConditionAllowInForest { get; set; }

    public bool? ConditionAllowOutsideForest { get; set; }

    public float? LevelUpDistance { get; set; }

    public float? MinLevel { get; set; }

    public float? MaxLevel { get; set; }

    //public float? LevelUpChance { get; set; }

    public float? ConditionMinAltitude { get; set; }

    public float? ConditionMaxAltitude { get; set; }

    public float? ConditionMinOceanDepth { get; set; }

    public float? ConditionMaxOceanDepth { get; set; }

    public float? ConditionMinTilt { get; set; }

    public float? ConditionMaxTilt { get; set; }

    public List<string>? ConditionEnvironments { get; set; }

    public List<string>? ConditionRequiredGlobalKeys { get; set; }

    public bool? ConditionAllowDuringDay { get; set; }

    public bool? ConditionAllowDuringNight { get; set; }

    public float? SpawnChance { get; set; }

    public float? MinDistanceToOther { get; set; }

    public float? SpawnAtDistanceToPlayerMin { get; set; }

    public float? SpawnAtDistanceToPlayerMax { get; set; }

    public float? SpawnAtDistanceToGround { get; set; }
}
