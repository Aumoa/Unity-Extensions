#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Avalon.Inspector.Utilities
{
    public static partial class Scopes
    {
        public readonly struct IndentLevelScopeBuilder : IDisposable
        {
            private readonly int m_PreviousIndentLevel;

            public IndentLevelScopeBuilder(int increaseIndentLevel)
            {
                m_PreviousIndentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel += increaseIndentLevel;
            }

            public void Dispose()
            {
                EditorGUI.indentLevel = m_PreviousIndentLevel;
            }
        }

        public static IndentLevelScopeBuilder IndentLevelScope(int increaseLevel)
        {
            return new IndentLevelScopeBuilder(increaseLevel);
        }
    }
}
#endif