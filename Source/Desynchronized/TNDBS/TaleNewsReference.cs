using Desynchronized.TNDBS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    /// <summary>
    /// A glorified key-value pair to express whether a pawn has heard of some certain TaleNews.
    /// <para/>
    /// Also contains some functions to help with implementing and running the TNDBS.
    /// </summary>
    public class TaleNewsReference: IExposable
    {
        // Pivate attributes as required by IExposable
        private bool hasBeenActivated = false;
        [Obsolete("Use the new int indexing method instead.", true)]
        private TaleNews underlyingTaleNews;
        private int uidOfReferencedTaleNews = -1;
        [Obsolete("Use subject instead.", true)]
        private Pawn recipient;
        private Pawn cachedSubject;
        private WitnessShockGrade shockGrade;
        private int tickReceived;
        private float cachedImportance = -1;

        /// <summary>
        /// True if the Receipient has reacted to the Underlying TaleNews before.
        /// </summary>
        public bool HasEverActivated
        {
            get
            {
                return hasBeenActivated;
            }
        }

        /// <summary>
        /// Retrieves the TaleNews referenced by this TNRef object.
        /// </summary>
        public TaleNews ReferencedTaleNews
        {
            get
            {
                return DesynchronizedMain.TaleNewsDatabaseSystem?[uidOfReferencedTaleNews];
            }
        }

        /// <summary>
        /// The underlying, actual TaleNews object that this reference is pointing to.
        /// </summary>
        [Obsolete("Use the new int indexing method instead.", true)]
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
        /// 
        [Obsolete("Use subject instead.", true)]
        public Pawn Recipient
        {
            get
            {
                return recipient;
            }
        }

        /// <summary>
        /// The subject of this TaleNewsReference, and the recipient of the referenced TaleNews.
        /// </summary>
        public Pawn CachedSubject
        {
            get
            {
                return cachedSubject;
            }
            internal set
            {
                if (cachedSubject == null)
                {
                    cachedSubject = value;
                }
            }
        }

        public int TickReceived
        {
            get
            {
                return tickReceived;
            }
        }

        public bool ReferenceIsValid
        {
            get
            {
                TaleNews tempTaleNews = ReferencedTaleNews;
                return CachedSubject != null && tempTaleNews != null && !(tempTaleNews is DefaultTaleNews);
            }
        }

        public WitnessShockGrade ShockGrade
        {
            get
            {
                return shockGrade;
            }
        }

        public float CachedNewsImportance
        {
            get
            {
                if (cachedImportance == -1)
                {
                    RecalculateNewsImportance();
                }

                return cachedImportance;
            }
        }

        public static readonly TaleNewsReference DefaultReference = new TaleNewsReference(null)
        {
            uidOfReferencedTaleNews = 0
        };

        [Obsolete("Use DefaultReference instead.", true)]
        public static TaleNewsReference NullReference = new TaleNewsReference()
        {
            underlyingTaleNews = null,
            recipient = null
        };

        /// <summary>
        /// DO NOT USE THIS CONSTRUCTOR EXPLICITLY.
        /// </summary>
        /// 
        [Obsolete("Rewrite code to reconstruct the TaleNewsReference properly.", true)]
        public TaleNewsReference()
        {

        }

        /// <summary>
        /// Used by RimWorld to reconstruct the TaleNewsReference when loading from save.
        /// </summary>
        /// <param name="subject">The owner of this TaleNewsReference</param>
        public TaleNewsReference(Pawn subject)
        {
            CachedSubject = subject;
        }

        [Obsolete("", true)]
        public TaleNewsReference(TaleNewsReference reference, Pawn recipient)
        {
            underlyingTaleNews = reference.UnderlyingTaleNews;
            this.recipient = recipient;
        }

        public TaleNewsReference(TaleNews news, Pawn recipient): this(recipient)
        {
            if (!news.IsRegistered)
            {
                throw new ArgumentException("Unregisterd TaleNews received.");
            }
            else
            {
                uidOfReferencedTaleNews = news.UniqueID;
            }
        }

        public override string ToString()
        {
            if (IsDefaultReference())
            {
                return "Default Reference";
            }
            else
            {
                return "Reference to TaleNews about " + ReferencedTaleNews.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        /// 
        // We are able to make this method extremely fast by switching over to the index-pointing method.
        public static bool RefsAreEquivalent(TaleNewsReference one, TaleNewsReference two)
        {
            return one.uidOfReferencedTaleNews == two.uidOfReferencedTaleNews;
        }

        public bool RefIsEquivalentTo(TaleNewsReference other)
        {
            return RefsAreEquivalent(this, other);
        }

        public bool IsReferencingTaleNews(TaleNews news)
        {
            return uidOfReferencedTaleNews == news.UniqueID;
        }

        internal void ChangeReferencedUID(int newID)
        {
            uidOfReferencedTaleNews = newID;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref uidOfReferencedTaleNews, "newsUID", -1);
            Scribe_Values.Look(ref hasBeenActivated, "activated", false);
            Scribe_Values.Look(ref shockGrade, "shockGrade", WitnessShockGrade.BY_NEWS);
            // test
            Scribe_Values.Look(ref tickReceived, "tickReceived", Find.TickManager.TicksGame);
            Scribe_Values.Look(ref cachedImportance, "cachedNewsImportance", -1);
            // Scribe_Deep.Look(ref underlyingTaleNews, "underlyingTaleNews");
            // Scribe_References.Look(ref recipient, "recipient");
        }

        public bool IsDefaultReference()
        {
            // Supposedly when UID == 0 it is the default reference.
            return uidOfReferencedTaleNews == 0;
            // return underlyingTaleNews is DefaultTaleNews;
        }

        /// <summary>
        /// Attempts to activate news, and let the recipient react to the news.
        /// <para/>
        /// Will do nothing if the news has already been activated before.
        /// </summary>
        public void ActivateNews(WitnessShockGrade shockGrade)
        {
            this.shockGrade = shockGrade;
            RecalculateNewsImportance();

            if (!HasEverActivated)
            {
                ReferencedTaleNews.ActivateForReceipient(CachedSubject);
                hasBeenActivated = true;
                tickReceived = Find.TickManager.TicksGame;
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

        public void RecalculateNewsImportance()
        {
            cachedImportance = ReferencedTaleNews.CalculateNewsImportanceForPawn(CachedSubject, this);
            // DesynchronizedMain.LogError("Recalculated #" + ReferencedTaleNews.UniqueID + " to have " + cachedImportance + " importance.");
        }
    }
}
