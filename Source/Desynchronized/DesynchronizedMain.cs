using Desynchronized.Handlers;
using Desynchronized.TNDBS;
using Desynchronized.Utilities;
using HugsLib;
using HugsLib.Settings;
using HugsLib.Utils;
using System;
using Verse;

namespace Desynchronized
{
    public class DesynchronizedMain : ModBase
    {
        public static string MODID => "com.vectorial1024.rimworld.desynchronized";

        /// <summary>
        /// Already includes a space character.
        /// </summary>
        public static string MODPREFIX => "[V1024-DESYNC] ";

        /// <summary>
        /// A very convenient property for myself to limit features to DevMode only.
        /// </summary>
        public static bool WeAreInDevMode => Prefs.DevMode;

        public override string ModIdentifier => MODID;

        private SettingHandle<bool> toggle;

        public static readonly string Desync_PawnIsReferencedString = "Desync_ReferencedByTaleNews";

        // public static InformationKnowledgeStorage InfoKnowStorage { get; private set; }
        public static TaleNewsDatabase TaleNewsDatabaseSystem { get; private set; }
        public static Linker_ArrivalActionAndSender ArrivalActionAndSenderLinker { get; private set; }
        public static DesynchronizedVersionTracker DesynchronizedVersionTracker { get; private set; }
        public static SettingHandle<bool> SettingHandle_NewsSpread { get; private set; }
        public static bool NewsSpreadIsActive => SettingHandle_NewsSpread;
        [Obsolete("Not implemented yet.")]
        public static SettingHandle<bool> SettingHandle_LocalizedNews { get; private set; }
        [Obsolete("For testing only.")]
        // public static SettingHandle<EnumTest> SettingHandle_EnumTest { get; private set; }
        public static bool LocalizedNews => SettingHandle_LocalizedNews;
        public static HallOfFigures TheHallOfFigures { get; private set; }
        public static SettingHandle<bool> SettingHandle_AutoPauseNewsInterface { get; private set; }
        public static bool NewsUI_ShouldAutoPause => SettingHandle_AutoPauseNewsInterface.Value;

        public override void WorldLoaded()
        {
            // InfoKnowStorage = UtilityWorldObjectManager.GetUtilityWorldObject<InformationKnowledgeStorage>();
            // CentralTaleDatabase = UtilityWorldObjectManager.GetUtilityWorldObject<CentralTaleDatabase>();
            TaleNewsDatabaseSystem = UtilityWorldObjectManager.GetUtilityWorldObject<TaleNewsDatabase>();
            ArrivalActionAndSenderLinker = UtilityWorldObjectManager.GetUtilityWorldObject<Linker_ArrivalActionAndSender>();
            DesynchronizedVersionTracker = UtilityWorldObjectManager.GetUtilityWorldObject<DesynchronizedVersionTracker>();
            TheHallOfFigures = UtilityWorldObjectManager.GetUtilityWorldObject<HallOfFigures>();
            // ASDF obj = UtilityWorldObjectManager.GetUtilityWorldObject<ASDF>();

            if (DesynchronizedVersionTracker.VersionOfModWithinSave < new Version(1, 4, 5, 0))
            {
                TaleNewsDatabaseSystem.SelfPatching_NullVictims();
            }
            TaleNewsDatabaseSystem.SelfVerify();
        }

        public override void DefsLoaded()
        {
            PrepareModSettingHandles();
        }

        private void PrepareModSettingHandles()
        {
            /*
            toggle = Settings.GetHandle("settingName", "toggleSetting_title", "This is just a test.", false);
            // If enabled, news of various events will only be known inside the local map.\n\nEnabled by default.
            SettingHandle_LocalizedNews = Settings.GetHandle("toggleLocalizedNews", "toggleLocalizedNews_title", "This is also a test. Do not touch. Default value is true.", true);
            // Is auto-disabled when \"Localized News\" is disabled.
            SettingHandle_NewsSpread = Settings.GetHandle("toggleNewsSpreading", "News Spreading", "If enabled, news of various events will follow Colonists around and spread to other places.\n\nEnabled by default.\n\n", true);
            */
            SettingHandle_AutoPauseNewsInterface = Settings.GetHandle("toggleAutoPauseNewsInterface", "NewsUIAutoPause_title".Translate(), "NewsUIAutoPause_descr".Translate(), true);

            // SettingHandle_EnumTest = Settings.GetHandle("enumTest", "Enum Test Title", "Enum Test Desc", EnumTest.EnumDefault, null, "enumTest_");
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

        public static void LogWarning(string message, bool ignoreLogLimit = false)
        {
            Log.Warning(MODPREFIX + " " + message, ignoreLogLimit);
        }
    }
}
