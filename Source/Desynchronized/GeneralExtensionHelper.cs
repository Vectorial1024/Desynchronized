using RimWorld;
using RimWorld.Planet;
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
            // Simplified from the glamorous usage of a try-catch block.
            return pawn?.needs?.mood?.thoughts != null;
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
                case "Spouse":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyWifeWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyHusbandWasKidnapped;
                    }
                    break;
                case "Fiance":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyWifeWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyHusbandWasKidnapped;
                    }
                    break;
                case "Lover":
                    resultingDef = Desynchronized_ThoughtDefOf.MyLoverWasKidnapped;
                    break;
                case "Sibling":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MySisterWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyBrotherWasKidnapped;
                    }
                    break;
                case "Grandchild":
                    resultingDef = Desynchronized_ThoughtDefOf.MyGrandchildWasKidnapped;
                    break;
                case "Parent":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyMotherWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyFatherWasKidnapped;
                    }
                    break;
                case "NephewOrNiece":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyNieceWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyNephewWasKidnapped;
                    }
                    break;
                case "HalfSibling":
                    resultingDef = Desynchronized_ThoughtDefOf.MyHalfSiblingWasKidnapped;
                    break;
                case "UncleOrAunt":
                    if (kidnapVictim.gender == Gender.Female)
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyAuntWasKidnapped;
                    }
                    else
                    {
                        resultingDef = Desynchronized_ThoughtDefOf.MyUncleWasKidnapped;
                    }
                    break;
                case "Grandparent":
                    resultingDef = Desynchronized_ThoughtDefOf.MyGrandparentWasKidnapped;
                    break;
                case "Cousin":
                    resultingDef = Desynchronized_ThoughtDefOf.MyCousinWasKidnapped;
                    break;
                case "Kin":
                    resultingDef = Desynchronized_ThoughtDefOf.MyKinWasKidnapped;
                    break;
                default:
                    break;
            }
            return resultingDef;
        }

        /// <summary>
        /// Extension method. Copied from vanilla code because vanilla code does not allow usage of this method.
        /// <para/>
        /// Already includes checking whether both pawns are in the same map.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool CanWitnessOtherPawn(this Pawn subject, Pawn other)
        {
            if (!subject.Awake() || !subject.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
            {
                return false;
            }
            if (other.IsCaravanMember())
            {
                return other.GetCaravan() == subject.GetCaravan();
            }
            if (!other.Spawned || !subject.Spawned)
            {
                return false;
            }
            if (!subject.Position.InHorDistOf(other.Position, 12f))
            {
                return false;
            }
            if (!GenSight.LineOfSight(other.Position, subject.Position, other.Map, false, null, 0, 0))
            {
                return false;
            }
            return true;
        }
    }
}
