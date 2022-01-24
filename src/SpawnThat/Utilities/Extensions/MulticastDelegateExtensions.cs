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
                    invocation.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    Log.LogError(messageOnError, e);
                }
            }
        }
    }
}
