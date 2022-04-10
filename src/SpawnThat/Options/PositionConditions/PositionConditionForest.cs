using SpawnThat.Spawners.Contexts;
using UnityEngine;

namespace SpawnThat.Options.PositionConditions;

public class PositionConditionForest : ISpawnPositionCondition
{
    public ForestState Required { get; set; }

    public PositionConditionForest()
    { }

    public PositionConditionForest(bool inForest, bool outsideForest)
    {
        Required = (inForest, outsideForest) switch
        {
            (true, true) => ForestState.Both,
            (false, true) => ForestState.OutsideForest,
            (true, false) => ForestState.InForest,
            _ => ForestState.None,
        };
    }

    public PositionConditionForest(ForestState requiredState)
    {
        Required = requiredState;
    }

    public bool IsValid(SpawnSessionContext context, Vector3 position)
    {
        return Required switch
        {
            ForestState.InForest => WorldGenerator.InForest(position),
            ForestState.OutsideForest => !WorldGenerator.InForest(position),
            ForestState.None => false,
            _ => true
        };
    }
}

public enum ForestState
{
    Both,
    None,
    InForest,
    OutsideForest,
}