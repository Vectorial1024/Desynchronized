using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class TaleNewsColonyEstablished : TaleNewsNeutralIndividual
    {
        public override string GetNewsIdentifier()
        {
            throw new NotImplementedException();
        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            throw new NotImplementedException();
        }
    }
}
