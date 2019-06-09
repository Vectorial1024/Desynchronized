using Desynchronized.TNDBS;
using Desynchronized.TNDBS.Utilities;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnDied
    {
        public static void HandlePawnDied(Pawn victim, DamageInfo? dinfo)
        {
            GenerateAndProcessNews(victim, dinfo);
        }

        /// <summary>
        /// Handles a pawn death situation.
        /// </summary>
        public static void HandlePawnDied(Pawn victim, DamageInfo? info, Hediff culpritHediff)
        {

        }

        /// <summary>
        /// The main difficulty lies in determining the correct thought to be given;
        /// there are so many methods out there that ultimately calls this method.
        /// </summary>
        /// <param name="victim"></param>
        /// <param name="dinfo"></param>
        private static void GenerateAndProcessNews(Pawn victim, DamageInfo? dinfo)
        {
            /*
             * Some possibilities:
             * 1. Victim died on the ground.
             * 2. Victim died while being carried around.
             * 
             * [General Case] ?? [General Case (victim died before code)] ?? [Victim died in Embrace (ewww)]
             */
            Map mapOfOccurence = victim.Map ?? victim.Corpse?.Map ?? victim.CarriedBy?.Map;
            if (mapOfOccurence == null)
            {
                return;
            }

            TaleNewsPawnDied deathNews = TaleNewsPawnDied.GenerateGenerally(victim, dinfo);

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                if (other.Map == mapOfOccurence)
                {
                    // Temp code for testing.
                    other.GetNewsKnowledgeTracker().KnowNews(deathNews, WitnessShockGrade.NEARBY_WITNESS);
                }
            }
        }

        private static void GenerateAndProcessNews(Pawn victim, DamageInfo? dinfo, Hediff culpritHediff)
        {

        }
    }
}
