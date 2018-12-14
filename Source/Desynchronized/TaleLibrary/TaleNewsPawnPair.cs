using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    public class TaleNewsPawnPair
    {
        public TaleNews News { get; set; }
        public Pawn Pawn { get; set; }

        public TaleNewsPawnPair(TaleNews news, Pawn pawn)
        {
            News = news;
            Pawn = pawn;
        }

        public override string ToString()
        {
            return "Pawn " + Pawn + " News " + News;
        }
    }
}
