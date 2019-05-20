using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS.Utilities
{
    public static class TaleNewsUtilExtender
    {
        public static readonly int ForgetImportanceThreshold = 1;

        /// <summary>
        /// The pawn forgets 1 news which is below the forgetfulness threshold.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="amount"></param>
        public static void ForgetNews(this Pawn subject, int amount = 1)
        {
            List<TaleNewsReference> allKnownNews = subject.GetNewsKnowledgeTracker().ListOfAllKnownNews;

            /*
            foreach ()
            {

            }
            */
        }
    }
}
