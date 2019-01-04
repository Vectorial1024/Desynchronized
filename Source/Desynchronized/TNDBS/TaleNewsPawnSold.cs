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
        public TaleNewsPawnSold(Pawn victim) : base(victim, InstigatorInfo.NoInstigator)
        {

        }

        public SoldMethod MethodOfSales { get; }

        public enum SoldMethod
        {
            /// <summary>
            /// Prisoner Sold
            /// </summary>
            SOLD_PRISONER,
            /// <summary>
            /// My Loved One Sold / Bonded Animal Sold
            /// </summary>
            SOLD_RELATIONSHIP
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            bool isAboutPrisonerSold = PrimaryVictim.RaceProps.Humanlike;
            bool isAboutAnimalSold = PrimaryVictim.RaceProps.Animal;

            if (recipient == PrimaryVictim)
            {
                if (isAboutPrisonerSold)
                {
                    // I was sold
                }
            }
            else
            {
                if (MethodOfSales == SoldMethod.SOLD_PRISONER)
                {
                    // recipient.needs.mood.thoughts.memories.TryGainMemory(mostImportantRelation.soldThought, playerNegotiator);
                }

                if (PrimaryVictim.RaceProps.Humanlike)
                {
                    /*
                    if ()
                    {

                    }
                    foreach (Pawn potentiallyRelatedPawn in PotentiallyRelatedPawns)
                    {
                        if (!potentiallyRelatedPawn.Dead && potentiallyRelatedPawn.needs.mood != null)
                        {
                            PawnRelationDef mostImportantRelation = potentiallyRelatedPawn.GetMostImportantRelation(pawn);
                            if (mostImportantRelation != null && mostImportantRelation.soldThought != null)
                            {
                                potentiallyRelatedPawn.needs.mood.thoughts.memories.TryGainMemory(mostImportantRelation.soldThought, playerNegotiator);
                            }
                        }
                    }
                    RemoveMySpouseMarriageRelatedThoughts();
                    */
                }
                if (isAboutPrisonerSold)
                {
                    // Prisoner sold
                    recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerSold);
                }
                else if (isAboutAnimalSold)
                {
                    PawnRelationDef relation = recipient.GetMostImportantRelation(PrimaryVictim);
                    if (relation == PawnRelationDefOf.Bond)
                    {
                        // Bonded sold
                    }
                }
            }
        }
    }
}
