using HarmonyLib;
using System;
using System.Reflection.Emit;
using SpawnThat.Core;

namespace SpawnThat.Utilities.Extensions;

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

    public static CodeMatcher GetOperand(this CodeMatcher codeMatcher, out object operand)
    {
        operand = codeMatcher.Operand;
        return codeMatcher;
    }

    internal static CodeMatcher Print(this CodeMatcher codeMatcher, int before, int after)
    {
#if DEBUG
        for (int i = -before; i <= after; ++i)
        {
            int currentOffset = i;
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
                Log.LogTrace($"[{currentOffset}] " + line.ToString());
            }
            catch (Exception e)
            {
                Log.LogTrace(e.Message);
            }
        }
#endif
        return codeMatcher;
    }
}
