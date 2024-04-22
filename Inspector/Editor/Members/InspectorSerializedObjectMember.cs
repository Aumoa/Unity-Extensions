﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ayla.Inspector.Editor.Extensions;
using Ayla.Inspector.Editor.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Members
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

        public override ReorderableList GenerateReorderableList()
        {
            return null;
        }

        public override Type GetMemberType()
        {
            return serializedObject.targetObject.GetType();
        }

        public override IEnumerable<InspectorMember> GetChildren()
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
            var list = ListUtility.AcquireScoped<SerializedProperty>();
            serializedObject.GetChildren(list.m_Source);
            cachedChildren = GetValue().GetInspectorChildren(GetUnityObject(), this, list.m_Source);
        }
    }
}
