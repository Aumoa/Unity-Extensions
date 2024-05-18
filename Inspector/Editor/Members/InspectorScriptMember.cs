#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector
{
    public class InspectorScriptMember : InspectorMember
    {
        private readonly SerializedProperty m_SerializedProperty;

        public InspectorScriptMember(InspectorMember parent, Object unityObject, SerializedProperty serializedProperty, string pathName)
            : base(parent, unityObject, null, null, null, pathName)
        {
            this.m_SerializedProperty = serializedProperty;
        }

        public override float GetHeight()
        {
            return EditorGUI.GetPropertyHeight(m_SerializedProperty);
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            EditorGUI.PropertyField(rect, m_SerializedProperty, label, includeChildren: false);
        }

        public override InspectorMember[] GetChildren()
        {
            return Array.Empty<InspectorMember>();
        }

        public override ReorderableList GenerateReorderableList()
        {
            return new ReorderableList(m_SerializedProperty.serializedObject, m_SerializedProperty);
        }

        public override Type GetMemberType()
        {
            return null;
        }

        public override string name => m_SerializedProperty.name;

        public override string displayName => "Avalon Script";

        public override bool isEditable => false;

        public override bool isExpanded { get; set; } = false;

        public override bool isExpandable => false;

        public override bool isList => false;
    }
}
#endif