using System;
using UnityEditor;
using UnityEngine;

namespace Ayla.Conversations
{
    internal class TimelineView : IDisposable
    {
        private readonly TimelineToolbar m_Toolbar;

        public TimelineView(ConversationStates states)
        {
            m_Toolbar = new TimelineToolbar(states);
        }

        public void Dispose()
        {
            m_Toolbar.Dispose();
            GC.SuppressFinalize(this);
        }

        public Vector2 Draw(Rect position)
        {
            EditorGUI.DrawRect(position, ConversationStyle.colorBackgroundColor);
            m_Toolbar.Draw(position);
            return position.size;
        }
    }
}