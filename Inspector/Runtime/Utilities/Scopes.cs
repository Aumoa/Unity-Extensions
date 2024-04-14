using System;
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector.Runtime.Utilities
{
    public static class Scopes
    {
        public readonly struct ColorScopeBuilder : IDisposable
        {
            private readonly Color previousColor;

            public ColorScopeBuilder(Color color)
            {
                previousColor = GUI.color;
                GUI.color = color;
            }

            public readonly void Dispose()
            {
                GUI.color = previousColor;
            }
        }

        public static ColorScopeBuilder ColorScope(Color color)
        {
            return new ColorScopeBuilder(color);
        }

        public readonly struct IndentLevelScopeBuilder : IDisposable
        {
            private readonly int previousIndentLevel;

            public IndentLevelScopeBuilder(int increaseIndentLevel)
            {
                previousIndentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel += increaseIndentLevel;
            }

            public readonly void Dispose()
            {
                EditorGUI.indentLevel = previousIndentLevel;
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
    }
}