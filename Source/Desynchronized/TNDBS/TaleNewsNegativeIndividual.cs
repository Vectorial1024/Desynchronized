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

        public Pawn PrimaryVictim
        {
            get
            {
                return primaryVictim;
            }
        }

        public InstigatorInfo InstigatorInfo
        {
            get
            {
                return instigatorInfo;
            }
            protected set
            {
                instigatorInfo = value;
            }
        }

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
            primaryVictim = victim;
            InstigatorInfo = instigInfo;
        }

        public static TaleNewsNegativeIndividual GenerateTaleNewsNegativeIndividual(TaleNewsTypeEnum typeEnum, Pawn primaryVictim, InstigatorInfo instigatorInfo)
        {
            TaleNewsNegativeIndividual taleNews = GenerateTaleNewsGenerally(typeEnum) as TaleNewsNegativeIndividual;
            taleNews.primaryVictim = primaryVictim;
            taleNews.instigatorInfo = instigatorInfo;
            return taleNews;
        }

        protected override void ConductSaveFileIO()
        {
            Scribe_References.Look(ref primaryVictim, "primaryVictim", true);
            Scribe_Deep.Look(ref instigatorInfo, "instigatorInfo");
        }
    }
}
