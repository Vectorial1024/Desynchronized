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
        [HarmonyPostfix]
        public static void PostFix(InteractionWorker __instance, Pawn initiator, Pawn recipient)
        {
            if (DesynchronizedMain.NewsSpreadIsActive)
            {
                if (__instance is InteractionWorker_Chitchat chitchatWorker)
                {
                    if (Rand.Value <= initiator.GetActualNewsSpreadChance() || true)
                    {
                        NewsSpreadUtility.SpreadNews(initiator, recipient, NewsSpreadUtility.SpreadMode.RANDOM);
                        // DesynchronizedMain.LogError(initiator + " has chit-chat with " + recipient);
                    }
                }
                else if (__instance is InteractionWorker_DeepTalk deeptalkWorker)
                {
                    if (Rand.Value <= initiator.GetActualNewsSpreadChance(3) || true)
                    {
                        NewsSpreadUtility.SpreadNews(initiator, recipient, NewsSpreadUtility.SpreadMode.DISTINCT);
                        // DesynchronizedMain.LogError(initiator + " has deep talk with " + recipient);
                    }
                }
            }
        }
    }
}
