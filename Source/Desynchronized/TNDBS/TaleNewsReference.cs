using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class TaleNewsReference: IExposable
    {
        // Pivate attributes as required by IExposable
        private bool hasBeenActivated = false;
        private TaleNews underlyingTaleNews;
        private Pawn recipient;

        /// <summary>
        /// True if the Receipient has reacted to the Underlying TaleNews before.
        /// </summary>
        public bool HasBeenActivated
        {
            get
            {
                return hasBeenActivated;
            }
        }

        /// <summary>
        /// The underlying, actual TaleNews object that this reference is pointing to.
        /// </summary>
        public TaleNews UnderlyingTaleNews
        {
            get
            {
                return underlyingTaleNews;
            }
        }

        /// <summary>
        /// The receipient of the Underlying TaleNews.
        /// Ask your TaleNews object to generate a TaleNewsReference for your receipient.
        /// </summary>
        public Pawn Recipient
        {
            get
            {
                return recipient;
            }
        }

        public bool ReferenceIsValid
        {
            get
            {
                return Recipient != null && UnderlyingTaleNews != null;
            }
        }

        public static TaleNewsReference NullReference = new TaleNewsReference()
        {
            underlyingTaleNews = null,
            recipient = null
        };

        /// <summary>
        /// DO NOT USE THIS CONSTRUCTOR EXPLICITLY.
        /// </summary>
        public TaleNewsReference()
        {

        }

        [Obsolete("", true)]
        public TaleNewsReference(TaleNews news)
        {
            underlyingTaleNews = news;
        }

        [Obsolete("", true)]
        public TaleNewsReference(TaleNewsReference reference, Pawn recipient)
        {
            underlyingTaleNews = reference.UnderlyingTaleNews;
            this.recipient = recipient;
        }

        public TaleNewsReference(TaleNews news, Pawn recipient)
        {
            underlyingTaleNews = news;
            this.recipient = recipient;
        }

        public override string ToString()
        {
            if (this == NullReference)
            {
                return "Null Reference";
            }
            else
            {
                return "Reference to TaleNews about " + UnderlyingTaleNews.ToString();
            }
        }

        public static bool BothRefsAreEqual(TaleNewsReference one, TaleNewsReference two)
        {
            return one.UnderlyingTaleNews == two.UnderlyingTaleNews;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref hasBeenActivated, "hasBeenActivated", false);
            Scribe_Deep.Look(ref underlyingTaleNews, "underlyingTaleNews");
            Scribe_References.Look(ref recipient, "recipient");
        }

        /// <summary>
        /// Attempts to activate news, and let the recipient react to the news.
        /// <para/>
        /// Will do nothing if the news has already been activated before.
        /// </summary>
        public void ActivateNews()
        {
            if (!HasBeenActivated)
            {
                UnderlyingTaleNews.ActivateForReceipient(Recipient);
                hasBeenActivated = true;
            }
        }

        /// <summary>
        /// The recipient inside this TaleNewsReference is able to react to the underlying TaleNews again.
        /// <para/>
        /// Mainly designed for Colonists with Alzheimers. Do NOT abuse.
        /// </summary>
        public void Forget()
        {
            hasBeenActivated = false;
        }
    }
}
