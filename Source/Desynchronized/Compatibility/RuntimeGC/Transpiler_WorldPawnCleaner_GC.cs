using Harmony;
using RuntimeGC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace Desynchronized.Compatibility.RuntimeGC
{
    /*
     * Latest deveopment: changing the method signature caused this exception: ArgumentException
     * I intend to target the method using targetmethod
     */

    /*
    [HarmonyPatch(typeof(WorldPawnCleaner))]
    [HarmonyPatch("GC", MethodType.Normal)]
    */
    [HarmonyPatch]
    public class Transpiler_WorldPawnCleaner_GC
    {
        private static bool PresenceOfRuntimeGC => LoadedModManager.RunningMods.Any((ModContentPack pack) => pack.Name.Contains("RuntimeGC"));

        public static MethodBase TargetMethod()
        {
            //DesynchronizedMain.LogError("Targetting. RuntimeGC engagement status: " + PresenceOfRuntimeGC);
            return AccessTools.Method("RuntimeGC.WorldPawnCleaner:GC"); ;
        }

        /*
        [HarmonyTargetMethod]
        public static MethodBase TargetRuntimeGC()
        {
            DesynchronizedMain.LogError("Targetting.");
            return AccessTools.Method("RuntimeGC.WorldPawnCleaner:GC");
        }
        */

        /// <summary>
        /// DetectRuntimeGC
        /// </summary>
        /// <returns></returns>
        public static bool Prepare()
        {
            //DesynchronizedMain.LogError("Preparing. RuntimeGC engagement status: " + PresenceOfRuntimeGC);
            return PresenceOfRuntimeGC;
        }

        /*
        [HarmonyPrepare]
        public static bool DetectRuntimeGC(MethodBase original)
        {
            // Supposedly the original method should be loaded.
            DesynchronizedMain.LogError("Is the original method null? " + (original == null));
            return original != null;

            foreach (ModContentPack pack in LoadedModManager.RunningMods)
            {
                // DesynchronizedMain.LogError("Current ModContentPack: " + pack.Name);
                if (pack.Name.Contains("RuntimeGC"))
                {
                    return true;
                }
            }
            
            return false;
            
            // Need to learn how to make predicates, they seem very useful, especially lambda ones.
            //Predicate<string> isUpper = delegate (string s) { return s.Equals(s.ToUpper()); };
            Func<ModContentPack, bool> funcDelegate = pack => pack.Name.Contains("RuntimeGC");
            //return LoadedModManager.RunningMods.Any((ModContentPack m) => m.Name == "RuntimeGC");
            //return LoadedModManager.RunningMods.Any(predicate: delegate (ModContentPack pack) { return pack.Name.Contains("RuntimeGC"); });
            return LoadedModManager.RunningMods.Any(funcDelegate);
            
        }
        */

        /*
        [HarmonyPostfix]
        public static void TestFunc()
        {

        }
        */
        
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ManipulateTalePawnList(IEnumerable<CodeInstruction> instructions)
        {
            // Find the 2nd "call" after the 6th "ldstr"
            // Insert a method after that position to manipulate the list.
            int occurencesLdstr = 0;
            int occurencesCall = 0;
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);
            int i = 0;

            while (occurencesLdstr < 6)
            {
                if (instructionList[i].opcode == OpCodes.Ldstr)
                {
                    occurencesLdstr++;
                }
                i++;
            }

            while (occurencesCall < 2)
            {
                if (instructionList[i].opcode == OpCodes.Call)
                {
                    occurencesCall++;
                }
                i++;
            }

            // i is at our insert position.
            // Insert new commands.
            List<CodeInstruction> insertionList = new List<CodeInstruction>();
            insertionList.Add(new CodeInstruction(OpCodes.Ldloc_1));
            insertionList.Add(new CodeInstruction(OpCodes.Call, typeof(TalePawnListManipulator).GetMethod("ManipulateListOfPawnsUsedByTales")));
            instructionList.InsertRange(i, insertionList);

            // Insertion complete.
            return instructionList;
        }

        /*
        private void testMethod()
        {
            List<string> lol;
            Foo(out lol);
            Console.WriteLine(lol.Count);
        }

        private void Foo(out List<string> bruh)
        {
            bruh = new List<string>();
        }
        */
    }

    /*
    [HarmonyPatch(typeof(Building))]
    [HarmonyPatch("Draw", MethodType.Normal)]
    public class TestPatcher
    {
        // Using method name Prepare instead of attribute [HarmonyPrepare]
        public static bool Prepare()
        {
            DesynchronizedMain.LogError("Sup.");
            return LoadedModManager.RunningMods.Any();
        }
    }
    */

        /*
    public class Target
    {
        public void Method()
        {

        }
    }

    [HarmonyPatch]
    public class Patcher
    {
        // OK
        public static MethodBase TargetMethod()
        {
            return typeof(Target).GetMethod("Method");
        }

        // Nope

        [HarmonyTargetMethod]
        public static MethodBase TargetTheTarget()
        {
            return typeof(Target).GetMethod("Method");
        }
    }
    */
}
