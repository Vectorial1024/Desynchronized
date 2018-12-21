using Desynchronized.TaleLibrary;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized
{
    /*
    public class PawnKidnappedThoughtsUtility
    {
        /// <summary>
        /// A rudimentary method to give thoughts to the pawns in the map before the victim actually leaves the map.
        /// Used in a transpilation of some Kidnapping classes.
        /// </summary>
        /// <param name="victim">The pawn (Colonist) that is being kidnapped.</param>
        /// <param name="kidnapper">The pawn (Raider) that is kidnapping the victim.</param>
        [Obsolete("Might soon be deprecated, but marked right now for later convenience. Refer to the AddTale PostFix for more details/updates")]
        public static void OnPawnAboutToBeKidnapped_Rudimentary(Pawn victim, Pawn kidnapper)
        {
            SendOutNotificationLetter(victim, kidnapper);

            // Ah screw this. I have been using a wrong method to approach this.
            // Reverting to standard algorithm now.
            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                if (other.Map == kidnapper.Map)
                {
                    other.needs.mood.thoughts.memories.TryGainMemory(Desynchronized_ThoughtDefOf.KnowColonistKidnapped);
                }
            }
        }

        private static void SendOutNotificationLetter(Pawn victim, Pawn kidnapper)
        {
            string letterLabel = "Kidnapped".Translate() + ": " + victim.LabelShortCap;
            string letterContent = string.Empty;
            letterContent += "PawnKidnapped".Translate(victim.LabelShort.CapitalizeFirst(), kidnapper.Faction.def.pawnsPlural, kidnapper.Faction.Name, victim.Named("PAWN"));

            Find.LetterStack.ReceiveLetter(letterLabel, letterContent, LetterDefOf.NegativeEvent, LookTargets.Invalid, null, null);
        }

        private static void RecordEvent(Pawn victim, Pawn kidnapper)
        {
            Tale tale = Find.TaleManager.GetLatestTale(TaleDefOf.KidnappedColonist, kidnapper);
            FileLog.Log("We have retrieved tale for victim [" + victim.Name + "]: [" + tale + "]");
            DesynchronizedMain.InfoKnowStorage.KidnappedPawns.Add(victim);
            foreach (Pawn pawn in DesynchronizedMain.InfoKnowStorage.KidnappedPawns)
            {
                FileLog.Log("Currently, pawn [" + pawn.Name + "] is kidnapped.");
            }
        }
    }
    */
}
