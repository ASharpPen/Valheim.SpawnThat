using System;
using UnityEngine;
using Valheim.SpawnThat.Caches;

namespace Valheim.SpawnThat.Spawners.Modifiers;

/// <summary>
/// Based on default level assignment logic of <c>SpawnSystem.Spawn</c>.
/// </summary>
public class SpawnModifierDefaultRollLevel : ISpawnModifier
{
    private int MinLevel { get; }
    private int MaxLevel { get; }
    private double MinDistanceForLevelups { get; }
    private double LevelupChance { get; }

    public SpawnModifierDefaultRollLevel(int minLevel, int maxLevel, double minDistanceForLevelups, double levelupChance)
    {
        MinLevel = minLevel;
        MaxLevel = maxLevel;
        MinDistanceForLevelups = minDistanceForLevelups;
        LevelupChance = levelupChance;
    }

    public void Apply(GameObject entity, ZDO entityZdo)
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
            Character character = ComponentCache.GetComponent<Character>(entity);

            if (character != null && character)
            {
                character.SetLevel(minLevel);
            }
        }
    }
}
