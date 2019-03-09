using HugsLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS.Storage
{
    public class ITNRA_Database: UtilityWorldObject
    {
        Dictionary<Pawn, Pawn_TaleNewsRefArchive> mappingPawnAndTNRA;

        public override void PostAdd()
        {
            base.PostAdd();
            mappingPawnAndTNRA = new Dictionary<Pawn, Pawn_TaleNewsRefArchive>();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref mappingPawnAndTNRA, "mappingPawnAndTNRA", LookMode.Reference, LookMode.Deep);
            // asdf
        }

        /// <summary>
        /// Returns the Tale News Reference Archive of the given pawn.
        /// <para/>
        /// The Database makes sure that the pawn has its entry inside, and will never return null.
        /// </summary>
        /// <returns></returns>
        public Pawn_TaleNewsRefArchive GetArchiveForPawn(Pawn target)
        {
            if (mappingPawnAndTNRA == null)
            {
                throw new InvalidOperationException("The Database is probably not ready yet. Try again later.");
            }
            else
            {
                if (mappingPawnAndTNRA.ContainsKey(target))
                {
                    return mappingPawnAndTNRA[target];
                }
                else
                {
                    Pawn_TaleNewsRefArchive value = new Pawn_TaleNewsRefArchive();
                    mappingPawnAndTNRA[target] = value;
                    return value;
                }
            }
        }
    }
}
