using Desynchronized.TaleLibrary;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnBanishment
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
            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                if (other != victim)
                {
                    TaleNewsPawnBanished news = new TaleNewsPawnBanished(other, victim, banishmentIsDeadly);

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
