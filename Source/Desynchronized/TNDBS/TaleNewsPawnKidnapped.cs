using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class TaleNewsPawnKidnapped : TaleNewsNegativeIndividual
    {
        public Pawn Kidnapper { get; }

        public Pawn KidnapVictim => PrimaryVictim;

        public Faction KidnapperFaction { get; }

        public TaleNewsPawnKidnapped(Pawn victim, InstigatorInfo instigatorInfo): base(victim, instigatorInfo)
        {
            Kidnapper = instigatorInfo.Instigator;
            KidnapperFaction = Kidnapper.Faction;
        }

        private static bool placeholderFoo(Pawn pawn, Pawn subject)
        {
            return pawn.Faction == subject.Faction || (!subject.IsWorldPawn() && !pawn.IsWorldPawn());
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            // Check if the receipient can receive any thoughts at all.
            // No need to check for victim's raceprops; only Colonists can be targetted to be kidnapped.
            if (!recipient.IsCapableOfThought())
            {
                return;
            }

            // Give generic Colonist Kidnapped thoughts
            recipient.needs.mood.thoughts.memories.TryGainMemory(Desynchronized_ThoughtDefOf.KnowColonistKidnapped);

            // Then give Friend/Rival Kidnapped thoughts
            if (recipient.RaceProps.IsFlesh && placeholderFoo(KidnapVictim, recipient))
            {
                int opinion = recipient.relations.OpinionOf(KidnapVictim);
                if (opinion >= 20)
                {
                    new IndividualThoughtToAdd(Desynchronized_ThoughtDefOf.PawnWithGoodOpinionWasKidnapped, recipient, KidnapVictim, KidnapVictim.relations.GetFriendDiedThoughtPowerFactor(opinion), 1f).Add();
                }
                else if (opinion <= -20)
                {
                    new IndividualThoughtToAdd(Desynchronized_ThoughtDefOf.PawnWithBadOpinionWasKidnapped, recipient, KidnapVictim, KidnapVictim.relations.GetRivalDiedThoughtPowerFactor(opinion), 1f).Add();
                }
            }

            // Finally give Family Member Kidnapped thoughts
            PawnRelationDef mostImportantRelation = recipient.GetMostImportantRelation(KidnapVictim);
            if (mostImportantRelation != null)
            {
                ThoughtDef genderSpecificKidnappedThought = mostImportantRelation.GetGenderSpecificKidnappedThought(KidnapVictim);
                if (genderSpecificKidnappedThought != null)
                {
                    new IndividualThoughtToAdd(genderSpecificKidnappedThought, recipient, KidnapVictim, 1f, 1f).Add();
                    // outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificDiedThought, potentiallyRelatedPawn, victim, 1f, 1f));
                }
            }
            // TODO
        }
    }
}
