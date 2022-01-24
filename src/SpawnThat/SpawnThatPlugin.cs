using BepInEx;
using HarmonyLib;
using SpawnThat.Configuration;
using SpawnThat.Core;

namespace SpawnThat
{
    [BepInDependency("RagnarsRokare.MobAILib", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("org.bepinex.plugins.creaturelevelcontrol", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("randyknapp.mods.epicloot", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("asharppen.valheim.spawn_that", "Spawn That!", "0.11.6")]
    internal class SpawnThatPlugin : BaseUnityPlugin
    {
        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            Log.Logger = Logger;

            ConfigurationManager.GeneralConfig = ConfigurationManager.LoadGeneral();

            Startup.SetupServices();

            new Harmony("mod.spawn_that").PatchAll();
        }
    }
}