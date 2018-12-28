using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    public class TaleNewsPawnDied : TaleNews
    {
        public Pawn Victim { get; }
        [Obsolete]
        private DamageInfo? DeathCauseNullable { get; }
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
        [Obsolete("I assume this is not really required; the Witnessed() function should already have covered this.")]
        public Map MapOfOccurence { get; }
        public PawnClassification VictimClassAtDeath { get; }
        public DeathBrutality BrutalityDegree { get; }

        public TaleNewsPawnDied(Pawn receipient, Pawn victim, DamageInfo? dinfo, Map map, PawnClassification classification, DeathBrutality brutality): base(receipient)
        {
            Victim = victim;
            DeathCause = dinfo;
            MapOfOccurence = map;
            VictimClassAtDeath = classification;
            BrutalityDegree = brutality;
        }

        public static void Generate(Pawn vicitm, DamageInfo dinfo, PawnDiedOrDownedThoughtsKind thoughtKind)
        {

        }

        /// <summary>
        /// Gives out Bloodlust Killed Somebody thought and Killed Faction Leader thought
        /// </summary>
        private void HandleInstantaneousThoughts()
        {
            // Killer != null => DamageInfo != null
            // The obligatory .Value is extremely triggering.
            if (Killer != null)
            {
                if (Killer == Victim)
                {
                    // We don't handle suicide cases.
                    return;
                }

                if (NewsReceipient == Killer && DeathCause.Value.Def.ExternalViolenceFor(Victim))
                {
                    // Confirmed NewsReceipient is Killer

                    // IsCapableOfThought has already been called.
                    // NewsReceipient is definitely capable of thought.

                    // Why this check tho
                    if (NewsReceipient.story != null)
                    {
                        // Try to add Bloodlust thoughts
                        new IndividualThoughtToAdd(ThoughtDefOf.KilledHumanlikeBloodlust, NewsReceipient).Add();

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
        /// Gives out Witnessed *name* Death thoughts and Colonist/Prisoner Died thoughts
        /// </summary>
        private void HandleWitnessAndGenericThoughts()
        {
            if (Victim.RaceProps.Humanlike)
            {
                if (NewsReceipient.Map == MapOfOccurence && Witnessed(NewsReceipient, Victim))
                {
                    // Eye-witness thoughts
                    if (NewsReceipient.Faction == Victim.Faction)
                    {
                        new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathAlly, NewsReceipient).Add();
                    }
                    else if (Victim.Faction == null || !Victim.Faction.HostileTo(NewsReceipient.Faction))
                    {
                        new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathNonAlly, NewsReceipient).Add();
                    }
                    if (NewsReceipient.relations.FamilyByBlood.Contains(Victim))
                    {
                        new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathFamily, NewsReceipient).Add();
                    }
                    new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathBloodlust, NewsReceipient).Add();
                }
                else
                {
                    if (Victim.Faction == Faction.OfPlayer && Victim.Faction == NewsReceipient.Faction && Victim.HostFaction != NewsReceipient.Faction)
                    {
                        NewsReceipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowColonistDied, Victim);
                    }
                    bool prisonerIsInnocent = Victim.IsPrisonerOfColony && !Victim.guilt.IsGuilty && !Victim.InAggroMentalState;
                    if (prisonerIsInnocent && NewsReceipient.Faction == Faction.OfPlayer && !NewsReceipient.IsPrisoner)
                    {
                        NewsReceipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerDiedInnocent, Victim);
                    }
                }
            }
        }

        /// <summary>
        /// Gives out My Relative Died, and Killer Killed My Relative/Friend/Rival (mostly social) thoughts
        /// </summary>
        private void HandleSocialBasedThoughts()
        {
            PawnRelationDef mostImportantRelation = NewsReceipient.GetMostImportantRelation(Victim);
            if (mostImportantRelation != null)
            {
                // Step 2.1: My relative died
                ThoughtDef genderSpecificDiedThought = mostImportantRelation.GetGenderSpecificDiedThought(Victim);
                if (genderSpecificDiedThought != null)
                {
                    new IndividualThoughtToAdd(genderSpecificDiedThought, NewsReceipient, Victim, 1f, 1f).Add();
                }

                // Step 2.2: Someone is responsible for this kill
                if (Killer != null && Killer != Victim && Killer != NewsReceipient)
                {
                    // This killer killed my relatives
                    ThoughtDef genderSpecificKilledThought = mostImportantRelation.GetGenderSpecificKilledThought(Victim);
                    if (genderSpecificKilledThought != null)
                    {
                        new IndividualThoughtToAdd(genderSpecificKilledThought, NewsReceipient, Killer, 1f, 1f).Add();
                    }

                    // This killer killed my friend/rival
                    if (NewsReceipient.RaceProps.IsFlesh)
                    {
                        int opinion = NewsReceipient.relations.OpinionOf(Victim);
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
                        new IndividualThoughtToAdd(thoughtToBeAdded, NewsReceipient, Killer, 1f, scalingFactor).Add();
                    }
                }
            }
        }

        /// <summary>
        /// Gives My Friend/Rival Died mood thoughts
        /// </summary>
        private void HandleFriendRivalDiedMoodThoughts()
        {
            int opinion = NewsReceipient.relations.OpinionOf(Victim);
            if (opinion >= 20)
            {
                new IndividualThoughtToAdd(ThoughtDefOf.PawnWithGoodOpinionDied, NewsReceipient, Victim, Victim.relations.GetFriendDiedThoughtPowerFactor(opinion), 1f).Add();
            }
            else if (opinion <= -20)
            {
                new IndividualThoughtToAdd(ThoughtDefOf.PawnWithBadOpinionDied, NewsReceipient, Victim, Victim.relations.GetRivalDiedThoughtPowerFactor(opinion), 1f).Add();
            }
        }

        protected override void GiveThoughtsToReceipient()
        {
            if (!NewsReceipient.IsCapableOfThought())
            {
                return;
            }

            // NOTE: Bonded (Master to Animal) is a legal PawnRelationship
            // Step 0: Give out "Killed somebody" Thoughts
            HandleInstantaneousThoughts();

            // Step 1: Eye-witnesses and generic thoughts
            HandleWitnessAndGenericThoughts();

            // Step 2: Process social-based/mood-based relationship thoughts
            HandleSocialBasedThoughts();

            // Step 3: Handle "My friend/rival died"
            HandleFriendRivalDiedMoodThoughts();
        }

        public static bool Witnessed(Pawn p, Pawn victim)
        {
            if (!p.Awake() || !p.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
            {
                return false;
            }
            if (victim.IsCaravanMember())
            {
                return victim.GetCaravan() == p.GetCaravan();
            }
            if (!victim.Spawned || !p.Spawned)
            {
                return false;
            }
            if (!p.Position.InHorDistOf(victim.Position, 12f))
            {
                return false;
            }
            if (!GenSight.LineOfSight(victim.Position, p.Position, victim.Map, false, null, 0, 0))
            {
                return false;
            }
            return true;
        }

    }
}
