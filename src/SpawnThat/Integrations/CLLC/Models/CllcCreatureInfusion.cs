using CreatureLevelControl;

namespace SpawnThat.Integrations.CLLC.Models;

public enum CllcCreatureInfusion
{
    None,
    Lightning,
    Fire,
    Frost,
    Poison,
    Chaos,
    Spirit
}

internal static class CllcCreatureInfusionExtensions
{
    public static CreatureInfusion Convert(this CllcCreatureInfusion infusion)
    {
        return infusion switch
        {
            CllcCreatureInfusion.Lightning => CreatureInfusion.Lightning,
            CllcCreatureInfusion.Fire => CreatureInfusion.Fire,
            CllcCreatureInfusion.Frost => CreatureInfusion.Frost,
            CllcCreatureInfusion.Poison => CreatureInfusion.Poison,
            CllcCreatureInfusion.Chaos => CreatureInfusion.Chaos,
            CllcCreatureInfusion.Spirit => CreatureInfusion.Spirit,
            _ => CreatureInfusion.None,
        };
    }
}
