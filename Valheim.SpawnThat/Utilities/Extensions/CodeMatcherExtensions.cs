using HarmonyLib;
using System;
using System.Reflection.Emit;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Utilities.Extensions;

public static class CodeMatcherExtensions
{
    public static CodeMatcher GetPosition(this CodeMatcher codeMatcher, out int position)
    {
        position = codeMatcher.Pos;
        return codeMatcher;
    }

    public static CodeMatcher AddLabel(this CodeMatcher codeMatcher, out Label label)
    {
        label = new Label();
        codeMatcher.AddLabels(new[] { label });
        return codeMatcher;
    }

    internal static CodeMatcher Print(this CodeMatcher codeMatcher, int count, int offset = 0)
    {
        for (int i = 0; i < count; ++i)
        {
            int currentOffset = offset + i;
            int index = codeMatcher.Pos + currentOffset;

            if (index <= 0)
            {
                continue;
            }

            if (index >= codeMatcher.Length)
            {
                break;
            }

            try
            {
                var line = codeMatcher.InstructionAt(currentOffset);
                Log.LogIL($"[{currentOffset}] " + line.ToString());
            }
            catch (Exception e)
            {
                Log.LogIL(e.Message);
            }
        }

        return codeMatcher;
    }
}
