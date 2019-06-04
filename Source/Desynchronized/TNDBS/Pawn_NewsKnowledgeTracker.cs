using Desynchronized.TNDBS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class Pawn_NewsKnowledgeTracker: IExposable
    {
        private Pawn pawn;
        private List<TaleNewsReference> newsKnowledgeList = new List<TaleNewsReference>();

        public Pawn Pawn
        {
            get
            {
                return pawn;
            }
        }

        public List<TaleNewsReference> ListOfAllKnownNews
        {
            get
            {
                return newsKnowledgeList;
            }
        }

        /// <summary>
        /// This constructor does nothing; better use the static generator method instead.
        /// </summary>
        public Pawn_NewsKnowledgeTracker()
        {

        }

        /// <summary>
        /// Generates a new Tracker using verified code for a pawn.
        /// </summary>
        /// <param name="beneficiary"></param>
        /// <returns></returns>
        public static Pawn_NewsKnowledgeTracker GenerateNewTrackerForPawn(Pawn beneficiary)
        {
            return new Pawn_NewsKnowledgeTracker()
            {
                pawn = beneficiary
            };
        }

        public bool IsValid()
        {
            return (pawn != null);
        }

        public void ExposeData()
        {
            Scribe_References.Look(ref pawn, "pawn");
            Scribe_Collections.Look(ref newsKnowledgeList, "newsKnowledgeList", LookMode.Deep);

            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                foreach (TaleNewsReference reference in newsKnowledgeList)
                {
                    reference.CachedSubject = pawn;
                }
            }
        }

        [Obsolete("Unsafe code. Use KnowNews instead.")]
        public void ReceiveReference(TaleNewsReference reference)
        {
            if (reference != null && !reference.IsDefaultReference())
            {
                newsKnowledgeList.Add(reference);
            }
        }

        /// <summary>
        /// Finds the TaleNewsReference of the given TaleNews in the known list (or generates a new one if it does not exist), and activates it.
        /// </summary>
        /// <param name="news"></param>
        public void KnowNews(TaleNews news, WitnessShockGrade shockGrade)
        {
            foreach (TaleNewsReference reference in newsKnowledgeList)
            {
                if (reference.IsReferencingTaleNews(news))
                {
                    reference.ActivateNews(shockGrade);
                    return;
                }
            }

            TaleNewsReference newReference = news.CreateReferenceForReceipient(Pawn);
            newsKnowledgeList.Add(newReference);
            newReference.ActivateNews(shockGrade);
            return;
        }

        /// <summary>
        /// Randomly selects a news, and forgets it.
        /// Does not check for whether the news has already been forgotten before.
        /// </summary>
        public void ForgetRandom()
        {
            int count = newsKnowledgeList.Count;
            int selectedIndex = Rand.Int % count;
            newsKnowledgeList[selectedIndex].Forget();
        }

        public void RemoveMemory()
        {
            throw new NotImplementedException();
        }

        public TaleNewsReference AttemptToObtainExistingReference(TaleNews news)
        {
            foreach (TaleNewsReference reference in newsKnowledgeList)
            {
                if (reference.ReferencedTaleNews.UniqueID == news.UniqueID)
                {
                    return reference;
                }
            }

            return null;
        }
    }
}
