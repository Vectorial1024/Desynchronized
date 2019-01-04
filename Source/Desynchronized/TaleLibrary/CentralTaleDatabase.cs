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
    [Obsolete("", true)]
    public class CentralTaleDatabase: UtilityWorldObject
    {
        // private List<TaleReference> tableReferences;
        // private List<Pawn> tablePawns;
        // private List<TaleNewsPawnPair> listOfReceivedTales;
        // private List<TaleNewsPawnPair> listUnreadTales;
        // private List<TaleNewsPawnPair> listReadedTales;

        private OutstandingNewsObject outstandingNewsObject;
        public OutstandingNewsObject OutstandingNews
        {
            get
            {
                return outstandingNewsObject;
            }
        }
        private ReceivedNewsObject receivedNewsObject;
        public ReceivedNewsObject ReceivedNews
        {
            get
            {
                return receivedNewsObject;
            }
        }

        // private List<TaleNewsPawnPair> listOfReadNews = new List<TaleNewsPawnPair>();

        /// <summary>
        /// Use this in place of the Constructor.
        /// </summary>
        public override void PostAdd()
        {
            base.PostAdd();
            // listOfReadNews = new List<TaleNewsPawnPair>();
            outstandingNewsObject = new OutstandingNewsObject();
            receivedNewsObject = new ReceivedNewsObject();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            // Scribe_Values.Look(ref listOfReadNews, "listOfReadNews");
            Scribe_Values.Look(ref outstandingNewsObject, "outstandingNewsObject");
            Scribe_Values.Look(ref receivedNewsObject, "receivedNewsObject");
        }

        [Obsolete]
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

        [Obsolete]
        public void foo(Tale tale, Pawn receiver)
        {
            foo(tale, new List<Pawn>() { receiver });
        }

        [Obsolete]
        public void foo(Tale tale, List<Pawn> receivers)
        {
            foreach (Pawn receiver in receivers)
            {
                // listOfReadNews.Add(new TaleNewsPawnPair(new TaleNews(tale), receiver));
            }
        }

        [Obsolete]
        public void dumpList()
        {
            /*
            foreach (TaleNewsPawnPair valuePair in listOfReadNews)
            {
                FileLog.Log(valuePair.ToString());
            }
            */
        }

        [Obsolete]
        public void UnpackAndGiveThoughts(TaleNewsPawnPair info)
        {
            if (info == null)
            {
                return;
            }
        }
    }
}
