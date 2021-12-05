using System;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.Utilities;
using Valheim.SpawnThat.Utilities.Extensions;
using Valheim.SpawnThat.Utilities.World;
using Random = UnityEngine.Random;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions;

public class PositionSuggesterDefault : ISuggestPosition
{
    public uint MaxAttempts { get; set; } = 20;

    public uint MinDistance { get; set; } = 40;

    public uint MaxDistance { get; set; } = 80;

    public PositionSuggesterDefault()
    {
    }

    public Vector3? SuggestPosition(SpawnContext context)
    {
        // Find nearby players
        var nearbyPlayers = PlayerUtils.GetPlayerZdosInRadius(context.SpawnSystemZdo.m_position, 64);

        if (nearbyPlayers.Count == 0)
        {
            return null;
        }

        var positionContext = new PositionContext(context);

        // Repeat x times:
        for (int i = 0; i < MaxAttempts; ++i)
        {
            // Select random nearby
            var player = nearbyPlayers.Random();

            // Select random point surrounding player
            var randomness = Quaternion.Euler(0f, Random.Range(0, 360), 0f)
                * Vector3.forward
                * Random.Range(MinDistance, MaxDistance);

            var position =
                player.GetPosition() + randomness;

            position.y = WorldData.GetZone(position).Height(position);

            positionContext.Point = position;

            // Verify position conditions
            if (context.Template.PositionConditions.All(condition =>
            {
                try
                {
#if true && DEBUG
                    var valid = condition?.IsValid(positionContext) ?? false;

                    if (!valid && context.Template.Index <= 1)
                    {
                        Log.LogTrace($"[{context.Template.Index}] PosCondition {condition.GetType().Name} is invalid.");
                    }

                    return valid;
#else
                    return condition?.IsValid(positionContext) ?? false;
#endif
                }
                catch (Exception e)
                {
                    Log.LogError($"Error while attempting to check position condition '{condition.GetType().Name}' for spawn template '{context.Template.Index}'.", e);
                    return false;
                }
            }))
            {
                return position;
            }
        }

        return null;
    }

    public bool IsValidPosition(SpawnContext context, Vector3 position)
    {
        if ((context.Template.PositionConditions?.Count ?? 0) == 0)
        {
            return true;
        }

        // Verify position conditions
        var positionContext = new PositionContext(context)
        {
            Point = position
        };

        if (context.Template.PositionConditions.All(condition =>
        {
            try
            {
                return condition?.IsValid(positionContext) ?? false;
            }
            catch (Exception e)
            {
                Log.LogError($"Error while attempting to check position condition '{condition.GetType().Name}' for spawn template '{context.Template.Index}'.", e);
                return false;
            }
        }))
        { 
            return true;
        }

        return false;
    }
}
