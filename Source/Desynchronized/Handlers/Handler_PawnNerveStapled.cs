using Desynchronized.TNDBS.NewsNegative;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Handlers
{
    public class Handler_PawnNerveStapled
    {
        public static void HandlePawnNerveStapled(Pawn victim, Pawn billDoer)
        {
            DesynchronizedMain.LogError("Yo");
            // Directly generate news.
            News_PawnNerveStapled news = new News_PawnNerveStapled(victim, billDoer);

            foreach (Pawn other in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                if (other.IsInSameMapOrCaravan(victim))
                {
                    other.GetNewsKnowledgeTracker().KnowNews(news);
                }
            }
        }
    }
}
