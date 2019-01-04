using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    /*
     * After weeks of intensive thinking and coding, we have come up with
     * a much more general solution to the out-of-map thought "censorship"
     * mechanics. This patch is no longer necessary.
     */
    /*
    [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility))]
    [HarmonyPatch("AppendThoughts_ForHumanlike", MethodType.Normal)]
    public class PostFix_SpecialThoughtsUtil_ThoughtsForHumans
    {
        /// <summary>
        /// A post-fix to remove out-of-the-map Colonist Died thought.
        /// Also fixes Prisoner Died Innocent thought.
        /// </summary>
        /// <param name="outIndividualThoughts"></param>
        [HarmonyPostfix]
        public static void PostFix(Pawn victim, ref List<IndividualThoughtToAdd> outIndividualThoughts)
        {
            // FileLog.Log("We have victim's information. Vistim's name is " + victim.Name + ", and his map is: [" + victim.Map + "]");
            // FileLog.Log("Just in case, his corpse is at map: [" + victim.Corpse.Map + "]");

            // Let's be less intense here and use a for loop.
            // Since we will be removing items, loop from the back of the loop.
            for (int i = outIndividualThoughts.Count - 1; i >= 0; i--)
            {
                IndividualThoughtToAdd thought = outIndividualThoughts[i];
                ThoughtDef theDef = thought.thought.def;
                bool shouldRemove = false;

                // FileLog.Log("Inspecting thought " + i + " for pawn " + thought.addTo.Name + "; the thought def is + " + theDef);

                if (theDef == ThoughtDefOf.KnowColonistDied || theDef == ThoughtDefOf.KnowPrisonerDiedInnocent)
                {
                    // Check that the map contained is the same.
                    if (thought.addTo.Map != victim.Map)
                    {
                        shouldRemove = true;
                    }
                }

                if (shouldRemove)
                {
                    outIndividualThoughts.RemoveAt(i);
                }
            }
        }
    }
    */
}
