using Desynchronized.TaleLibrary;
using Desynchronized.TNDBS;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            TaleNewsPawnHarvested news = new TaleNewsPawnHarvested(victim);

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                if (DesynchronizedMain.WeAreInDevMode)
                {
                    FileLog.Log("Processing Pawn " + other + ", " + other.Name);
                }
                TaleNewsReference reference = news.CreateReferenceForReceipient(other);
                if (DesynchronizedMain.WeAreInDevMode)
                {
                    DesynchronizedMain.TaleNewsDatabaseSystem.AssociateNewsRefToPawn(other, reference);
                }

                // If in the same map, activate right away
                // otherwise, the social interaction code would take care of it
                if (other.Map == mapOfOccurence)
                {
                    reference.ActivateNews();
                }
            }
        }
    }
}
