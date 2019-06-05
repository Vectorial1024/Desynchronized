using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(GenGuest))]
    [HarmonyPatch("AddPrisonerSoldThoughts", MethodType.Normal)]
    public class PreFix_GenGuest_PrisonerSoldThoughts
    {
        [HarmonyPrefix]
        public static bool PreFix()
        {
            //DesynchronizedMain.LogError("Mod incompatibility detected. Remove other mods until this error no longer appears, and report this to Desynchronized:\n" + Environment.StackTrace);
            return false;
        }
    }
}
