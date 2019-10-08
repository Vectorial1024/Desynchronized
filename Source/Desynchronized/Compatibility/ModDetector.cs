using System.Linq;
using Verse;

namespace Desynchronized.Compatibility
{
    public class ModDetector
    {
        public static bool PsychologyIsLoaded => LoadedModManager.RunningMods.Any((ModContentPack pack) => pack.Name.Contains("Psychology"));
        public static bool QuestionableEthicsIsLoaded => LoadedModManager.RunningMods.Any((ModContentPack pack) => pack.Name.Contains("Questionable Ethics"));
    }
}
