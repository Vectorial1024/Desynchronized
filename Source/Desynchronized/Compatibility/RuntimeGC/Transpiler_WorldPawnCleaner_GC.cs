using Harmony;
using RuntimeGC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace Desynchronized.Compatibility.RuntimeGC
{
    [HarmonyPatch(typeof(WorldPawnCleaner))]
    [HarmonyPatch("GC")]
    public class Transpiler_WorldPawnCleaner_GC
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ManipulateTalePawnList(IEnumerable<CodeInstruction> instructions)
        {
            // Find the 2nd "call" after the 6th "ldstr"
            // Insert a method after that position to manipulate the list.
            int occurencesLdstr = 0;
            int occurencesCall = 0;
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);
            int i = 0;

            while (occurencesLdstr < 6)
            {
                if (instructionList[i].opcode == OpCodes.Ldstr)
                {
                    occurencesLdstr++;
                }
                i++;
            }

            while (occurencesCall < 2)
            {
                if (instructionList[i].opcode == OpCodes.Call)
                {
                    occurencesCall++;
                }
                i++;
            }

            // i is at our insert position.
            // Insert new commands.
            List<CodeInstruction> insertionList = new List<CodeInstruction>();
            insertionList.Add(new CodeInstruction(OpCodes.Ldloc_1));
            insertionList.Add(new CodeInstruction(OpCodes.Call, typeof(TalePawnListManipulator).GetMethod("ManipulateListOfPawnsUsedByTales")));
            instructionList.InsertRange(i, insertionList);

            // Insertion complete.
            return instructionList;
        }
    }
}
