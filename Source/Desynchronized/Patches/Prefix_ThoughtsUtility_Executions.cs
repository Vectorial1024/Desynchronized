using Desynchronized.Handlers;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.Patches
{
    [HarmonyPatch(typeof(ThoughtUtility))]
    [HarmonyPatch("GiveThoughtsForPawnExecuted", MethodType.Normal)]
    public class Prefix_ThoughtsUtility_Executions
    {
        [HarmonyPrefix]
        public static bool Prefix(Pawn victim, PawnExecutionKind kind)
        {
            if (victim.RaceProps.Humanlike)
            {
                DeathBrutality brutality = DeathBrutality.HUMANE;
                if (!victim.guilt.IsGuilty)
                {
                    switch (kind)
                    {
                        case PawnExecutionKind.GenericHumane:
                            brutality = DeathBrutality.HUMANE;
                            break;
                        case PawnExecutionKind.GenericBrutal:
                            brutality = DeathBrutality.BRUTAL;
                            break;
                        case PawnExecutionKind.OrganHarvesting:
                            brutality = DeathBrutality.UNETHICAL;
                            break;
                    }
                }
                else
                {
                    brutality = DeathBrutality.JUSTIFIED;
                }
                Handler_PawnExecuted.HandlePawnExecuted(victim, brutality);
            }

            // It's not like we have anything more to process or anything, baka.
            return false;
        }
    }
}
