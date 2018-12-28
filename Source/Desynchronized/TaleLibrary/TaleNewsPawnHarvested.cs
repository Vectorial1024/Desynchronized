using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TaleLibrary
{
    public class TaleNewsPawnHarvested: TaleNews
    {
        public Pawn Victim { get; }

        public TaleNewsPawnHarvested(Pawn receipient, Pawn victim): base(receipient)
        {
            Victim = victim;
        }

        protected override void GiveThoughtsToReceipient()
        {
            if (NewsReceipient == Victim)
            {
                NewsReceipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested);
            }
            else
            {
                // Not the same guy
                // Determine the correct thought to be given out
                if (Victim.IsColonist)
                {
                    NewsReceipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowColonistOrganHarvested);
                }
                else if (Victim.HostFaction == Faction.OfPlayer)
                {
                    NewsReceipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowGuestOrganHarvested);
                }
            }
        }
    }
}
