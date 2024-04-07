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
        private InspectorSerializedObjectMember inspectorMember;

        public static Vector2 OnGUI_Element(InspectorMember aylaMember, Vector2 position, bool isLayout)
        {
            if (aylaMember.isVisible == false)
            {
                return position;
            }

            var rect = new Rect()
            {
                x = position.x,
                y = position.y,
                width = EditorGUIUtility.currentViewWidth - position.x,
                height = aylaMember.GetHeight()
            };

            bool isDisabled = !aylaMember.isEditable || !aylaMember.isEnabled;
            using var disabledScope = new EditorGUI.DisabledScope(disabled: isDisabled);
            aylaMember.OnGUI(rect, aylaMember.label);

            float spacing = rect.height + EditorGUIUtility.standardVerticalSpacing;
            if (isLayout && aylaMember is not InspectorScriptMember)
            {
                EditorGUILayout.Space(spacing);
            }
            position.y += spacing;

            if (aylaMember.isExpanded)
            {
                if (aylaMember.isList)
                {
                    var list = GetReorderableList(aylaMember);
                    rect.y = position.y;
                    rect.height = list.GetHeight();
                    list.DoList(rect);

                    position.y += rect.height;
                    position.y += EditorGUIUtility.standardVerticalSpacing;

                    if (isLayout)
                    {
                        EditorGUILayout.Space(rect.height);
                    }
                }
                else
                {
                    using var indentScope = new EditorGUI.IndentLevelScope();
                    foreach (var child in aylaMember.GetChildren())
                    {
                        position = OnGUI_Element(child, position, isLayout);
                    }
                }
            }

            return position;
        }

        public static float GetHeight_Element(InspectorMember aylaMember)
        {
            if (aylaMember.isVisible == false)
            {
                return 0;
            }

            float height = aylaMember.GetHeight();
            if (aylaMember.isExpanded)
            {
                if (aylaMember.isList)
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
                inspectorMember ??= serializedObject.GetInspector(serializedObject.targetObject, null, string.Empty);
                inspectorMember.OnUpdateInspectorGUI();
                foreach (var child in inspectorMember.GetChildren())
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
            Current.s_IsExpanded.TryGetValue(member.propertyPath, out bool value);
            return value;
        }

        public static void UpdateExpanded(InspectorMember member, bool expanded)
        {
            Current.s_IsExpanded[member.propertyPath] = expanded;
        }

        public static ReorderableList GetReorderableList(InspectorMember member)
        {
            if (Current.s_ReorderableLists.TryGetValue(member.propertyPath, out var list) == false)
            {
                list = member.GenerateReorderableList();
                Current.s_ReorderableLists.Add(member.propertyPath, list);
            }
            return list;
        }
    }
}
