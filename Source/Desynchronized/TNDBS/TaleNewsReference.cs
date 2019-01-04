using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class TaleNewsReference
    {
        /// <summary>
        /// True if the Receipient has reacted to the Underlying TaleNews before.
        /// </summary>
        public bool HasBeenActivated { get; private set; } = false;

        public int UidOfReferencedNews { get; }

        /// <summary>
        /// The underlying, actual TaleNews object that this reference is pointing to.
        /// </summary>
        public TaleNews UnderlyingTaleNews { get; }

        /// <summary>
        /// The receipient of the Underlying TaleNews.
        /// Ask your TaleNews object to generate a TaleNewsReference for your receipient.
        /// </summary>
        public Pawn Receipient { get; private set; }

        public bool ReferenceIsComplete
        {
            get
            {
                return Receipient != null;
            }
        }

        public TaleNewsReference(TaleNews news)
        {
            UnderlyingTaleNews = news;
        }

        public TaleNewsReference(TaleNewsReference reference, Pawn receipient)
        {
            UnderlyingTaleNews = reference.UnderlyingTaleNews;
            Receipient = receipient;
        }

        public void ActivateNews()
        {
            if (!HasBeenActivated)
            {
                UnderlyingTaleNews.ActivateForReceipient(Receipient);
                HasBeenActivated = true;
            }
        }
    }
}
