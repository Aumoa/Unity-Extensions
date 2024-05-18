using System;
using UnityEngine;

namespace Ayla.Inspector
{
    public static partial class Scopes
    {
        public readonly struct ColorScopeBuilder : IDisposable
        {
            private readonly Color m_PreviousColor;

            public ColorScopeBuilder(Color color)
            {
                m_PreviousColor = GUI.color;
                GUI.color = color;
            }

            public void Dispose()
            {
                GUI.color = m_PreviousColor;
            }
        }

        public static ColorScopeBuilder ColorScope(Color color)
        {
            return new ColorScopeBuilder(color);
        }

        public readonly struct HorizontalScopeBuilder : IDisposable
        {
            public readonly void Dispose()
            {
                GUILayout.EndHorizontal();
            }
        }

        public static HorizontalScopeBuilder HorizontalScope()
        {
            GUILayout.BeginHorizontal();
            return default;
        }

        public readonly struct VerticalScopeBuilder : IDisposable
        {
            public readonly void Dispose()
            {
                GUILayout.EndVertical();
            }
        }

        public static VerticalScopeBuilder VerticalScope()
        {
            GUILayout.BeginVertical();
            return default;
        }

        public readonly struct ChangedScopeBuilder : IDisposable
        {
            private readonly bool m_PreviousChanged;
            private readonly bool m_IgnoreCurrent;

            public ChangedScopeBuilder(bool previousChanged, bool ignoreCurrent)
            {
                m_PreviousChanged = previousChanged;
                m_IgnoreCurrent = ignoreCurrent;
            }

            public void Dispose()
            {
                bool changed = m_PreviousChanged;
                if (m_IgnoreCurrent == false && GUI.changed)
                {
                    changed = true;
                }
                GUI.changed = changed;
            }
        }

        public static ChangedScopeBuilder ChangedScope(bool ignoreCurrent = false)
        {
            return new ChangedScopeBuilder(GUI.changed, ignoreCurrent);
        }

        public readonly struct DisabledScopeBuilder : IDisposable
        {
            private readonly bool m_PreviousEnabled;

            public DisabledScopeBuilder(bool disabled)
            {
                m_PreviousEnabled = GUI.enabled;
                GUI.enabled = !disabled;
            }

            public void Dispose()
            {
                GUI.enabled = m_PreviousEnabled;
            }
        }

        public static DisabledScopeBuilder DisabledScope(bool disabled = true)
        {
            return new DisabledScopeBuilder(disabled);
        }
    }
}