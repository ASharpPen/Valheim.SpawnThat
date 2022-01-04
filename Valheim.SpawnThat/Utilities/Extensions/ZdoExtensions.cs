namespace Valheim.SpawnThat.Utilities.Extensions;

public static class ZdoExtensions
{
    private static int NoiseHash = "noise".GetStableHashCode();
    private static int TamedHash = "tamed".GetStableHashCode();
    private static int EventCreatureHash = "EventCreature".GetStableHashCode();

    public static float GetNoise(this ZDO zdo)
    {
        return zdo.GetFloat(NoiseHash);
    }

    public static bool GetTamed(this ZDO zdo)
    {
        return zdo.GetBool(TamedHash);
    }

    public static bool GetEventCreature(this ZDO zdo)
    {
        return zdo.GetBool(EventCreatureHash);
    }
}