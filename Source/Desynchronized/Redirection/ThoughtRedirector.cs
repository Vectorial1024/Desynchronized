using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Redirection
{
    public class ThoughtRedirector
    {
        /// <summary>
        /// A redirection from ThoughtUtility.GiveThoughtsForPawnExecuted,
        /// as prepared in Transpiler_ThoughtUtil_PawnExecution.
        /// Colonists outside of the map should not know of Executions.
        /// This style sacrifices compatibility for better readability.
        /// </summary>
        /// <param name="receipient">The pawn that should receive the ThoughtDef</param>
        /// <param name="victim">The pawn of this Execution</param>
        /// <param name="def">The correct ThoughtDef as determined by ThoughtUtility</param>
        /// <param name="stage">Stage index, required to genearte the correct thought details</param>
        public static void GiveThoughtsAboutExecutionToPawn(Pawn receipient, Pawn victim, ThoughtDef def, int stage)
        {
            if (true)
            {
                receipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(def, stage), null);
            }
        }

        /// <summary>
        /// A redirection from PawnDiedOrDownedThoughtsUtility.TryGiveThoughts,
        /// as constructed in Transpiler_SpecialThoughtsUtil_DiedOrDowned.
        /// Colonists outside of the map should not know of Banishments.
        /// This style sacrifices compatibility for better readability.
        /// </summary>
        /// <param name="receipient">The pawn that should receive the ThoughtDef</param>
        /// <param name="victim">The pawn that the ThoughtDef is about; should not be null to guarantee functionality</param>
        /// <param name="def">The actual ThoughtDef to be added</param>
        public static void GiveThoughtsAboutBanishmentToPawn(Pawn receipient, Pawn victim, ThoughtDef def)
        {
            if (def == ThoughtDefOf.ColonistBanished || def == ThoughtDefOf.ColonistBanishedToDie || def == ThoughtDefOf.PrisonerBanishedToDie)
            {
                if (receipient.Map == victim.Map)
                {
                    receipient.needs.mood.thoughts.memories.TryGainMemory(def, null);
                    return;
                }
            }
            else
            {
                // Basically if (def == ThoughtDefOf.BondedAnimalBanished)
                // The original line of code that is used by the game.
                receipient.needs.mood.thoughts.memories.TryGainMemory(def, null);
            }
        }

        /// <summary>
        /// A redirection from GenGuest.AddPrisonerSoldThoughts,
        /// as constructed in Transpiler_GenGuest_AddPrisonerSoldThoughts.
        /// Colonists and Prisoners outside of the map should not know of Prisoner Sales.
        /// </summary>
        /// <param name="receipient">The pawn: either a Colonist or a Prisoner</param>
        /// <param name="prisoner">The prisoner that was sold; currently unused, but I see potential.</param>
        public static void GiveThoughtsAboutPrisonerSalesToPawn(Pawn receipient, Pawn prisoner)
        {
            /*
             * TODO: Add traits that can affect "Prisoner Sold" thought.
             * Example could be "My rival [] was sold: +5" or something.
             * Yeah. For the Rim is dark and full of terrors.
             */
            if (receipient.Map == prisoner.Map)
            {
                receipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerSold, null);
            }
        }
    }
}
