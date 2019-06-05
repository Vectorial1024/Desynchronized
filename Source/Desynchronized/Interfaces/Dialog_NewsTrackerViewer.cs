using Desynchronized.TNDBS;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace Desynchronized.Interfaces
{
    public class Dialog_NewsTrackerViewer : Window
    {
        public override Vector2 InitialSize => new Vector2(800, UI.screenHeight - 100);

        public readonly int EntryWidth = 800;

        public readonly int EntryHeight = 30;

        public readonly int TopAreaHeight = 58;

        public readonly int ScrollerMargin = 16;

        public readonly int HeaderLineHeight = 6;

        public Vector2 scrollPosition = Vector2.zero;

        public Pawn subjectPawn;

        public List<TaleNewsReference> knownNews;

        /// <summary>
        /// Instantiates a new Tale-News Knowledge Tracker dialog.
        /// </summary>
        /// <param name="subject">Optional. Causes the dialog to focus on this specific pawn if provided.</param>
        public Dialog_NewsTrackerViewer(Pawn subject = null)
        {
            subjectPawn = subject;
            string pawnName = (subject != null ? subject.Name.ToStringFull : "all pawns");
            optionalTitle = "Viewing news knowledge of: " + pawnName;
            resizeable = false;
            forcePause = DesynchronizedMain.NewsUI_ShouldAutoPause;
            doCloseX = true;
            doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            // Draw first row
            Rect headerRect = new Rect(0, 0, EntryWidth, EntryHeight);
            DrawHeaderRow(headerRect);

            Rect mainRect = new Rect(0, EntryHeight, inRect.width, inRect.height - TopAreaHeight - EntryHeight);
            DrawRemainingRows(mainRect);
            /*
            DesynchronizedMain.LogError("Rectangle is: " + inRect.ToString());
            throw new NotImplementedException();
            */
        }

        private IEnumerable<TaleNews> ObtainTaleNewsForPawn(Pawn subject = null)
        {
            if (subject != null)
            {
                foreach (TaleNewsReference reference in subject.GetNewsKnowledgeTracker().ListOfAllKnownNews)
                {
                    yield return reference.ReferencedTaleNews;
                }
                yield break;
            }
            else
            {
                foreach (TaleNews news in DesynchronizedMain.TaleNewsDatabaseSystem.TalesOfImportance_ReadOnly)
                {
                    yield return news;
                }
                yield break;
            }
        }

        private void DrawRemainingRows(Rect givenArea)
        {
            // Draw remaining rows
            // 6 is for the white line
            float scrollableHeight = HeaderLineHeight + 30 * EntryHeight;
            Rect viewingRect = new Rect(0, 0, givenArea.width - ScrollerMargin, scrollableHeight);
            Widgets.BeginScrollView(givenArea, ref scrollPosition, viewingRect);
            int currentHeight = HeaderLineHeight;
            float upperPosition = scrollPosition.y - EntryHeight;
            float lowerPosition = scrollPosition.y + givenArea.height;
            // Iterate through
            List<TaleNews> taleNewsList = new List<TaleNews>(ObtainTaleNewsForPawn(subjectPawn));
            for (int i = 0; i < taleNewsList.Count; i++)
            {
                // > or >= ?
                if (currentHeight > upperPosition && currentHeight < lowerPosition)
                {
                    Rect rowRect = new Rect(0, currentHeight, viewingRect.width, EntryHeight);
                    DrawRow(rowRect, i, taleNewsList[i]);
                }
                currentHeight += EntryHeight;
            }
            Widgets.EndScrollView();
        }

        private void DrawHeaderRow(Rect givenArea)
        {
            Text.Font = GameFont.Small;
            Widgets.DrawLightHighlight(givenArea);
            GUI.BeginGroup(givenArea);
            DrawNewsID_Header();
            DrawNewsType_Header();
            // reserved 80px for future use
            DrawNewsImportance_Header();
            DrawNewsDetails_Header();
            GUI.EndGroup();

            GUI.color = Color.gray;
            Widgets.DrawLineHorizontal(0, EntryHeight, EntryWidth);
            GUI.color = Color.white;
        }

        private void DrawRow(Rect givenArea, int index, TaleNews news)
        {
            // Determine if highlight is needed
            if (index % 2 == 1)
            {
                Widgets.DrawLightHighlight(givenArea);
            }

            Text.Font = GameFont.Small;
            GUI.BeginGroup(givenArea);
            DrawNewsID(news);
            DrawNewsType(news);
            // reserved 80px for future use
            DrawNewsImportanceSpace(news);
            DrawNewsDetails(news);
            GUI.EndGroup();
        }

        private void DrawNewsID_Header()
        {
            Rect boundingRect = new Rect(0, 0, 80, EntryHeight);
            Rect textRect = boundingRect;
            textRect.xMin += 10;
            textRect.xMax -= 10;
            Widgets.Label(textRect, "ID #");
        }

        private void DrawNewsID(TaleNews news)
        {
            Rect boundingRect = new Rect(0, 0, 80, EntryHeight);
            Widgets.DrawHighlightIfMouseover(boundingRect);
            Rect textRect = boundingRect;
            textRect.xMin += 10;
            textRect.xMax -= 10;
            Widgets.Label(textRect, news.UniqueID.ToString());
            TooltipHandler.TipRegion(boundingRect, "The id of this tale-news.");
        }

        private void DrawNewsType_Header()
        {
            Rect boundingRect = new Rect(80, 0, 240, EntryHeight);
            Rect textRect = boundingRect;
            textRect.xMin += 10;
            textRect.xMax -= 10;
            Widgets.Label(textRect, "Tale-news type");
        }

        private void DrawNewsType(TaleNews news)
        {
            Rect boundingRect = new Rect(80, 0, 240, EntryHeight);
            Widgets.DrawHighlightIfMouseover(boundingRect);
            Rect textRect = boundingRect;
            textRect.xMin += 10;
            textRect.xMax -= 10;
            Widgets.Label(textRect, news.GetNewsTypeName());
            TooltipHandler.TipRegion(boundingRect, "The type of this tale-news. Probably some more description.");
        }

        private void DrawNewsImportance_Header()
        {
            // Placeholder
            // Probably don't need a header, quite self-explanatory.
        }

        private void DrawNewsImportanceSpace(TaleNews news)
        {
            Rect boundingRect = new Rect(320, 0, 80, EntryHeight);
            Widgets.DrawHighlightIfMouseover(boundingRect);
            if (subjectPawn != null)
            {
                // Individual pawn
                Rect textRect = boundingRect;
                textRect.xMin += 10;
                textRect.xMax -= 10;
                float importance = subjectPawn.GetNewsKnowledgeTracker().AttemptToObtainExistingReference(news).CachedNewsImportance;
                Widgets.Label(textRect, Math.Round(importance, 2).ToString());
                StringBuilder builder = new StringBuilder("Importance score of this news: ");
                builder.Append(importance);
                builder.AppendLine();
                builder.Append("News with higher importance score will be more likely to be passed on.");
                TooltipHandler.TipRegion(boundingRect, builder.ToString());
            }
            else
            {
                // All pawns
                TooltipHandler.TipRegion(boundingRect, "(Reserved.)");
            }
            
        }

        private void DrawNewsDetails_Header()
        {
            Rect boundingRect = new Rect(400, 0, 400 - ScrollerMargin, EntryHeight);
            Rect textRect = boundingRect;
            textRect.xMin += 10;
            textRect.xMax -= 10;
            Widgets.Label(textRect, "Tale-news details");
        }

        private void DrawNewsDetails(TaleNews news)
        {
            Rect boundingRect = new Rect(400, 0, 400 - ScrollerMargin, EntryHeight);
            Widgets.DrawHighlightIfMouseover(boundingRect);
            Rect textRect = boundingRect;
            textRect.xMin += 10;
            textRect.xMax -= 10;
            // Only the first row is displayed; others are viewed in the tip region.
            string originalString;
            try
            {
                originalString = news.GetDetailsPrintout();
            }
            catch (Exception ex)
            {
                originalString = DesynchronizedMain.MODPREFIX + "Error: " + ex.Message + "\n" + ex.StackTrace;
            }
            // Guaranteed to have non-zero string.
            string[] splitStrings = originalString.Split('\n');
            Widgets.Label(textRect, splitStrings[0]);
            TooltipHandler.TipRegion(boundingRect, originalString);
        }
    }
}
