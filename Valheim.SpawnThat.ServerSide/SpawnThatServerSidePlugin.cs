using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.ServerSide
{
    [BepInDependency(SpawnThatPlugin.ModID, SpawnThatPlugin.ModVersion)] // Might be a bit much to require direct version matching, but in this case it's probably for the best.
    [BepInPlugin("asharppen.valheim.spawn_that.serverside", "Spawn That! ServerSide", "0.0.1")]
    public class SpawnThatServerSidePlugin : BaseUnityPlugin
    {        
        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            Log.Logger = Logger;

            new Harmony("mod.spawn_that.serverside").PatchAll();

            StateResetter.Subscribe(() =>
            {
                Configuration.Multiplayer.ConfigMultiplayerPatch.EnableSync = false;
            });
        }
    }
}
