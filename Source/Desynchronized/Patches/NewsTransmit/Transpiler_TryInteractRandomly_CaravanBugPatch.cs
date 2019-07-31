using Desynchronized.Utilities;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace Desynchronized.Patches.NewsTransmit
{
    [HarmonyPatch(typeof(Pawn_InteractionsTracker))]
    [HarmonyPatch("TryInteractRandomly", MethodType.Normal)]
    public class Transpiler_TryInteractRandomly_CaravanBugPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            bool patchIsComplete = false;

            foreach (CodeInstruction instr in instructions)
            {
                yield return instr;

                if (!patchIsComplete && instr.opcode == OpCodes.Stloc_0)
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Pawn_InteractionsTracker), "pawn"));
                    yield return new CodeInstruction(OpCodes.Call, typeof(CaravanUtilities).GetMethod("ManipulateInteractionTargetsList"));

                    patchIsComplete = true;
                }
            }
        }
    }
}
