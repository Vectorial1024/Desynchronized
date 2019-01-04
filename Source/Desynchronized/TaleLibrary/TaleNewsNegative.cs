using Desynchronized.TNDBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    [Obsolete("", true)]
    public abstract class TaleNewsNegativeIndividual : TaleNews
    {
        public Pawn PrimaryVictim { get; }
        public Pawn Instigator { get; }

        public TaleNewsNegativeIndividual(Pawn receipient, Pawn victim, InstigatorInfo instigInfo): base(receipient)
        {
            PrimaryVictim = victim;
            Instigator = instigInfo.Instigator;
        }
    }
}
