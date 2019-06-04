using Desynchronized.TNDBS;
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
        //private static int debuggingChitChatCount = 0;
        //private static int debuggingDeepTalkCount = 0;

        [HarmonyPostfix]
        public static void PostFix(InteractionWorker __instance, Pawn initiator, Pawn recipient)
        {
            if (true)
            //if (DesynchronizedMain.NewsSpreadIsActive)
            {
                if (__instance is InteractionWorker_Chitchat chitchatWorker)
                {
                    // debuggingChitChatCount++;
                    if (Rand.Value <= initiator.GetActualNewsSpreadChance())
                    {
                        NewsSpreadUtility.SpreadNews(initiator, recipient, NewsSpreadUtility.SpreadMode.RANDOM);
                        // DesynchronizedMain.LogError(initiator + " has chit-chat with " + recipient + "; it has been the " + debuggingChitChatCount + "th chit-chat so far.");
                    }
                }
                else if (__instance is InteractionWorker_DeepTalk deeptalkWorker)
                {
                    if (Rand.Value <= initiator.GetActualNewsSpreadChance(5))
                    {
                        NewsSpreadUtility.SpreadNews(initiator, recipient, NewsSpreadUtility.SpreadMode.DISTINCT);
                        // DesynchronizedMain.LogError(initiator + " has deep talk with " + recipient + "; it has been the " + debuggingDeepTalkCount + "th deep-talk so far.");
                    }
                }
            }
        }
    }
}
