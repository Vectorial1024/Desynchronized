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
    public abstract class TaleNews: IExposable
    {
        private int uniqueID = -1;

        public int UniqueID
        {
            get
            {
                return uniqueID;
            }
        }

        public bool IsRegistered
        {
            get
            {
                return UniqueID >= 0;
            }
        }

        /// <summary>
        /// DO NOT USE THIS CONSTRUCTOR
        /// </summary>
        [Obsolete("This constructor is reserved. Keep this empty.")]
        public TaleNews()
        {
            
        }

        public TaleNews(int dummy)
        {
            if (DesynchronizedMain.WeAreInDevMode)
            {
                DesynchronizedMain.TaleNewsDatabaseSystem.RegisterNewTale(this);
            }
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref uniqueID, "uniqueID", -1);
            ConductSaveFileIO();
        }

        /// <summary>
        /// Carry out Scribe IO operations here to recreate your instance.
        /// <para/>
        /// That said, please also provide an empty constructor for the whole mechanic to work.
        /// </summary>
        protected abstract void ConductSaveFileIO();

        public TaleNewsReference CreateReferenceForReceipient(Pawn receipient)
        {
            return new TaleNewsReference(this, receipient);
        }

        public void BecomeRegistered(int ID)
        {
            if (!IsRegistered)
            {
                uniqueID = ID;
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
