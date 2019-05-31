using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace Desynchronized.Compatibility
{
    /*
    [HarmonyPatch(typeof(CleanserUtil))]
    [HarmonyPatch("InitUsedTalePawns", MethodType.Normal)]
    public class PostFix_RuntimeGC_CleanserUtils_TaleNewsPawns
    {
        [HarmonyPrepare]
        private static bool DetectRuntimeGC()
        {
            return LoadedModManager.RunningMods.Any((ModContentPack m) => m.Name.Contains("RuntimeGC"));
        }


    }
    */

        /*
    [HarmonyPatch]
    public class TestPatcher_RuntimeGC
    {
        public static MethodBase TargetMethod()
        {
            DesynchronizedMain.LogError("Attempt to target internal class.");
            return AccessTools.Method("RuntimeGC.CleanserUtil:InitUsedTalePawns");
        }
    }
    */
}
