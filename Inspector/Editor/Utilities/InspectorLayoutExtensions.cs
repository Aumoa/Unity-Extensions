#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector
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

        public static float GetLeftSpace(this GUIStyle style)
        {
            return style.margin.right + style.padding.right;
        }

        public static float GetRightSpace(this GUIStyle style)
        {
            return style.margin.right + style.padding.right;
        }

        public static float GetHorizontalSpace(this GUIStyle style)
        {
            return style.margin.horizontal + style.padding.horizontal;
        }

        public static Rect GetFieldPosition(this Rect position)
        {
            var leftSpace = EditorStyles.inspectorDefaultMargins.GetHorizontalSpace() - GetIndentSpace();
            position.x = EditorGUIUtility.labelWidth - EditorGUIUtility.standardVerticalSpacing + leftSpace;
            position.width = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - leftSpace - EditorStyles.inspectorDefaultMargins.GetRightSpace() + EditorGUIUtility.standardVerticalSpacing;
            return position;
        }

        public static float GetIndentSpace()
        {
            return EditorGUI.indentLevel * 15.0f;
        }

        public static float GetDefaultLeftSpace()
        {
            var style = EditorStyles.inspectorDefaultMargins;
            return style.margin.left + style.padding.left;
        }
    }
}
#endif