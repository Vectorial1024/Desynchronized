using Desynchronized.TaleLibrary;
using Desynchronized.TNDBS;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnExecuted
    {
        public static void HandlePawnExecuted(Pawn deadman, DeathBrutality brutality)
        {
            SendOutNotificationLetter(deadman);
            GenerateAndProcessNews(deadman, brutality);
        }

        private static void SendOutNotificationLetter(Pawn deadman)
        {
            // string letterLabel = "Kidnapped".Translate() + ": " + victim.LabelShortCap;
            // string letterContent = string.Empty;
            // letterContent += "PawnKidnapped".Translate(victim.LabelShort.CapitalizeFirst(), kidnapper.Faction.def.pawnsPlural, kidnapper.Faction.Name, victim.Named("PAWN"));

            Find.LetterStack.ReceiveLetter("Colonist/Guest executed", "Colonist/Guest was executed. Name of Pawn: " + deadman.Name, LetterDefOf.NegativeEvent, deadman, null, null);
        }

        private static void GenerateAndProcessNews(Pawn deadman, DeathBrutality brutality)
        {
            Map mapOfOccurence = deadman.Corpse.Map;
            TaleNewsPawnDied news = TaleNewsPawnDied.GenerateAsExecution(deadman, brutality);

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                TaleNewsReference reference = news.CreateReferenceForReceipient(other);
                DesynchronizedMain.TaleNewsDatabaseSystem.LinkNewsReferenceToPawn(reference, other);

                if (other.Map == mapOfOccurence)
                {
                    reference.ActivateNews();
                }
            }
        }
    }
}
