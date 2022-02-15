using HarmonyLib;
using System;
using System.Reflection.Emit;
using SpawnThat.Core;
using System.Reflection;

namespace SpawnThat.Utilities.Extensions;

public static class CodeMatcherExtensions
{
    public static CodeMatcher GetPosition(this CodeMatcher codeMatcher, out int position)
    {
        position = codeMatcher.Pos;
        return codeMatcher;
    }

    public static CodeMatcher InsertAndAdvance(this CodeMatcher codeMatcher, OpCode opcode)
    {
        codeMatcher.InsertAndAdvance(new CodeInstruction(opcode));
        return codeMatcher;
    }

    public static CodeMatcher InsertAndAdvance(this CodeMatcher codeMatcher, MethodInfo method)
    {
        codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Call, method));
        return codeMatcher;
    }

    public static CodeMatcher InsertAndAdvance<T>(this CodeMatcher codeMatcher, string methodName)
    {
        return InsertAndAdvance(codeMatcher, AccessTools.Method(typeof(T), methodName));
    }

    public static CodeMatcher GetOpcode(this CodeMatcher codeMatcher, out OpCode opcode)
    {
        opcode = codeMatcher.Opcode;
        return codeMatcher;
    }

    public static CodeMatcher GetInstruction(this CodeMatcher codeMatcher, out CodeInstruction instruction)
    {
        instruction = codeMatcher.Instruction;
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
                Log.LogError(e.Message);
            }
        }
#endif
        return codeMatcher;
    }
}
