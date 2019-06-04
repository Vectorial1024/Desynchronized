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
        public Faction tradeDeal_OtherParty;

        public TaleNewsPawnSold()
        {

        }

        public TaleNewsPawnSold(Pawn victim): this(victim, InstigatorInfo.NoInstigator)
        {

        }

        public TaleNewsPawnSold(Pawn victim, InstigatorInfo info): base (victim, info)
        {

        }

        public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
        {
            // Placeholder
            return 3;
        }

        public override string GetDetailsPrintout()
        {
            string basic = base.GetDetailsPrintout();
            basic += "\nSold to: ";
            if (tradeDeal_OtherParty != null)
            {
                basic += tradeDeal_OtherParty.Name;
            }
            else
            {
                basic += "unknown group";
            }
            return basic;
        }

        public override string GetNewsTypeName()
        {
            return "Pawn Sold";
        }

        protected override void ConductSaveFileIO()
        {
            base.ConductSaveFileIO();

            Scribe_References.Look(ref tradeDeal_OtherParty, "tradeDeal_OtherParty");
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                if (tradeDeal_OtherParty == null)
                {
                    tradeDeal_OtherParty = PrimaryVictim?.Faction ?? PrimaryVictim?.HostFaction ?? null;
                }
            }
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

                // Remove marriage-related memories, etc.
                Pawn spouse = PrimaryVictim.GetSpouse();
                if (spouse != null && recipient == spouse && !recipient.Dead)
                {
                    MemoryThoughtHandler memories = recipient.needs.mood.thoughts.memories;
                    memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
                    memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
                }
            }
        }
    }
}
