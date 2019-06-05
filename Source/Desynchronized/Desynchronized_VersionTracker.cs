using HugsLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized
{
    public class Desynchronized_VersionTracker: UtilityWorldObject
    {
        private string versionOfMod;

        public string VersionOfMod
        {
            get
            {
                return versionOfMod;
            }
        }

        public override void PostAdd()
        {
            base.PostAdd();
            versionOfMod = typeof(DesynchronizedMain).Assembly.GetName().Version.ToString();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref versionOfMod, "versionOfMod");
            Version versionWithinSaveFile = new Version(versionOfMod);
            // DesynchronizedMain.LogError("It is now " + Scribe.mode);
            // DesynchronizedMain.LogError("Saved with version (string) " + versionOfMod);

            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                if (versionOfMod == null)
                {
                    versionOfMod = typeof(DesynchronizedMain).Assembly.GetName().Version.ToString();
                }
            }
            // DesynchronizedMain.LogError("Check again, saved with version (string) " + versionOfMod);
            // Sanity check; only do this after the vars are loaded.
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                /*
                if (versionWithinSaveFile <= new Version(1, 4, 0, 0))
                {
                    // Fixing the bug: Forgetting to initialize the Pawn Knowledge List
                    DesynchronizedMain.TaleNewsDatabaseSystem.PopulatePawnKnowledgeMapping();
                }
                */
                if (versionWithinSaveFile < new Version(1, 4, 5, 0))
                {
                    DesynchronizedMain.TaleNewsDatabaseSystem.SelfPatching_NullVictims();
                }
            }
        }

        /// <summary>
        /// This is the self-patcher method targetting the 
        /// </summary>
        internal void SelfPatcher_NullVictim()
        {

        }
    }
}
