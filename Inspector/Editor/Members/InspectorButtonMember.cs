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
    public class InspectorButtonMember : InspectorMember
    {
        private readonly MethodInfo methodInfo;
        private readonly ButtonAttribute attribute;
        private readonly bool isMismatch;
        
        public InspectorButtonMember(InspectorMember parent, Object unityObject, MethodInfo memberInfo, string pathName, ButtonAttribute attribute)
            : base(parent, unityObject, null, null, memberInfo, pathName)
        {
            methodInfo = memberInfo;
            this.attribute = attribute;

            var parameters = methodInfo.GetParameters();
            if (parameters.Length != attribute.bindings.Length)
            {
                Debug.LogErrorFormat("Signature error: binding count mismatch. expected: {0}.", attribute.bindings.Length);
                isMismatch = true;
                return;
            }

            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType != typeof(SerializedProperty))
                {
                    Debug.LogErrorFormat("Signature error: argument type mismatch. expected: 'SerializedProperty'.");
                    isMismatch = true;
                    return;
                }
            }
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            if (GUI.Button(rect, label))
            {
                Invoke();
            }
        }

        public override float GetHeight()
        {
            var height = attribute.buttonHeight;
            if (height == 0)
            {
                return EditorGUIUtility.singleLineHeight;
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

        public override string name => methodInfo.Name;
        
        public override bool isEditable => true;
        
        public override bool isExpanded => false;
        
        public override bool isList => false;

        private void Invoke()
        {
            if (isMismatch)
            {
                return;
            }

            var dispatch = GetParent()
                .GetChildren()
                .ToDictionary(p => p.name, p => p);

            var arguments = attribute.bindings
                .Select(p =>
                {
                    dispatch.TryGetValue(p, out var v);
                    return v;
                })
                .Select(p => p as InspectorSerializedFieldMember)
                .Select(p => (object)p?.serializedProperty)
                .ToArray();
            
            var ownedObject = GetParent().GetValue();
            methodInfo.Invoke(ownedObject, arguments);
        }
    }
}