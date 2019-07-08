﻿using Desynchronized.TNDBS;
using Desynchronized.TNDBS.Extenders;
using Desynchronized.TNDBS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private bool shouldDisplayForgottenNews = true;

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
            preventCameraMotion = false;
            closeOnClickedOutside = true;
            //absorbInputAroundWindow = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            int mainAreaBegin = 0;
            DrawToggleDisplayForgottenNews(ref mainAreaBegin);

            // Draw first row
            Rect headerRect = new Rect(0, mainAreaBegin, EntryWidth, EntryHeight);
            DrawHeaderRow(headerRect);

            Rect mainRect = new Rect(0, mainAreaBegin + EntryHeight, inRect.width, inRect.height - TopAreaHeight - EntryHeight - mainAreaBegin);
            DrawRemainingRows(mainRect);

            GenUI.ResetLabelAlign();
            /*
            DesynchronizedMain.LogError("Rectangle is: " + inRect.ToString());
            throw new NotImplementedException();
            */
        }

        private void DrawToggleDisplayForgottenNews(ref int mainAreaBeginPos)
        {
            if (subjectPawn == null)
            {
                // Button for overall view: filter forgotten news?
                Rect buttonRect = new Rect(0, 0 + 2, 400, EntryHeight - 4);
                // Rect rect4 = new Rect(x, rect.y + 2f, num2, rect.height - 4f);
                if (Widgets.ButtonText(buttonRect, "Toggle display permanently forgotten news"))
                {
                    shouldDisplayForgottenNews = !shouldDisplayForgottenNews;
                }
                mainAreaBeginPos += 30;

                Rect boundingRect = new Rect(400, 0, 400, EntryHeight);
                Rect textRect = boundingRect;
                textRect.xMin += 10;
                textRect.xMax -= 10;
                string readout;
                if (shouldDisplayForgottenNews)
                {
                    readout = "Permanently-forgotten news are not hidden.";
                }
                else
                {
                    readout = "Hiding permanently-forgotten news.";
                }
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(textRect, readout);
            }
        }

        [Obsolete("Consider patronizing NewsSelector.")]
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

            // Step 1: Determine What to draw
            List<TaleNews> taleNewsList;
            if (subjectPawn == null)
            {
                // Viewing all pawns
                if (shouldDisplayForgottenNews)
                {
                    taleNewsList = DesynchronizedMain.TaleNewsDatabaseSystem.ListOfAllTaleNews;
                }
                else
                {
                    taleNewsList = DesynchronizedMain.TaleNewsDatabaseSystem.GetAllNonPermForgottenNews().ToList();
                }
            }
            else
            {
                // Viewing individual pawns
                taleNewsList = new List<TaleNews>(subjectPawn.GetNewsKnowledgeTracker().GetAllNonForgottenNews());
            }
            int newsCount = taleNewsList.Count;

            // Step 2: Setup the area
            float scrollableHeight = HeaderLineHeight + newsCount * EntryHeight;
            Rect viewingRect = new Rect(0, 0, givenArea.width - ScrollerMargin, scrollableHeight);
            Widgets.BeginScrollView(givenArea, ref scrollPosition, viewingRect);
            int currentHeight = HeaderLineHeight;
            float upperPosition = scrollPosition.y - EntryHeight;
            float lowerPosition = scrollPosition.y + givenArea.height;
            // Iterate through
            
            for (int i = 0; i < newsCount; i++)
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

        /// <summary>
        /// Draws the header row, along with its white separation line.
        /// </summary>
        /// <param name="givenArea"></param>
        private void DrawHeaderRow(Rect givenArea)
        {
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.DrawLightHighlight(givenArea);
            GUI.BeginGroup(givenArea);
            DrawNewsID_Header();
            DrawNewsType_Header();
            // reserved 80px for future use
            DrawNewsImportance_Header();
            DrawNewsDetails_Header();
            GUI.EndGroup();

            GUI.color = Color.gray;
            Widgets.DrawLineHorizontal(0, EntryHeight + givenArea.yMin, EntryWidth);
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
            Text.Anchor = TextAnchor.MiddleLeft;
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
                float importance = subjectPawn.GetNewsKnowledgeTracker().AttemptToObtainExistingReference(news).NewsImportance;
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
            string labelString = "";
            string readoutString = "";
            // Check if the stuff is forgotten
            if (subjectPawn == null && news.PermanentlyForgotten)
            {
                // All pawns list, all pawns forgot the news.
                labelString = "Permanently forgotten";
                readoutString = "No one can remember the details of this tale-news anymore.";
            }
            else if (subjectPawn != null && subjectPawn.GetNewsKnowledgeTracker().AttemptToObtainExistingReference(news).NewsIsLocallyForgotten)
            {
                // Individual pawns list, individual pawn forgot the news.
                labelString = "Forgotten";
                readoutString = "This pawn cannot remember the details of this tale-news, but perhaps it will when others talk about it.";
            }
            else
            {
                // In any case, someone remembers; print the details now.
                string originalString;
                try
                {
                    originalString = news.GetDetailsPrintout();
                }
                catch (Exception ex)
                {
                    originalString = DesynchronizedMain.MODPREFIX + "Error: " + ex.Message + "\n" + ex.StackTrace;
                }
                // At this stage, originalString guaranteed to be non-zero.
                string[] splitStrings = originalString.Split('\n');
                labelString = splitStrings[0];
                readoutString = originalString;
            }

            Widgets.Label(textRect, labelString);
            TooltipHandler.TipRegion(boundingRect, readoutString);
        }
    }
}
