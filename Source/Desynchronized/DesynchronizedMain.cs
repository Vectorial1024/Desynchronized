using Harmony;
using HugsLib;
using HugsLib.Settings;
using System.Reflection;
using Verse;

namespace Desynchronized
{
    public class DesynchronizedMain : ModBase
    {
        public static string MODID
        {
            get
            {
                return "com.vectorial1024.rimworld.desynchronized";
            }
        }

        public override string ModIdentifier
        {
            get
            {
                return MODID;
            }
        }

        private SettingHandle<bool> toggle;

        public override void DefsLoaded()
        {
            PrepareModSettingHandles();
        }

        private void PrepareModSettingHandles()
        {
            toggle = Settings.GetHandle<bool>("settingName", "toggleSetting_title", "This is just a test.", false);
        }
    }
}
