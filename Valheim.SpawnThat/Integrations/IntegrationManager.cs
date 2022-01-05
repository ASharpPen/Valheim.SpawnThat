using System;

namespace Valheim.SpawnThat.Integrations;

public static class IntegrationManager
{
    private static bool? _installedEpicLoot;
    private static bool? _installedCllc;
    private static bool? _installedMobAi;

    public static bool InstalledEpicLoot { get; } = _installedEpicLoot ??= Type.GetType("EpicLoot.EpicLoot, EpicLoot") is not null;

    public static bool InstalledCLLC { get; } = _installedCllc ??= Type.GetType("CreatureLevelControl.API, CreatureLevelControl") is not null;

    public static bool InstalledMobAI { get; } = _installedMobAi ??= Type.GetType("RagnarsRokare.MobAI.MobAILib, MobAILib") is not null;
}
