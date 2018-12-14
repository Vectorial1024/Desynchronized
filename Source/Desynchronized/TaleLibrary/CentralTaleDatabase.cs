using Harmony;
using HugsLib.Utils;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    public class CentralTaleDatabase: UtilityWorldObject
    {
        private List<TaleReference> tableReferences;
        private List<Pawn> tablePawns;
        private List<TaleNewsPawnPair> listOfReceivedTales;
        private List<TaleNewsPawnPair> listUnreadTales;
        private List<TaleNewsPawnPair> listReadedTales;

        private List<TaleNewsPawnPair> listOfReadNews = new List<TaleNewsPawnPair>();

        public override void PostAdd()
        {
            base.PostAdd();
            listOfReadNews = new List<TaleNewsPawnPair>();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref listOfReadNews, "listOfReadNews");
        }

        public static IEnumerable<Pawn> GetPawnList()
        {
            foreach (Pawn pawn in Find.World.PlayerPawnsForStoryteller)
            {
                yield return pawn;
            }
            foreach (Pawn pawn in Find.World.worldPawns.AllPawnsAlive)
            {
                if (pawn.Faction == Find.FactionManager.OfPlayer)
                {
                    yield return pawn;
                }
            }
        }

        public void foo(Tale tale, Pawn receiver)
        {
            foo(tale, new List<Pawn>() { receiver });
        }

        public void foo(Tale tale, List<Pawn> receivers)
        {
            foreach (Pawn receiver in receivers)
            {
                listOfReadNews.Add(new TaleNewsPawnPair(new TaleNews(tale), receiver));
            }
        }

        public void dumpList()
        {
            foreach (TaleNewsPawnPair valuePair in listOfReadNews)
            {
                FileLog.Log(valuePair.ToString());
            }
        }
    }
}
