using Desynchronized.Redirection;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace Desynchronized.Transpilers
{
    /// <summary>
    /// Redirects code to hand-written ThoughtRedirector.GiveThoughtsAboutDeathToPawn for maximum sanity
    /// </summary>
    /*
    [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility))]
    [HarmonyPatch("TryGiveThoughts", MethodType.Normal)]
    [HarmonyPatch(new Type[] {typeof(Pawn), typeof(DamageInfo), typeof(PawnDiedOrDownedThoughtsKind)})]
    public class Transpiler_SpecialThoughtsUtil_Banishment
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
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 5);
                    yield return new CodeInstruction(OpCodes.Call, typeof(ThoughtRedirector).GetMethod("GiveThoughtsAboutBanishmentToPawn"));
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
