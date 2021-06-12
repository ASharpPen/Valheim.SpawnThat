using BepInEx;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Utilities.Images;
using UnityEngine.Networking;


namespace Valheim.SpawnThat.WorldMap
{
    public static class MapPrinter
    {
        private static IImagePrinter _printer;

        public static IImagePrinter Printer
        {
            get
            {
                return _printer ??= new ImagePrinter();
            }
            set
            {
                _printer = value;
            }
        }

        public static void PrintAreaMap(AreaMap map)
        {
            Printer.PrintSquareMap(map.GridIds, "grid_ids_rainbow_map", true, 0, 0);
            Printer.PrintSquareMap(map.GridIds, "grid_ids_map", false, 0, 0);

            Printer.PrintBiomes(map.Grid, "grid_biome_rainbow_map");
        }
    }
}
