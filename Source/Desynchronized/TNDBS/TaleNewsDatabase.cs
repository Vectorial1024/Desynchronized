using Desynchronized.TaleLibrary;
using HugsLib.Utils;
using RimWorld;
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

        private List<PawnKnowledgeCard> knowledgeMappings;

        public List<TaleNews> TalesOfImportance
        {
            get
            {
                return talesOfImportance;
            }
        }

        [Obsolete("")]
        public Dictionary<Pawn, List<TaleNewsReference>> NewsKnowledgeMapping
        {
            get
            {
                return newsKnowledgeMapping;
            }
        }

        public List<PawnKnowledgeCard> KnowledgeMappings
        {
            get
            {
                return knowledgeMappings;
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
            knowledgeMappings = new List<PawnKnowledgeCard>();
        }

        /// <summary>
        /// Used by RimWorld's Scribe system to I/O data.
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            // Scribe_Values.Look(ref listOfReadNews, "listOfReadNews");
            Scribe_Collections.Look(ref talesOfImportance, "talesOfImportance", LookMode.Deep);
            // Scribe_Values.Look(ref talesOfImportance, "talesOfImportance", new List<TaleNews>());
            // Scribe_Collections.Look(ref newsKnowledgeMapping, "newsKnowledgeMapping", LookMode.Reference, LookMode.Deep);
            Scribe_Values.Look(ref nextUID, "nextUID", -1);
            Scribe_Collections.Look(ref knowledgeMappings, "knowledgeMappings", LookMode.Deep);
        }

        public int GetNextUID()
        {
            try
            {
                nextUID = checked(nextUID + 1);
            }
            catch (OverflowException ex)
            {
                Find.LetterStack.ReceiveLetter(DesynchronizedMain.MODPREFIX + "Overflow Occured", "Report this situation to Desynchronized; it is time for an upgrade.", LetterDefOf.ThreatBig);
                DesynchronizedMain.LogError("Greetings, Ancient One. You have sucessfully broken this mod without exploiting any bug.\n" + ex.StackTrace);
                nextUID = -1;
            }

            return nextUID;
        }

        /// <summary>
        /// Registers a TaleNews object by giving it a valid UID, then adding it to the List.
        /// <para/>
        /// In current implementation, the constructor of TaleNews will also register itself to here.
        /// </summary>
        /// <param name="news"></param>
        public void RegisterNewTale(TaleNews news)
        {
            if (news == null)
            {
                DesynchronizedMain.LogError("An unexpected null TaleNews was received. Report this to Desynchronized.\n" + Environment.StackTrace);
                return;
            }

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

        [Obsolete("", true)]
        public void AssociateNewsRefToPawn(Pawn recipient, TaleNewsReference reference)
        {
            /*
            if (!NewsKnowledgeMapping.Keys.Contains(recipient))
            {
                NewsKnowledgeMapping.Add(recipient, null);
            }
            if (NewsKnowledgeMapping[recipient] == null)
            {
                NewsKnowledgeMapping[recipient] = new List<TaleNewsReference>();
            }
            NewsKnowledgeMapping[recipient].Add(reference);
            */
        }

        public void LinkPawnToNewsReference(Pawn recipient, TaleNewsReference reference)
        {
            foreach (PawnKnowledgeCard card in KnowledgeMappings)
            {
                if (recipient == card.Subject)
                {
                    card.ReceiveReference(reference);
                    return;
                }
            }

            // You are here because the given pawn does not have its own card.
            PawnKnowledgeCard newCard = new PawnKnowledgeCard(recipient, reference);
            KnowledgeMappings.Add(newCard);
        }
    }
}
