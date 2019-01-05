using Desynchronized.TaleLibrary;
using HugsLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// Initialized in PostAdd and ExposeData
        /// </summary>
        private List<TaleNews> talesOfImportance;
        /// <summary>
        /// Initialized in PostAdd and ExposeData
        /// </summary>
        private Dictionary<Pawn, List<TaleNewsReference>> newsKnowledgeMapping;

        public List<TaleNews> TalesOfImportance
        {
            get
            {
                return talesOfImportance;
            }
        }
        public Dictionary<Pawn, List<TaleNewsReference>> NewsKnowledgeMapping
        {
            get
            {
                return newsKnowledgeMapping;
            }
        }

        /// <summary>
        /// Initialized in PostAdd and ExposeData
        /// </summary>
        private int nextUID;

        /// <summary>
        /// Use this in place of the Constructor.
        /// </summary>
        public override void PostAdd()
        {
            base.PostAdd();
            talesOfImportance = new List<TaleNews>();
            newsKnowledgeMapping = new Dictionary<Pawn, List<TaleNewsReference>>();
            nextUID = -1;
        }

        /// <summary>
        /// Used by RimWorld's Scribe system to I/O data.
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            // Scribe_Values.Look(ref listOfReadNews, "listOfReadNews");
            Scribe_Values.Look(ref talesOfImportance, "talesOfImportance", new List<TaleNews>());
            Scribe_Values.Look(ref newsKnowledgeMapping, "newsKnowledgeMapping", new Dictionary<Pawn, List<TaleNewsReference>>());
            Scribe_Values.Look(ref nextUID, "nextUID", -1);
        }

        public int GetNextUID()
        {
            return ++nextUID;
        }

        /// <summary>
        /// Registers a TaleNews object by giving it a valid UID, then adding it to the List.
        /// <para/>
        /// In current implementation, the constructor of TaleNews will also register itself to here.
        /// </summary>
        /// <param name="news"></param>
        public void RegisterNewTale(TaleNews news)
        {
            if (!news.IsRegistered)
            {
                news.BecomeRegistered(GetNextUID());
                TalesOfImportance.Add(news);
            }
        }

        public TaleNews SafelyGetTaleNews(int id)
        {
            if (id < 0 || id >= TalesOfImportance.Count)
            {
                return null;
            }

            return TalesOfImportance[id];
        }

        public void AssociateNewsRefToPawn(Pawn recipient, TaleNewsReference reference)
        {
            if (!NewsKnowledgeMapping.Keys.Contains(recipient))
            {
                NewsKnowledgeMapping.Add(recipient, null);
            }
            if (NewsKnowledgeMapping[recipient] == null)
            {
                NewsKnowledgeMapping[recipient] = new List<TaleNewsReference>();
            }
            NewsKnowledgeMapping[recipient].Add(reference);
        }
    }
}
