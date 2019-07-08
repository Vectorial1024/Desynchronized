﻿using Desynchronized.TNDBS.Extenders;
using HugsLib.Utils;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Desynchronized.TNDBS
{
    /// <summary>
    /// The namespace name "TNDBS" is derived from the name of this class:
    /// Tale-News-Data-Base-System.
    /// </summary>
    public class TaleNewsDatabase: UtilityWorldObject
    {
        /// <summary>
        /// Auto-initialized in ExposeData
        /// </summary>
        private List<TaleNews> talesOfImportance;

        /// <summary>
        /// Auto-initialized in ExposeData
        /// </summary>
        private List<Pawn_NewsKnowledgeTracker> knowledgeTrackerMasterList;

        private bool safetyValve_ShouldConductImportanceUpdate = true;

        public IEnumerable<TaleNews> TalesOfImportance_ReadOnly
        {
            get
            {
                return talesOfImportance;
            }
        }

        internal List<TaleNews> ListOfAllTaleNews => talesOfImportance;

        public List<Pawn_NewsKnowledgeTracker> KnowledgeTrackerMasterList
        {
            get
            {
                return knowledgeTrackerMasterList;
            }
        }

        private int tickerInternal;

        /// <summary>
        /// Stores the next UID that will be given to newer TaleNews.
        /// </summary>
        private int nextUID;

        /// <summary>
        /// Lets us access the TaleNews inside the TaleNewsDatabase. You should supply a proper index.
        /// <para/>
        /// Usually, you need not and should not set the index of TaleNews manually.
        /// </summary>
        /// <param name="param">The UID of the TaleNews you intend to retrieve</param>
        /// <returns>The TaleNews if it exists; null otherwise.</returns>
        public TaleNews this[int param]
        {
            get
            {
                if (param < 0)
                {
                    return null;
                }
                if (param < talesOfImportance.Count)
                {
                    return talesOfImportance[param];
                }
                else
                {
                    return null;
                }
            }
        }

        private void ResetOrInitialize()
        {
            nextUID = 0;
            if (talesOfImportance == null)
            {
                talesOfImportance = new List<TaleNews>();
            }
            else
            {
                talesOfImportance.Clear();
            }
            // Doing this ensures the DefaultTaleNews always getting the 0th position.
            RegisterNewTaleNews(TaleNews.DefaultTaleNews);
            if (knowledgeTrackerMasterList == null)
            {
                knowledgeTrackerMasterList = new List<Pawn_NewsKnowledgeTracker>();
            }
            else
            {
                knowledgeTrackerMasterList.Clear();
            }
        }

        /// <summary>
        /// Use this method as a constructor
        /// </summary>
        public override void PostAdd()
        {
            base.PostAdd();
            ResetOrInitialize();
        }

        /// <summary>
        /// Used by RimWorld's Scribe system to I/O data.
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                // We can run the clean-up code during saving.
                ConsolidateLists();
            }
            
            Scribe_Collections.Look(ref talesOfImportance, "talesOfImportance", LookMode.Deep);
            Scribe_Values.Look(ref nextUID, "nextUID", 0);
            Scribe_Collections.Look(ref knowledgeTrackerMasterList, "knowledgeTrackerMasterList", LookMode.Deep);
            Scribe_Values.Look(ref tickerInternal, "tickerInternal", 0);
            
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                if (knowledgeTrackerMasterList == null)
                {
                    knowledgeTrackerMasterList = new List<Pawn_NewsKnowledgeTracker>();
                }
            }
        }

        /// <summary>
        /// This method is a gateway to let us access the database to "clean it up" before actually saving it.
        /// </summary>
        [Obsolete("Conside connecting this to something existing")]
        private void ConsolidateLists()
        {
            // DumpAllTaleNewsReferences_v1450();
        }

        public int GetNextUID()
        {
            int result = nextUID;
            try
            {
                nextUID = checked(nextUID + 1);
            }
            catch (OverflowException ex)
            {
                Find.LetterStack.ReceiveLetter(DesynchronizedMain.MODPREFIX + "Overflow Occured", "Report this situation to Desynchronized; it is time for an upgrade.", LetterDefOf.ThreatBig);
                DesynchronizedMain.LogError("Greetings, Ancient One. You have sucessfully broken this mod without exploiting any bug.\n" + ex.StackTrace);
                nextUID = 0;
            }

            return result;
        }

        /// <summary>
        /// Registers a TaleNews object by giving it a valid UID, then adding it to the List.
        /// <para/>
        /// In current implementation, the constructor of TaleNews will also register itself to here.
        /// </summary>
        /// <param name="news"></param>
        public void RegisterNewTaleNews(TaleNews news)
        {
            if (news == null)
            {
                DesynchronizedMain.LogError("An unexpected null TaleNews was received. Report this to Desynchronized.\n" + Environment.StackTrace);
                return;
            }

            if (!news.IsRegistered)
            {
                news.ReceiveRegistrationID(GetNextUID());
                talesOfImportance.Add(news);
            }
        }

        /// <summary>
        /// This is the self-patching method to be released in v1.4.5, targetting the v1.4 family.
        /// <para/>
        /// It completely wipes the TaleNews database, and also removes 
        /// invalid PawnKnowledgeCards that has a null subject.
        /// </summary>
        internal void SelfPatching_NullVictims()
        {
            ResetOrInitialize();
        }

        /// <summary>
        /// Method used to self-verify and self-patch this TNDBS object in case when it is oaded form a previous version.
        /// </summary>
        internal void SelfVerify()
        {
            // Confirm all variables/structs are initialized.
            if (talesOfImportance == null)
            {
                talesOfImportance = new List<TaleNews>();
            }
            if (knowledgeTrackerMasterList == null)
            {
                knowledgeTrackerMasterList = new List<Pawn_NewsKnowledgeTracker>();
            }

            // Validate all variables/structs
            RemoveAllInvalidTaleNews();

            // Verify all tale-news references
            VerifyAllTaleNewsReferences();
        }

        private void RemoveAllInvalidTaleNews()
        {
            // Step 1: Identify all invalid TaleNews, and label them.
            Dictionary<int, int> newsIDPairing = new Dictionary<int, int>();
            int currentNewsID = 0;
            const int invalidID = -1;

            foreach (TaleNews taleNews in talesOfImportance)
            {
                if (taleNews.IsValid())
                {
                    newsIDPairing.Add(taleNews.UniqueID, currentNewsID);
                    currentNewsID++;
                }
                else
                {
                    newsIDPairing.Add(taleNews.UniqueID, invalidID);
                }
            }

            // Step 2: Remove all invalid TaleNewsReferences, and update valid ones to point to the new ID.
            // Also drops invalid Pawn_NewsKnowledgeTrackers
            for (int i = knowledgeTrackerMasterList.Count - 1; i >= 0; i--)
            {
                Pawn_NewsKnowledgeTracker knowledgeTracker = knowledgeTrackerMasterList[i];

                if (!knowledgeTracker.IsValid())
                {
                    knowledgeTrackerMasterList.RemoveAt(i);
                }
                else
                {
                    for (int j = knowledgeTracker.ListOfAllKnownNews.Count - 1; j >= 0; j--)
                    {
                        int mappedResult = newsIDPairing[knowledgeTracker.ListOfAllKnownNews[j].ReferencedTaleNews.UniqueID];
                        if (mappedResult == invalidID)
                        {
                            knowledgeTracker.ListOfAllKnownNews.RemoveAt(j);
                        }
                        else
                        {
                            knowledgeTracker.ListOfAllKnownNews[j].ChangeReferencedUID(mappedResult);
                        }
                    }
                }
            }

            // Step 3: Re-enter TaleNews
            List<TaleNews> tempList = new List<TaleNews>();
            tempList.AddRange(talesOfImportance);
            talesOfImportance = new List<TaleNews>();
            nextUID = 0;

            foreach (TaleNews taleNews in tempList)
            {
                if (newsIDPairing[taleNews.UniqueID] != invalidID)
                {
                    taleNews.ReRegisterWithID(nextUID);
                    talesOfImportance.Add(taleNews);
                    nextUID++;
                }
            }

            // Should be ready now.
            return;
        }

        private void VerifyAllTaleNewsReferences()
        {
            foreach (Pawn_NewsKnowledgeTracker tracker in KnowledgeTrackerMasterList)
            {
                foreach (TaleNewsReference reference in tracker.AllNewsReferences)
                {
                    reference.SelfVerify();
                }
            }
        }

        public override void Tick()
        {
            base.Tick();
            tickerInternal++;

            if (safetyValve_ShouldConductImportanceUpdate)
            {
                try
                {
                    // 8 calculations per day, should be enough -> 3 in-game hours
                    // 60000 / 8 = 7500
                    while (tickerInternal > 7500)
                    {
                        ImportanceUpdateCycle_DoOnce();
                        tickerInternal -= 7500;
                    }
                }
                catch (Exception ex)
                {
                    DesynchronizedMain.LogError("A critical error has occured while attempting to update news importance scores. This periodical process has been terminated. Please also include the full log (using HugsLib's log-sharing) when reporting this error.");
                    DesynchronizedMain.LogError(ex.ToString());
                    safetyValve_ShouldConductImportanceUpdate = false;
                }
            }
        }

        private void ImportanceUpdateCycle_DoOnce()
        {
            TaleNewsDatabase database = DesynchronizedMain.TaleNewsDatabaseSystem;

            // Establish a counter of all *non-perm-forgot* tale-news
            Dictionary<TaleNews, int> remembranceCounter = new Dictionary<TaleNews, int>();
            foreach (TaleNews news in database.GetAllNonPermForgottenNews())
            {
                remembranceCounter.Add(news, 0);
            }

            // Update the importance scores of all *non-forgotten* news,
            // see if any of them are to be forgotten.
            foreach (Pawn_NewsKnowledgeTracker tracker in DesynchronizedMain.TaleNewsDatabaseSystem.KnowledgeTrackerMasterList)
            {
                foreach (TaleNewsReference reference in tracker.GetAllNonForgottenNewsReferences())
                {
                    reference.UpdateNewsImportance();
                    if (!reference.NewsIsLocallyForgotten)
                    {
                        remembranceCounter[reference.ReferencedTaleNews]++;
                    }
                }
            }

            // Purge all forgotten news
            foreach (KeyValuePair<TaleNews, int> kvPair in remembranceCounter)
            {
                if (kvPair.Key.UniqueID == TaleNews.DefaultTaleNews.UniqueID)
                {
                    continue;
                }
                if (kvPair.Value == 0)
                {
                    kvPair.Key.Signal_NewsIsPermanentlyForgotten();
                }
            }
        }

        public bool PawnIsInvolvedInSomeTaleNews(Pawn pawn)
        {
            int totalNewsCount = talesOfImportance.Count;
            for (int i = 0; i < totalNewsCount; i++)
            {
                if (this[i].PawnIsInvolved(pawn))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
