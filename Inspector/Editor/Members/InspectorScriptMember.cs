using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Members
{
    public class InspectorScriptMember : InspectorMember
    {
        private readonly SerializedProperty serializedProperty;

        public InspectorScriptMember(InspectorMember parent, Object unityObject, SerializedProperty serializedProperty, string pathName)
            : base(parent, unityObject, null, null, null, pathName)
        {
            this.serializedProperty = serializedProperty;
        }

        public override float GetHeight()
        {
            return EditorGUI.GetPropertyHeight(serializedProperty);
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            EditorGUI.PropertyField(rect, serializedProperty, label, includeChildren: false);
        }

        public override IEnumerable<InspectorMember> GetChildren()
        {
            return Enumerable.Empty<InspectorMember>();
        }

        public override ReorderableList GenerateReorderableList()
        {
            return new ReorderableList(serializedProperty.serializedObject, serializedProperty);
        }

        public override Type GetMemberType()
        {
            return null;
        }

        public override string name => serializedProperty.name;

        public override string displayName => "Ayla Script";

        public override bool isEditable => false;

        public override bool isExpanded { get; set; } = false;

        public override bool isExpandable => false;

        public override bool isList => false;
    }
}
