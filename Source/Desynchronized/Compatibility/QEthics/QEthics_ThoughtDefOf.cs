using RimWorld;
using Verse;

namespace Desynchronized.Compatibility.QEthics
{
    public class QEthics_ThoughtDefOf
    {
        // Using DefDatabase instead of the DefOf annotation to allow for silent errors.
        public static ThoughtDef QE_RecentlyNerveStapled = DefDatabase<ThoughtDef>.GetNamed("QE_RecentlyNerveStapled");
        public static ThoughtDef QE_NerveStapledMe = DefDatabase<ThoughtDef>.GetNamed("QE_NerveStapledMe");
        public static ThoughtDef QE_NerveStapledColonist = DefDatabase<ThoughtDef>.GetNamed("QE_NerveStapledColonist");
        public static ThoughtDef QE_NerveStapledPawn = DefDatabase<ThoughtDef>.GetNamed("QE_NerveStapledPawn");
    }
}
