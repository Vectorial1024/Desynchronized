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
    /*
    /// <summary>
    /// Redirects code to hand-written ThoughtRedirector.GiveThoughtsAboutExecutionToPawn for maximum sanity
    /// </summary>
    [HarmonyPatch(typeof(ThoughtUtility))]
    [HarmonyPatch("GiveThoughtsForPawnExecuted", MethodType.Normal)]
    public class Transpiler_ThoughtUtil_PawnExecution
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            // Label labelContinue = generator.DefineLabel();
            bool patchIsComplete = false;
            short occurenceLdfld = 0;
            short ignoreCount = 0;

            //FileLog.Log("Beginning function log");
            foreach (CodeInstruction instruction in instructions)
            {
                if (!patchIsComplete && instruction.opcode == OpCodes.Ldfld)
                {
                    occurenceLdfld++;

                    if (occurenceLdfld == 2)
                    {
                        List<CodeInstruction> tempList = new List<CodeInstruction>
                        {
                            // Load victim
                            new CodeInstruction(OpCodes.Ldarg_0),
                            // Load def
                            new CodeInstruction(OpCodes.Ldloc_1),
                            // Load stage
                            new CodeInstruction(OpCodes.Ldloc_0),
                            new CodeInstruction(OpCodes.Call, typeof(ThoughtRedirector).GetMethod("GiveThoughtsAboutExecutionToPawn"))

                            /*
                            new CodeInstruction(OpCodes.Ldloc_2),
                            new CodeInstruction(OpCodes.Callvirt, typeof(Pawn).GetProperty("Map").GetGetMethod()),
                            new CodeInstruction(OpCodes.Ldarg_0),
                            new CodeInstruction(OpCodes.Callvirt, typeof(Pawn).GetProperty("Map").GetGetMethod()),
                            new CodeInstruction(OpCodes.Bne_Un, labelContinue),
                            // new CodeInstruction(OpCodes.Stloc_S, 4),
                            // new CodeInstruction(OpCodes.Ldloc_S, 4),
                            // new CodeInstruction(OpCodes.Brfalse_S, labelContinue)
                            
                        };

                        foreach (CodeInstruction temp in tempList)
                        {
                            // FileLog.Log(temp.ToString());
                            yield return temp;
                        }

                        ignoreCount = 9;
                        patchIsComplete = true;
                    }
                }

                if (ignoreCount > 0)
                {
                    instruction.opcode = OpCodes.Nop;
                    ignoreCount--;
                }
                
                /*
                if (!patchLoopLabelComplete && instruction.opcode == OpCodes.Ldloc_3)
                {
                    occurenceLdloc3++;
                    if (occurenceLdloc3 == 2)
                    {
                        instruction.labels.Add(labelContinue);
                        patchLoopLabelComplete = true;
                    }
                }

                //FileLog.Log(instruction.ToString());
                yield return instruction;
            }
            //FileLog.Log("Function log ends");
        }
    }
*/
}
