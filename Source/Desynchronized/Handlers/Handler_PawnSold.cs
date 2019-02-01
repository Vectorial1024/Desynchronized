using Desynchronized.TNDBS;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnSold
    {
        [Obsolete("", true)]
        public static void HandlePawnSold(Pawn victim)
        {
            SendOutNotificationLetter(victim);
            GenerateAndProcessNews(victim);
        }

        public static void HandlePawnSold_ByTrade(Pawn victim, Pawn negotiator)
        {
            TaleNewsPawnSold news = new TaleNewsPawnSold(victim, (InstigatorInfo) negotiator);
            SendOutNotificationLetter(victim);
            DistributeNews(news, negotiator.Map);
        }

        public static void HandlePawnSold_ByGiftingViaPods(Pawn victim, Map mapOfSender)
        {
            TaleNewsPawnSold news = new TaleNewsPawnSold(victim);
            SendOutNotificationLetter(victim);
            DistributeNews(news, mapOfSender);
        }

        private static void SendOutNotificationLetter(Pawn victim)
        {
            /*
            string letterLabel = "Kidnapped".Translate() + ": " + victim.LabelShortCap;
            string letterContent = string.Empty;
            letterContent += "PawnKidnapped".Translate(victim.LabelShort.CapitalizeFirst(), kidnapper.Faction.def.pawnsPlural, kidnapper.Faction.Name, victim.Named("PAWN"));
            */

            // It is a bit difficult; we need to determine to what extent our letters should cover.
            // Right now, I have thought that Bonded Sold and Prisoner Sold would work well.

            Find.LetterStack.ReceiveLetter("Pawn Sold", "A Pawn of your own has been sold away. Pawn name: " + victim.Name, LetterDefOf.NeutralEvent, LookTargets.Invalid, null, null);
        }

        /// <summary>
        /// A change in code structure results in this slight variation from the usual.
        /// </summary>
        /// <param name="soldNews">The TaleNews object for this sales.</param>
        /// <param name="mapOfOccurence">The Map where the sales occured/was initiated.</param>
        private static void DistributeNews(TaleNewsPawnSold soldNews, Map mapOfOccurence)
        {
            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                if (other.Map == mapOfOccurence)
                {
                    soldNews.ActivateForReceipient(other);
                }
            }
        }

        /// <summary>
        /// Generate a TaleNews for everybody.
        /// I have no better idea right now, so this would have to suffice.
        /// </summary>
        /// <param name="victim"></param>
        private static void GenerateAndProcessNews(Pawn victim)
        {
            TaleNewsPawnSold news = new TaleNewsPawnSold(victim);
            // TaleNewsPawnKidnapped news = new TaleNewsPawnKidnapped(victim, (InstigatorInfo)kidnapper);
            Map mapOfOccurence = victim.Map;

            // The criteria for activating this thought is not foolproof, will need work later
            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                TaleNewsReference reference = news.CreateReferenceForReceipient(other);
                DesynchronizedMain.TaleNewsDatabaseSystem.LinkNewsReferenceToPawn(reference, other);

                // If the news can be given directly, do so, else store it "somewhere else".
                if (other.Map == mapOfOccurence)
                {
                    reference.ActivateNews();
                }
            }
        }
    }
}
