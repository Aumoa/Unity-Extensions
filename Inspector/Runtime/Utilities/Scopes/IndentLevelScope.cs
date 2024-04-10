using System;
using UnityEditor;

namespace Ayla.Inspector.Runtime.Utilities.Scopes
{
    public readonly struct IndentLevelScope : IDisposable
    {
        private readonly int level;

        public IndentLevelScope(int level)
        {
            this.level = level;
            EditorGUI.indentLevel += level;
        }

        public void Dispose()
        {
            EditorGUI.indentLevel -= level;
        }
    }
}
