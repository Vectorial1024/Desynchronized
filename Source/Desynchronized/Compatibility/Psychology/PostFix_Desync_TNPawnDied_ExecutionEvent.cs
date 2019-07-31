﻿using Desynchronized.TNDBS;
using Harmony;
using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology
{
    [HarmonyPatch(typeof(TaleNewsPawnDied))]
    [HarmonyPatch("TryProcessAsExecutionEvent", MethodType.Normal)]
    public class PostFix_Desync_TNPawnDied_ExecutionEvent
    {
        public static bool Prepare()
        {
            return ModDetector.PsychologyIsLoaded;
        }

        [HarmonyPostfix]
        public static void ApplyPsychologyThoughts(TaleNewsPawnDied __instance, bool __result, Pawn recipient)
        {
            // Original method returns true if successfully identifying execution event.
            if (__result)
            {
                // Copied from Desynchronized code.
                int forcedStage = (int)__instance.BrutalityDegree;
                ThoughtDef thoughtToGive = __instance.Victim.IsColonist ? Psycho_ThoughtDefOf.KnowColonistExecutedBleedingHeart : Psycho_ThoughtDefOf.KnowGuestExecutedBleedingHeart;
                recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtToGive, forcedStage), null);
            }
        }
    }
}
