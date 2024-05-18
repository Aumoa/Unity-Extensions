using System;
using Ayla.Inspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Conversations
{
    internal class TimelineBreadcrumb : IDisposable
    {
        private readonly ConversationStates m_States;

        private GUIContent m_TextContent;

        public TimelineBreadcrumb(ConversationStates states)
        {
            m_States = states;
            m_TextContent = new GUIContent();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Vector2 Draw(Rect position)
        {
            var breadcrumbAreaWidth = position.width;
            string[] labels = new[] { "Breadcrumb" };
            Vector2 desiredSize = Vector2.zero;

            void Assign(Rect position)
            {
                desiredSize.x += position.width;
                desiredSize.y = Mathf.Max(position.height, desiredSize.y);
            }

            using (Scopes.HorizontalScope())
            {
                var labelWidth = (int)(breadcrumbAreaWidth / labels.Length);

                for (var i = 0; i < labels.Length; i++)
                {
                    var label = labels[i];

                    var style = i == 0 ? ConversationStyle.s_BreadCrumbLeft : ConversationStyle.s_BreadCrumbMid;
                    var backgroundStyle = i == 0 ? ConversationStyle.s_BreadCrumbLeftBg : ConversationStyle.s_BreadCrumbMidBg;

                    if (i == labels.Length - 1)
                    {
                        if (i > 0) // Only tint last breadcrumb if we are dug-in
                        {
                            Assign(DrawBreadcrumbAsSelectedSubSequence(labelWidth, label, ConversationStyle.s_BreadCrumbMidSelected, ConversationStyle.s_BreadCrumbMidBgSelected));
                        }
                        else
                        {
                            Assign(DrawActiveBreadcrumb(labelWidth, label, style, backgroundStyle));
                        }
                    }
                    else
                    {
                        var previousContentColor = GUI.contentColor;
                        GUI.contentColor = new Color(previousContentColor.r,
                            previousContentColor.g,
                            previousContentColor.b,
                            previousContentColor.a * 0.6f);
                        var content = GetTextContent(labelWidth, label, style);
                        var rect = GetBreadcrumbLayoutRect(content, style);

                        if (Event.current.type == EventType.Repaint)
                        {
                            backgroundStyle.Draw(rect, GUIContent.none, 0);
                        }

                        if (GUI.Button(rect, content, style))
                        {
                            // DO NOT ANYTHING YET.
                            //navigateToBreadcrumbIndex.Invoke(i);
                        }
                        GUI.contentColor = previousContentColor;
                        Assign(rect);
                    }
                }
            }

            return desiredSize;
        }

        private Rect GetBreadcrumbLayoutRect(GUIContent content, GUIStyle style)
        {
            // the image makes the button far too big compared to non-image versions
            var image = content.image;
            content.image = null;
            var size = style.CalcSizeWithConstraints(content, Vector2.zero);
            content.image = image;
            if (image != null)
            {
                size.x += size.y; // assumes square image, constrained by height
            }

            return GUILayoutUtility.GetRect(content, style, GUILayout.MaxWidth(size.x));
        }

        private Rect DrawActiveBreadcrumb(int width, string label, GUIStyle style, GUIStyle backgroundStyle)
        {
            var content = GetTextContent(width, label, style);
            var rect = GetBreadcrumbLayoutRect(content, style);

            if (Event.current.type == EventType.Repaint)
            {
                backgroundStyle.Draw(rect, GUIContent.none, 0);
            }

            if (GUI.Button(rect, content, style))
            {
                Object target = m_States.selectedAsset;
                if (target != null)
                {
                    EditorGUIUtility.PingObject(target);
                }
            }

            return rect;
        }

        private Rect DrawBreadcrumbAsSelectedSubSequence(int width, string label, GUIStyle style, GUIStyle backgroundStyle)
        {
            var rect = DrawActiveBreadcrumb(width, label, style, backgroundStyle);
            const float underlineThickness = 2.0f;
            const float underlineVerticalOffset = 0.0f;
            var underlineHorizontalOffset = backgroundStyle.border.right * 0.333f;
            var underlineRect = Rect.MinMaxRect(
                rect.xMin - underlineHorizontalOffset,
                rect.yMax - underlineThickness - underlineVerticalOffset,
                rect.xMax - underlineHorizontalOffset,
                rect.yMax - underlineVerticalOffset);

            EditorGUI.DrawRect(underlineRect, ConversationStyle.colorSubSequenceDurationLine);
            return rect;
        }

        private static string FitTextInArea(float areaWidth, string text, GUIStyle style)
        {
            var borderWidth = style.border.left + style.border.right;
            var textWidth = style.CalcSize(Internals.TextContent(text)).x;

            if (borderWidth + textWidth < areaWidth)
            {
                return text;
            }

            // Need to truncate the text to fit in the areaWidth
            var textAreaWidth = areaWidth - borderWidth;
            var pixByChar = textWidth / text.Length;
            var charNeeded = (int)Mathf.Floor(textAreaWidth / pixByChar);
            charNeeded -= ConversationStyle.k_Elipsis.Length;

            if (charNeeded <= 0)
            {
                return ConversationStyle.k_Elipsis;
            }

            if (charNeeded <= text.Length)
            {
                return ConversationStyle.k_Elipsis + " " + text.Substring(text.Length - charNeeded);
            }

            return ConversationStyle.k_Elipsis;
        }

        private GUIContent GetTextContent(int width, string text, GUIStyle style)
        {
            m_TextContent.tooltip = string.Empty;
            m_TextContent.image = null;
            m_TextContent.image = ConversationStyle.s_TimelineIcon;

            if (m_TextContent.image != null)
            {
                width = Math.Max(0, width - m_TextContent.image.width);
            }
            m_TextContent.text = FitTextInArea(width, text, style);

            return m_TextContent;
        }
    }
}