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
    [HarmonyPatch(typeof(ThoughtUtility))]
    [HarmonyPatch("GiveThoughtsForPawnOrganHarvested", MethodType.Normal)]
    public class Transpiler_ThoughtUtil_OrganHarvesting
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            // Some preparation
            LocalBuilder tempBool = generator.DeclareLocal(typeof(bool));
            Label loopContinue = generator.DefineLabel();
            bool patchMapCheckComplete = false;
            bool patchLoopLabelComplete = false;
            short occurenceLdloc2 = 0;

            //FileLog.Log("Beginning function log");
            foreach (CodeInstruction instruction in instructions)
            {
                // Find the 1st and only ldloc1
                if (!patchMapCheckComplete && instruction.opcode == OpCodes.Ldloc_1)
                {
                    patchMapCheckComplete = true;

                    List<CodeInstruction> tempList = new List<CodeInstruction>
                    {
                        new CodeInstruction(OpCodes.Ldloc_1),
                        new CodeInstruction(OpCodes.Callvirt, typeof(Pawn).GetProperty("Map").GetGetMethod()),
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Callvirt, typeof(Pawn).GetProperty("Map").GetGetMethod()),
                        new CodeInstruction(OpCodes.Ceq),
                        new CodeInstruction(OpCodes.Stloc_S, 3),
                        new CodeInstruction(OpCodes.Ldloc_S, 3),
                        new CodeInstruction(OpCodes.Brfalse_S, loopContinue)
                    };
                    
                    foreach (CodeInstruction temp in tempList)
                    {
                        // FileLog.Log(temp.ToString());
                        yield return temp;
                    }
                }
                if (!patchLoopLabelComplete && instruction.opcode == OpCodes.Ldloc_2)
                {
                    occurenceLdloc2++;
                    if (occurenceLdloc2 == 2)
                    {
                        instruction.labels.Add(loopContinue);
                        patchLoopLabelComplete = true;
                    }
                }
                // FileLog.Log(instruction.ToString());
                yield return instruction;
            }
            // FileLog.Log("Function log ends");
        }
    }
}
