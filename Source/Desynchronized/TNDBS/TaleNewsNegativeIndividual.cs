using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public abstract class TaleNewsNegativeIndividual : TaleNews
    {
        public Pawn PrimaryVictim { get; }
        public InstigatorInfo InstigatorInfo { get; protected set; }
        public Pawn Instigator { get; }

        public TaleNewsNegativeIndividual(Pawn victim, InstigatorInfo instigInfo)
        {
            PrimaryVictim = victim;
            InstigatorInfo = instigInfo;
            Instigator = InstigatorInfo.Instigator;
        }
    }
}
