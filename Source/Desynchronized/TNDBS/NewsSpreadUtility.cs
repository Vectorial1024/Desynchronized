using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Desynchronized.TNDBS
{
    public class NewsSpreadUtility
    {
        public enum SpreadMode
        {
            RANDOM,
            DISTINCT
        }

        public static void SpreadNews(Pawn initiator, Pawn receiver, SpreadMode mode = SpreadMode.RANDOM)
        {
            TaleNewsReference newsToSend = DetermineTaleNewsToTransmit(initiator, receiver, mode);
            AttemptToTransmitNews(initiator, receiver, newsToSend);
        }

        private static TaleNewsReference DetermineTaleNewsToTransmit(Pawn initiator, Pawn receiver, SpreadMode mode)
        {
            TaleNewsReference result;

            switch (mode)
            {
                case SpreadMode.DISTINCT:
                    SelectNewsDistinctly(initiator, receiver, out result);
                    break;
                case SpreadMode.RANDOM:
                    SelectNewsRandomly(initiator, receiver, out result);
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private static void SelectNewsRandomly(Pawn initiator, Pawn receiver, out TaleNewsReference result)
        {
            List<TaleNewsReference> listInitiator = DesynchronizedMain.TaleNewsDatabaseSystem.ListAllKnowledgeOfPawn(initiator);

            if (listInitiator.Count == 0)
            {
                result = TaleNewsReference.NullReference;
            }
            else
            {
                result = listInitiator[(int)((uint)Rand.Int % listInitiator.Count)];
            }
        }

        private static void SelectNewsDistinctly(Pawn initiator, Pawn receiver, out TaleNewsReference result)
        {
            List<TaleNewsReference> listInitiator = DesynchronizedMain.TaleNewsDatabaseSystem.ListAllKnowledgeOfPawn(initiator);
            List<TaleNewsReference> listReceiver = DesynchronizedMain.TaleNewsDatabaseSystem.ListAllKnowledgeOfPawn(receiver);

            // Distinct List
            List<TaleNewsReference> listDistinct = new List<TaleNewsReference>();

            // Find out the contents of the distinct list
            foreach (TaleNewsReference reference in listInitiator)
            {
                if (!listReceiver.Contains(reference))
                {
                    listDistinct.Add(reference);
                }
            }

            // Select one random entry from the distinct list
            if (listDistinct.Count == 0)
            {
                result = TaleNewsReference.NullReference;
            }
            else
            {
                result = listDistinct[(int)((uint)Rand.Int % listDistinct.Count)];
            }
        }

        private static void AttemptToTransmitNews(Pawn initiator, Pawn receiver, TaleNewsReference news)
        {
            // DesynchronizedMain.LogError("Attempting to transmit " + news.ToString());

            if (news == TaleNewsReference.NullReference)
            {
                // DesynchronizedMain.LogError("It was a null news. Nothing was done.");
                return;
            }

            /*
             * We will lazily handle the KnowledgeCards:
             * whenever an attempt was made to transmit news,
             * the KnowledgeCards of both the initiator and the receiver is validated
             * before we do any processing.
             */

            // DesynchronizedMain.TaleNewsDatabaseSystem.GetOrInitializePawnKnowledgeCard(initiator);
            // DesynchronizedMain.TaleNewsDatabaseSystem.GetOrInitializePawnKnowledgeCard(receiver);

            foreach (TaleNewsReference reference in DesynchronizedMain.TaleNewsDatabaseSystem.ListAllKnowledgeOfPawn(receiver))
            {
                if (TaleNewsReference.BothRefsAreEqual(reference, news))
                {
                    reference.ActivateNews();
                    return;
                }
            }

            DesynchronizedMain.TaleNewsDatabaseSystem.LinkNewsReferenceToPawn(news, receiver);
            news.ActivateNews();
        }
    }
}
