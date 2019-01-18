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

        public static InstigatorInfo NoInstigator => new InstigatorInfo(null);

        private bool playerIsInstigator;
        private Pawn instigator;

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
        public Pawn Instigator
        {
            get
            {
                return instigator;
            }
            private set
            {
                instigator = value;
            }
        }

        /// <summary>
        /// For general usage. Input null for no instigator.
        /// </summary>
        /// <param name="instigator"></param>
        private InstigatorInfo(Pawn instigator = null)
        {
            // Pawn-instigated
            PlayerIsInstigator = false;
            Instigator = instigator;
        }

        /// <summary>
        /// For when the Player instigated the process.
        /// </summary>
        private InstigatorInfo()
        {
            // Player-instigated
            PlayerIsInstigator = true;
            Instigator = RepresentativeOfPlayer;
        }

        public static InstigatorInfo GenerateNormally(Pawn instigator)
        {
            return new InstigatorInfo(instigator);
        }

        public static InstigatorInfo GenerateAsPlayerInstigated()
        {
            return new InstigatorInfo();
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref playerIsInstigator, "playerIsInstigator", false);
            Scribe_References.Look(ref instigator, "instigator");
        }

        /// <summary>
        /// Explicit cast for streamlining stuff.
        /// </summary>
        /// <param name="pawn"></param>
        public static explicit operator InstigatorInfo(Pawn pawn)
        {
            return GenerateNormally(pawn);
        }
    }
}
