using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class PawnKnowledgeCard : IExposable
    {
        private Pawn subject;
        private List<TaleNewsReference> knowledgeList = new List<TaleNewsReference>();

        public Pawn Subject
        {
            get
            {
                return subject;
            }
        }

        public List<TaleNewsReference> KnowledgeList
        {
            get
            {
                return knowledgeList;
            }
        }

        /// <summary>
        /// DO NOT USE THIS EXPLICITLY.
        /// </summary>
        public PawnKnowledgeCard()
        {

        }

        /// <summary>
        /// Constructs this object with the subject pawn defined.
        /// </summary>
        /// <param name="subject"></param>
        public PawnKnowledgeCard(Pawn subject): this()
        {
            this.subject = subject;
        }

        /// <summary>
        /// A constructor that also gives the KnowledgeList its first entry.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="firstRef"></param>
        public PawnKnowledgeCard(Pawn subject, TaleNewsReference firstRef): this(subject)
        {
            knowledgeList.Add(firstRef);
        }

        public void ExposeData()
        {
            Scribe_References.Look(ref subject, "subject");
            Scribe_Collections.Look(ref knowledgeList, "knowledgeList", LookMode.Deep);
        }

        public void ReceiveReference(TaleNewsReference reference)
        {
            KnowledgeList.Add(reference);
        }
    }
}
