using Desynchronized.Redirection;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace Desynchronized.Transpilers
{
    /*
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("PreKidnapped", MethodType.Normal)]
    public class Transpiler_Pawn_PreKidnapped
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool patchIsComplete = false;

            foreach (CodeInstruction instruction in instructions)
            {
                if (!patchIsComplete && instruction.opcode == OpCodes.Pop)
                {
                    yield return new CodeInstruction(OpCodes.Call, typeof(KidnappingTaleRedirector).GetMethod("RedirectTale"));
                    patchIsComplete = true;
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
    */
}
