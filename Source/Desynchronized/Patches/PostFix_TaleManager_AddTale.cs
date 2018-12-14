using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.Patches
{
    /// <summary>
    /// Post-fixes to intercept and inspect newly-generated Tales.
    /// </summary>
    [HarmonyPatch(typeof(TaleManager))]
    [HarmonyPatch("Add", MethodType.Normal)]
    public class PostFix_TaleManager_AddTale
    {
        [HarmonyPostfix]
        public static void PostFix(Tale tale)
        {
            if (DesynchronizedMain.WeAreInDevMode)
            {
                FileLog.Log("This is centralized patching. Tale is received: " + tale);
                if (tale.def == TaleDefOf.KidnappedColonist)
                {
                    Tale_DoublePawn actualTale = tale as Tale_DoublePawn;
                    if (actualTale != null)
                    {
                        FileLog.Log("Redirecting now.");
                        // FileLog.Log("Both pawns involved are: " + actualTale.firstPawnData.pawn + actualTale.secondPawnData.pawn);
                        DesynchronizedMain.CentralTaleDatabase.foo(actualTale, actualTale.secondPawnData.pawn);
                        DesynchronizedMain.CentralTaleDatabase.dumpList();
                    }
                }
            }
        }
    }
}
