using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Ayla.Inspector.Editor.Members
{
    public class InspectorScriptMember : InspectorMember
    {
        private readonly SerializedProperty serializedProperty;

        public InspectorScriptMember(InspectorMember parent, SerializedProperty serializedProperty, string pathName)
            : base(parent, null, null, pathName)
        {
            this.serializedProperty = serializedProperty;
        }

        public override float GetHeight()
        {
            return EditorGUI.GetPropertyHeight(serializedProperty);
        }

        public override void OnGUI(Rect rect, GUIContent label, bool isLayout)
        {
            if (isLayout)
            {
                EditorGUILayout.PropertyField(serializedProperty, label, includeChildren: false);
            }
            else
            {
                EditorGUI.PropertyField(rect, serializedProperty, label, includeChildren: false);
            }
        }

        public override IEnumerable<InspectorMember> GetChildren()
        {
            return Enumerable.Empty<InspectorMember>();
        }

        public override ReorderableList GenerateReorderableList()
        {
            return new ReorderableList(serializedProperty.serializedObject, serializedProperty);
        }

        public override string name => serializedProperty.name;

        public override string displayName => "Ayla Script";

        public override bool isEditable => false;

        public override bool isExpanded => false;

        public override bool isList => false;
    }
}
