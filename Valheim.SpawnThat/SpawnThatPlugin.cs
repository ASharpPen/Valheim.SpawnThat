using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat
{
    [BepInPlugin("asharppen.valheim.spawn_that", "Spawn That!", "0.1.0")]
    public class SpawnThatPlugin : BaseUnityPlugin
    {        
        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            ConfigurationManager.LoadAllConfigurations();

            new Harmony("mod.custom_raids").PatchAll();
        }
    }
}
