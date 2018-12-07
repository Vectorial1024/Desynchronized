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
    /// Pawns outside of the map should not know of deaths.
    /// This includes Bonded Pet Banishment too.
    /// </summary>
    [HarmonyPatch(typeof(PawnUtility))]
    [HarmonyPatch("ShouldGetThoughtAbout", MethodType.Normal)]
    public class PostFix_SpecialThoughtsUtil_RelationThoughts
    {
        [HarmonyPostfix]
        public static void PostFix(ref bool __result, Pawn pawn, Pawn subject)
        {
            // FileLog.Log("Pawn subject map " + subject.Map + "; other map " + pawn.Map);
            if (pawn.Map != subject.Map)
            {
                __result = false;
                // FileLog.Log("Map is different. Result is now " + __result);
            }
        }
    }
}
