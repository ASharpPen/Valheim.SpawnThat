using BepInEx.Logging;
using System;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat.ConfigurationCore
{
    public class Log
    {
        internal static ManualLogSource Logger;

        private static string PluginName => "Spawn That!";

        public static void LogDebug(string message)
        {
            if (ConfigurationManager.GeneralConfig?.DebugLoggingOn?.Value == true)
            {
                //Logger.LogDebug($"[{PluginName}]: {message}");
                Logger.LogDebug($"{message}");
            }
        }

        public static void LogTrace(string message)
        {
            if (ConfigurationManager.GeneralConfig?.TraceLoggingOn?.Value == true)
            {
                //Logger.LogDebug($"[{PluginName}]: {message}");
                Logger.LogDebug($"{PluginName}: {message}");
            }
        }

        public static void LogInfo(string message) => Logger.LogInfo($"{message}");
        //Logger.LogInfo($"[{PluginName}]: {message}");

        public static void LogWarning(string message) => Logger.LogWarning($"{message}");
        //Logger.LogWarning($"[{PluginName}]: {message}");

        public static void LogError(string message, Exception e = null) => Logger.LogError($"{message}; {e?.Message ?? ""}");
        //Logger.LogError($"[{PluginName}]: {message}; {e?.Message ?? ""}");
    }
}
