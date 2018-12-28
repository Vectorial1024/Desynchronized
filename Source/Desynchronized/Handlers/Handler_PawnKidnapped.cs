using Desynchronized.TaleLibrary;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                // Victims are irrelevant in this
                if (other == victim)
                {
                    continue;
                }
                if (!other.IsCapableOfThought())
                {
                    continue;
                }

                // Generate the TaleNews first;
                TaleNewsPawnKidnapped news = new TaleNewsPawnKidnapped(other, victim, kidnapper);
                // If the news can be given directly, do so, else store it "somewhere else".
                if (other.Map == kidnapper.Map)
                {
                    news.ActivateAndGiveThoughts();
                    if (false && DesynchronizedMain.WeAreInDevMode)
                    {
                        DesynchronizedMain.CentralTaleDatabase.ReceivedNews.AddTaleNews(news);
                        // FileLog.Log("Receipient [" + other.Name + "] is in the same Map. The relevant Thoughts should already have been given.");
                    }
                }
                else
                {
                    if (false && DesynchronizedMain.WeAreInDevMode)
                    {
                        DesynchronizedMain.CentralTaleDatabase.OutstandingNews.AddTaleNewsToPending(news);
                        // FileLog.Log("Receipient [" + other.Name + "] is NOT in the same Map. Already added to the Pending List.");
                    }
                }
            }
        }
    }
}
