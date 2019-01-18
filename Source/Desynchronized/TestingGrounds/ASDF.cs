using HugsLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TestingGrounds
{
    /*
    public class ASDF : UtilityWorldObject
    {
        List<int> list1;
        List<bool> list2;
        List<long> list3;
        List<QWER> list4;
        List<ZXCV> list5;

        public override void PostAdd()
        {
            base.PostAdd();
            list3 = new List<long>();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref list1, "list1");
            Scribe_Values.Look(ref list2, "list2", new List<bool>());
            Scribe_Values.Look(ref list3, "list3", new List<long>());
            Scribe_Values.Look(ref list4, "list4", new List<QWER>());
            Scribe_Values.Look(ref list5, "list5", new List<ZXCV>());
            PrintStuff();
        }

        private void PrintStuff()
        {
            DesynchronizedMain.LogError("list 1: " + list1 + "; is it null? " + (list1 == null));
            DesynchronizedMain.LogError("list 2: " + list2 + "; is it null? " + (list2 == null));
            DesynchronizedMain.LogError("list 3: " + list3 + "; is it null? " + (list3 == null));
            DesynchronizedMain.LogError("list 4: " + list4 + "; is it null? " + (list4 == null));
            DesynchronizedMain.LogError("list 5: " + list5 + "; is it null? " + (list5 == null));
        }
    }
    */
}
