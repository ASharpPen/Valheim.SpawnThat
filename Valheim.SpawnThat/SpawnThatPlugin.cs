using BepInEx;
using HarmonyLib;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat
{
    [BepInPlugin("asharppen.valheim.spawn_that", "Spawn That!", "0.5.0")]
    public class SpawnThatPlugin : BaseUnityPlugin
    {        
        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            Log.Logger = Logger;

            ConfigurationManager.LoadAllConfigurations();

            new Harmony("mod.spawn_that").PatchAll();
        }
    }
}
