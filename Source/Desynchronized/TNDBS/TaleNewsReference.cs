using Desynchronized.TNDBS.Utilities;
using System;
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
        /// <summary>
        /// Returns the previously-calculated news importance score.
        /// </summary>
        /// 
        [Obsolete("Adhere to the new standard.", true)]
        private float cachedImportance;
        /// <summary>
        /// Returns true if the news is locally forgotten. Does not necessarily mean news is forgotten globally.
        /// </summary>
        /// 
        [Obsolete("Adhere to the new standard.", true)]
        private bool locallyForgotten;
        private bool newsIsShocking;
        [Obsolete("", true)]
        private ForgetfulnessStage forgetfulness;
        [Obsolete("", true)]
        private ForgetfulnessState forgetfulnessState;
        private NullableType<float> newsImportance;

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

        public float NewsImportance => newsImportance.GetValueOrDefault();

        /// <summary>
        /// Returns the (previously) calculated news importance score of this news.
        /// <para/>
        /// Will calculate the correct value if necessary.
        /// </summary>
        /// 
        [Obsolete("Adnere to the new standard.", true)]
        public float CachedNewsImportance
        {
            get
            {
                if (IsLocallyForgotten)
                {
                    return 0;
                }
                if (cachedImportance < 0)
                {
                    // Probably needs some init
                    RecalculateNewsImportance();
                }

                return cachedImportance;
            }
        }
        
        [Obsolete("Adhere to the new standard.", true)]
        public bool IsLocallyForgotten
        {
            get
            {
                return locallyForgotten;
            }
            internal set
            {
                locallyForgotten = value;
            }
        }

        /// <summary>
        /// Returns true if the pawn has forgotten about the news.
        /// </summary>
        public bool NewsIsLocallyForgotten => !newsImportance.HasValue;

        /// <summary>
        /// Returns true if no pawn ever remembers about the news.
        /// </summary>
        public bool NewsIsGloballyForgotten => ReferencedTaleNews.PermanentlyForgotten;

        public bool IsShockingNews
        {
            get
            {
                return newsIsShocking;
            }
        }

        [Obsolete("", true)]
        public ForgetfulnessStage ForgetfulnessStage
        {
            get
            {
                return forgetfulness;
            }
        }

        /// <summary>
        /// Returns true if the news is forgotten, either locally or globally.
        /// </summary>
        /// 
        [Obsolete("", true)]
        public bool NewsIsForgottenLocally
        {
            get
            {
                return forgetfulness == ForgetfulnessStage.FORGOTTEN || forgetfulness == ForgetfulnessStage.PERM_LOST;
            }
        }

        [Obsolete("Use the other one instead.", true)]
        public bool IsForgottenLocally => CachedNewsImportance < 1;

        [Obsolete("Adhere to the new standard.", true)]
        public ForgetfulnessState ForgetState
        {
            get
            {
                if (ReferencedTaleNews.PermanentlyForgotten)
                {
                    return ForgetfulnessState.PERM_FORGOT;
                }
                if (locallyForgotten)
                {
                    return ForgetfulnessState.LOCALLY_FORGOT;
                }
                else
                {
                    return ForgetfulnessState.KNOWN;
                }
            }
        }

        [Obsolete("", true)]
        private void UpdateForgetState()
        {
            if (ReferencedTaleNews.PermanentlyForgotten)
            {
                forgetfulnessState = ForgetfulnessState.PERM_FORGOT;
            }
            else if (locallyForgotten)
            {
                forgetfulnessState = ForgetfulnessState.LOCALLY_FORGOT;
            }
            else
            {
                forgetfulnessState = ForgetfulnessState.KNOWN;
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
                return "Reference to tale-news with id = " + uidOfReferencedTaleNews;
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
            // Scribe_Values.Look(ref cachedImportance, "cachedNewsImportance", -1);
            // Scribe_Values.Look(ref locallyForgotten, "locallyForgotten", false);
            Scribe_Values.Look(ref newsIsShocking, "newsIsShocking", false);
            Scribe_Deep.Look(ref newsImportance, "newsImportance");
            //Scribe_Values.Look(ref forgetfulness, "forgetfulness", ForgetfulnessStage.UNKNOWN);
            //Scribe_Values.Look(ref forgetfulnessState, "forgetfulnessState", ForgetfulnessState.UNKNOWN);
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

            if (!hasBeenActivated)
            {
                // First time activating.
                ReferencedTaleNews.ActivateForReceipient(CachedSubject);
                UpdateNewsImportance(true);
                tickReceived = Find.TickManager.TicksGame;
                hasBeenActivated = true;
            }
        }

        /// <summary>
        /// The subject forgets about the news. May be able to regain memory by being reminded, but that's another story.
        /// <para/>
        /// May also be used by Alzheimers.
        /// </summary>
        public void Forget()
        {
            newsImportance = null;
        }

        /// <summary>
        /// Also determines whether the news is forgotten due to low importance score.
        /// </summary>
        /// 
        [Obsolete("Use UpdateNewsImportance() instead", true)]
        public void Notify_UpdateNewsImportance()
        {
            if (IsLocallyForgotten || ReferencedTaleNews.PermanentlyForgotten)
            {
                // Just to be safe. Do nothing and return.
                DesynchronizedMain.LogWarning("Someone attempted to update importance score of a forgotten tale-news reference. This is unsafe behavior, and has been prevented.\n" + Environment.StackTrace);
                return;
            }

            cachedImportance = ReferencedTaleNews.CalculateNewsImportanceForPawn(CachedSubject, this);

            if (cachedImportance < 1)
            {
                Forget();
            }
            // CachedNewsImportance can already return 0 when IsLocallyForgotten
        }

        [Obsolete("Use UpdateNewsImportance()", true)]
        public void RecalculateNewsImportance()
        {
            // Safety checks; should not attempt to calculate importance score when the news is forgotten
            // The importance should technically be null, but let's make it simpler by using a 0 here.
            if (IsLocallyForgotten)
            {
                cachedImportance = 0;
            }
            else
            {
                cachedImportance = ReferencedTaleNews.CalculateNewsImportanceForPawn(CachedSubject, this);
            }
            
            // DesynchronizedMain.LogError("Recalculated #" + ReferencedTaleNews.UniqueID + " to have " + cachedImportance + " importance.");
        }

        public void FlagAsShockingNews()
        {
            newsIsShocking = true;
        }

        /// <summary>
        /// Ensures that the news reference correctly knows that the news is permanently forgotten.
        /// </summary>
        /// 
        [Obsolete("This is now automated.", true)]
        internal void Notify_PermanentlyForgotten()
        {
            locallyForgotten = true;
        }

        // Unused?
        [Obsolete("Is this even used anymore?", true)]
        internal void Notify_LocallyForgotten()
        {
            locallyForgotten = true;
        }

        /// <summary>
        /// Called by the TNDBS regularly to see if can transit KNOWN -> LOCALLY_FORGOT
        /// </summary>
        /// 
        [Obsolete("Use UpdateNewsImportance()", true)]
        internal void Notify_UpdateForgetfulnessState()
        {
            if (!locallyForgotten && cachedImportance < 1)
            {
                locallyForgotten = true;
            }
        }

        /// <summary>
        /// Called by the TNDBS regularly to recalculate news importance score.
        /// </summary>
        /// 
        [Obsolete("Use UpdateNewsImportance()", true)]
        internal void Notify_ConductImportanceUpdateCycle()
        {
            switch (ForgetState)
            {
                case ForgetfulnessState.KNOWN:
                    cachedImportance = ReferencedTaleNews.CalculateNewsImportanceForPawn(CachedSubject, this);
                    if (cachedImportance < 1)
                    {
                        locallyForgotten = true;
                        cachedImportance = 0;
                    }
                    break;
                case ForgetfulnessState.LOCALLY_FORGOT:
                case ForgetfulnessState.PERM_FORGOT:
                    cachedImportance = 0;
                    break;
                default:
                    break;
            }
            if (!locallyForgotten)
            {

            }


            // Safety checks; should not attempt to calculate importance score when the news is forgotten
            // The importance should technically be null, but let's make it simpler by using a 0 here.
            if (ReferencedTaleNews.PermanentlyForgotten)
            {
                IsLocallyForgotten = true;
            }
            if (locallyForgotten)
            {
                cachedImportance = 0;
            }
            else
            {
                cachedImportance = ReferencedTaleNews.CalculateNewsImportanceForPawn(CachedSubject, this);
            }
        }
        
        internal void SelfVerify()
        {
            // No need to initialize newsImportance; as a struct, when it exists, it is already initialized.
            if (!newsImportance.HasValue)
            {
                UpdateNewsImportance(true);
            }
        }

        /// <summary>
        /// Calculates and updates the news importance. If the importance score is low enough, the news is forgotten.
        /// </summary>
        /// <param name="forceRecheck"></param>
        public void UpdateNewsImportance(bool forceRecheck = false)
        {
            if (!forceRecheck && NewsIsLocallyForgotten)
            {
                return;
            }
            if (ReferencedTaleNews.PermanentlyForgotten)
            {
                // It's pointless to calculate an importance of something that does not exist.
                newsImportance = null;
            }
            else
            {
                float result = ReferencedTaleNews.CalculateNewsImportanceForPawn(cachedSubject, this);

                if (result < 1)
                {
                    // Forgets
                    newsImportance = null;
                }
                else
                {
                    newsImportance = result;
                }
            }
        }
    }
}
