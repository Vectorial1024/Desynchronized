using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.TaleLibrary
{
    public class OutstandingNewsObject
    {
        List<TaleNews> listOfOutstandingNews;

        public List<TaleNews> PendingNews
        {
            get
            {
                if (listOfOutstandingNews == null)
                {
                    listOfOutstandingNews = new List<TaleNews>();
                }

                return listOfOutstandingNews;
            }
        }

        /// <summary>
        /// Adds a TaleNews to the Pending List. The TaleNews will have to be given to its receipient in the future.
        /// </summary>
        /// <param name="news"></param>
        public void AddTaleNewsToPending(TaleNews news)
        {
            PendingNews.Add(news);
        }
    }
}
