using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities.Enums;

namespace SpawnThat.Options.Conditions;

public class ConditionForest : ISpawnCondition
{
    public ForestState Required { get; set; }

    public ConditionForest()
    { }

    public ConditionForest(bool inForest, bool outsideForest)
    {
        Required = (inForest, outsideForest) switch
        {
            (true, true) => ForestState.Both,
            (false, true) => ForestState.OutsideForest,
            (true, false) => ForestState.InForest,
            _ => ForestState.None,
        };
    }

    public ConditionForest(ForestState requiredState)
    {
        Required = requiredState;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        return Required switch
        {
            ForestState.InForest => WorldGenerator.InForest(context.SpawnerZdo.GetPosition()),
            ForestState.OutsideForest => !WorldGenerator.InForest(context.SpawnerZdo.GetPosition()),
            ForestState.None => false,
            _ => true
        };
    }
}