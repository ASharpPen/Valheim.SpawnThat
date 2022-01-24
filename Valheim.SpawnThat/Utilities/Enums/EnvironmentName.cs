namespace Valheim.SpawnThat.Utilities.Enums;

/// <summary>
/// Known vanilla environments.
/// </summary>
public enum EnvironmentName
{
    Clear,
    Twilight_Clear,
    Misty,
    Darklands_dark,
    Heath_clear,
    DeepForest_Mist,
    GDKing,
    Rain,
    LightRain,
    ThunderStorm,
    Eikthyr,
    GoblinKing,
    nofogts,
    SwampRain,
    Bonemass,
    Snow,
    Twilight_Snow,
    Twilight_SnowStorm,
    SnowStorm,
    Moder,
    Ashrain,
    Crypt,
    SunkenCrypt
}

public static class EnvironmentNameExtensions
{
    public static string GetName(this EnvironmentName environment)
    {
        return environment switch
        {
            EnvironmentName.Clear => "Clear",
            EnvironmentName.Twilight_Clear => "Twilight_Clear",
            EnvironmentName.Misty => "Misty",
            EnvironmentName.Darklands_dark => "Darklands_dark",
            EnvironmentName.Heath_clear => "Heath clear",
            EnvironmentName.DeepForest_Mist => "DeepForest Mist",
            EnvironmentName.GDKing => "GDKing",
            EnvironmentName.Rain => "Rain",
            EnvironmentName.LightRain => "LightRain",
            EnvironmentName.ThunderStorm => "ThunderStorm",
            EnvironmentName.Eikthyr => "Eikthyr",
            EnvironmentName.GoblinKing => "GoblinKing",
            EnvironmentName.nofogts => "nofogts",
            EnvironmentName.SwampRain => "SwampRain",
            EnvironmentName.Bonemass => "Bonemass",
            EnvironmentName.Snow => "Snow",
            EnvironmentName.Twilight_Snow => "Twilight_Snow",
            EnvironmentName.Twilight_SnowStorm => "Twilight_SnowStorm",
            EnvironmentName.SnowStorm => "SnowStorm",
            EnvironmentName.Moder => "Moder",
            EnvironmentName.Ashrain => "Ashrain",
            EnvironmentName.Crypt => "Crypt",
            EnvironmentName.SunkenCrypt => "SunkenCrypt",
            _ => null,
        };
    }
}