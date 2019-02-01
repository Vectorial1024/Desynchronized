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
    public class Handler_PawnDied
    {
        public static void HandlePawnDied(Pawn victim, DamageInfo? dinfo)
        {
            GenerateAndProcessNews(victim, dinfo);
        }

        /// <summary>
        /// The main difficulty lies in determining the correct thought to be given;
        /// there are so many methods out there that ultimately calls this method.
        /// </summary>
        /// <param name="victim"></param>
        /// <param name="dinfo"></param>
        private static void GenerateAndProcessNews(Pawn victim, DamageInfo? dinfo)
        {
            /*
             * Some possibilities:
             * 1. Victim died on the ground.
             * 2. Victim died while being carried around.
             */
            TaleNewsPawnDied news = TaleNewsPawnDied.GenerateGenerally(victim, dinfo);
            Map mapOfOccurence = victim.Map ?? victim.Corpse?.Map ?? victim.CarriedBy?.Map ?? null;
            if (mapOfOccurence == null)
            {
                return;
            }

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
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
