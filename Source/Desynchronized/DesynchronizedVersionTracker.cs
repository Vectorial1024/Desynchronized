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
        // We will do "previous version" and "current version"

        private string versionOfMod;

        public Version VersionOfModWithinSave
        {
            get
            {
                if (versionOfMod == null)
                {
                    return new Version(0, 0, 0, 0);
                }
                else
                {
                    return new Version(versionOfMod);
                }
            }
        }

        public static Version CurrentVersion => typeof(DesynchronizedMain).Assembly.GetName().Version;

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
            // Actually IO-ing
            base.ExposeData();
            Scribe_Values.Look(ref versionOfMod, "versionOfMod");
            // DesynchronizedMain.LogError("It is now " + Scribe.mode);
            // DesynchronizedMain.LogError("Saved with version (string) " + versionOfMod);

            // The actual processing
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                // For some reason this value did not get included in the save-file.
                // Just making sure the string is stored properly, so it could be saved properly too.
                if (versionOfMod == null)
                {
                    versionOfMod = typeof(DesynchronizedMain).Assembly.GetName().Version.ToString();
                }
            }
            // DesynchronizedMain.LogError("Ending phase; string is " + VersionOfMod);
        }
    }
}
