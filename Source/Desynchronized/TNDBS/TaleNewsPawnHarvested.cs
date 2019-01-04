using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class TaleNewsPawnHarvested: TaleNewsNegativeIndividual
    {
        public TaleNewsPawnHarvested(Pawn victim): base(victim, InstigatorInfo.NoInstigator)
        {

        }

        protected override void GiveThoughtsToReceipient(Pawn recipient)
        {
            if (recipient == PrimaryVictim)
            {
                Log.Error("Report: receipient is gaining MyOrganHarvested.");
                recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested);
            }
            else
            {
                // Not the same guy
                // Determine the correct thought to be given out
                if (PrimaryVictim.IsColonist)
                {
                    recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowColonistOrganHarvested);
                }
                else if (PrimaryVictim.HostFaction == Faction.OfPlayer)
                {
                    recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowGuestOrganHarvested);
                }
            }
        }
    }
}
