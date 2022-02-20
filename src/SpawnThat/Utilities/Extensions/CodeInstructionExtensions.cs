using System;
using System.Reflection.Emit;
using HarmonyLib;

namespace SpawnThat.Utilities.Extensions;

internal static class CodeInstructionExtensions
{
    /// <summary>
    /// Returns corresponding ldloc for stloc instructions.
    /// If instruction is not an stloc, it returns itself.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public static CodeInstruction GetLdlocFromStLoc(this CodeInstruction instruction)
    {
        if (instruction.opcode == OpCodes.Stloc_0)
        {
            return new(OpCodes.Ldloc_0);
        }
        if (instruction.opcode == OpCodes.Stloc_1)
        {
            return new(OpCodes.Ldloc_1);
        }
        if (instruction.opcode == OpCodes.Stloc_2)
        {
            return new(OpCodes.Ldloc_2);
        }
        if (instruction.opcode == OpCodes.Stloc_3)
        {
            return new(OpCodes.Ldloc_3);
        }
        if (instruction.opcode == OpCodes.Stloc_S)
        {
            return new(OpCodes.Ldloca_S, instruction.operand);
        }
        if (instruction.opcode == OpCodes.Stloc)
        {
            return new(OpCodes.Ldloc, instruction.operand);
        }

        throw new ArgumentException($"Unexpected instruction '{instruction}' encountered. Expected an {OpCodes.Stloc}.");
    }

    /// <summary>
    /// Returns corresponding stloc for ldloc instructions.
    /// If instruction is not an ldloc, it returns itself.
    /// </summary>
    public static CodeInstruction GetStlocFromLdloc(this CodeInstruction instruction)
    {
        if (instruction.opcode == OpCodes.Ldloc_0)
        {
            return new(OpCodes.Stloc_0);
        }
        if (instruction.opcode == OpCodes.Ldloc_1)
        {
            return new(OpCodes.Stloc_1);
        }
        if (instruction.opcode == OpCodes.Ldloc_2)
        {
            return new(OpCodes.Stloc_2);
        }
        if (instruction.opcode == OpCodes.Ldloc_3)
        {
            return new(OpCodes.Stloc_3);
        }
        if (instruction.opcode == OpCodes.Ldloca_S)
        {
            return new(OpCodes.Stloc_S, instruction.operand);
        }
        if (instruction.opcode == OpCodes.Ldloc)
        {
            return new(OpCodes.Stloc, instruction.operand);
        }

        throw new ArgumentException($"Unexpected instruction '{instruction}' encountered. Expected an {OpCodes.Ldloc}.");
    }
}
