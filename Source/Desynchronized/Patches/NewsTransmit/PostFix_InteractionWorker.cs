﻿using Desynchronized.TNDBS.Utilities;
using Harmony;
using RimWorld;
using Verse;

namespace Desynchronized.Patches.NewsTransmit
{
    [HarmonyPatch(typeof(InteractionWorker))]
    [HarmonyPatch("Interacted", MethodType.Normal)]
    public class PostFix_InteractionWorker
    {
        [HarmonyPostfix]
        public static void ExecuteNewsTarnsmission(InteractionWorker __instance, Pawn initiator, Pawn recipient)
        {
            if (__instance is InteractionWorker_Chitchat chitchatWorker)
            {
                if (Rand.Value <= initiator.GetActualNewsSpreadChance())
                {
                    NewsSpreadUtility.SpreadNews(initiator, recipient, NewsSpreadUtility.SpreadMode.RANDOM);
                }
            }
            else if (__instance is InteractionWorker_DeepTalk deeptalkWorker)
            {
                if (Rand.Value <= initiator.GetActualNewsSpreadChance(5))
                {
                    NewsSpreadUtility.SpreadNews(initiator, recipient, NewsSpreadUtility.SpreadMode.DISTINCT);
                }
            }
        }
    }
}
