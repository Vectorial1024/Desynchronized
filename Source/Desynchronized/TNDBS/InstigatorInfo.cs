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
        /// Do not use this constructor explicitly.
        /// </summary>
        public InstigatorInfo()
        {

        }

        public static InstigatorInfo GenerateNormally(Pawn instigator = null)
        {
            return new InstigatorInfo()
            {
                PlayerIsInstigator = false,
                Instigator = instigator
            };
        }

        public static InstigatorInfo GenerateAsPlayerInstigated()
        {
            return new InstigatorInfo()
            {
                PlayerIsInstigator = true,
                Instigator = null
            };
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
