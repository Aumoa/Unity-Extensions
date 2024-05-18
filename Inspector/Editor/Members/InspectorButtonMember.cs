#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector
{
    public class InspectorButtonMember : InspectorMember
    {
        private readonly MethodInfo m_MethodInfo;
        private readonly ButtonAttribute m_Attribute;
        private readonly bool m_IsMismatch;
        
        public InspectorButtonMember(InspectorMember parent, Object unityObject, MethodInfo memberInfo, string pathName, ButtonAttribute attribute)
            : base(parent, unityObject, null, null, memberInfo, pathName)
        {
            m_MethodInfo = memberInfo;
            m_Attribute = attribute;

            var parameters = m_MethodInfo.GetParameters();
            if (parameters.Length != attribute.bindings.Length)
            {
                LogErrorSignature("binding count mismatch", attribute.bindings.Length.ToString());
                m_IsMismatch = true;
                return;
            }

            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType != typeof(SerializedProperty))
                {
                    LogErrorSignature("argument type mismatch", "SerializedProperty");
                    m_IsMismatch = true;
                    return;
                }
            }
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            float spacing = InspectorDrawer.EvaluateDecorators(rect, this, false, true);
            rect.y += spacing;
            rect.height -= spacing;

            using (Scopes.DisabledScope(m_IsMismatch || !GUI.enabled))
            {
                if (GUI.Button(rect, label))
                {
                    Invoke();
                }
            }
        }

        public override float GetHeight()
        {
            var height = m_Attribute.buttonHeight;
            if (height == 0)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            return height + InspectorDrawer.EvaluateDecorators(default, this, false, false);
        }

        public override InspectorMember[] GetChildren()
        {
            return Array.Empty<InspectorMember>();
        }

        public override ReorderableList GenerateReorderableList()
        {
            return null;
        }

        public override Type GetMemberType()
        {
            return null;
        }

        public override string name => m_MethodInfo.Name;
        
        public override bool isEditable => true;

        public override bool isExpanded { get; set; } = false;

        public override bool isExpandable => false;
        
        public override bool isList => false;

        private void Invoke()
        {
            if (m_IsMismatch)
            {
                return;
            }

            var dispatch = GetParent().siblings;
            var arguments = m_Attribute.bindings
                .Select(p =>
                {
                    dispatch.TryGetValue(p, out var v);
                    return v;
                })
                .Select(p => p as InspectorSerializedFieldMember)
                .Select(p => (object)p?.m_SerializedProperty)
                .ToArray();
            
            var ownedObject = GetParent().GetValue();
            m_MethodInfo.Invoke(ownedObject, arguments);
        }

        private void LogErrorSignature(string message, string expected)
        {
            Debug.LogErrorFormat("{0}.{1}: Signature error: {2}. expected: {3}.", GetUnityObject().GetType().Name, propertyPath, message, expected);
        }
    }
}
#endif