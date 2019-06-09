using Desynchronized.Interfaces;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    /*
    [HarmonyPatch(typeof(Window))]
    [HarmonyPatch("PostOpen", MethodType.Normal)]
    public class PostFix_Window_PostOpen
    {
        [HarmonyPostfix]
        public static void DetectAndCloseDesyncWindows(Window __instance)
        {
            if (__instance is MainTabWindow_Menu)
            {
                foreach (Window window in Find.WindowStack.Windows)
                {
                    if (window is Dialog_NewsTrackerViewer)
                    {
                        window.Close();
                    }
                }
            }
        }
    }
    */
}
