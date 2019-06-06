using Desynchronized.TNDBS;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Compatibility.RuntimeGC
{
    public class TalePawnListManipulator
    {
        public static void ManipulateListOfPawnsUsedByTales(List<Pawn> original)
        {
            // Note: currently, the list contains only the pawns that aer married, which is no where near optimal.

            List<Pawn> excludedByTaleNews = new List<Pawn>();
            foreach (Pawn pawn in Find.WorldPawns.AllPawnsAliveOrDead)
            {
                foreach (TaleNews news in DesynchronizedMain.TaleNewsDatabaseSystem.TalesOfImportance_ReadOnly)
                {
                    if (news.PawnIsInvolved(pawn) && !excludedByTaleNews.Contains(pawn))
                    {
                        excludedByTaleNews.Add(pawn);
                    }
                }
            }

            original.AddRange(excludedByTaleNews);

            // DesynchronizedMain.LogError("Method called! Processing complete! Original has " + original.Count + " pawns.");
        }

        private static void ReflectiveTestMethod(out List<Pawn> reflection)
        {
            //Patch:TaleType.PermanentHistorical
            List<Tale> usedTales = Find.TaleManager.AllTalesListForReading.FindAll(t => t.Uses > 0 || t.def.type == TaleType.PermanentHistorical);
            List<Pawn> pawnlist = new List<Pawn>();
            int count = usedTales.Count;
            int i = 0;
            foreach (Pawn pawn in Find.WorldPawns.AllPawnsAliveOrDead)
            {
                try
                {
                    for (i = 0; i < count; i++)
                        if (usedTales[i].Concerns(pawn))
                        {
                            pawnlist.Add(pawn);
                            break;
                        }
                }
                catch (System.Exception e)
                {
                    Verse.Log.Error("Exception in InitUsedTalePawns with Tale id=" + usedTales[i].id + " :\n" + e.ToString());
                    pawnlist.Add(pawn);
                }
            }
            reflection = pawnlist;
        }
    }
}
