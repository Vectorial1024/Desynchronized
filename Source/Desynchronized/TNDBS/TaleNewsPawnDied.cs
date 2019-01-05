using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class TaleNewsPawnDied : TaleNewsNegativeIndividual
    {
        public Pawn Victim => PrimaryVictim;
        /// <summary>
        /// This member is of type DamageInfo?. Be careful.
        /// </summary>
        public DamageInfo? DeathCause { get; }
        public Pawn Killer
        {
            get
            {
                if (DeathCause.HasValue)
                {
                    return DeathCause.Value.Instigator as Pawn;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Defaults to HUMANE if something went wrong in the constructor.
        /// </summary>
        public DeathBrutality BrutalityDegree { get; }

        public enum DeathMethod
        {
            NON_EXECUTION = 0,
            EXECUTION = 1
        }

        private DeathMethod MethodOfDeath { get; }

        private TaleNewsPawnDied(Pawn victim, DeathMethod method, object argument): base(victim, InstigatorInfo.NoInstigator)
        {
            MethodOfDeath = method;
            if (MethodOfDeath == DeathMethod.NON_EXECUTION)
            {
                DeathCause = argument as DamageInfo?;
                InstigatorInfo = (InstigatorInfo) Killer;
            }
            else
            {
                DeathBrutality? temp = argument as DeathBrutality?;
                if (temp.HasValue)
                {
                    BrutalityDegree = temp.Value;
                }
                else
                {
                    BrutalityDegree = DeathBrutality.HUMANE;
                    Log.Error("[V1024-DESYNC] Cannot recognize \"DeathBrutality\" argument for Pawn execution; did something go wrong?\n" + Environment.StackTrace, true);
                }
            }
        }

        public static TaleNewsPawnDied GenerateAsExecution(Pawn victim, DeathBrutality brutality)
        {
            return new TaleNewsPawnDied(victim, DeathMethod.EXECUTION, brutality);
        }

        public static TaleNewsPawnDied GenerateGenerally(Pawn victim, DamageInfo? dinfo)
        {
            return new TaleNewsPawnDied(victim, DeathMethod.NON_EXECUTION, dinfo);
        }

        /// <summary>
        /// Handles the excitement of the killer when the killer has just killed somebody.
        /// Can also be used to handle disappointment as well.
        /// <para/>
        /// You should check legitimacy before calling this method.
        /// </summary>
        private void HandleExcitementOfKiller(Pawn recipient)
        {
            // Killer != null => DamageInfo != null
            // The obligatory .Value is extremely triggering.
            if (Killer != null)
            {
                if (Killer == PrimaryVictim)
                {
                    // We don't handle suicide cases.
                    return;
                }

                if (recipient == Killer && DeathCause.Value.Def.ExternalViolenceFor(Victim))
                {
                    // Confirmed NewsReceipient is Killer

                    // IsCapableOfThought has already been called.
                    // NewsReceipient is definitely capable of thought.

                    // Why this check tho
                    if (recipient.story != null)
                    {
                        // Try to add Bloodlust thoughts
                        new IndividualThoughtToAdd(ThoughtDefOf.KilledHumanlikeBloodlust, recipient).Add();

                        // Try to add Defeated Hostile Leader thoughts
                        if (Victim.HostileTo(Killer) && Victim.Faction != null && PawnUtility.IsFactionLeader(Victim) && Victim.Faction.HostileTo(Killer.Faction))
                        {
                            new IndividualThoughtToAdd(ThoughtDefOf.DefeatedHostileFactionLeader, Killer, Victim).Add();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gives out eyewitness Thoughts.
        /// <para/>
        /// Only for eyewitnesses, that is, those Pawns whose .CanWitnessOtherPawn(Victim) returns true
        /// </summary>
        private void GiveOutEyewitnessThoughts(Pawn recipient)
        {
            if (recipient.Faction == Victim.Faction)
            {
                new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathAlly, recipient).Add();
            }
            else if (Victim.Faction == null || !Victim.Faction.HostileTo(recipient.Faction))
            {
                new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathNonAlly, recipient).Add();
            }
            if (recipient.relations.FamilyByBlood.Contains(Victim))
            {
                new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathFamily, recipient).Add();
            }
            new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathBloodlust, recipient).Add();
        }

        /// <summary>
        /// Gives out generic thoughts such as Colonist Died or Prisoner Died, etc.
        /// <para/>
        /// For everyone, though eyewitnesses should preferrably use GiveOutEyewitnessThoughts()
        /// </summary>
        private void GiveOutGenericThoughts(Pawn recipient)
        {
            if (Victim.Faction == Faction.OfPlayer && Victim.Faction == recipient.Faction && Victim.HostFaction != recipient.Faction)
            {
                recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowColonistDied, Victim);
            }
            bool prisonerIsInnocent = Victim.IsPrisonerOfColony && !Victim.guilt.IsGuilty && !Victim.InAggroMentalState;
            if (prisonerIsInnocent && recipient.Faction == Faction.OfPlayer && !recipient.IsPrisoner)
            {
                recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerDiedInnocent, Victim);
            }
        }

        /// <summary>
        /// Gives out relationship-based Thoughts, such as My Relative Died, and Killer Killed My Relative/Friend/Rival.
        /// <para/>
        /// Most thoughts given out here are all social thoughts. The very few excptions to this are the Bonded Animal Died thought
        /// </summary>
        private void GiveOutRelationshipBasedThoughts(Pawn recipient)
        {
            PawnRelationDef mostImportantRelation = recipient.GetMostImportantRelation(Victim);
            if (mostImportantRelation != null)
            {
                // Step 2.1: My relative died
                ThoughtDef genderSpecificDiedThought = mostImportantRelation.GetGenderSpecificDiedThought(Victim);
                if (genderSpecificDiedThought != null)
                {
                    new IndividualThoughtToAdd(genderSpecificDiedThought, recipient, Victim, 1f, 1f).Add();
                }

                // Step 2.2: Hatred towards killer; does not allow self-hating
                if (Killer != null && Killer != Victim && Killer != recipient)
                {
                    // This killer killed my relatives
                    GiveOutHatredTowardsKiller_ForRelatives(mostImportantRelation, recipient);

                    // This killer killed my friend/rival
                    if (recipient.RaceProps.IsFlesh)
                    {
                        GiveOutHatredTowardsKiller_ForFriendsOrRivals(recipient);
                    }
                }
            }
        }

        private void GiveOutHatredTowardsKiller_ForRelatives(PawnRelationDef mostImportantRelation, Pawn recipient)
        {
            ThoughtDef genderSpecificKilledThought = mostImportantRelation.GetGenderSpecificKilledThought(Victim);
            if (genderSpecificKilledThought != null)
            {
                new IndividualThoughtToAdd(genderSpecificKilledThought, recipient, Killer, 1f, 1f).Add();
            }
        }

        private void GiveOutHatredTowardsKiller_ForFriendsOrRivals(Pawn recipient)
        {
            int opinion = recipient.relations.OpinionOf(Victim);
            ThoughtDef thoughtToBeAdded = null;
            float scalingFactor = 0;
            if (opinion >= 20)
            {
                thoughtToBeAdded = ThoughtDefOf.KilledMyFriend;
                scalingFactor = Victim.relations.GetFriendDiedThoughtPowerFactor(opinion);
            }
            else if (opinion <= -20)
            {
                thoughtToBeAdded = ThoughtDefOf.KilledMyRival;
                scalingFactor = Victim.relations.GetRivalDiedThoughtPowerFactor(opinion);
            }
            new IndividualThoughtToAdd(thoughtToBeAdded, recipient, Killer, 1f, scalingFactor).Add();
        }

        /// <summary>
        /// Gives My Friend/Rival Died mood thoughts
        /// </summary>
        private void GiveOutFriendOrRivalDiedThoughts(Pawn recipient)
        {
            int opinion = recipient.relations.OpinionOf(Victim);
            if (opinion >= 20)
            {
                new IndividualThoughtToAdd(ThoughtDefOf.PawnWithGoodOpinionDied, recipient, Victim, Victim.relations.GetFriendDiedThoughtPowerFactor(opinion), 1f).Add();
            }
            else if (opinion <= -20)
            {
                new IndividualThoughtToAdd(ThoughtDefOf.PawnWithBadOpinionDied, recipient, Victim, Victim.relations.GetRivalDiedThoughtPowerFactor(opinion), 1f).Add();
            }
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            if (!recipient.IsCapableOfThought())
            {
                return;
            }

            if (MethodOfDeath == DeathMethod.EXECUTION)
            {
                // Rather simple. Copied from vanilla code.
                int forcedStage = (int)BrutalityDegree;
                ThoughtDef thoughtToGive = Victim.IsColonist ? ThoughtDefOf.KnowColonistExecuted : ThoughtDefOf.KnowGuestExecuted;
                recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtToGive, forcedStage), null);
            }
            else
            {
                // Note that "animal bonds" is a type of relationships.
                if (Killer != Victim && Killer == recipient)
                {
                    HandleExcitementOfKiller(recipient);
                }

                // There is potential to expand upon this condition.
                if (Victim.RaceProps.Humanlike)
                {
                    if (recipient.CanWitnessOtherPawn(Victim))
                    {
                        GiveOutEyewitnessThoughts(recipient);
                    }
                    else
                    {
                        GiveOutGenericThoughts(recipient);
                    }
                }
            }

            // Check if there is any relationship at all to make the code clearer
            if (Victim.relations.PotentiallyRelatedPawns.Contains(recipient))
            {
                GiveOutRelationshipBasedThoughts(recipient);
            }

            GiveOutFriendOrRivalDiedThoughts(recipient);
        }
    }
}
