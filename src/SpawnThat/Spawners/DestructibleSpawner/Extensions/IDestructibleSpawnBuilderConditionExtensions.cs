using SpawnThat.Options.Conditions;

namespace SpawnThat.Spawners.DestructibleSpawner;

public static class IDestructibleSpawnConditionExtensions
{
    /// <summary>
    /// <para>Set spawn chance based on each area.</para>
    /// <para>
    ///     Each area rolls chance pr template id once pr seed.
    ///     If the chance roll is less than listed here,
    ///     this area will never activate this template, and vice versa.
    /// </para>
    /// <para>
    ///     This allows for sitations where only some areas (eg., 10% of blackforests) will have a spawn show up.
    /// </para>
    /// </summary>
    public static IDestructibleSpawnBuilder SetConditionAreaSpawnChance(this IDestructibleSpawnBuilder builder, float areaChance)
    {
        builder.SetCondition(new ConditionAreaSpawnChance(areaChance, (int)builder.Id));
        return builder;
    }
}

