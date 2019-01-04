using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    [Obsolete]
    public class ReceivedNewsObject
    {
        Dictionary<Pawn, List<TaleNews>> dictionaryOfReceivedNews = new Dictionary<Pawn, List<TaleNews>>();

        /// <summary>
        /// Adds a TaleNews to the list of received TaleNews, categorized by the receipient pawn of the TaleNews.
        /// The list of received TaleNews will be useful later.
        /// </summary>
        /// <param name="news"></param>
        public void AddTaleNews(TaleNews news)
        {
            Pawn receipient = news.NewsReceipient;
            if (receipient == null)
            {
                return;
            }

            if (!dictionaryOfReceivedNews.Keys.Contains(receipient))
            {
                dictionaryOfReceivedNews.Add(receipient, new List<TaleNews>());
            }

            dictionaryOfReceivedNews[receipient].Add(news);
        }
    }
}
