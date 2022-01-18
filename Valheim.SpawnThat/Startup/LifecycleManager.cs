using System;
using System.Collections.Generic;
using HarmonyLib;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Startup;

public enum GameState
{
    Invalid = 0,
    Singleplayer,
    Multiplayer,
    DedicatedServer
}

[HarmonyPatch]
public static class LifecycleManager
{
    public delegate void WorldInitEvent();
    public static event WorldInitEvent OnWorldInit;

    public delegate void AfterInitEvent();
    public static event AfterInitEvent AfterInit;

    public delegate void SinglePlayerInitEvent();
    public static event SinglePlayerInitEvent OnSinglePlayerInit;

    public delegate void MultiplayerInitEvent();
    public static event MultiplayerInitEvent OnMultiplayerInit;

    public delegate void DedicatedServerInitEvent();
    public static event DedicatedServerInitEvent OnDedicatedServerInit;

    private static HashSet<Action> OnResetActions = new HashSet<Action>();

    public static GameState GameState { get; private set; }

    public static void SubscribeToWorldInit(Action onReset)
    {
        OnResetActions.Add(onReset);
    }

    private static void WorldInit()
    {
        Log.LogDebug("Running world init actions");
        if (OnWorldInit is not null)
        {
            OnWorldInit();
        }

        foreach (var onReset in OnResetActions)
        {
            onReset.Invoke();
        }
    }

    /// <summary>
    /// Singleplayer
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.OnWorldStart))]
    [HarmonyPrefix]
    private static void ResetState()
    {
        GameState = GameState.Singleplayer;
        WorldInit();

        Log.LogDebug("Running singleplayer init actions");
        if (OnSinglePlayerInit is not null)
        { 
            OnSinglePlayerInit();
        }

        if (AfterInit is not null)
        {
            AfterInit();
        }
    }

    /// <summary>
    /// Multiplayer
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.JoinServer))]
    [HarmonyPrefix]
    private static void ResetStateMultiplayer()
    {
        GameState = GameState.Multiplayer;
        WorldInit();

        Log.LogDebug("Running multiplayer init actions");
        if (OnMultiplayerInit is not null)
        {
            OnMultiplayerInit();
        }

        if (AfterInit is not null)
        {
            AfterInit();
        }
    }

    /// <summary>
    /// Server
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.ParseServerArguments))]
    [HarmonyPrefix]
    private static void ResetStateServer()
    {
        GameState = GameState.DedicatedServer;

        WorldInit();

        Log.LogDebug("Running dedicated server init actions");
        if (OnDedicatedServerInit is not null)
        {
            OnDedicatedServerInit();
        }

        if (AfterInit is not null)
        {
            AfterInit();
        }
    }
}
