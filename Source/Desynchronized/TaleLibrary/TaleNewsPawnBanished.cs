using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    public class TaleNewsPawnBanished : TaleNews
    {
        public Pawn BanishmentVictim { get; }
        public bool BanishmentIsDeadly { get; }

        public TaleNewsPawnBanished(Pawn receipient, Pawn victim, bool isBanishedToDie): base(receipient)
        {
            BanishmentVictim = victim;
            BanishmentIsDeadly = isBanishedToDie;
        }

        protected override void GiveThoughtsToReceipient()
        {
            // FileLog.Log("BanishmentVictim: " + BanishmentVictim.Name + "; is he a prisoner? " + BanishmentVictim.IsPrisoner + "; is he a prisoner of the Colony? " + BanishmentVictim.IsPrisonerOfColony);
            int test;

            if (!NewsReceipient.IsCapableOfThought())
            {
                return;
            }

            ThoughtDef thoughtDefToGain = null;
            if (!BanishmentVictim.IsPrisonerOfColony)
            {
                if (BanishmentIsDeadly)
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
                if (BanishmentIsDeadly)
                {
                    thoughtDefToGain = ThoughtDefOf.PrisonerBanishedToDie;
                }
                else
                {
                    // Adjust for traits concerning prisoner released dangerously.
                    // Bloodlust trait has higher priority.
                    if (NewsReceipient.story.traits.HasTrait(TraitDefOf.Bloodlust))
                    {
                        thoughtDefToGain = Desynchronized_ThoughtDefOf.PrisonerReleasedDangerously_Bloodlust;
                    }
                    else if (NewsReceipient.story.traits.HasTrait(TraitDefOf.Psychopath))
                    {
                        thoughtDefToGain = Desynchronized_ThoughtDefOf.PrisonerReleasedDangerously_Psychopath;
                    }
                    else
                    {
                        thoughtDefToGain = Desynchronized_ThoughtDefOf.PrisonerReleasedDangerously;
                    }
                }
            }

            NewsReceipient.needs.mood.thoughts.memories.TryGainMemory(thoughtDefToGain, BanishmentVictim);
        }
    }
}
