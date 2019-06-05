using Desynchronized.Handlers;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(MapDeiniter))]
    [HarmonyPatch("Deinit", MethodType.Normal)]
    public class MultiFix_MapDeiniter
    {
        // Just in case we have pawns lost in offensive battles.
        [HarmonyPrefix]
        public static bool PreFix(Map map)
        {
            //DesynchronizedMain.LogError("Map from patcher: " + map);
            Handler_PawnKidnapped.Signal_OffensiveBattle_BeginBlock(map);
            return true;
        }

        // Reset the signal for future usage.
        [HarmonyPostfix]
        public static void PostFix()
        {
            Handler_PawnKidnapped.Signal_OffensiveBattle_EndBlock();
        }
    }
}
