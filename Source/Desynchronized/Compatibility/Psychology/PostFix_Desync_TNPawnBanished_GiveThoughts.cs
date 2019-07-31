﻿using Desynchronized.TNDBS;
using Harmony;
using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.Psychology
{
    [HarmonyPatch(typeof(TaleNewsPawnBanished))]
    [HarmonyPatch("GiveThoughtsToReceipient", MethodType.Normal)]
    public class PostFix_Desync_TNPawnBanished_GiveThoughts
    {
        public static bool Prepare()
        {
            return ModDetector.PsychologyIsLoaded;
        }

        [HarmonyPostfix]
        public static void ApplyPsychologyThoughts_BleedingHeart(TaleNewsPawnBanished __instance, Pawn recipient)
        {
            Pawn banishmentVictim = __instance.BanishmentVictim;

            if (banishmentVictim.RaceProps.Humanlike && recipient != banishmentVictim)
            {
                ThoughtDef thoughtDefToGain = null;
                if (!banishmentVictim.IsPrisonerOfColony)
                {
                    if (__instance.IsDeadly)
                    {
                        thoughtDefToGain = Psycho_ThoughtDefOf.ColonistAbandonedToDieBleedingHeart;
                    }
                    else
                    {
                        thoughtDefToGain = Psycho_ThoughtDefOf.ColonistAbandonedBleedingHeart;
                    }
                }
                else
                {
                    if (__instance.IsDeadly)
                    {
                        thoughtDefToGain = Psycho_ThoughtDefOf.PrisonerAbandonedToDieBleedingHeart;
                    }
                }

                recipient.needs.mood.thoughts.memories.TryGainMemory(thoughtDefToGain, banishmentVictim);
            }
        }
    }
}
