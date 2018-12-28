using Desynchronized.TaleLibrary;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.Patches
{
    /*
    /// <summary>
    /// Post-fix to intercept and inspect newly-generated Tales. This post-fix will then call
    /// the various methods to give relevant Thoughts to Pawns and Colonists. Class does not
    /// contain any method other than the PostFix method.
    /// </summary>
    [HarmonyPatch(typeof(TaleManager))]
    [HarmonyPatch("Add", MethodType.Normal)]
    public class PostFix_TaleManager_AddTale
    {
        [HarmonyPostfix]
        public static void PostFix(Tale tale)
        {
            // We are actually recommended by Visual Studio to try pattern-matching. Here goes.
            switch (tale)
            {
                case Tale_DoublePawn actualTale when tale.def == TaleDefOf.KidnappedColonist:
                    EventHandler_Kidnapping.HandleEventColonistKidnapped(actualTale);
                    break;
                default:
                    break;
            }
        }
    }
    */
}
