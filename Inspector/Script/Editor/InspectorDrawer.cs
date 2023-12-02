using System;
using System.Collections.Generic;
using System.Linq;

using Ayla.Inspector.Editor.Extensions;
using Ayla.Inspector.Editor.Members;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor
{
    [CustomEditor(typeof(Object), true)]
    [CanEditMultipleObjects]
    public class InspectorDrawer : UnityEditor.Editor
    {
        public static Vector2 OnGUI_Element(InspectorMember aylaMember, Vector2 position, bool layout)
        {
            var rect = new Rect()
            {
                x = position.x,
                y = position.y,
                width = EditorGUIUtility.currentViewWidth - position.x,
                height = aylaMember.GetHeight()
            };

            if (aylaMember.IsEditable)
            {
                aylaMember.OnGUI(rect, aylaMember.Label);
            }
            else
            {
                using var scope = new EditorGUI.DisabledScope(disabled: true);
                aylaMember.OnGUI(rect, aylaMember.Label);
            }

            float spacing = rect.height + EditorGUIUtility.standardVerticalSpacing;
            if (layout && aylaMember is not InspectorScriptMember)
            {
                EditorGUILayout.Space(spacing);
            }
            position.y += spacing;

            if (aylaMember.IsExpanded)
            {
                if (aylaMember.IsList)
                {
                    var list = GetReorderableList(aylaMember);
                    rect.y = position.y;
                    rect.height = list.GetHeight();
                    list.DoList(rect);

                    position.y += rect.height;
                    position.y += EditorGUIUtility.standardVerticalSpacing;

                    if (layout)
                    {
                        EditorGUILayout.Space(rect.height);
                    }
                }
                else
                {
                    using var scope = new EditorGUI.IndentLevelScope();
                    foreach (var child in aylaMember.GetChildren())
                    {
                        position = OnGUI_Element(child, position, layout);
                    }
                }
            }

            return position;
        }

        public static float GetHeight_Element(InspectorMember aylaMember)
        {
            float height = aylaMember.GetHeight();
            if (aylaMember.IsExpanded)
            {
                if (aylaMember.IsList)
                {
                    var list = GetReorderableList(aylaMember);
                    height += list.GetHeight();
                }
                else
                {
                    height += aylaMember.GetChildren().Select(p => GetHeight_Element(p) + EditorGUIUtility.standardVerticalSpacing).Sum();
                }
            }
            return height;
        }

        public override void OnInspectorGUI()
        {
            Current = this;

            UpdateAndDrawInspectorGUI(serializedObject, () =>
            {
                // get starting position.
                var rc = EditorGUILayout.GetControlRect();
                Vector2 position = new(rc.x, rc.y);
                foreach (var child in serializedObject.GetInspectorChildren())
                {
                    position = OnGUI_Element(child, position, true);
                }
            });
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            Current = null;
        }

        private readonly Dictionary<string, bool> s_IsExpanded = new();
        private readonly Dictionary<string, ReorderableList> s_ReorderableLists = new();

        private static void UpdateAndDrawInspectorGUI(SerializedObject serializedObject, Action drawer)
        {
            serializedObject.Update();
            drawer?.Invoke();
            serializedObject.ApplyModifiedProperties();
        }

        public static InspectorDrawer Current { get; private set; }

        public static bool IsExpanded(InspectorMember member)
        {
            Current.s_IsExpanded.TryGetValue(member.PropertyPath, out bool value);
            return value;
        }

        public static void UpdateExpanded(InspectorMember member, bool expanded)
        {
            Current.s_IsExpanded[member.PropertyPath] = expanded;
        }

        public static ReorderableList GetReorderableList(InspectorMember member)
        {
            if (Current.s_ReorderableLists.TryGetValue(member.PropertyPath, out var list) == false)
            {
                list = member.GenerateReorderableList();
                Current.s_ReorderableLists.Add(member.PropertyPath, list);
            }
            return list;
        }
    }
}
