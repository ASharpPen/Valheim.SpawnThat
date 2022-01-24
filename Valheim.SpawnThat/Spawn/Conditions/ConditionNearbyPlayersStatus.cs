using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawn.Conditions;

/// <summary>
/// Require player within distance to have one of the listed status effects.
/// </summary>
public class ConditionNearbyPlayersStatus : ISpawnCondition
{
    public int SearchDistance { get; set; }
    public HashSet<string> RequiredStatusEffects { get; set; }

    public ConditionNearbyPlayersStatus()
    { }

    public ConditionNearbyPlayersStatus(int distanceToSearch, params string[] requireOneOfStatusEffects)
    {
        SearchDistance = distanceToSearch;
        RequiredStatusEffects = requireOneOfStatusEffects
            .Select(x => x.Trim())
            .ToHashSet();
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (RequiredStatusEffects.Count == 0)
        {
            return true;
        }

        if (SearchDistance <= 0)
        {
            return false;
        }

        List<Player> players = PlayerUtils.GetPlayersInRadius(context.SpawnerZdo.GetPosition(), SearchDistance);

        foreach (var player in players)
        {
            SEMan statusEffectManager = player.GetSEMan();

            if (RequiredStatusEffects.Any(x => statusEffectManager.HaveStatusEffect(x)))
            {
                return true;
            }
        }

        return false;
    }
}
