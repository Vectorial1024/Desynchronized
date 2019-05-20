using Desynchronized.Handlers;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    /// <summary>
    /// Post-fixes to generate relevant kidnapped thoughts.
    /// </summary>
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("PreKidnapped", MethodType.Normal)]
    public class PostFix_Pawn_PreKidnapped
    {
        [HarmonyPostfix]
        public static void PostFix(Pawn __instance, Pawn kidnapper)
        {
            //Handler_PawnKidnapped.Signal_DefensiveBattle_PawnKidnapped(__instance.MapHeld);
            Handler_PawnKidnapped.HandlePawnKidnapped(__instance, kidnapper);
            //Handler_PawnKidnapped.Signal_ClearSignal();
            // PawnKidnappedThoughtsUtility.OnPawnAboutToBeKidnapped_Rudimentary(__instance, kidnapper);
        }
    }
}
