using System.Collections.Generic;
using System.Diagnostics;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Lifecycle;
using SpawnThat.World.Maps.Images;
using SpawnThat.World.Maps.Images.Colors;

namespace SpawnThat.World.Maps.Sectors;

internal class SectorManager
{
    internal static Dictionary<BiomeSector, int> IdBySector = new();
    internal static Dictionary<int, BiomeSector> SectorById = new();
    private static int IdSequence;

    public static bool IdsAssigned { get; private set; }

    public static BiomeSectorIdMap IdMap { get; private set; } = null;
    public static BiomeSectorBiomeMap BiomeMap { get; private set; } = null;

    static SectorManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            IdBySector.Clear();
            SectorById.Clear();
            IdSequence = 0;
            IdsAssigned = false;
            IdMap = null;
            BiomeMap = null;
        });
    }

    public static void AssignIds()
    {
        if (IdsAssigned ||
            ZNet.World?.m_biomeData?.Sectors is null)
        {
            return;
        }

        IdsAssigned = true;

        foreach (var sector in ZNet.World.m_biomeData.Sectors)
        {
            if (!IdBySector.TryGetValue(sector, out _))
            {
                IdBySector[sector] = IdSequence;
                SectorById[IdSequence] = sector;
                IdSequence++;
            }
        }
    }

    public static void InitMap()
    {
        Stopwatch watch = Stopwatch.StartNew();

        if (IdMap is null)
        {
            IdMap = new BiomeSectorIdMap(ZNet.World);
        }
        if (BiomeMap is null)
        {
            BiomeMap = new BiomeSectorBiomeMap(ZNet.World);
        }

        watch.Stop();

        Log.LogTrace($"BiomeSector map scan took: {watch.Elapsed}");
    }

    public static void PrintMaps()
    {
        PrintSectorIdMap();
        PrintSectorBiomeMap();
    }

    public static void PrintSectorIdMap()
    {
        if (ConfigurationManager.GeneralConfig?.PrintAreaMap?.Value == true)
        {
            ImageBuilder
                .Init(IdMap.GridWidth)
                .Apply(IdMap, RainbowColorMapper.FromIntMap())
                .Print("biomesectors_by_id");
        }
    }

    public static void PrintSectorBiomeMap()
    {
        if (ConfigurationManager.GeneralConfig?.PrintBiomeMap?.Value == true)
        {
            ImageBuilder
                .Init(BiomeMap.GridWidth)
                .Apply(BiomeMap, BiomeColorMapper.FromBiomeMap())
                .Print("biomesector_biomes");
        }
    }

    public static bool TryGetSectorId(BiomeSector sector, out int id) =>
        IdBySector.TryGetValue(sector, out id);

    public static bool TryGetSector(int sectorId, out BiomeSector sector) =>
        SectorById.TryGetValue(sectorId, out sector);
}
