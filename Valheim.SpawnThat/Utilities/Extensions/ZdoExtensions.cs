namespace Valheim.SpawnThat.Utilities.Extensions;

public static class ZdoExtensions
{
    private static int NoiseHash = "noise".GetStableHashCode();
    private static int TamedHash = "tamed".GetStableHashCode();
    private static int EventCreatureHash = "EventCreature".GetStableHashCode();

    // Custom ZDO entries
    private static int FactionHash = "faction".GetStableHashCode();

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

    public static Character.Faction? GetFaction(this ZDO zdo)
    {
        var faction = zdo.GetInt(FactionHash, -1);

        if (faction < 0)
        {
            return null;
        }

        return (Character.Faction)faction;
    }

    public static void SetFaction(this ZDO zdo, Character.Faction faction)
    {
        zdo.Set(FactionHash, (int)faction);
    }
}