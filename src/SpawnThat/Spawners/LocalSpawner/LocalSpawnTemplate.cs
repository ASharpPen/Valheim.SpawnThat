using System;
using System.Collections.Generic;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using YamlDotNet.Serialization;

namespace SpawnThat.Spawners.LocalSpawner.Models;

public class LocalSpawnTemplate
{
    private string _prefabName;
    private int? _prefabHash;

    /// <summary>
    /// Toggles this template on / off. Does not disable the spawner itself.
    /// </summary>
    public bool Enabled { get; set; } = true;

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
                int hash = PrefabName.GetStableHashCode();
                _prefabHash = hash;
                return hash;
            }

            return _prefabHash ?? -1;
        }
    }

    /// <summary>
    /// <para>Sets respawn frequency. To only spawn once, set to TimeSpan.Zero.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    public TimeSpan? SpawnInterval { get; set; }

    /// <summary>
    /// <para>Sets minimum level of spawn. Values below 1 are ignored.</para>
    /// <para>Vanilla modifier.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    public int? MinLevel { get; set; }

    /// <summary>
    /// <para>Sets maximum level of spawn. Values below 1 or MinLevel are ignored.</para>
    /// <para>Vanilla modifier.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    public int? MaxLevel { get; set; }

    /// <summary>
    /// <para>Allows spawning during day.</para>
    /// <para>Vanilla condition.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    public bool? SpawnAtDay { get; set; } = true;

    /// <summary>
    /// <para>Allows spawning during night.</para>
    /// <para>Vanilla condition.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    public bool? SpawnAtNight { get; set; } = true;

    /// <summary>
    /// <para>Allows spawning if within usual player base protected areas, such as workbench.</para>
    /// <para>Vanilla condition.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    public bool? AllowSpawnInPlayerBase { get; set; }

    /// <summary>
    /// <para>Sets patrol point at spawn position. Creatures will run back to this point.</para>
    /// <para>Vanilla modifier.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    public bool? SetPatrolSpawn { get; set; }

    /// <summary>
    /// <para>Chance to level up from MinLevel. Levels are gained one by one, 
    /// by rolling this chance until failed or MaxLevel is reached.</para>
    /// <para>Vanilla modifier.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    public float? LevelUpChance { get; set; }

    /// <summary>
    /// <para>Minimum distance to player to spawn.</para>
    /// <para>Vanilla condition.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    public float? ConditionPlayerDistance { get; set; }

    /// <summary>
    /// <para>Set spawners "hearing". Only spawn if a player is generating more noise than indicated 
    /// and is within ConditionPlayerDistance of the same distance.
    /// Noise also acts as a distance requirement.</para>
    /// <para>Eg., if 10, a player generating 5 noise will not trigger spawning no matter how close.</para>
    /// <para>Eg., if 10, a player generating 15 noise, must be within 15 distance to spawner.</para>
    /// <para>Vanilla condition.</para>
    /// <para>If null, uses existing.</para>
    /// </summary>
    /// <remarks>See https://github.com/ASharpPen/SpawnThat/wiki/field-options#noise </remarks>
    public float? ConditionPlayerNoise { get; set; }

    /// <summary>
    /// Set of conditions which must be all valid, for local spawner to spawn its prefab.
    /// </summary>
    public List<ISpawnCondition> SpawnConditions { get; set; } = new();

    /// <summary>
    /// Set of modifiers which will be run/applied on spawned entity.
    /// </summary>
    public List<ISpawnModifier> Modifiers { get; set; } = new();
}
