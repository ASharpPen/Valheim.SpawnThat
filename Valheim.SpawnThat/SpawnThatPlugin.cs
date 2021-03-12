using BepInEx;
using HarmonyLib;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat
{
    [BepInPlugin("asharppen.valheim.spawn_that", "Spawn That!", "0.2.0")]
    public class SpawnThatPlugin : BaseUnityPlugin
    {        
        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            Log.Logger = Logger;

            ConfigurationManager.LoadAllConfigurations();

            new Harmony("mod.custom_raids").PatchAll();
        }
    }
}
