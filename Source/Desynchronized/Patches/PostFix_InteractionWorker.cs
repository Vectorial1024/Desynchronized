using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(InteractionWorker))]
    [HarmonyPatch("Interacted", MethodType.Normal)]
    public class PostFix_InteractionWorker
    {
        [HarmonyPostfix]
        public static void PostFix(InteractionWorker __instance, Pawn initiator, Pawn recipient)
        {
            if (__instance is InteractionWorker_Chitchat chitchatWorker)
            {

            }
            else if (__instance is InteractionWorker_DeepTalk deeptalkWorker)
            {

            }
        }
    }
}
