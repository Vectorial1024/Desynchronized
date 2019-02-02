using HugsLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized
{
    public class DesynchronizedVersionTracker : UtilityWorldObject
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
            Scribe_Values.Look(ref versionOfMod, "versionOfMod", typeof(DesynchronizedMain).Assembly.GetName().Version.ToString());
            Version versionWithinSaveFile = new Version(versionOfMod);

            // Sanity check; only do this after the vars are loaded.
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (versionWithinSaveFile <= new Version(1, 4, 0, 0))
                {
                    // Fixing the bug: Forgetting to initialize the Pawn Knowledge List
                    DesynchronizedMain.TaleNewsDatabaseSystem.PopulatePawnKnowledgeMapping();
                }
            }
        }
    }
}
