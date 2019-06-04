using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.Interfaces
{
    public class MainTabWindow_Desynchronized : MainTabWindow_PawnTable
    {
        protected override PawnTableDef PawnTableDef => Desynchronized_PawnTableDefOf.NewsKnowledge;
    }
}
