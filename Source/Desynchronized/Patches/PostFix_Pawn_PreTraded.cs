using Desynchronized.Handlers;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("PreTraded", MethodType.Normal)]
    public class PostFix_Pawn_PreTraded
    {
        [HarmonyPostfix]
        public static void PostFix(Pawn __instance, TradeAction action, Pawn playerNegotiator)
        {
            if (action == TradeAction.PlayerSells)
            {
                if (__instance.RaceProps.Humanlike)
                {
                    Handler_PawnSold.HandlePawnSold_ByTrade(__instance, playerNegotiator);
                }
            }
        }
    }
}
