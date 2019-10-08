using Desynchronized.Handlers;
using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace Desynchronized.Compatibility.QEthics
{
    [HarmonyPatch]
    public class Transpiler_QEthics_RecipeWorker_NerveStapling
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method("QEthics.RecipeWorker_NerveStapling:ApplyOnPawn");
        }

        public static bool Prepare()
        {
            //DesynchronizedMain.LogError("loaded? " + ModDetector.QuestionableEthicsIsLoaded);
            return ModDetector.QuestionableEthicsIsLoaded;
        }

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ReplaceNewsDistribution(IEnumerable<CodeInstruction> instructions)
        {
            // Insert handler method after 6th NOP
            // Also return directly.

            int countNop = 0;
            foreach (CodeInstruction instr in instructions)
            {
                yield return instr;

                if (instr.opcode == OpCodes.Nop)
                {
                    countNop++;
                    if (countNop == 6)
                    {
                        //yield return new CodeInstruction(OpCodes.Ldarg_0);
                        //yield return new CodeInstruction(OpCodes.Ldarg_1);
                        //yield return new CodeInstruction(OpCodes.Ldarg_3);
                        //yield return new CodeInstruction(OpCodes.Call, typeof(Handler_PawnNerveStapled).GetMethod("HandlePawnNerveStapled"));
                        yield return new CodeInstruction(OpCodes.Ret);
                    }
                }
            }
        }

        [HarmonyPostfix]
        public static void ASDF(object __instance, Pawn pawn, Pawn billDoer)
        {
            Recipe_InstallImplant instance = __instance as Recipe_InstallImplant;
            if (pawn.health.hediffSet.HasHediff(instance.recipe.addsHediff))
            {
                // The surgery can fail, so we need this check.
                Handler_PawnNerveStapled.HandlePawnNerveStapled(pawn, billDoer);
            }  
        }
    }
}
