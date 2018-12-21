using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized
{
    public static class GeneralExtensionHelper
    {
        /// <summary>
        /// Extension method. Returns true if can have Thoughts. Is NullReference-safe.
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        public static bool IsCapableOfThought(this Pawn pawn)
        {
            // It is a simple spell, but quite unbreakable.
            try
            {
                // This statement, combined with the try-catch block, checks if all of the 4 variables/properties are non-null
                ThoughtHandler handler = pawn.needs.mood.thoughts;
                return true;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        /*
        public static bool IsAboutColonistDeath()
        {

        }
        */

        public static ThoughtDef GetGenderSpecificKidnappedThought(this PawnRelationDef relation, Pawn kidnapVictim)
        {
            ThoughtDef resultingDef = null;
            switch (relation.defName)
            {
                case "Child":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyDaughterWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MySonWasKidnapped;
                    }
                    break;
                default:
                    break;
            }
            return resultingDef;
        }
    }
}
