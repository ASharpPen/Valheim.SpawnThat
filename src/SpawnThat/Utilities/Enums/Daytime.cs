namespace SpawnThat.Utilities.Enums;

public enum Daytime
{
    All,

    /// <summary>
    /// From dayfraction 0.25 to 0.75
    /// </summary>
    Day,

    /// <summary>
    /// From dayfraction 0.75 to 0.25
    /// </summary>
    Night,

    /// <summary>
    /// From dayfraction 0.25 to 0.50
    /// </summary>
    Morning,

    /// <summary>
    /// From dayfraction 0.50 to 0.75
    /// </summary>
    /// 
    Afternoon,

    /// <summary>
    /// From dayfraction 0.90 to 0.10
    /// </summary>
    /// 
    Midnight,

    /// <summary>
    /// From dayfraction 0.40 to 0.60
    /// </summary>
    Midday,
}

public static class DaytimeExtensions
{
    public static bool IsActive(this Daytime daytime, float? fraction = null)
    {
        fraction ??= EnvMan.instance.GetDayFraction();

        return (daytime, fraction) switch
        {
            (Daytime.All, _) => true,
            (Daytime.Day, >= 0.25f and <= 0.75f) => true,
            (Daytime.Night, <= 0.25f or >= 0.75f) => true,
            (Daytime.Morning, >= 0.25f and <= 0.50f) => true,
            (Daytime.Afternoon, >= 0.50f and <= 0.75f) => true,
            (Daytime.Midnight, >= 0.90f or <= 0.10f) => true,
            (Daytime.Midday, >= 0.40f and <= 0.60f) => true,
            _ => false
        };
    }
}