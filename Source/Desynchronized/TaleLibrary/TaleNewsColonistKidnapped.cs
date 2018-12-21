using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    public class TaleNewsColonistKidnapped : TaleNews
    {
        public Pawn Kidnapper { get; }

        public Pawn KidnapVictim { get; }

        public Faction KidnapperFaction { get; }

        [Obsolete]
        private TaleNewsColonistKidnapped(Pawn receipient, Tale_DoublePawn tale) : base(receipient)
        {
            Kidnapper = tale.firstPawnData.pawn;
            KidnapVictim = tale.secondPawnData.pawn;
        }

        public TaleNewsColonistKidnapped(Pawn receipient, Pawn victim, Faction kidnapper): base(receipient)
        {
            KidnapVictim = victim;
            KidnapperFaction = kidnapper;
        }

        public TaleNewsColonistKidnapped(Pawn receipient, Pawn victim, Pawn kidnapper) : base(receipient)
        {
            KidnapVictim = victim;
            KidnapperFaction = kidnapper.Faction;
        }

        [Obsolete]
        public static TaleNewsColonistKidnapped GenerateTaleNewsForReceipient(Pawn receipient, Tale_DoublePawn tale)
        {
            if (tale.def == TaleDefOf.KidnappedColonist)
            {
                return new TaleNewsColonistKidnapped(receipient, tale);
            }
            return null;
        }

        protected override void GiveThoughts()
        {
            // Check if the receipient can receive any thoughts at all.
            if (NewsReceipient.IsCapableOfThought())
            {
                // Give generic Colonist Kidnapped thoughts
                NewsReceipient.needs.mood.thoughts.memories.TryGainMemory(Desynchronized_ThoughtDefOf.KnowColonistKidnapped);

                // Then give Friend/Rival Kidnapped thoughts
                if (NewsReceipient.RaceProps.IsFlesh && placeholderFoo(KidnapVictim, NewsReceipient))
                {
                    int opinion = NewsReceipient.relations.OpinionOf(KidnapVictim);
                    if (opinion >= 20)
                    {
                        new IndividualThoughtToAdd(Desynchronized_ThoughtDefOf.PawnWithGoodOpinionWasKidnapped, NewsReceipient, KidnapVictim, KidnapVictim.relations.GetFriendDiedThoughtPowerFactor(opinion), 1f).Add();
                    }
                    else if (opinion <= -20)
                    {
                        new IndividualThoughtToAdd(Desynchronized_ThoughtDefOf.PawnWithBadOpinionWasKidnapped, NewsReceipient, KidnapVictim, KidnapVictim.relations.GetRivalDiedThoughtPowerFactor(opinion), 1f).Add();
                    }
                }

                // Finally give Family Member Kidnapped thoughts
                // Currently test only for son/daughter relations
                PawnRelationDef mostImportantRelation = NewsReceipient.GetMostImportantRelation(KidnapVictim);
                if (mostImportantRelation != null && mostImportantRelation.defName == "Child")
                {
                    ThoughtDef genderSpecificKidnappedThought = mostImportantRelation.GetGenderSpecificKidnappedThought(KidnapVictim);
                    if (genderSpecificKidnappedThought != null)
                    {
                        new IndividualThoughtToAdd(genderSpecificKidnappedThought, NewsReceipient, KidnapVictim, 1f, 1f).Add();
                        // outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificDiedThought, potentiallyRelatedPawn, victim, 1f, 1f));
                    }
                }
                // TODO
            }
        }

        private static bool placeholderFoo(Pawn pawn, Pawn subject)
        {
            return pawn.Faction == subject.Faction || (!subject.IsWorldPawn() && !pawn.IsWorldPawn());
        }
    }
}
