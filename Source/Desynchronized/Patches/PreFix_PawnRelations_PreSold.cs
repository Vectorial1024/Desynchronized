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
    /// <summary>
    /// Patches for the case when the player sells a Prisoner
    /// </summary>
    [HarmonyPatch(typeof(Pawn_RelationsTracker))]
    [HarmonyPatch("Notify_PawnSold", MethodType.Normal)]
    public class PreFix_PawnRelations_PreSold
    {
        [HarmonyPrefix]
        public static bool PreFix(Pawn_RelationsTracker __instance, Pawn playerNegotiator)
        {
            // Find out the underlying pawn of this tracker exhausively
            foreach (Pawn potential in Find.WorldPawns.AllPawnsAlive)
            {
                if (potential.relations == __instance)
                {
                    Handler_PawnSold.HandlePawnSold_ByTrade(potential, playerNegotiator);
                    return false;
                }
            }

            DesynchronizedMain.LogError("Failed to determine owner of Pawn_RelationsTracker. Falling back to vanilla behavior.");
            return true;
        }
    }
}
