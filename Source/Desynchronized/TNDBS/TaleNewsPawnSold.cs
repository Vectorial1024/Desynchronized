using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class TaleNewsPawnSold : TaleNewsNegativeIndividual
    {
        public TaleNewsPawnSold()
        {

        }

        public TaleNewsPawnSold(Pawn victim): this(victim, InstigatorInfo.NoInstigator)
        {

        }

        public TaleNewsPawnSold(Pawn victim, InstigatorInfo info): base (victim, info)
        {

        }

        public override string GetNewsIdentifier()
        {
            return "Pawn Sold";
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            if (!recipient.IsCapableOfThought())
            {
                return;
            }

            if (recipient == PrimaryVictim)
            {
                // I was sold
            }
            else
            {
                // Animal or Prisoner sold
                if (PrimaryVictim.RaceProps.Animal)
                {
                    // ADDITIONAL TODO
                }
                else if (PrimaryVictim.RaceProps.Humanlike)
                {
                    // Some prisoner was sold
                    if (PrimaryVictim.IsPrisonerOfColony)
                    {
                        recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerSold, Instigator);
                    }
                }

                // Relationship was sold, if there is any
                PawnRelationDef relation = recipient.GetMostImportantRelation(PrimaryVictim);
                if (relation != null)
                {
                    ThoughtDef soldThought = relation.soldThought;
                    if (soldThought != null)
                    {
                        recipient.needs.mood.thoughts.memories.TryGainMemory(soldThought, Instigator);
                    }
                }
            }
        }
    }
}
