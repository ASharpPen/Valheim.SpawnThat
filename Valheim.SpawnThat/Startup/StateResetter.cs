using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.Startup;

public static class StateResetter
{
    private static HashSet<IReset> Resetables = new HashSet<IReset>();
    private static HashSet<Action> OnResetActions = new HashSet<Action>();

    public static void Subscribe(IReset reset)
    {
        Resetables.Add(reset);
    }

    public static void Subscribe(Action onReset)
    {
        OnResetActions.Add(onReset);
    }

    public static void Unsubscribe(IReset reset)
    {
        Resetables.Remove(reset);
    }

    public static void Unsubscribe(Action onReset)
    {
        OnResetActions.Remove(onReset);
    }

    internal static void Reset()
    {
        Log.LogDebug("Resetting mod state.");

        foreach (var resetable in Resetables)
        {
            resetable.Reset();
        }

        foreach (var onReset in OnResetActions)
        {
            onReset.Invoke();
        }
    }
}
