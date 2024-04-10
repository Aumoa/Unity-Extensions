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
        private readonly int paramsCount;

        public InspectorCustomMember(InspectorMember parent, Object unityObject, MethodInfo methodInfo, string pathName, CustomInspectorAttribute attribute)
            : base(parent, unityObject, null, null, methodInfo, pathName)
        {
            this.methodInfo = methodInfo;
            this.attribute = attribute;

            var parameters = methodInfo.GetParameters();
            if (parameters.Length >= 3)
            {
                Debug.LogErrorFormat("Signature error: parameter must be accept (), (Rect position), (Rect position, GUIContent label).");
                isMismatch = true;
                return;
            }

            if (parameters.Length >= 1 && parameters[0].ParameterType != typeof(Rect))
            {
                Debug.LogErrorFormat("Signature error: parameter must be accept (), (Rect position), (Rect position, GUIContent label).");
                isMismatch = true;
                return;
            }

            if (parameters.Length >= 2 && parameters[1].ParameterType != typeof(GUIContent))
            {
                Debug.LogErrorFormat("Signature error: parameter must be accept (), (Rect position), (Rect position, GUIContent label).");
                isMismatch = true;
                return;
            }

            paramsCount = parameters.Length;
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            if (isMismatch)
            {
                return;
            }

            rect = EditorGUI.IndentedRect(rect);

            var ownedObject = GetParent().GetValue();
            GUILayout.BeginArea(rect, EditorStyles.inspectorDefaultMargins);
            var parameters = paramsCount switch
            {
                0 => Array.Empty<object>(),
                1 => new object[] { rect },
                2 => new object[] { rect, label },
                _ => throw new InvalidOperationException("Unexpected error")
            };
            methodInfo.Invoke(ownedObject, parameters);
            GUILayout.EndArea();
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
