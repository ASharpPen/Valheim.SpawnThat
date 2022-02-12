using System;
using SpawnThat.Core;

namespace SpawnThat.Utilities.Extensions;

internal static class MulticastDelegateExtensions
{
    public static void RaiseSafely(this MulticastDelegate deleg, string messageOnError, params object[] args)
    {
        if (deleg is not null)
        {
            foreach (var invocation in deleg.GetInvocationList())
            {
                try
                {
                    invocation?.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    var target = $"{invocation.Method.DeclaringType.Assembly.GetName().Name}.{invocation.Method.DeclaringType.Name}.{invocation.Method.Name}";
                    Log.LogError($"[{target}]: {messageOnError}", e);
                }
            }
        }
    }

    public static void RaiseSafely(this Action events, string messageOnError)
    {
        if (events is not null)
        {
            foreach (Action invocation in events.GetInvocationList())
            {
                try
                {
                    invocation();
                }
                catch (Exception e)
                {
                    var target = $"{invocation.Method.DeclaringType.Assembly.GetName().Name}.{invocation.Method.DeclaringType.Name}.{invocation.Method.Name}";
                    Log.LogError($"[{target}]: {messageOnError}", e);
                }
            }
        }

    }

    public static void RaiseSafely<T>(this Action<T> events, string messageOnError, T arg)
    {
        if (events is not null)
        {
            foreach (Action<T> invocation in events.GetInvocationList())
            {
                try
                {
                    invocation(arg);
                }
                catch (Exception e)
                {
                    var target = $"{invocation.Method.DeclaringType.Assembly.GetName().Name}.{invocation.Method.DeclaringType.Name}.{invocation.Method.Name}";
                    Log.LogError($"[{target}]: {messageOnError}", e);
                }
            }
        }

    }
}
