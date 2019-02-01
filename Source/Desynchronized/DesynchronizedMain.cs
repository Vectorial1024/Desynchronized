using Desynchronized.TaleLibrary;
using Desynchronized.TestingGrounds;
using Desynchronized.TNDBS;
using Desynchronized.WorldObjects;
using Harmony;
using HugsLib;
using HugsLib.Settings;
using HugsLib.Utils;
using System;
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
        /// Already includes a space character.
        /// </summary>
        public static string MODPREFIX
        {
            get
            {
                return "[V1024-DESYNC] ";
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
        public static ArrivalAction_Sender_Linker ArrivalActionAndSenderLinker { get; private set; }
        public static DesynchronizedVersionTracker DesynchronizedVersionTracker { get; private set; }
        public static SettingHandle<bool> SettingHandle_NewsSpread { get; private set; }
        public static bool NewsSpreadIsActive => SettingHandle_NewsSpread;
        [Obsolete("Not implemented yet.")]
        public static SettingHandle<bool> SettingHandle_LocalizedNews { get; private set; }
        public static bool LocalizedNews => SettingHandle_LocalizedNews;

        public override void WorldLoaded()
        {
            // InfoKnowStorage = UtilityWorldObjectManager.GetUtilityWorldObject<InformationKnowledgeStorage>();
            // CentralTaleDatabase = UtilityWorldObjectManager.GetUtilityWorldObject<CentralTaleDatabase>();
            TaleNewsDatabaseSystem = UtilityWorldObjectManager.GetUtilityWorldObject<TaleNewsDatabase>();
            ArrivalActionAndSenderLinker = UtilityWorldObjectManager.GetUtilityWorldObject<ArrivalAction_Sender_Linker>();
            DesynchronizedVersionTracker = UtilityWorldObjectManager.GetUtilityWorldObject<DesynchronizedVersionTracker>();
            // ASDF obj = UtilityWorldObjectManager.GetUtilityWorldObject<ASDF>();
        }

        public override void DefsLoaded()
        {
            PrepareModSettingHandles();
        }

        private void PrepareModSettingHandles()
        {
            toggle = Settings.GetHandle("settingName", "toggleSetting_title", "This is just a test.", false);
            // If enabled, news of various events will only be known inside the local map.\n\nEnabled by default.
            SettingHandle_LocalizedNews = Settings.GetHandle("toggleLocalizedNews", "toggleLocalizedNews_title", "This is also a test. Do not touch. Default value is true.", true);
            // Is auto-disabled when \"Localized News\" is disabled.
            SettingHandle_NewsSpread = Settings.GetHandle("toggleNewsSpreading", "News Spreading", "If enabled, news of various events will follow Colonists around and spread to other places.\n\nEnabled by default.\n\n", true);
        }

        public override void SettingsChanged()
        {
            // Check settings
            /*
            if (!LocalizedNews)
            {
                SettingHandle_NewsSpread.Value = false;
            }
            */
        }

        public static void LogError(string message, bool ignoreLogLimit = false)
        {
            Log.Error(MODPREFIX + " " + message, ignoreLogLimit);
        }
    }
}
