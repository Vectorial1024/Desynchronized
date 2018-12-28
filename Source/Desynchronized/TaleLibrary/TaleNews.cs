using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    /// <summary>
    /// A TaleNews object links a Tale and a receipient Pawn together.
    /// </summary>
    public abstract class TaleNews
    {
        bool hasBeenReceived = false;

        public Pawn NewsReceipient { get; }

        public bool ReceipientHasReceivedNews
        {
            get
            {
                return hasBeenReceived;
            }
        }

        protected TaleNews(Pawn receipient)
        {
            NewsReceipient = receipient;
        }

        /// <summary>
        /// Generates a correct TaleNews object. Can return null if none can be generated.
        /// </summary>
        /// <returns></returns>
        public static TaleNews GenerateTaleNewsForReceipient(Tale tale, Pawn receipient)
        {
            /*
            if (tale.def == TaleDefOf.KidnappedColonist)
            {
                return new TaleNewsColonistKidnapped(tale as Tale_DoublePawn, receipient);
            }
            */
            return null;
            
        }

        public static TaleNews GenerateTaleNewsForRecipient(Pawn recipient, TaleTypeEnum type)
        {
            return null;
        }

        /// <summary>
        /// Generic method. Actual implementation depends on classes.
        /// </summary>
        public void ActivateAndGiveThoughts()
        {
            // TODO
            GiveThoughtsToReceipient();
            hasBeenReceived = true;
        }

        /// <summary>
        /// Called to apply Thoughts to the receipient.
        /// You can assume that the Receipient hears of this TaleNews at the correct timing.
        /// </summary>
        protected abstract void GiveThoughtsToReceipient();
    }
}
