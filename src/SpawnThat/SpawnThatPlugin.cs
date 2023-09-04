using System;
using BepInEx;
using HarmonyLib;
using SpawnThat.Configuration;
using SpawnThat.Core;

namespace SpawnThat;

[BepInDependency("RagnarsRokare.MobAILib", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("org.bepinex.plugins.creaturelevelcontrol", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("randyknapp.mods.epicloot", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(ModId, PluginName, Version)]
public class SpawnThatPlugin : BaseUnityPlugin
{
    public const string ModId = "asharppen.valheim.spawn_that";
    public const string PluginName = "Spawn That!";
    public const string Version = "1.2.7";

    // Awake is called once when both the game and the plug-in are loaded
    void Awake()
    {
        Log.Logger = Logger;

        ConfigurationManager.GeneralConfig = ConfigurationManager.LoadGeneral();

        Startup.SetupServices();

        Type testType = Type.GetType("YamlDotNet.Serialization.SerializerBuilder, YamlDotNet");

        if (testType is null)
        {
            Log.LogWarning("Unable to detect required YamlDotNet type. Verify that YamlDotNet.dll is installed.");
        }

        new Harmony(ModId).PatchAll();
    }
}
