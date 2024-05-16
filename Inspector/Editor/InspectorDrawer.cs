#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Avalon.Inspector.Decorator;
using Avalon.Inspector.Members;
using Avalon.Inspector.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Avalon.Inspector
{
    [CustomEditor(typeof(Object), true)]
    [CanEditMultipleObjects]
    public class InspectorDrawer : Editor
    {
        public static InspectorDrawer current { get; private set; }
        public static InspectorMember currentHandled { get; private set; }

        private readonly Dictionary<string, bool> s_IsExpanded = new();
        private readonly Dictionary<string, ReorderableList> s_ReorderableLists = new();
        
        private InspectorSerializedObjectMember m_InspectorMember;
        private Action m_CallbacksOnApplyModifies;

        public static Rect OnGUI_Element(InspectorMember inspectorMember, Rect position, bool isLayout)
        {
            if (inspectorMember.isVisible == false)
            {
                return position;
            }

            currentHandled = inspectorMember;
            position.height = inspectorMember.GetHeight();

            bool isDisabled = !inspectorMember.isEditable || !inspectorMember.isEnabled;
            using var disabledScope = new EditorGUI.DisabledScope(disabled: isDisabled);
            using var indent = Scopes.IndentLevelScope(inspectorMember.indentLevel);
            bool isInline = inspectorMember.isInline;
            if (isInline)
            {
                position.y += EvaluateDecorators(position, inspectorMember, isLayout, true);
            }
            else
            {
                using var scope = Scopes.ChangedScope();
                float spacing = EditorGUIUtility.standardVerticalSpacing;
                inspectorMember.OnGUI(position, inspectorMember.label);
                if (GUI.changed)
                {
                    inspectorMember.HandleOnValueChanged();
                }

                spacing += position.height;

                if (isLayout && inspectorMember is not InspectorScriptMember)
                {
                    EditorGUILayout.Space(spacing);
                }
                position.y += spacing;
            }

            if (inspectorMember.isExpanded || isInline)
            {
                if (inspectorMember.isList)
                {
                    var list = GetReorderableList(inspectorMember);
                    position.height = list.GetHeight();
                    list.DoList(position);

                    position.y += position.height;
                    position.y += EditorGUIUtility.standardVerticalSpacing;

                    if (isLayout)
                    {
                        EditorGUILayout.Space(position.height);
                    }
                }
                else
                {
                    using var innerIndent = Scopes.IndentLevelScope(isInline ? 0 : 1);
                    foreach (var child in inspectorMember.GetChildren())
                    {
                        position = OnGUI_Element(child, position, isLayout);
                    }
                }
            }

            return position;
        }

        public static float GetHeight_Element(InspectorMember inspectorMember)
        {
            if (inspectorMember.isVisible == false)
            {
                return 0;
            }

            currentHandled = inspectorMember;
            float height = inspectorMember.GetHeight();
            if (inspectorMember.isExpanded)
            {
                if (inspectorMember.isList)
                {
                    var list = GetReorderableList(inspectorMember);
                    height += list.GetHeight();
                }
                else
                {
                    height += inspectorMember.GetChildren()
                        .Select(p => GetHeight_Element(p) + EditorGUIUtility.standardVerticalSpacing)
                        .Sum() - EditorGUIUtility.standardVerticalSpacing;
                }
            }
            return height;
        }

        public static void RegisterCallbackOnApplyModifies(Action callback)
        {
            current.m_CallbacksOnApplyModifies += callback;
        }

        public override void OnInspectorGUI()
        {
            current = this;

            // Component 상속, MonoBehaviour 아님: 유니티 내장 형식들.
            if (serializedObject.targetObject is Component and not MonoBehaviour)
            {
                base.OnInspectorGUI();
                return;
            }

            bool changed = UpdateAndDrawInspectorGUI(serializedObject, () =>
            {
                // get starting position.
                m_InspectorMember ??= serializedObject.GetInspector(serializedObject.targetObject, null, string.Empty);
                
                var position = EditorGUILayout.GetControlRect();
                var defaultMargins = EditorStyles.inspectorDefaultMargins;
                position.x = defaultMargins.padding.left + defaultMargins.margin.left;
                position.y = defaultMargins.padding.top + defaultMargins.margin.top;
                position.width = EditorGUIUtility.currentViewWidth - defaultMargins.padding.horizontal - defaultMargins.margin.horizontal;
                
                OnGUI_Element(m_InspectorMember, position, true);
            });

            if (changed)
            {
                // do refresh.
                m_InspectorMember = null;
                s_ReorderableLists.Clear();
            }
        }

        private bool UpdateAndDrawInspectorGUI(SerializedObject serializedObject, Action drawer)
        {
            try
            {
                serializedObject.Update();
                drawer?.Invoke();
                bool hasModified = serializedObject.ApplyModifiedProperties();
                var callbacks = m_CallbacksOnApplyModifies;
                m_CallbacksOnApplyModifies = null;
                callbacks?.Invoke();
                return hasModified;
            }
            catch (TerminateInspectorTickAndRepaint)
            {
                m_InspectorMember = null;
                return false;
            }
            catch (Exception e)
            {
                if (e.InnerException is TerminateInspectorTickAndRepaint)
                {
                    m_InspectorMember = null;
                    return false;
                }

                throw;
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
            current = null;
        }

        public static bool IsExpanded(InspectorMember member)
        {
            current.s_IsExpanded.TryGetValue(member.propertyPath, out bool value);
            return value;
        }

        public static void UpdateExpanded(InspectorMember member, bool expanded)
        {
            current.s_IsExpanded[member.propertyPath] = expanded;
        }

        public static ReorderableList GetReorderableList(InspectorMember member)
        {
            if (current.s_ReorderableLists.TryGetValue(member.propertyPath, out var list) == false)
            {
                list = member.GenerateReorderableList();
                current.s_ReorderableLists.Add(member.propertyPath, list);
            }
            return list;
        }
    }
}
#endif