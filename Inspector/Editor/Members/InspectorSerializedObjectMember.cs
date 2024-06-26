﻿#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Ayla.Inspector
{
    public class InspectorSerializedObjectMember : InspectorMember
    {
        private readonly SerializedObject serializedObject;

        private InspectorMember[] cachedChildren;

        public InspectorSerializedObjectMember(InspectorMember parent, Object unityObject, SerializedObject serializedObject, string pathName)
            : base(parent, unityObject, () => serializedObject.targetObject, null, null, pathName)
        {
            this.serializedObject = serializedObject;
        }

        public override string name => serializedObject.targetObject.name;

        public override bool isEditable => true;

        public override bool isExpanded { get; set; } = true;

        public override bool isExpandable => true;

        public override bool isList => false;

        public override bool isInline => true;

        public override ReorderableList GenerateReorderableList()
        {
            return null;
        }

        public override Type GetMemberType()
        {
            return serializedObject.targetObject.GetType();
        }

        public override InspectorMember[] GetChildren()
        {
            if (cachedChildren == null)
            {
                CacheChildren();
            }
            return cachedChildren;
        }

        public override float GetHeight()
        {
            return 0.0f;
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
        }

        private void CacheChildren()
        {
            using (ListPool<SerializedProperty>.Get(out var list))
            {
                serializedObject.GetChildren(list);
                cachedChildren = GetValue().GetInspectorChildren(GetUnityObject(), this, list);
            }
        }
    }
}
#endif