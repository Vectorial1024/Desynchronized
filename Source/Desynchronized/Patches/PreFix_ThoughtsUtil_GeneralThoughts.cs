using Desynchronized.Handlers;
using Desynchronized.TaleLibrary;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility))]
    [HarmonyPatch("TryGiveThoughts", MethodType.Normal)]
    [HarmonyPatch(new Type[] { typeof(Pawn), typeof(DamageInfo), typeof(PawnDiedOrDownedThoughtsKind) })]
    public class PreFix_ThoughtsUtil_GeneralThoughts
    {
        [HarmonyPrefix]
        public static bool PreFix(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
        {
            try
            {
                if (!PawnGenerator.IsBeingGenerated(victim) && Current.ProgramState == ProgramState.Playing)
                {
                    // This is the main redirection for our patches
                    switch (thoughtsKind)
                    {
                        case PawnDiedOrDownedThoughtsKind.Downed:
                            // There is potential here, simply wasted.
                            return false;
                        case PawnDiedOrDownedThoughtsKind.Died:
                            // Let's see if this works; I hope it does.
                            // It works.
                            Handler_PawnDied.HandlePawnDied(victim, dinfo);
                            return false;
                        case PawnDiedOrDownedThoughtsKind.BanishedToDie:
                            Handler_PawnBanished.HandlePawnBanished(victim, true);
                            return false;
                        case PawnDiedOrDownedThoughtsKind.Banished:
                            Handler_PawnBanished.HandlePawnBanished(victim, false);
                            return false;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }
            catch (Exception arg)
            {
                Log.Error("[V1024-DESYNC] Could not give thought, falling back to vanilla thought-giving procedures: " + arg, false);
            }

            return true;
        }
    }
}
