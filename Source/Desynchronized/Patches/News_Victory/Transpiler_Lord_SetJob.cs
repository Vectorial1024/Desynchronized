using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Verse.AI.Group;

namespace Desynchronized.Patches.News_Victory
{
    [HarmonyPatch(typeof(Lord))]
    [HarmonyPatch("SetJob", MethodType.Normal)]
    public class Transpiler_Lord_SetJob
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> InjectFleeDetection(IEnumerable<CodeInstruction> instructions)
        {
            // Add additional assembly code after 9th callvirt
            int countCallVirt = 0;

            foreach (CodeInstruction instr in instructions)
            {
                yield return instr;

                if (instr.opcode == OpCodes.Callvirt)
                {
                    countCallVirt++;
                    if (countCallVirt == 9)
                    {
                        yield return new CodeInstruction(OpCodes.Ldloc_2);
                        yield return new CodeInstruction(OpCodes.Call, typeof(Transpiler_Lord_SetJob).GetMethod("ManipulateStateTransitionObject"));
                    }
                }
            }

            yield break;
        }

        private static void ManipulateStateTransitionObject(Transition transition)
        {
            transition.AddPostAction(new TransitionAction_Custom(delegate(Transition t)
            {
                // Do your stuff here.
            }));
        }
    }
}
