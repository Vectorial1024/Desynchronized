using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Desynchronized.Redirection
{
    public class KidnappingTaleRedirector
    {
        public static void RedirectTale(Tale tale)
        {
            FileLog.Log("A Tale instance is received: [" + tale);
            if (tale.def == TaleDefOf.KidnappedColonist)
            {
                FileLog.Log("Redirecting.");
                DesynchronizedMain.CentralTaleDatabase.foo(tale, tale.DominantPawn);
                DesynchronizedMain.CentralTaleDatabase.dumpList();
            }
        }
    }
}
