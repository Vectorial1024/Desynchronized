using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse.AI;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(MentalState_WanderConfused))]
    [HarmonyPatch("PostStart", MethodType.Normal)]
    public class PostFix_MentalBreakWanderConfused_Start
    {
        [HarmonyPostfix]
        public static void ForceForgetMemories(MentalState_WanderConfused __instance)
        {
            return;
            //__instance.pawn.GetNewsKnowledgeTracker().ForgetRandom();
        }
    }
}
