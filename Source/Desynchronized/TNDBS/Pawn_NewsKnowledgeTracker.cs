﻿using Desynchronized.TNDBS.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Obsolete("Unsafe code.")]
        public List<TaleNewsReference> ListOfAllKnownNews
        {
            get
            {
                return newsKnowledgeList;
            }
        }

        internal List<TaleNewsReference> NewsKnowledgeList => newsKnowledgeList;

        [Obsolete("Unsafe code.")]
        public List<TaleNewsReference> AllNewsReferences_Raw => newsKnowledgeList;

        public IEnumerable<TaleNewsReference> AllNewsReferences_ReadOnlyEnumerable => newsKnowledgeList.AsEnumerable();

        public List<TaleNewsReference> AllNewsReferences_ReadOnlyList => new List<TaleNewsReference>(newsKnowledgeList);

        public IEnumerable<TaleNewsReference> AllValidNewsReferences
        {
            get
            {
                foreach (TaleNewsReference reference in newsKnowledgeList)
                {
                    if (reference.ReferenceIsValid)
                    {
                        yield return reference;
                    }
                }

                yield break;
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

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                foreach (TaleNewsReference reference in newsKnowledgeList)
                {
                    reference.CachedSubject = pawn;
                }
            }
        }

        /// <summary>
        /// Finds the TaleNewsReference of the given TaleNews in the known list (or generates a new one if it does not exist), and activates it.
        /// </summary>
        /// <param name="news"></param>
        public void KnowNews(TaleNews news)
        {
            if (AttemptToObtainExistingReference(news) == null)
            {
                // Pawn is receiving this for the first time.
                TaleNewsReference newReference = news.CreateReferenceForReceipient(Pawn);
                newsKnowledgeList.Add(newReference);
                newReference.ActivateNews();
            }
            else
            {
                // Pawn might have forgotten about the news, so let's see.
                // Not implemented for now.
            }
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

        /// <summary>
        /// Forgets one known tale-news. Returns true if successfully forgetting one.
        /// </summary>
        /// <returns></returns>
        public bool ForgetOneRandom()
        {
            List<TaleNewsReference> listOfKnownNews = this.GetAllValidNonForgottenNewsReferences().ToList();
            if (listOfKnownNews.Count == 0)
            {
                return false;
            }

            int selectedIndex = (int)(((uint)Rand.Int) % listOfKnownNews.Count);
            listOfKnownNews[selectedIndex].Forget();
            return true;
        }

        /// <summary>
        /// Forgets a number of tale-news that this pawn knows.
        /// </summary>
        /// <param name="count"></param>
        public void ForgetRandomly(int count = 1)
        {
            if (count <= 0)
            {
                throw new ArgumentException("You cannot forget " + count + " tale-news.");
            }

            while (count > 0)
            {
                if (ForgetOneRandom())
                {
                    count--;
                }
                else
                {
                    break;
                }
            }
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
