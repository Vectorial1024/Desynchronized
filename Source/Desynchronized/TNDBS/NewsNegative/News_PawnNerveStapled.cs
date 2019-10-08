using Desynchronized.Compatibility;
using Desynchronized.Compatibility.QEthics;
using Desynchronized.TNDBS.Datatypes;
using RimWorld;
using System;
using Verse;

namespace Desynchronized.TNDBS.NewsNegative
{
    public class News_PawnNerveStapled : TaleNewsNegativeIndividual
    {
        public News_PawnNerveStapled(Pawn victim, Pawn billDoer): base(victim, new InstigationInfo(billDoer))
        {
            if (billDoer == null)
            {
                throw new ArgumentNullException("Bill-doer cannot be null for News_PawnNerveStapled.");
            }
        }

        public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
        {
            return 3;
        }

        public override string GetNewsTypeName()
        {
            return "Pawn nerve-stapled";
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            // VERY IMPORTANT
            if (!ModDetector.QuestionableEthicsIsLoaded)
            {
                DesynchronizedMain.LogWarning("Attempted to transmit News_PawnNerveStapled but QuestionableEthics is not loaded! Cancelling.");
                return;
            }

            MemoryThoughtHandler handler = recipient.needs.mood.thoughts.memories;
            if (recipient == PrimaryVictim)
            {
                handler.TryGainMemory(QEthics_ThoughtDefOf.QE_RecentlyNerveStapled);
                handler.TryGainMemory(QEthics_ThoughtDefOf.QE_NerveStapledMe, Instigator);
            }
            else
            {
                // I'm a bystander.
                if (recipient.Faction == PrimaryVictim.Faction)
                {
                    handler.TryGainMemory(QEthics_ThoughtDefOf.QE_NerveStapledColonist);
                }
                else
                {
                    handler.TryGainMemory(QEthics_ThoughtDefOf.QE_NerveStapledPawn);
                }
            }
        }
    }
}
