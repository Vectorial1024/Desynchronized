using Harmony;
using RimWorld;

namespace Desynchronized.Patches.News_Sold
{
    [HarmonyPatch(typeof(GenGuest))]
    [HarmonyPatch("AddPrisonerSoldThoughts", MethodType.Normal)]
    public class PreFix_GenGuest_PrisonerSoldThoughts
    {
        [HarmonyPrefix]
        public static bool PreventVanillaThoughts()
        {
            //DesynchronizedMain.LogError("Mod incompatibility detected. Remove other mods until this error no longer appears, and report this to Desynchronized:\n" + Environment.StackTrace);
            return false;
        }
    }
}
