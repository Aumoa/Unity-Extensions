using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ayla.Inspector.SpecialCase;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Members
{
    public class InspectorCustomMember : InspectorMember
    {
        private readonly MethodInfo methodInfo;
        private readonly CustomInspectorAttribute attribute;
        private readonly bool isMismatch;

        public InspectorCustomMember(InspectorMember parent, Object unityObject, MethodInfo methodInfo, string pathName, CustomInspectorAttribute attribute)
            : base(parent, unityObject, null, null, methodInfo, pathName)
        {
            this.methodInfo = methodInfo;
            this.attribute = attribute;

            var parameters = methodInfo.GetParameters();
            if (parameters.Length != 2)
            {
                Debug.LogErrorFormat("Signature error: parameter must be accept (Rect position, GUIContent label).");
                isMismatch = true;
                return;
            }

            if (parameters[0].ParameterType != typeof(Rect) || parameters[1].ParameterType != typeof(GUIContent))
            {
                Debug.LogErrorFormat("Signature error: parameter must be accept (Rect position, GUIContent label).");
                isMismatch = true;
                return;
            }
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            if (isMismatch)
            {
                return;
            }

            var ownedObject = GetParent().GetValue();
            methodInfo.Invoke(ownedObject, new object[] { rect, label });
        }

        public override float GetHeight()
        {
            float height = EditorGUIUtility.singleLineHeight;
            if (string.IsNullOrEmpty(attribute.heightMemberName) == false)
            {
                var child = GetParent().GetChild(attribute.heightMemberName);
                if (child != null)
                {
                    var memberType = Type.GetTypeCode(child.GetMemberType());
                    if (memberType == TypeCode.Single)
                    {
                        height = (float)child.GetValue();
                    }
                }
            }

            return height;
        }

        public override IEnumerable<InspectorMember> GetChildren()
        {
            return Enumerable.Empty<InspectorMember>();
        }

        public override ReorderableList GenerateReorderableList()
        {
            return null;
        }

        public override Type GetMemberType()
        {
            return null;
        }

        public override string name => methodInfo.Name;

        public override bool isEditable => true;

        public override bool isExpanded { get; set; } = false;

        public override bool isExpandable => false;

        public override bool isList => false;
    }
}
