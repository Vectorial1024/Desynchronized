using Desynchronized.TaleLibrary;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility))]
    [HarmonyPatch("TryGiveThoughts", MethodType.Normal)]
    [HarmonyPatch(new Type[] { typeof(Pawn), typeof(DamageInfo), typeof(PawnDiedOrDownedThoughtsKind) })]
    public class Prefix_ThoughtsUtility_GivingThoughts
    {
        private static List<IndividualThoughtToAdd> tmpIndividualThoughtsToAdd = new List<IndividualThoughtToAdd>();
        private static List<ThoughtDef> tmpAllColonistsThoughts = new List<ThoughtDef>();

        [HarmonyPrefix]
        public static bool PreFix(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
        {
            try
            {
                if (!PawnGenerator.IsBeingGenerated(victim) && Current.ProgramState == ProgramState.Playing)
                {
                    // We currently only handle Pawn Banishments here.
                    if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Banished || thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie)
                    {
                        bool banishmentIsDeadly = (thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie);

                        foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
                        {
                            if (other != victim)
                            {
                                TaleNewsColonistBanished news = new TaleNewsColonistBanished(other, victim, banishmentIsDeadly);

                                if (other.Map == victim.Map)
                                {
                                    news.ActivateAndGiveThoughts();
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception arg)
            {
                Log.Error("[V1024-DESYNC] Could not give thought, but vanilla thought-giving is not affected: " + arg, false);
            }

            return true;
        }
    }
}
