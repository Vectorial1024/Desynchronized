using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Desynchronized.TNDBS.Utilities;
using Desynchronized.TNDBS;

namespace Desynchronized.Interfaces
{
    public class PawnColumnWorker_ForgottenNewsCount : PawnColumnWorker_Text
    {
        private int GetForgottenNewsCount(Pawn pawn)
        {
            return pawn.GetNewsKnowledgeTracker().GetAllForgottenNewsReferences().Count();
        }

        protected override string GetTextFor(Pawn pawn)
        {
            return GetForgottenNewsCount(pawn).ToString();
        }

        protected override string GetTip(Pawn pawn)
        {
            StringBuilder builder = new StringBuilder("Number of forgotten tale-news: ");
            builder.Append(GetForgottenNewsCount(pawn));
            builder.AppendLine("\n");
            builder.Append("Old, unimportant tale-news are forgotten, and some of them are never remembered again.");

            return builder.ToString();
        }
    }
}
