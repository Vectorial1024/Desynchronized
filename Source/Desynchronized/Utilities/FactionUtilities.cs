using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.Utilities
{
    public static class FactionUtilities
    {
        public static int CalculateFactionGoodwillSafely(Faction subject, Faction other)
        {
            if (subject == other)
            {
                if (subject == null)
                {
                    // Both are null
                    return 0;
                }
                else
                {
                    // Both are non-null
                    return 100;
                }
            }
            else
            {
                if (subject == null || other == null)
                {
                    // One of them is null
                    return 0;
                }
                else
                {
                    // Both of them are non-null
                    return subject.RelationWith(other).goodwill;
                }
            }
        }

        public static int GetGoodwillWith(this Faction subject, Faction other)
        {
            return CalculateFactionGoodwillSafely(subject, other);
        }
    }
}
