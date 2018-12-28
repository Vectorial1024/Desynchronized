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
    [HarmonyPatch(typeof(ThoughtUtility))]
    [HarmonyPatch("GiveThoughtsForPawnOrganHarvested", MethodType.Normal)]
    public class PreFix_ThoughtUtil_OrganHarvests
    {
        [HarmonyPrefix]
        public static bool PreFix(Pawn victim)
        {
            Handler_PawnHarvested.HandlePawnHarvested(victim);

            // It's not we have anything left to process anymore or anything, baka.
            return false;
        }
    }
}
