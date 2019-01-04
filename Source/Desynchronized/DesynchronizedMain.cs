using Desynchronized.TaleLibrary;
using Desynchronized.TNDBS;
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

        /// <summary>
        /// A very convenient property for myself to limit features to DevMode only.
        /// </summary>
        public static bool WeAreInDevMode => Prefs.DevMode;

        public override string ModIdentifier
        {
            get
            {
                return MODID;
            }
        }

        private SettingHandle<bool> toggle;

        // public static InformationKnowledgeStorage InfoKnowStorage { get; private set; }
        public static TaleNewsDatabase TaleNewsDatabaseSystem { get; private set; }

        public override void WorldLoaded()
        {
            // InfoKnowStorage = UtilityWorldObjectManager.GetUtilityWorldObject<InformationKnowledgeStorage>();
            // CentralTaleDatabase = UtilityWorldObjectManager.GetUtilityWorldObject<CentralTaleDatabase>();
            TaleNewsDatabaseSystem = UtilityWorldObjectManager.GetUtilityWorldObject<TaleNewsDatabase>();
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
