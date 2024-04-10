using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ayla.Inspector.Editor.Drawer;
using Ayla.Inspector.Editor.Extensions;
using Ayla.Inspector.Editor.Utilities;
using Ayla.Inspector.Meta;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Members
{
    public class InspectorNativePropertyMember : InspectorMember
    {
        private NativePropertyDrawer drawer;
        private InspectorMember[] cachedChildren;
        private PropertyInfo propertyInfo;

        public InspectorNativePropertyMember(InspectorMember parent, Object unityObject, Func<object> getter, Action<object> setter, PropertyInfo propertyInfo, string pathName)
            : base(parent, unityObject, getter, setter, propertyInfo, pathName)
        {
            this.propertyInfo = propertyInfo;
            drawer = ScriptAttributeUtility.InstantiateNativePropertyDrawer(propertyInfo.PropertyType);
        }

        private void CacheChildren()
        {
            cachedChildren = GetValue().GetInspectorChildren(GetUnityObject(), this, Enumerable.Empty<SerializedProperty>()).ToArray();
        }

        public override float GetHeight()
        {
            if (isVisible)
            {
                return drawer?.GetPropertyHeight(this, label) ?? 0;
            }
            return 0;
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            drawer?.OnGUI(rect, this, label);
        }

        public override ReorderableList GenerateReorderableList()
        {
            var list = new ReorderableList((IList)GetValue(), propertyInfo.PropertyType, true, false, true, true)
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
            return propertyInfo.PropertyType;
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

        public override IEnumerable<InspectorMember> GetChildren() => children;

        public override string name => propertyInfo.Name;

        public override bool isEditable => propertyInfo.GetSetMethod() != null;

        public override bool isExpanded
        {
            get => InspectorDrawer.IsExpanded(this);
            set => InspectorDrawer.UpdateExpanded(this, value);
        }

        public override bool isExpandable => GetMemberType().IsExpandable();

        public override bool isList => propertyInfo.PropertyType == typeof(IList) || propertyInfo.PropertyType.IsSubclassOf(typeof(IList));

        public override bool isVisible => HasCustomAttribute<ShowNativeMemberAttribute>();
    }
}
