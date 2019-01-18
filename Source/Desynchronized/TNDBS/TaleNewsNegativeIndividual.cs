using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public abstract class TaleNewsNegativeIndividual : TaleNews
    {
        private Pawn primaryVictim;
        protected InstigatorInfo instigatorInfo;

        public Pawn PrimaryVictim { get; }
        public InstigatorInfo InstigatorInfo { get; protected set; }
        /// <summary>
        /// The (primary) instigator of this negative tale-news, if there exists one.
        /// </summary>
        public Pawn Instigator
        {
            get
            {
                return InstigatorInfo.Instigator;
            }
        }

        public TaleNewsNegativeIndividual()
        {

        }

        public TaleNewsNegativeIndividual(Pawn victim, InstigatorInfo instigInfo): base(167)
        {
            PrimaryVictim = victim;
            InstigatorInfo = instigInfo;
        }

        protected override void ConductSaveFileIO()
        {
            Scribe_References.Look(ref primaryVictim, "primaryVictim");
            Scribe_Deep.Look(ref instigatorInfo, "instigatorInfo");
        }
    }
}
