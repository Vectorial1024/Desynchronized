using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class InstigatorInfo: IExposable
    {
        /// <summary>
        /// Our patch-mod should override this with Psychology mayor. Value can be null.
        /// </summary>
        /// <returns></returns>
        public static Pawn GetRepresentativeOfPlayerInGame()
        {
            return Faction.OfPlayer.leader;
        }

        public static Pawn RepresentativeOfPlayer => GetRepresentativeOfPlayerInGame();

        public static InstigatorInfo NoInstigator => GenerateNormally();

        private bool playerIsInstigator;
        private Pawn instigator;
        private Faction instigatingFaction;

        public bool PlayerIsInstigator
        {
            get
            {
                return playerIsInstigator;
            }
            private set
            {
                playerIsInstigator = value;
            }
        }

        /// <summary>
        /// The instigator. Can be null.
        /// </summary>
        public Pawn InstigatingPawn
        {
            get
            {
                return instigator;
            }
            internal set
            {
                instigator = value;
            }
        }

        public Faction InstigatingFaction
        {
            get
            {
                return instigatingFaction;
            }
            internal set
            {
                instigatingFaction = value;
            }
        }

        /// <summary>
        /// Do not use this constructor explicitly.
        /// </summary>
        public InstigatorInfo()
        {

        }

        public InstigatorInfo(Faction instigFaction = null, Pawn instigPawn = null)
        {
            instigatingFaction = instigFaction;
            instigator = instigPawn;
            playerIsInstigator = false;
        }

        [Obsolete]
        public static InstigatorInfo GenerateNormally(Pawn instigator = null)
        {
            return new InstigatorInfo()
            {
                PlayerIsInstigator = false,
                InstigatingPawn = instigator
            };
        }

        public static InstigatorInfo GenerateAsPlayerInstigated()
        {
            return new InstigatorInfo()
            {
                PlayerIsInstigator = true,
                InstigatingPawn = null
            };
        }

        public void ExposeData()
        {
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                // Do some init here
                if (instigatingFaction == null)
                {
                    instigatingFaction = instigator?.Faction ?? null;
                }
            }

            Scribe_Values.Look(ref playerIsInstigator, "playerIsInstigator", false);
            Scribe_References.Look(ref instigator, "instigator");
            Scribe_References.Look(ref instigatingFaction, "instigatingFaction");

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                // Do some init here
                if (instigatingFaction == null)
                {
                    instigatingFaction = instigator?.Faction ?? null;
                }
            }
        }

        public static explicit operator InstigatorInfo(Faction faction)
        {
            return new InstigatorInfo(faction);
        }

        public static explicit operator InstigatorInfo(Pawn pawn)
        {
            return new InstigatorInfo(pawn?.Faction, pawn);
        }
    }
}
