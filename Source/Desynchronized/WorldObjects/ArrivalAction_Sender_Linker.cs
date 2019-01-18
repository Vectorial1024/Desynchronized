using HugsLib.Utils;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.WorldObjects
{
    public class ArrivalAction_Sender_Linker: UtilityWorldObject
    {
        private Dictionary<TransportPodsArrivalAction_GiveGift, int> internalMapping;

        public override void PostAdd()
        {
            base.PostAdd();
            internalMapping = new Dictionary<TransportPodsArrivalAction_GiveGift, int>();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref internalMapping, "internalMapping", LookMode.Deep, LookMode.Value);
            // Scribe_Values.Look(ref internalMapping, "internalMapping", new Dictionary<TransportPodsArrivalAction_GiveGift, int>());
        }

        public void EstablishRelationship(TransportPodsArrivalAction_GiveGift actionInstance, int senderTileID)
        {
            if (!internalMapping.ContainsKey(actionInstance))
            {
                internalMapping.Add(actionInstance, senderTileID);
            }
        }

        public Map SafelyGetMapOfGivenAction(TransportPodsArrivalAction_GiveGift actionInstance)
        {
            if (!internalMapping.ContainsKey(actionInstance))
            {
                return null;
            }

            Map map = Current.Game.FindMap(internalMapping[actionInstance]);

            // Clean up the datatype
            internalMapping.Remove(actionInstance);

            return map;
        }
    }
}
