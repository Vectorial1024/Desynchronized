using HugsLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Utilities
{
    public class HallOfFigures : UtilityWorldObject
    {
        private HashSet<Pawn> pawnsRetained = new HashSet<Pawn>();

        public IEnumerable<Pawn> PawnsRetainedByTaleNews => pawnsRetained;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref pawnsRetained, true, "pawnsRetained", LookMode.Deep);
        }

        public void RetainPawn(Pawn deceased)
        {
            pawnsRetained.Add(deceased);
        }

        public bool ContainsPawn(Pawn target)
        {
            return pawnsRetained.Contains(target);
        }
    }
}
