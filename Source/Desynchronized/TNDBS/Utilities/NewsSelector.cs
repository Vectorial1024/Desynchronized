using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.TNDBS.Utilities
{
    public static class NewsSelector
    {
        public static IEnumerable<TaleNews> GetAllNonForgottenNews(this Pawn_NewsKnowledgeTracker tracker)
        {
            if (tracker == null)
            {
                return Enumerable.Empty<TaleNews>();
            }

            // EMBRACE THE POWER OF LINQ; LINQ PROTECTS
            return tracker.ListOfAllKnownNews.FindAll((TaleNewsReference reference) => !reference.NewsIsLocallyForgotten).Select((TaleNewsReference reference) => reference.ReferencedTaleNews);
        }

        public static IEnumerable<TaleNewsReference> GetAllNonForgottenNewsReferences(this Pawn_NewsKnowledgeTracker tracker)
        {
            if (tracker == null)
            {
                return Enumerable.Empty<TaleNewsReference>();
            }

            // EMBRACE THE POWER OF LINQ; LINQ PROTECTS
            return tracker.ListOfAllKnownNews.FindAll((TaleNewsReference reference) => !reference.NewsIsLocallyForgotten);
        }

        public static IEnumerable<TaleNewsReference> GetAllForgottenNewsReferences(this Pawn_NewsKnowledgeTracker tracker)
        {
            if (tracker == null)
            {
                return Enumerable.Empty<TaleNewsReference>();
            }

            // EMBRACE THE POWER OF LINQ; LINQ PROTECTS
            return tracker.ListOfAllKnownNews.FindAll((TaleNewsReference reference) => reference.NewsIsLocallyForgotten);
        }

        public static IEnumerable<TaleNews> GetAllNonPermForgottenNews(this TaleNewsDatabase database)
        {
            if (database == null)
            {
                // This is highly unlikely, but be prepared.
                return Enumerable.Empty<TaleNews>();
            }

            return database.ListOfAllTaleNews.FindAll((TaleNews news) => !news.PermanentlyForgotten).Select((TaleNews news) => news);
        }
    }
}
