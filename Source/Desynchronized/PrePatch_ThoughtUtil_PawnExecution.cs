using RimWorld;
using Verse;

namespace Desynchronized
{

    public class PrePatch_ThoughtUtil_PawnExecution
    {
        
        
        public static bool PrePatch_PawnExecutionThoughts(ref Pawn victim, ref PawnExecutionKind kind)
        {
            if (victim.RaceProps.Humanlike)
            {
                int forcedStage = 1;
                if (!victim.guilt.IsGuilty)
                {
                    switch (kind)
                    {
                        case PawnExecutionKind.GenericHumane:
                            forcedStage = 1;
                            break;
                        case PawnExecutionKind.GenericBrutal:
                            forcedStage = 2;
                            break;
                        case PawnExecutionKind.OrganHarvesting:
                            forcedStage = 3;
                            break;
                    }
                }
                else
                {
                    forcedStage = 0;
                }
                ThoughtDef def = (!victim.IsColonist) ? ThoughtDefOf.KnowGuestExecuted : ThoughtDefOf.KnowColonistExecuted;
                foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
                {
                    if (pawn.Map == victim.Map)
                    {
                        pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(def, forcedStage), null);
                    }
                }
            }

            return false;
        }
    }
}
