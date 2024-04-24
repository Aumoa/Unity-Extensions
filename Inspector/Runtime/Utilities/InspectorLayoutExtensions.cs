using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector.Utilities
{
    public static class InspectorLayoutExtensions
    {
        public static Rect ApplyMargins(this GUIStyle style, Rect position)
        {
            position.x += style.margin.left + style.padding.left;
            position.y += style.margin.top + style.padding.top;
            position.width -= style.margin.horizontal + style.padding.horizontal;
            position.height -= style.margin.vertical + style.padding.vertical;
            return position;
        }
        
        public static Rect RevertMargins(this GUIStyle style, Rect position)
        {
            position.x -= style.margin.left + style.padding.left;
            position.y -= style.margin.top + style.padding.top;
            position.width += style.margin.horizontal + style.padding.horizontal;
            position.height += style.margin.vertical + style.padding.vertical;
            return position;
        }

        public static Rect ApplyMarginsLeft(this GUIStyle style, Rect position)
        {
            position.x += style.margin.left + style.padding.left;
            position.width -= style.margin.left + style.padding.left;
            return position;
        }

        public static Rect RevertMarginsLeft(this GUIStyle style, Rect position)
        {
            position.x -= style.margin.left + style.padding.left;
            position.width += style.margin.left + style.padding.left;
            return position;
        }

        public static float GetIndentSpace()
        {
            return EditorGUI.indentLevel * 15.0f;
        }
    }
}