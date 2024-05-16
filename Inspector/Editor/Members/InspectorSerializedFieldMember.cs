#if UNITY_EDITOR
using System;
using System.Reflection;
using Avalon.Inspector.Meta;
using Avalon.Inspector.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Avalon.Inspector.Members
{
    public class InspectorSerializedFieldMember : InspectorMember
    {
        private static GUIStyle s_FoldoutStyle;
        
        public readonly SerializedProperty m_SerializedProperty;
        private InspectorMember[] m_CachedChildren;


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
            this.m_SerializedProperty = serializedProperty;
        }

        private void CacheChildren()
        {
            var list = ListUtility.AcquireScoped<SerializedProperty>();
            m_SerializedProperty.GetChildren(list.m_Source);
            m_CachedChildren = GetValue().GetInspectorChildren(GetUnityObject(), this, list.m_Source);
        }

        public override float GetHeight()
        {
            if (isVisible)
            {
                return EditorGUI.GetPropertyHeight(m_SerializedProperty, includeChildren: false);
            }
            return 0;
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            if (isList)
            {
                float spacing = InspectorDrawer.EvaluateDecorators(rect, this, false, true);
                rect.y += spacing;
                rect.height -= spacing;
                m_SerializedProperty.isExpanded = EditorGUI.Foldout(rect, m_SerializedProperty.isExpanded, label, s_FoldoutStyle);
            }
            else
            {
                EditorGUI.PropertyField(rect, m_SerializedProperty, label, includeChildren: false);
            }
        }

        public override ReorderableList GenerateReorderableList()
        {
            var list = new ReorderableList(m_SerializedProperty.serializedObject, m_SerializedProperty, true, false, true, true)
            {
                headerHeight = 0
            };
            list.elementHeightCallback += index => InspectorDrawer.GetHeight_Element(children[index]);
            list.drawElementCallback += (position, index, _, _) =>
            {
                position = EditorStyles.inspectorDefaultMargins.ApplyMarginsLeft(position);
                if (children.Length <= index)
                {
                    m_CachedChildren = null;
                    return;
                }
                InspectorDrawer.OnGUI_Element(children[index], position, false);
            };
            list.onAddCallback += innerList =>
            {
                ReorderableList.defaultBehaviours.DoAddButton(innerList);
                m_CachedChildren = null;
            };
            list.onRemoveCallback += innerList =>
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(innerList);
                m_CachedChildren = null;
            };
            list.onReorderCallback += _ =>
            {
                m_CachedChildren = null;
            };

            return list;
        }

        public override Type GetMemberType()
        {
            return ((FieldInfo)GetMemberInfo()).FieldType;
        }

        public override void HandleOnValueChanged()
        {
            InspectorDrawer.RegisterCallbackOnApplyModifies(base.HandleOnValueChanged);
        }

        public InspectorMember[] children
        {
            get
            {
                if (m_CachedChildren == null)
                {
                    CacheChildren();
                }
                return m_CachedChildren;
            }
        }

        public override InspectorMember[] GetChildren()
        {
            return children;
        }

        public override string name => m_SerializedProperty.name;

        public override string displayName
        {
            get
            {
                var attribute = GetCustomAttribute<LabelAttribute>();
                if (attribute != null)
                {
                    return attribute.labelName;
                }

                return m_SerializedProperty.displayName;
            }
        }

        public override bool isEditable => m_SerializedProperty.editable;

        public override bool isExpanded
        {
            get => m_SerializedProperty.isExpanded;
            set => m_SerializedProperty.isExpanded = value;
        }

        public override bool isExpandable => GetMemberType().IsExpandable();

        public override bool isList => m_SerializedProperty.IsList();
    }
}
#endif