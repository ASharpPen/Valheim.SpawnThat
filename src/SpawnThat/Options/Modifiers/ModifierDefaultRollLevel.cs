using System;
using UnityEngine;
using SpawnThat.Caches;

namespace SpawnThat.Options.Modifiers;

/// <summary>
/// <para>
/// Simulates the vanilla level assignment logic.
///</para>
/// <para>
/// Level is assigned by setting level to Min(1, <c>minLevel</c>).
/// </para>
/// <para>
/// If <c>minDistanceForLevelups</c> is greater than distance to center,
/// and <c>maxLevel</c> is greater than level, the <c>levelupChance</c> is rolled.
/// This is repeated until either <c>maxLevel</c> is reached, or a levelup roll fails.
/// </para>
/// </summary>
/// <remarks>Based on default level assignment logic of <c>SpawnSystem.Spawn</c>.</remarks>
public class ModifierDefaultRollLevel : ISpawnModifier
{
    public int MinLevel { get; set; }
    public int MaxLevel { get; set; }
    public double MinDistanceForLevelups { get; set; }
    public double LevelupChance { get; set; }

    internal ModifierDefaultRollLevel()
    { }

    public ModifierDefaultRollLevel(int minLevel, int maxLevel, double minDistanceForLevelups, double levelupChance)
    {
        MinLevel = minLevel;
        MaxLevel = maxLevel;
        MinDistanceForLevelups = minDistanceForLevelups;
        LevelupChance = levelupChance;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (MaxLevel <= 1)
        {
            return;
        }

        if (MinDistanceForLevelups > 0 && entityZdo.m_position.magnitude < MinDistanceForLevelups)
        {
            return;
        }

        int minLevel = Math.Max(1, MinLevel);
        int level = minLevel;

        for (; level < MaxLevel; ++level)
        {
            if (UnityEngine.Random.Range(0f, 100f) > LevelupChance)
            {
                break;
            }
        }

        if (minLevel > 1)
        {
            Character character = ComponentCache.Get<Character>(entity);

            if (character != null && character)
            {
                character.SetLevel(minLevel);
            }
        }
    }
}
