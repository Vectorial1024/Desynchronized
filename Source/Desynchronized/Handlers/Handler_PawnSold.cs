using Desynchronized.TNDBS;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnSold
    {
        public static void HandlePawnSold(Pawn victim)
        {
            SendOutNotificationLetter(victim);
            GenerateAndProcessNews(victim);
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
                // If the news can be given directly, do so, else store it "somewhere else".
                if (other.Map == mapOfOccurence)
                {
                    reference.ActivateNews();
                }
            }
        }
    }
}
