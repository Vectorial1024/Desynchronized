using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    public class TaleNewsPawnExecuted : TaleNews
    {
        public Pawn Victim { get; }
        public Pawn Executioner { get; }
        public DeathBrutality BrutalityDegree { get; }

        /// <summary>
        /// The class of victim is derived from the parameter victim.
        /// </summary>
        /// <param name="receipient"></param>
        /// <param name="victim"></param>
        /// <param name="executioner"></param>
        /// <param name="brutality"></param>
        public TaleNewsPawnExecuted(Pawn receipient, Pawn victim, Pawn executioner, DeathBrutality brutality): base(receipient)
        {
            Victim = victim;
            Executioner = executioner;
            BrutalityDegree = brutality;
        }

        protected override void GiveThoughtsToReceipient()
        {
            if (!NewsReceipient.IsCapableOfThought())
            {
                return;
            }

            int forcedStage = (int) BrutalityDegree;
            ThoughtDef thoughtToGive = Victim.IsColonist ? ThoughtDefOf.KnowColonistExecuted : ThoughtDefOf.KnowGuestExecuted;
            NewsReceipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtToGive, forcedStage), null);
        }
    }
}
