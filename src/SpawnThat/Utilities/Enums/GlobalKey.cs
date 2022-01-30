namespace SpawnThat.Utilities.Enums;

/// <summary>
/// Default vanilla global keys.
/// </summary>
public enum GlobalKey
{
    /// <summary>
    /// defeated_eikthyr
    /// </summary>
    Eikthyr,

    /// <summary>
    /// defeated_gdking
    /// </summary>
    Elder,

    /// <summary>
    /// defeated_bonemass
    /// </summary>
    Bonemass,

    /// <summary>
    /// defeated_dragon
    /// </summary>
    Moder,

    /// <summary>
    /// defeated_goblinking
    /// </summary>
    Yagluth,

    /// <summary>
    /// KilledTroll
    /// </summary>
    Troll,

    /// <summary>
    /// killed_surtling
    /// </summary>
    Surtling,
}

public static class GlobalKeyExtensions
{ 
    public static string GetName(this GlobalKey key)
    {
        return key switch
        {
            GlobalKey.Eikthyr => "defeated_eikthyr",
            GlobalKey.Elder => "defeated_gdking",
            GlobalKey.Bonemass => "defeated_bonemass",
            GlobalKey.Moder => "defeated_dragon",
            GlobalKey.Yagluth => "defeated_goblinking",
            GlobalKey.Troll => "KilledTroll",
            GlobalKey.Surtling => "killed_surtling",
            _ => ""
        };
    }
}

