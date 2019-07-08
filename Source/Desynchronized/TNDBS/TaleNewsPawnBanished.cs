﻿using Desynchronized.TNDBS.Datatypes;
using RimWorld;
using Verse;

namespace Desynchronized.TNDBS
{
    public class TaleNewsPawnBanished : TaleNewsNegativeIndividual
    {
        private bool isDeadly;

        public Pawn BanishmentVictim => PrimaryVictim;

        public bool IsDeadly
        {
            get
            {
                return isDeadly;
            }
        }

        public TaleNewsPawnBanished()
        {

        }

        public TaleNewsPawnBanished(Pawn victim, bool isBanishedToDie): base(victim, InstigationInfo.NoInstigator)
        {
            isDeadly = isBanishedToDie;
        }

        public override string GetNewsTypeName()
        {
            return "Pawn Banished";
        }

        protected override void ConductSaveFileIO()
        {
            base.ConductSaveFileIO();
            Scribe_Values.Look(ref isDeadly, "isDeadly");
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            if (!recipient.IsCapableOfThought())
            {
                return;
            }

            // Switch for handling Bonded Animal Banished
            if (BanishmentVictim.RaceProps.Animal)
            {
                if (recipient.relations.GetDirectRelation(PawnRelationDefOf.Bond, BanishmentVictim) != null)
                {
                    new IndividualThoughtToAdd(ThoughtDefOf.BondedAnimalBanished, recipient, BanishmentVictim).Add();
                }
            }
            else if (recipient == BanishmentVictim)
            {
                // We have potential here. Next version perhaps.
            }
            else
            {
                ThoughtDef thoughtDefToGain = null;
                if (!BanishmentVictim.IsPrisonerOfColony)
                {
                    if (IsDeadly)
                    {
                        thoughtDefToGain = ThoughtDefOf.ColonistBanishedToDie;
                    }
                    else
                    {
                        thoughtDefToGain = ThoughtDefOf.ColonistBanished;
                    }
                }
                else
                {
                    if (IsDeadly)
                    {
                        thoughtDefToGain = ThoughtDefOf.PrisonerBanishedToDie;
                    }
                    else
                    {
                        // Adjust for traits concerning prisoner released dangerously.
                        // Bloodlust trait has higher priority.
                        if (recipient.story.traits.HasTrait(TraitDefOf.Bloodlust))
                        {
                            thoughtDefToGain = Desynchronized_ThoughtDefOf.PrisonerReleasedDangerously_Bloodlust;
                        }
                        else if (recipient.story.traits.HasTrait(TraitDefOf.Psychopath))
                        {
                            thoughtDefToGain = Desynchronized_ThoughtDefOf.PrisonerReleasedDangerously_Psychopath;
                        }
                        else
                        {
                            thoughtDefToGain = Desynchronized_ThoughtDefOf.PrisonerReleasedDangerously;
                        }
                    }
                }

                recipient.needs.mood.thoughts.memories.TryGainMemory(thoughtDefToGain, BanishmentVictim);
            }
        }

        public override float CalculateNewsImportanceForPawn(Pawn pawn, TaleNewsReference reference)
        {
            // Placeholder
            return 3;
        }

        public override string GetDetailsPrintout()
        {
            string basic = base.GetDetailsPrintout();
            basic += "\nDeadly? " + isDeadly;
            return basic;
        }
    }
}
