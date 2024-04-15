using System;
using System.Collections.Generic;
using System.Linq;
using Ayla.Inspector.Decorator;
using Ayla.Inspector.Editor.Extensions;
using Ayla.Inspector.Editor.Members;
using Ayla.Inspector.Editor.Utilities;
using Ayla.Inspector.Meta;
using Ayla.Inspector.Runtime.Utilities;
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

        public static Vector2 OnGUI_Element(InspectorMember inspectorMember, Vector2 position, bool isLayout)
        {
            if (inspectorMember.isVisible == false)
            {
                return position;
            }

            var rect = new Rect()
            {
                x = position.x,
                y = position.y,
                width = EditorGUIUtility.currentViewWidth - position.x,
                height = inspectorMember.GetHeight()
            };

            bool isDisabled = !inspectorMember.isEditable || !inspectorMember.isEnabled;
            using var disabledScope = new EditorGUI.DisabledScope(disabled: isDisabled);
            var inlineAttribute = inspectorMember.GetCustomAttribute<InlineAttribute>();
            if (inlineAttribute != null)
            {
                position.y += EvaluateDecorators(rect, inspectorMember, isLayout, true);
            }
            else
            {
                float spacing = EditorGUIUtility.standardVerticalSpacing;
                inspectorMember.OnGUI(rect, inspectorMember.label);
                spacing += rect.height;

                if (isLayout && inspectorMember is not InspectorScriptMember)
                {
                    EditorGUILayout.Space(spacing);
                }
                position.y += spacing;
            }

            if (inspectorMember.isExpanded || inlineAttribute != null)
            {
                if (inspectorMember.isList)
                {
                    var list = GetReorderableList(inspectorMember);
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
                    using var indentScope = Scopes.IndentLevelScope(inlineAttribute == null ? 1 : 0);
                    foreach (var child in inspectorMember.GetChildren())
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
                    height += aylaMember.GetChildren()
                        .Select(p => GetHeight_Element(p) + EditorGUIUtility.standardVerticalSpacing)
                        .Sum() - EditorGUIUtility.standardVerticalSpacing;
                }
            }
            return height;
        }

        public override void OnInspectorGUI()
        {
            Current = this;

            bool changed = UpdateAndDrawInspectorGUI(serializedObject, () =>
            {
                // get starting position.
                var rc = EditorGUILayout.GetControlRect();
                Vector2 position = new(rc.x, rc.y);
                inspectorMember ??= serializedObject.GetInspector(serializedObject.targetObject, null, string.Empty);
                foreach (var child in inspectorMember.GetChildren())
                {
                    position = OnGUI_Element(child, position, true);
                }
            });

            if (changed)
            {
                // do refresh.
                inspectorMember = null;
                s_ReorderableLists.Clear();
            }
        }

        public static float EvaluateDecorators(Rect position, InspectorMember inspectorMember, bool isLayout, bool doRender)
        {
            float spacing = 0;

            void EvaluateDecorator(PropertyAttribute attribute)
            {
                var drawer = ScriptAttributeUtility.InstantiateDecoratorDrawer(attribute);
                if (drawer != null)
                {
                    position.height = drawer.GetHeight();
                    if (doRender)
                    {
                        drawer.OnGUI(position);
                    }

                    position.y += position.height;
                    if (isLayout && doRender)
                    {
                        EditorGUILayout.Space(position.height);
                    }
                    spacing += position.height;
                }
            }
            
            foreach (var unityAttribute in inspectorMember.GetCustomAttributes<PropertyAttribute>(inherit: true))
            {
                if (unityAttribute is IInheritableDecorator inheritableDecorator && inheritableDecorator.forDerived)
                {
                    EvaluateDecorator(unityAttribute);
                }
            }
            foreach (var unityAttribute in inspectorMember.GetCustomAttributes<PropertyAttribute>(inherit: false))
            {
                if (unityAttribute is IInheritableDecorator inheritableDecorator && inheritableDecorator.forDerived)
                {
                    continue;
                }
                
                EvaluateDecorator(unityAttribute);
            }

            return spacing;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            Current = null;
        }

        private readonly Dictionary<string, bool> s_IsExpanded = new();
        private readonly Dictionary<string, ReorderableList> s_ReorderableLists = new();

        private static bool UpdateAndDrawInspectorGUI(SerializedObject serializedObject, Action drawer)
        {
            serializedObject.Update();
            drawer?.Invoke();
            return serializedObject.ApplyModifiedProperties();
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
