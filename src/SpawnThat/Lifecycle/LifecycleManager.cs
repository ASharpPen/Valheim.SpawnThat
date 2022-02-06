using System;
using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Lifecycle;

public static class LifecycleManager
{
    private static HashSet<Action> OnWorldInitActions = new HashSet<Action>();

    public static event Action OnWorldInit;

    public static event Action OnLateInit;

    public static event Action OnSinglePlayerInit;

    public static event Action OnMultiplayerInit;

    public static event Action OnDedicatedServerInit;

    public static Action OnFindSpawnPointFirstTime;

    public static GameState GameState { get; private set; }

    public static void SubscribeToWorldInit(Action onInit)
    {
        OnWorldInitActions.Add(onInit);
    }

    private static void WorldInit()
    {
        Log.LogDebug("Running world init actions");
        OnWorldInit.RaiseSafely("Error during world init event");

        foreach (var onInit in OnWorldInitActions)
        {
            onInit();
        }
    }

    internal static void InitSingleplayer()
    {
        GameState = GameState.Singleplayer;
        WorldInit();

        Log.LogDebug("Running singleplayer init actions");
        OnSinglePlayerInit.RaiseSafely("Error during singleplayer init event");

        OnLateInit.RaiseSafely("Error during singleplayer late init event");
    }

    internal static void InitMultiplayer()
    {
        GameState = GameState.Multiplayer;
        WorldInit();

        Log.LogDebug("Running multiplayer init actions");
        OnMultiplayerInit.RaiseSafely("Error during multiplayer init event");

        OnLateInit.RaiseSafely("Error during multiplayer late init event");
    }

    internal static void InitDedicated()
    {
        GameState = GameState.DedicatedServer;
        WorldInit();

        Log.LogDebug("Running dedicated server init actions");
        OnDedicatedServerInit.RaiseSafely("Error during dedicated server init event");

        OnLateInit.RaiseSafely("Error during dedicated server late init event");
    }
}
