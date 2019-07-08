﻿using Desynchronized.Handlers;
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

        public static readonly string Desync_PawnIsReferencedString = "Desync_ReferencedByTaleNews";

        // public static InformationKnowledgeStorage InfoKnowStorage { get; private set; }
        public static TaleNewsDatabase TaleNewsDatabaseSystem { get; private set; }
        public static Linker_ArrivalActionAndSender ArrivalActionAndSenderLinker { get; private set; }
        public static DesynchronizedVersionTracker DesynchronizedVersionTracker { get; private set; }
        public static HallOfFigures TheHallOfFigures { get; private set; }
        public static SettingHandle<bool> SettingHandle_AutoPauseNewsInterface { get; private set; }
        public static bool NewsUI_ShouldAutoPause => SettingHandle_AutoPauseNewsInterface.Value;

        public override void WorldLoaded()
        {
            TaleNewsDatabaseSystem = UtilityWorldObjectManager.GetUtilityWorldObject<TaleNewsDatabase>();
            ArrivalActionAndSenderLinker = UtilityWorldObjectManager.GetUtilityWorldObject<Linker_ArrivalActionAndSender>();
            DesynchronizedVersionTracker = UtilityWorldObjectManager.GetUtilityWorldObject<DesynchronizedVersionTracker>();
            TheHallOfFigures = UtilityWorldObjectManager.GetUtilityWorldObject<HallOfFigures>();

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
            SettingHandle_AutoPauseNewsInterface = Settings.GetHandle("toggleAutoPauseNewsInterface", "NewsUIAutoPause_title".Translate(), "NewsUIAutoPause_descr".Translate(), true);
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
