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
    [HarmonyPatch("RemovePawn", MethodType.Normal)]
    public class PostFix_WorldPawns_RemovePawn
    {
        [HarmonyPostfix]
        public static void PostFix(Pawn p)
        {
            /*
            DesynchronizedMain.LogError("Retaining pawn " + p);
            DesynchronizedMain.TheHallOfFigures.RetainPawn(p);
            */
        }
    }
}
