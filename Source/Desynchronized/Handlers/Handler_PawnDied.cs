using Desynchronized.TaleLibrary;
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
             * Two possibilities:
             * 1. Victim died on the ground.
             * 2. Victim died while being carried around.
             */
            Map mapOfOccurence = victim.Corpse.Map ?? victim.CarriedBy.Map;
            if (mapOfOccurence == null)
            {
                return;
            }

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                if (other == victim)
                {
                    continue;
                }

                TaleNewsPawnDied news = new TaleNewsPawnDied(other, victim, dinfo, mapOfOccurence, PawnClassification.COLONIST, DeathBrutality.HUMANE);
                if (other.Map == mapOfOccurence)
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
