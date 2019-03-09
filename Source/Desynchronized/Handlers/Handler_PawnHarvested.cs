using Desynchronized.TNDBS;
using Harmony;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnHarvested
    {
        public static void HandlePawnHarvested(Pawn victim)
        {
            // No need to send out letters, the player has full control of the entire operation.
            GenerateAndProcessNews(victim);
        }

        private static void GenerateAndProcessNews(Pawn victim)
        {
            // Definitely has potential here.
            if (!victim.RaceProps.Humanlike)
            {
                return;
            }

            // There is no certainty to say that the victim is dead;
            // Removing one of the two lungs will still trigger this.
            // Had better also check for its corpse.
            Map mapOfOccurence = victim.Map ?? victim.Corpse.Map;
            if (mapOfOccurence == null)
            {
                return;
            }

            TaleNewsPawnHarvested harvestNews = new TaleNewsPawnHarvested(victim);

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                if (other.Map == mapOfOccurence)
                {
                    other.GetNewsKnowledgeTracker().KnowNews(harvestNews);
                }
            }
        }
    }
}
