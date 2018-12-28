using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility))]
    [HarmonyPatch("AppendThoughts_ForHumanlike", MethodType.Normal)]
    public class Prefix_ThoughtsUtility_ThoughtsForHumanlike
    {
        [HarmonyPrefix]
        public static bool PreFix()
        {
            return true;
        }
    }
}
