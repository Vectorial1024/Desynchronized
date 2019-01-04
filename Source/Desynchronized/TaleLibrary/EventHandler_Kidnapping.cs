using Desynchronized.TNDBS;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    /*
    public class EventHandler_Kidnapping
    {
        /// <summary>
        /// This method is called when a kidnapped Colonist is about to be "thrown out of the Map"
        /// by the kidnapper.
        /// <para/>
        /// The kidnapper will leave the Map after this method call so it should
        /// be safe to use the kidnapper to refer to the Map.
        /// </summary>
        /// <param name="tale"></param>
        public static void HandleEventColonistKidnapped(Tale_DoublePawn tale)
        {
            Pawn kidnapper = tale.firstPawnData.pawn;
            Pawn victim = tale.secondPawnData.pawn;

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
        private static void GenerateAndProcessNews(Pawn victim, Pawn kidnapper)
        {
            TaleNewsPawnKidnapped news = new TaleNewsPawnKidnapped(victim, (InstigatorInfo) kidnapper);
            Map mapOfOccurence = kidnapper.Map;

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction)
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
    */
}
