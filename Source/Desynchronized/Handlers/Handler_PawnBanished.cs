using Desynchronized.TNDBS;
using RimWorld;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnBanished
    {
        public static void HandlePawnBanished(Pawn victim, bool banishmentIsDeadly)
        {
            SendOutNotificationLetter(victim);
            GenerateAndProcessNews(victim, banishmentIsDeadly);
        }

        private static void SendOutNotificationLetter(Pawn victim)
        {
            // string letterLabel = "Kidnapped".Translate() + ": " + victim.LabelShortCap;
            // string letterContent = string.Empty;
            // letterContent += "PawnKidnapped".Translate(victim.LabelShort.CapitalizeFirst(), kidnapper.Faction.def.pawnsPlural, kidnapper.Faction.Name, victim.Named("PAWN"));

            Find.LetterStack.ReceiveLetter("Colonist banished", "Colonist banished. Name of Colonist: " + victim.Name, LetterDefOf.NegativeEvent, victim, null, null);
        }

        private static void GenerateAndProcessNews(Pawn victim, bool banishmentIsDeadly)
        {
            Map mapOfOccurence = victim.Map;
            if (mapOfOccurence == null)
            {
                return;
            }

            TaleNewsPawnBanished banishmentNews = new TaleNewsPawnBanished(victim, banishmentIsDeadly);

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                if (other.Map == mapOfOccurence)
                {
                    other.GetNewsKnowledgeTracker().KnowNews(banishmentNews);
                }
            }
        }
    }
}
