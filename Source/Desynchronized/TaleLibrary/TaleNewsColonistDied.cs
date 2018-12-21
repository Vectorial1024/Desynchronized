using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    public class TaleNewsColonistDied : TaleNews
    {
        public Pawn Victim { get; }

        private TaleNewsColonistDied(Pawn receipient, Tale_DoublePawn tale): base(receipient)
        {

        }

        protected override void GiveThoughts()
        {
            throw new NotImplementedException();
        }
    }
}
