using System;
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector.Utilities
{
    public static class Scopes
    {
        public readonly struct ColorScopeBuilder : IDisposable
        {
            private readonly Color m_PreviousColor;

            public ColorScopeBuilder(Color color)
            {
                m_PreviousColor = GUI.color;
                GUI.color = color;
            }

            public readonly void Dispose()
            {
                GUI.color = m_PreviousColor;
            }
        }

        public static ColorScopeBuilder ColorScope(Color color)
        {
            return new ColorScopeBuilder(color);
        }

        public readonly struct IndentLevelScopeBuilder : IDisposable
        {
            private readonly int m_PreviousIndentLevel;

            public IndentLevelScopeBuilder(int increaseIndentLevel)
            {
                m_PreviousIndentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel += increaseIndentLevel;
            }

            public readonly void Dispose()
            {
                EditorGUI.indentLevel = m_PreviousIndentLevel;
            }
        }

        public static IndentLevelScopeBuilder IndentLevelScope(int increaseLevel)
        {
            return new IndentLevelScopeBuilder(increaseLevel);
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
    }
}