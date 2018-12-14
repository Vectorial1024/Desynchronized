using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    public class TaleEntry
    {
        private TaleReference reference;
        private List<Pawn> receipients;

        public TaleEntry(TaleReference reference)
        {
            InitializeWithReference(reference);
        }

        public TaleEntry(Tale tale)
        {
            InitializeWithReference(new TaleReference(tale));
        }

        private void InitializeWithReference(TaleReference reference)
        {
            this.reference = reference;
            receipients.AddRange(PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive);
        }

        public void AppendReceipientListWith(Pawn pawn)
        {

        }
    }
}
