using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor
{
    public static class CustomInspectorLayout
    {
        private static readonly Stack<(float, bool)> s_SpacingStack = new();
        private static float s_Spacing;
        private static float s_RemoveMargin;
        private static bool s_HorizontalMode;
        
        internal static void BeginLayout(bool renderMode)
        {
            s_Spacing = 0;
            s_SpacingStack.Clear();
            s_HorizontalMode = false;
            CustomInspectorLayout.renderMode = renderMode;
        }
        
        internal static float EndLayout()
        {
            if (s_SpacingStack.Count != 0)
            {
                throw new InvalidOperationException("Begin/End is not match.");
            }
            
            return s_Spacing - s_RemoveMargin;
        }
        
        private static void ApplySpacing(float spacing, float margin)
        {
            if (s_HorizontalMode)
            {
                s_Spacing = Mathf.Max(spacing + margin, s_Spacing);
                s_RemoveMargin = Mathf.Max(s_RemoveMargin, margin);
            }
            else
            {
                s_Spacing += spacing + margin;
                s_RemoveMargin = margin;
            }
        }

        public static Object ObjectField(Object unityObject, Type objectType, bool allowSceneObjects)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.ObjectField(unityObject, objectType, allowSceneObjects);
            }
            else
            {
                return unityObject;
            }
        }

        public static Object ObjectField(string label, Object unityObject, Type objectType, bool allowSceneObjects)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.ObjectField(label, unityObject, objectType, allowSceneObjects);
            }
            else
            {
                return unityObject;
            }
        }

        public static int Popup(string label, int index, string[] names)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.Popup(label, index, names);
            }
            else
            {
                return index;
            }
        }

        public static string TextField(string label, string value)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.TextField(label, value);
            }

            return value;
        }

        public static Vector3 Vector3Field(string label, in Vector3 value)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.Vector3Field(label, value);
            }

            return value;
        }

        public static int IntField(string label, int value)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.IntField(label, value);
            }
            else
            {
                return value;
            }
        }

        public static void LabelField(string label)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                EditorGUILayout.LabelField(label);
            }
        }

        public static void LabelField(string label, GUIStyle style)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                EditorGUILayout.LabelField(label, style);
            }
        }

        public static void LabelField(string label, string value)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                EditorGUILayout.LabelField(label, value);
            }
        }

        public static float Slider(string label, float value, float minValue, float maxValue)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.Slider(label, value, minValue, maxValue);
            }

            return value;
        }

        public static int IntSlider(string label, int value, int minValue, int maxValue)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.IntSlider(label, value, minValue, maxValue);
            }

            return value;
        }

        public static int IntSlider(int value, int minValue, int maxValue)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.IntSlider(value, minValue, maxValue);
            }

            return value;
        }

        public static Color ColorField(string label, Color value)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.ColorField(label, value);
            }

            return value;
        }

        public static void Space(float width)
        {
            ApplySpacing(width, 0);
            if (renderMode)
            {
                EditorGUILayout.Space(width);
            }
        }

        public static bool BeginFoldoutHeaderGroup(bool value, string label)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return EditorGUILayout.BeginFoldoutHeaderGroup(value, label);
            }
            else
            {
                return value;
            }
        }

        public static void EndFoldoutHeaderGroup()
        {
            if (renderMode)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }

        public static bool Button(string text)
        {
            ApplySpacing(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, EditorGUIUtility.standardVerticalSpacing);
            if (renderMode)
            {
                return GUILayout.Button(text);
            }

            return false;
        }

        public static void BeginHorizontal()
        {
            s_SpacingStack.Push((s_Spacing, s_HorizontalMode));
            s_Spacing = 0;
            s_RemoveMargin = 0;
            s_HorizontalMode = true;
            if (renderMode)
            {
                EditorGUILayout.BeginHorizontal();
            }
        }

        public static void EndHorizontal()
        {
            var (spacing, horizontalMode) = s_SpacingStack.Pop();
            (s_Spacing, spacing) = (spacing, s_Spacing);
            s_HorizontalMode = horizontalMode;
            ApplySpacing(spacing - s_RemoveMargin, s_RemoveMargin);
            if (renderMode)
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        public static bool renderMode { get; private set; }
    }
}