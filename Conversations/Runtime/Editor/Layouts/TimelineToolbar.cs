using System;
using UnityEngine;

namespace Ayla.Conversations
{
    internal class TimelineToolbar : IDisposable
    {
        private readonly TimelineBreadcrumb m_Breadcrumb;

        public TimelineToolbar(ConversationStates states)
        {
            m_Breadcrumb = new TimelineBreadcrumb(states);
        }

        public void Dispose()
        {
            m_Breadcrumb.Dispose();
            GC.SuppressFinalize(this);
        }

        public Vector2 Draw(Rect position)
        {
            return m_Breadcrumb.Draw(position);
        }
    }
}