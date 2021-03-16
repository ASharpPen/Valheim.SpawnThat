using BepInEx.Logging;
using System;
using UnityEngine;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat.ConfigurationCore
{
    public class Log
    {
        internal static ManualLogSource Logger;

        private const string Prefix = "[Spawn That!]: ";

        public static void LogDebug(string message)
        {
            if (ConfigurationManager.GeneralConfig?.DebugLoggingOn?.Value == true)
            {
                Logger.LogInfo($"{message}");
            }
        }

        public static void LogTrace(string message)
        {
            if (ConfigurationManager.GeneralConfig?.TraceLoggingOn?.Value == true)
            {
                Logger.LogDebug($"{message}");
            }
        }

        public static void LogInfo(string message) => Logger.LogMessage($"{message}");

        public static void LogWarning(string message) => Logger.LogWarning($"{message}");

        public static void LogError(string message, Exception e = null) => Logger.LogError($"{Prefix}{message}; {e?.Message ?? ""}");
    }
}
