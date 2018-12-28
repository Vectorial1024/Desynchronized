using Desynchronized.Handlers;
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
                    // This is the main redirection for our patches
                    switch (thoughtsKind)
                    {
                        case PawnDiedOrDownedThoughtsKind.Downed:
                            // There is potential here, simply wasted.
                            return false;
                        case PawnDiedOrDownedThoughtsKind.Died:
                            return true;
                        case PawnDiedOrDownedThoughtsKind.BanishedToDie:
                            Handler_PawnBanishment.HandlePawnBanished(victim, true);
                            return false;
                        case PawnDiedOrDownedThoughtsKind.Banished:
                            Handler_PawnBanishment.HandlePawnBanished(victim, false);
                            return false;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }
            catch (Exception arg)
            {
                Log.Error("[V1024-DESYNC] Could not give thought. Depending on the exact thought type, vanilla thought-giving might not be affected: " + arg, false);
            }

            return true;
        }

        private static void HandleBanishmentThoughts(Pawn banishmentVictim, bool deadlyBanishment)
        {
            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                if (other != banishmentVictim)
                {
                    TaleNewsPawnBanished news = new TaleNewsPawnBanished(other, banishmentVictim, deadlyBanishment);

                    if (other.Map == banishmentVictim.Map)
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
