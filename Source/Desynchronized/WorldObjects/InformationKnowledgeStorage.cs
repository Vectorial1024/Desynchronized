using HugsLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.WorldObjects
{
    [Obsolete("Use CentralTaleDatabase instaed.", true)]
    public class InformationKnowledgeStorage: UtilityWorldObject
    {
        private List<Pawn> kidnappedPawns = new List<Pawn>();

        public override void PostAdd()
        {
            base.PostAdd();
            kidnappedPawns = new List<Pawn>();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref kidnappedPawns, "kidnappedPawns");
        }

        public List<Pawn> KidnappedPawns
        {
            get
            {
                return kidnappedPawns;
            }
        }
    }
}
