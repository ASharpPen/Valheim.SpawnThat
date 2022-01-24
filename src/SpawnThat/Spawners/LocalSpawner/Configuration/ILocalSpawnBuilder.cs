using System;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Spawners.LocalSpawner.Models;

namespace SpawnThat.Spawners.LocalSpawner.Configuration;

public interface ILocalSpawnBuilder
{
    ILocalSpawnBuilder SetEnabled(bool enabled = true);

    ILocalSpawnBuilder SetPrefabName(string prefabName);

    ILocalSpawnBuilder SetSpawnInterval(TimeSpan? frequency = null);

    ILocalSpawnBuilder SetMinLevel(int minLevel = 1);

    ILocalSpawnBuilder SetMaxLevel(int maxLevel = 1);

    ILocalSpawnBuilder SetLevelUpChance(float chance);

    ILocalSpawnBuilder SetSpawnAtNight(bool spawnAtNight = true);

    ILocalSpawnBuilder SetSpawnAtDay(bool spawnAtDay = true);

    ILocalSpawnBuilder SetSpawnInPlayerBase(bool spawnInPlayerBase = false);

    ILocalSpawnBuilder SetPatrolSpawn(bool patrolSpawn = false);

    /// <summary>
    /// <para>Minimum distance to player to spawn.</para>
    /// <para>Vanilla condition.</para>
    /// </summary>
    ILocalSpawnBuilder SetConditionPlayerDistance(float withinDistance);

    /// <summary>
    /// <para>Set spawners "hearing". Only spawn if a player is generating more noise than indicated 
    /// and is within ConditionPlayerDistance of the same distance.
    /// Noise also acts as a distance requirement.</para>
    /// <para>Eg., if 10, a player generating 5 noise will not trigger spawning no matter how close.</para>
    /// <para>Eg., if 10, a player generating 15 noise, must be within 15 distance to spawner.</para>
    /// <para>Vanilla condition.</para>
    /// </summary>
    /// <remarks>See https://github.com/ASharpPen/SpawnThat/wiki/field-options#noise </remarks>
    ILocalSpawnBuilder SetConditionPlayerNoise(float noise);

    ILocalSpawnBuilder AddCondition(ISpawnCondition condition);

    ILocalSpawnBuilder AddOrReplaceCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition;

    ILocalSpawnBuilder AddModifier(ISpawnModifier modifier);

    ILocalSpawnBuilder AddOrReplaceModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier;

    ILocalSpawnBuilder AddPostConfiguration(Action<LocalSpawnTemplate> configure);
}
