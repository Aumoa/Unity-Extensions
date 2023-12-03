using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Ayla.Inspector.Editor.Extensions;
using Ayla.Inspector.Meta;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace Ayla.Inspector.Editor.Members
{
    public class InspectorSerializedFieldMember : InspectorMember
    {
        private readonly SerializedProperty serializedProperty;
        private InspectorMember[] children;

        private static GUIStyle s_FoldoutStyle;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            s_FoldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold
            };
        }

        static InspectorSerializedFieldMember()
        {
            Initialize();
        }

        public InspectorSerializedFieldMember(InspectorMember parent, Func<object> ownedObject, SerializedProperty serializedProperty, MemberInfo memberInfo, string pathName)
            : base(parent, ownedObject, memberInfo, pathName)
        {
            this.serializedProperty = serializedProperty;
            CacheChildren();
        }

        private void CacheChildren()
        {
            children = GetValue().GetInspectorChildren(this, serializedProperty.GetChildren()).ToArray();
        }

        public override float GetHeight()
        {
            return EditorGUI.GetPropertyHeight(serializedProperty, includeChildren: false);
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            if (IsList)
            {
                serializedProperty.isExpanded = EditorGUI.Foldout(rect, serializedProperty.isExpanded, label, s_FoldoutStyle);
            }
            else
            {
                EditorGUI.PropertyField(rect, serializedProperty, label, includeChildren: false);
            }
        }

        public override ReorderableList GenerateReorderableList()
        {
            var list = new ReorderableList(serializedProperty.serializedObject, serializedProperty, true, false, true, true)
            {
                headerHeight = 0
            };
            list.elementHeightCallback += index =>
            {
                return InspectorDrawer.GetHeight_Element(Children[index]);
            };
            list.drawElementCallback += (rect, index, isActive, isFocused) =>
            {
                rect.x += 10.0f;
                if (Children.Length <= index)
                {
                    children = null;
                    return;
                }
                InspectorDrawer.OnGUI_Element(Children[index], new Vector2(rect.x, rect.y), false);
            };
            list.onAddCallback += list =>
            {
                ReorderableList.defaultBehaviours.DoAddButton(list);
                children = null;
            };
            list.onRemoveCallback += list =>
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
                children = null;
            };
            list.onReorderCallback += list =>
            {
                children = null;
            };

            return list;
        }

        public InspectorMember[] Children
        {
            get
            {
                if (children == null)
                {
                    CacheChildren();
                }
                return children;
            }
        }

        public override IEnumerable<InspectorMember> GetChildren()
        {
            return Children;
        }

        public override string Name => serializedProperty.name;

        public override string DisplayName => serializedProperty.displayName;

        public override bool IsEditable => serializedProperty.editable;

        public override bool IsExpanded => serializedProperty.isExpanded;

        public override bool IsList => serializedProperty.IsList();
    }
}
