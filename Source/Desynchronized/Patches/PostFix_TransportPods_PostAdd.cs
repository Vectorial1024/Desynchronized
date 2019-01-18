using Harmony;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.Patches
{
    /// <summary>
    /// Marks down all the Gift Sender pods leaving their launching map.
    /// </summary>
    [HarmonyPatch(typeof(TravelingTransportPods))]
    [HarmonyPatch("PostAdd", MethodType.Normal)]
    public class PostFix_TransportPods_PostAdd
    {
        [HarmonyPostfix]
        public static void PostFix(TravelingTransportPods __instance)
        {
            TransportPodsArrivalAction arrivalAction = __instance.arrivalAction;
            if (arrivalAction is TransportPodsArrivalAction_GiveGift arrivalActionGG)
            {
                DesynchronizedMain.ArrivalActionAndSenderLinker.EstablishRelationship(arrivalActionGG, __instance.Tile);
            }
        }
    }
}
