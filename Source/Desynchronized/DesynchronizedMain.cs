using Harmony;
using System.Reflection;
using Verse;

namespace Desynchronized
{
    public class DesynchronizedMain : Mod
    {
        public static string MODID
        {
            get
            {
                return "com.vectorial1024.rimworld.desynchronized";
            }
        }

        public DesynchronizedMain(ModContentPack content) : base(content)
        {
            HarmonyInstance harmony = HarmonyInstance.Create(MODID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
