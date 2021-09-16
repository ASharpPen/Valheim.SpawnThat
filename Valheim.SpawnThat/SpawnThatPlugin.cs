using BepInEx;
using HarmonyLib;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat
{
    [BepInDependency("RagnarsRokare.MobAILib", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("org.bepinex.plugins.creaturelevelcontrol", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("randyknapp.mods.epicloot", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(ModID, ModName, ModVersion)]
    public class SpawnThatPlugin : BaseUnityPlugin
    {
        public const string ModID = "asharppen.valheim.spawn_that";
        public const string ModName = "Spawn That!";
        public const string ModVersion = "0.11.4";

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            Log.Logger = Logger;

            ConfigurationManager.GeneralConfig = ConfigurationManager.LoadGeneral();

            new Harmony("mod.spawn_that").PatchAll();
        }
    }
}
