﻿using HarmonyLib;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches.NewsTransmit
{
    [HarmonyPatch(typeof(Caravan))]
    [HarmonyPatch("Tick", MethodType.Normal)]
    public class PostFix_Caravan_Ticking
    {
        [HarmonyPostfix]
        public static void TickTheInteractionWorkers(Caravan __instance)
        {
            // Foreach pawn
            ThingOwner<Pawn> cache = __instance.pawns;
            for (int i = 0; i < cache.Count; i++)
            {
                Pawn target = cache[i];
                //target.interactions.InteractionsTrackerTick();
            }
        }
    }
}
