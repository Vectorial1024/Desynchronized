using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    /// <summary>
    /// A TaleNews object links a Tale and a receipient Pawn together.
    /// </summary>
    public abstract class TaleNews
    {
        public int UniqueID { get; private set; } = -1;

        public bool IsRegistered
        {
            get
            {
                return UniqueID >= 0;
            }
        }

        private readonly TaleNewsReference SampleReference;

        /// <summary>
        /// The mother constructor of all. Also registers itself to the TNDBS.
        /// <para/>
        /// Do NOT abuse this constructor.
        /// </summary>
        public TaleNews()
        {
            if (DesynchronizedMain.WeAreInDevMode)
            {
                DesynchronizedMain.TaleNewsDatabaseSystem.RegisterNewTale(this);
            }
            SampleReference = new TaleNewsReference(this);
        }

        public TaleNewsReference CreateReferenceForReceipient(Pawn receipient)
        {
            return new TaleNewsReference(SampleReference, receipient);
        }

        public void BecomeRegistered(int ID)
        {
            if (!IsRegistered)
            {
                UniqueID = ID;
            }
        }

        public void ActivateForReceipient(Pawn receipient)
        {
            GiveThoughtsToReceipient(receipient);
        }

        /// <summary>
        /// Called to apply Thoughts to the receipient.
        /// You can assume that the Receipient hears of this TaleNews at the correct timing.
        /// <para/>
        /// The Restructuring has separated the recipient from this template class.
        /// </summary>
        /// <param name="recipient"></param>
        protected abstract void GiveThoughtsToReceipient(Pawn recipient);
    }
}
