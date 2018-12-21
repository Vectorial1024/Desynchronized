using Desynchronized.TaleLibrary;
using Desynchronized.WorldObjects;
using Harmony;
using HugsLib;
using HugsLib.Settings;
using HugsLib.Utils;
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

        public static bool WeAreInDevMode
        {
            get
            {
                return Prefs.DevMode;
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

        // public static InformationKnowledgeStorage InfoKnowStorage { get; private set; }
        public static CentralTaleDatabase CentralTaleDatabase { get; private set; }

        public override void WorldLoaded()
        {
            // InfoKnowStorage = UtilityWorldObjectManager.GetUtilityWorldObject<InformationKnowledgeStorage>();
            // CentralTaleDatabase = UtilityWorldObjectManager.GetUtilityWorldObject<CentralTaleDatabase>();
        }

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
