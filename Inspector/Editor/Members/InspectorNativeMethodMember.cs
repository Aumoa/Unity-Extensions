#if UNITY_EDITOR
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Avalon.Inspector.Drawer;
using Avalon.Inspector.Meta;
using Avalon.Inspector.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Avalon.Inspector.Members
{
    public class InspectorNativeMethodMember : InspectorMember
    {
        private readonly NativePropertyDrawer m_Drawer;
        private readonly MethodInfo m_MethodInfo;
        private InspectorMember[] m_CachedChildren;

        public InspectorNativeMethodMember(InspectorMember parent, Object unityObject, Func<object> getter, MethodInfo methodInfo, string pathName)
            : base(parent, unityObject, getter, null, methodInfo, pathName)
        {
            m_MethodInfo = methodInfo;
            m_Drawer = ScriptAttributeUtility.InstantiateNativePropertyDrawerForChain(methodInfo.ReturnType, methodInfo.GetCustomAttributes<PropertyAttribute>());
        }

        private void CacheChildren()
        {
            m_CachedChildren = GetValue().GetInspectorChildren(GetUnityObject(), this, Enumerable.Empty<SerializedProperty>()).ToArray();
        }

        public override float GetHeight()
        {
            if (isVisible)
            {
                float spacing = m_Drawer?.GetPropertyHeight(this, label) ?? 0;
                return spacing + InspectorDrawer.EvaluateDecorators(default, this, false, false);
            }
            return 0;
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            float spacing = InspectorDrawer.EvaluateDecorators(rect, this, false, true);
            rect.y += spacing;
            rect.height -= spacing;

            m_Drawer?.OnGUI(rect, this, label);
        }

        public override ReorderableList GenerateReorderableList()
        {
            var list = new ReorderableList((IList)GetValue(), m_MethodInfo.ReturnType, true, false, true, true)
            {
                headerHeight = 0
            };
            list.elementHeightCallback += index =>
            {
                return InspectorDrawer.GetHeight_Element(children[index]);
            };
            list.drawElementCallback += (position, index, _, _) =>
            {
                position.x += 10.0f;
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
            return m_MethodInfo.ReturnType;
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

        public override InspectorMember[] GetChildren() => children;

        public override string name => m_MethodInfo.Name;

        public override bool isEditable => false;

        public override bool isExpanded
        {
            get => InspectorDrawer.IsExpanded(this);
            set => InspectorDrawer.UpdateExpanded(this, value);
        }

        public override bool isExpandable => GetMemberType().IsExpandable();

        public override bool isList => m_MethodInfo.ReturnType == typeof(IList) || m_MethodInfo.ReturnType.IsSubclassOf(typeof(IList));

        public override bool isVisible => HasCustomAttribute<ShowNativeMemberAttribute>();
    }
}
#endif