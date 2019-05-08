using Desynchronized.TNDBS;
using Desynchronized.TNDBS.Utilities;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnKidnapped
    {
        public static void HandlePawnKidnapped(Pawn victim, Pawn kidnapper)
        {
            SendOutNotificationLetter(victim, kidnapper);
            GenerateAndProcessNews(victim, kidnapper);
        }

        private static void SendOutNotificationLetter(Pawn victim, Pawn kidnapper)
        {
            string letterLabel = "Kidnapped".Translate() + ": " + victim.LabelShortCap;
            string letterContent = string.Empty;
            letterContent += "PawnKidnapped".Translate(victim.LabelShort.CapitalizeFirst(), kidnapper.Faction.def.pawnsPlural, kidnapper.Faction.Name, victim.Named("PAWN"));

            Find.LetterStack.ReceiveLetter(letterLabel, letterContent, LetterDefOf.NegativeEvent, LookTargets.Invalid, null, null);
        }

        /// <summary>
        /// Generate a TaleNews for everybody.
        /// I have no better idea right now, so this would have to suffice.
        /// </summary>
        /// <param name="victim"></param>
        /// <param name="kidnapper"></param>
        /// 
        private static void GenerateAndProcessNews(Pawn victim, Pawn kidnapper)
        {
            // The map can't possibly be null;
            // I don;t think the player can abandon the map in the middle/at the end of a firefight.
            Map mapOfOccurence = kidnapper.Map;
            TaleNewsPawnKidnapped kidnapNews = new TaleNewsPawnKidnapped(victim, kidnapper);

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                // Victims are irrelevant in this
                if (other == victim)
                {
                    continue;
                }

                if (other.Map == mapOfOccurence)
                {
                    other.GetNewsKnowledgeTracker().KnowNews(kidnapNews, WitnessShockGrade.UNDEFINED);
                }
            }
        }
    }
}
