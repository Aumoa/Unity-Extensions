using System;
using System.Linq;
using Ayla.Inspector.Editor;
using Ayla.Inspector.Editor.Members;
using Ayla.Inspector.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Members.Editor
{
    public class InspectorGroup : InspectorMember
    {
        private readonly InspectorMember[] m_Children;
        private readonly GUIStyle m_Style;
        
        public InspectorGroup(InspectorMember parent, Object unityObject, string groupName, string pathName, InspectorMember[] children)
            : base(parent, unityObject, null, null, null, pathName)
        {
            name = groupName;
            m_Children = children;
            m_Style = new GUIStyle(EditorStyles.helpBox);
        }

        public override void OnGUI(Rect position, GUIContent label)
        {
            if (GetChildren().Any(p => p.isVisible) == false)
            {
                return;
            }
            
            position = EditorStyles.inspectorDefaultMargins.RevertMarginsLeft(position);
            position = m_Style.RevertMarginsLeft(position);
            GUI.Box(position, GUIContent.none, m_Style);
            position = m_Style.ApplyMargins(position);
            position = EditorStyles.inspectorDefaultMargins.ApplyMarginsLeft(position);
            
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(position, label, EditorStyles.boldLabel);
            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            
            foreach (var child in GetChildren())
            {
                if (child.isVisible)
                {
                    position.height = InspectorDrawer.GetHeight_Element(child);
                    InspectorDrawer.OnGUI_Element(child, position, false);
                    position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                }
            }
        }

        public override float GetHeight()
        {
            float spacing = m_Style.margin.vertical + m_Style.padding.vertical;
            spacing += EditorGUIUtility.singleLineHeight;
            spacing += EditorGUIUtility.standardVerticalSpacing;
            bool hasVisible = false;
            foreach (var child in GetChildren())
            {
                spacing += InspectorDrawer.GetHeight_Element(child);
                if (child.isVisible)
                {
                    spacing += EditorGUIUtility.standardVerticalSpacing;
                    hasVisible = true;
                }
            }

            if (hasVisible == false)
            {
                return 0;
            }
            
            return spacing;
        }

        public override InspectorMember[] GetChildren()
        {
            return m_Children;
        }

        public override ReorderableList GenerateReorderableList()
        {
            return null;
        }

        public override Type GetMemberType()
        {
            return null;
        }

        public override string name { get; }
        
        public override bool isEditable => true;
        
        public override bool isExpanded { get; set; }
        
        public override bool isExpandable => false;

        public override bool isList => false;
    }
}