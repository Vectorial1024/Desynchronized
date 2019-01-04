using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    /*
     * Actually, we have already fully redirected from TryGiveThoughts,
     * so we can release this patch for better compatibility.
     */
    /// <summary>
    /// Pawns outside of the map should not know of deaths.
    /// This includes Bonded Pet Banishment too.
    /// </summary>
    /*
    [HarmonyPatch(typeof(PawnUtility))]
    [HarmonyPatch("ShouldGetThoughtAbout", MethodType.Normal)]
    public class PostFix_SpecialThoughtsUtil_RelationThoughts
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="__result"></param>
        /// <param name="pawn">The Other Pawn</param>
        /// <param name="subject">The This Pawn</param>
        [HarmonyPostfix]
        public static void PostFix(ref bool __result, Pawn pawn, Pawn subject)
        {
            // Warning: must also check if This pawn is currently being carried when he died
            Map mapOfOccurence;
            // That satisfying moment of compressing code tho
            // Assignments return the value that is assigned, so it is possible to chain it up with a conditional
            if ((mapOfOccurence = subject.Map) == null)
            {
                // I have seen this kind of usage of the ? operator in X Rebirth's Mission Director.
                mapOfOccurence = subject.CarriedBy?.Map;
            }
            // Can still be null.
            // But then, if it is still null, then something must have bugged out.
            // Or maybe Giddy Up will result in these bizarre situations?
            // We don't god-damn care.
            if (pawn.Map != mapOfOccurence)
            {
                __result = false;
            }
        }
    }
    */
}
