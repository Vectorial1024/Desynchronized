using Desynchronized.TNDBS;
using Desynchronized.TNDBS.Utilities;
using RimWorld;
using System;
using Verse;

namespace Desynchronized.Handlers
{
    /// <summary>
    /// You must signal this class before instructing it to handle the kidnap event so that the correct response can be given.
    /// </summary>
    public class Handler_PawnKidnapped
    {
        private static Map mapOfOccurence = null;
        private static Faction kidnappingFaction = null;
        private static bool? shouldEmitLetter = null;
        private static bool flagIsDuringOffensiveBattle = false;

        [Obsolete("", true)]
        public static void Signal_DefensiveBattle_PawnKidnapped(Map map)
        {
            mapOfOccurence = map;
            shouldEmitLetter = true;
        }

        public static void Signal_OffensiveBattle_BeginBlock(Map map)
        {
            //DesynchronizedMain.LogError("Receiving map: " + map);
            // The game has already implemented the "Caravan lost" letter, so we don't have to re-invent the wheel.
            mapOfOccurence = map;
            //DesynchronizedMain.LogError("Map is now: " + mapOfOccurence);
            flagIsDuringOffensiveBattle = true;
        }

        public static void Signal_OffensiveBattle_EndBlock()
        {
            flagIsDuringOffensiveBattle = false;
        }

        public static void HandlePawnKidnapped(Pawn victim, Pawn kidnapper)
        {
            /*
            if (!shouldEmitLetter.HasValue)
            {
                DesynchronizedMain.LogError("Fatal error: caller of method did not specify mode of operation. " + Environment.StackTrace);
                return;
            }
            */

            // Debug flushing
            
            //DesynchronizedMain.LogError("Kidnapper: " + kidnapper);

            // mapOfOccurence should already have been determined before calling this.
            DetermineVariables(kidnapper);

            // DesynchronizedMain.LogError("Map of occurence: " + mapOfOccurence);

            if (!flagIsDuringOffensiveBattle)
            {
                // Vanilla already generates its own letter, so no need to reinvent the wheel.
                SendOutNotificationLetter(victim);
            }
            GenerateAndProcessNews(victim, kidnapper);
        }

        private static void DetermineVariables(Pawn kidnapper)
        {
            // Confirm the faction that is responsible for this kidnap
            if (flagIsDuringOffensiveBattle)
            {
                // This is during an offensive battle, the map owner is responsible.
                // mapOfOccurence should have been set in the Signal_BeginBlock function
                kidnappingFaction = mapOfOccurence.ParentFaction;
            }
            else
            {
                // This is during a defensive battle, the kidnapper's faction is responsible.
                mapOfOccurence = kidnapper.MapHeld;
                kidnappingFaction = kidnapper.Faction;
            }
        }

        private static void SendOutNotificationLetter(Pawn victim)
        {
            string letterLabel = "Kidnapped".Translate() + ": " + victim.LabelShortCap;
            string letterContent = string.Empty;
            letterContent += "PawnKidnapped".Translate(victim.LabelShort.CapitalizeFirst(), kidnappingFaction.def.pawnsPlural, kidnappingFaction.Name, victim.Named("PAWN"));

            Find.LetterStack.ReceiveLetter(letterLabel, letterContent, LetterDefOf.NegativeEvent, LookTargets.Invalid);
        }

        [Obsolete]
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
            // Confirm instigator, hence the TaleNews to be transmitted
            TaleNewsPawnKidnapped newsPawnKidnapped = null;
            if (kidnapper == null)
            {
                // Pawns lost during offensive battles will have null kidnapper
                newsPawnKidnapped = new TaleNewsPawnKidnapped(victim, kidnappingFaction);
            }
            else
            {
                // Pawns lost during defensive battles will have non-null kidnapper
                newsPawnKidnapped = new TaleNewsPawnKidnapped(victim, kidnapper);
            }

            // Distribute news; quite standard procedure
            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                // DesynchronizedMain.LogError("Parsing " + other.Name + "; he is in map " + other.MapHeld);

                // No special thoughts for victims for now
                if (other == victim)
                {
                    continue;
                }

                if (other.Map == mapOfOccurence)
                {
                    other.GetNewsKnowledgeTracker().KnowNews(newsPawnKidnapped, WitnessShockGrade.SAME_MAP);
                }
            }
        }
    }
}
