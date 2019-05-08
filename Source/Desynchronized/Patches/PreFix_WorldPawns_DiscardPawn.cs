using Harmony;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(WorldPawns))]
    [HarmonyPatch("DiscardPawn", MethodType.Normal)]
    public class PreFix_WorldPawns_DiscardPawn
    {
        [HarmonyPrefix]
        public static bool PreFix(Pawn p)
        {
            /*
            DesynchronizedMain.LogError("Processing " + p + "; does this exist in our Hall of Figures? " + DesynchronizedMain.TheHallOfFigures.ContainsPawn(p));
            if (DesynchronizedMain.TheHallOfFigures.ContainsPawn(p))
            {
                return false;
            }
            else
            {
                return true;
            }
            */

            return true;
        }
    }
}
