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

    /// <summary>
    /// KilledBat
    /// </summary>
    Bat,

    /// <summary>
    /// defeated_hive
    /// </summary>
    Hive,

    /// <summary>
    /// defeated_queen
    /// </summary>
    Queen,

    /// <summary>
    /// hildir1
    /// </summary>
    Hildir1,

    /// <summary>
    /// hildir2
    /// </summary>
    Hildir2,

    /// <summary>
    /// hildir3
    /// </summary>
    Hildir3,

    /// <summary>
    /// BossHildir1
    /// </summary>
    BossHildir1,

    /// <summary>
    /// BossHildir2
    /// </summary>
    BossHildir2,

    /// <summary>
    /// BossHildir3
    /// </summary>
    BossHildir3,
}

public static class GlobalKeyExtensions
{ 
    /// <summary>
    /// Gets the vanilla global key string corresponding to <c>key</c>.
    /// </summary>
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
            GlobalKey.Bat => "KilledBat",
            GlobalKey.Hive => "defeated_hive",
            GlobalKey.Queen => "defeated_queen",
            GlobalKey.Hildir1 => "hildir1",
            GlobalKey.Hildir2 => "hildir2",
            GlobalKey.Hildir3 => "hildir3",
            GlobalKey.BossHildir1 => "BossHildir1",
            GlobalKey.BossHildir2 => "BossHildir2",
            GlobalKey.BossHildir3 => "BossHildir3",
            _ => ""
        };
    }
}

