using SpawnThat.Core;

namespace SpawnThat.ConsoleCommands;

internal static class CommandExtensions
{
    /// <summary>
    /// Adds text to terminal, while also printing it to <see cref="Log.LogInfo"/> to ensure it can get recorded.
    /// </summary>
    public static Terminal Print(
        this Terminal terminal, 
        string commandName, 
        string text)
    {
        terminal.AddString(text);

        Log.LogInfo($"{commandName}: {text}");

        return terminal;
    }
}
