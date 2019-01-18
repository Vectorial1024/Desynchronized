﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public abstract class TaleNewsNeutralIndividual: TaleNews
    {
        private Pawn receiver;

        public Pawn Receiver
        {
            get
            {
                return receiver;
            }
        }

        public TaleNewsNeutralIndividual()
        {

        }

        public TaleNewsNeutralIndividual(Pawn receiver): base (128)
        {
            this.receiver = receiver;
        }

        protected override void ConductSaveFileIO()
        {
            Scribe_References.Look(ref receiver, "receiver");
            throw new NotImplementedException();
        }
    }
}
