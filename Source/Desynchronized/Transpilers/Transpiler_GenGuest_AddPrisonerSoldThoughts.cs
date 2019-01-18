using Desynchronized.Redirection;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace Desynchronized.Transpilers
{
    /*
    /// <summary>
    /// Redirects code to hand-written ThoughtRedirector.GiveThoughtsAboutPrisonerSalesToPawn for maximum sanity
    /// </summary>
    [HarmonyPatch(typeof(GenGuest))]
    [HarmonyPatch("AddPrisonerSoldThoughts", MethodType.Normal)]
    public class Transpiler_GenGuest_AddPrisonerSoldThoughts
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            int ignoreCount = 0;
            bool patchIsComplete = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (!patchIsComplete && instruction.opcode == OpCodes.Ldfld)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0, null);
                    yield return new CodeInstruction(OpCodes.Call, typeof(ThoughtRedirector).GetMethod("GiveThoughtsAboutPrisonerSalesToPawn"));
                    ignoreCount = 7;

                    patchIsComplete = true;
                }

                if (ignoreCount > 0)
                {
                    instruction.opcode = OpCodes.Nop;
                    ignoreCount--;
                }

                yield return instruction;
            }
        }
    }
    */
}
