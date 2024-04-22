using System;
using System.Collections.Generic;
using System.Reflection;
using Ayla.Inspector.Editor.Extensions;
using Ayla.Inspector.Editor.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Members
{
    public class InspectorSerializedFieldMember : InspectorMember
    {
        public readonly SerializedProperty serializedProperty;
        private InspectorMember[] cachedChildren;

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

        public InspectorSerializedFieldMember(InspectorMember parent, Object unityObject, Func<object> getter, Action<object> setter, SerializedProperty serializedProperty, MemberInfo memberInfo, string pathName)
            : base(parent, unityObject, getter, setter, memberInfo, pathName)
        {
            this.serializedProperty = serializedProperty;
        }

        private void CacheChildren()
        {
            var list = ListUtility.AcquireScoped<SerializedProperty>();
            serializedProperty.GetChildren(list.m_Source);
            cachedChildren = GetValue().GetInspectorChildren(GetUnityObject(), this, list.m_Source);
        }

        public override float GetHeight()
        {
            if (isVisible)
            {
                return EditorGUI.GetPropertyHeight(serializedProperty, includeChildren: false);
            }
            return 0;
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            if (isList)
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
                return InspectorDrawer.GetHeight_Element(children[index]);
            };
            list.drawElementCallback += (rect, index, isActive, isFocused) =>
            {
                rect.x += 10.0f;
                if (children.Length <= index)
                {
                    cachedChildren = null;
                    return;
                }
                InspectorDrawer.OnGUI_Element(children[index], new Vector2(rect.x, rect.y), false);
            };
            list.onAddCallback += list =>
            {
                ReorderableList.defaultBehaviours.DoAddButton(list);
                cachedChildren = null;
            };
            list.onRemoveCallback += list =>
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
                cachedChildren = null;
            };
            list.onReorderCallback += list =>
            {
                cachedChildren = null;
            };

            return list;
        }

        public override Type GetMemberType()
        {
            return ((FieldInfo)GetMemberInfo()).FieldType;
        }

        public InspectorMember[] children
        {
            get
            {
                if (cachedChildren == null)
                {
                    CacheChildren();
                }
                return cachedChildren;
            }
        }

        public override IEnumerable<InspectorMember> GetChildren()
        {
            return children;
        }

        public override string name => serializedProperty.name;

        public override string displayName => serializedProperty.displayName;

        public override bool isEditable => serializedProperty.editable;

        public override bool isExpanded
        {
            get => serializedProperty.isExpanded;
            set => serializedProperty.isExpanded = value;
        }

        public override bool isExpandable => GetMemberType().IsExpandable();

        public override bool isList => serializedProperty.IsList();
    }
}
