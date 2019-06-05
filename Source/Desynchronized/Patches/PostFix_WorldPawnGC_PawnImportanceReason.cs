using Harmony;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(WorldPawnGC))]
    [HarmonyPatch("GetCriticalPawnReason", MethodType.Normal)]
    public class PostFix_WorldPawnGC_PawnImportanceReason
    {
        [HarmonyPostfix]
        public static void PostFix(Pawn pawn, ref string __result)
        {
            // DesynchronizedMain.LogError("Requesting GC status for pawn " + pawn + "; game gave reason of " + __result);
            if (__result == null && pawn != null && DesynchronizedMain.TaleNewsDatabaseSystem.PawnIsInvolvedInSomeTaleNews(pawn))
            {
                // DesynchronizedMain.LogError("This pawn should be reserved because TNDBS is referencing it.");
                __result = DesynchronizedMain.Desync_PawnIsReferencedString;
            }
        }
    }
}
