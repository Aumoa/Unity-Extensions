using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ayla.Inspector.Editor.Drawer;
using Ayla.Inspector.Editor.Extensions;
using Ayla.Inspector.Editor.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Members
{
    public class InspectorNonSerializedFieldMember : InspectorMember
    {
        private NativePropertyDrawer drawer;
        private InspectorMember[] cachedChildren;
        private FieldInfo fieldInfo;

        public InspectorNonSerializedFieldMember(InspectorMember parent, Object unityObject, Func<object> getter, Action<object> setter, FieldInfo fieldInfo, string pathName)
            : base(parent, unityObject, getter, setter, fieldInfo, pathName)
        {
            this.fieldInfo = fieldInfo;
            drawer = ScriptAttributeUtility.InstantiateNativePropertyDrawer(fieldInfo.FieldType);
            CacheChildren();
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
            var list = new ReorderableList((IList)GetValue(), fieldInfo.FieldType, true, false, true, true)
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
            return fieldInfo.FieldType;
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

        public override string name => fieldInfo.Name;

        public override bool isEditable => !fieldInfo.IsInitOnly;

        public override bool isExpanded
        {
            get => InspectorDrawer.IsExpanded(this);
            set => InspectorDrawer.UpdateExpanded(this, value);
        }

        public override bool isExpandable => GetMemberType().IsExpandable();

        public override bool isList => fieldInfo.FieldType == typeof(IList) || fieldInfo.FieldType.IsSubclassOf(typeof(IList));
    }
}
