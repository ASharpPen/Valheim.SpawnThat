namespace SpawnThat.Utilities.Extensions;

public static class ZdoExtensions
{
    private static int NoiseHash = "noise".HashInteger();
    private static int TamedHash = "tamed".HashInteger();
    private static int EventCreatureHash = "EventCreature".HashInteger();
    private static int HuntPlayerHash = "huntplayer".HashInteger();

    // Custom ZDO entries
    private static int FactionHash = "faction".HashInteger();

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

    /// <summary>
    /// Gets "faction" from zdo.
    /// </summary>
    /// <remarks>Spawn That setting.</remarks>
    public static Character.Faction? GetFaction(this ZDO zdo)
    {
        var faction = zdo.GetInt(FactionHash, -1);

        if (faction < 0)
        {
            return null;
        }

        return (Character.Faction)faction;
    }

    /// <summary>
    /// Sets "faction" in zdo.
    /// </summary>
    /// <remarks>Spawn That setting.</remarks>
    public static void SetFaction(this ZDO zdo, Character.Faction faction)
    {
        zdo.Set(FactionHash, (int)faction);
    }

    public static bool GetHuntPlayer(this ZDO zdo)
    {
        return zdo.GetBool(HuntPlayerHash);
    }

    public static void SetHuntPlayer(this ZDO zdo, bool huntPlayer)
    {
        zdo.Set(HuntPlayerHash, huntPlayer);
    }
}