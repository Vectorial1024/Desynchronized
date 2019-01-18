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
            Version before = new Version(versionOfMod);
            Version after = typeof(DesynchronizedMain).Assembly.GetName().Version;
            if (before < after)
            {
                // Condition is true only when an older version is loaded;
                // Latest version and no version should just have the default (latest) version
                // Check the version, and apply updates accordingly.
            }
        }
    }
}
