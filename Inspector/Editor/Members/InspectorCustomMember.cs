#if UNITY_EDITOR
using System;
using System.Reflection;
using Avalon.Inspector.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Avalon.Inspector.Members
{
    public class InspectorCustomMember : InspectorMember
    {
        private readonly MethodInfo m_MethodInfo;
        private readonly bool m_IsMismatch;
        private readonly int m_ParamsCount;
        private readonly object[] m_ParamsCache;

        public InspectorCustomMember(InspectorMember parent, Object unityObject, MethodInfo methodInfo, string pathName)
            : base(parent, unityObject, null, null, methodInfo, pathName)
        {
            m_MethodInfo = methodInfo;

            var parameters = methodInfo.GetParameters();
            if (parameters.Length >= 3)
            {
                Debug.LogErrorFormat("Signature error: parameter must be accept (), (Rect position), (Rect position, GUIContent label).");
                m_IsMismatch = true;
                return;
            }

            if (parameters.Length >= 1 && parameters[0].ParameterType != typeof(Rect))
            {
                Debug.LogErrorFormat("Signature error: parameter must be accept (), (Rect position), (Rect position, GUIContent label).");
                m_IsMismatch = true;
                return;
            }

            if (parameters.Length >= 2 && parameters[1].ParameterType != typeof(GUIContent))
            {
                Debug.LogErrorFormat("Signature error: parameter must be accept (), (Rect position), (Rect position, GUIContent label).");
                m_IsMismatch = true;
                return;
            }

            m_ParamsCount = parameters.Length;
            m_ParamsCache = m_ParamsCount > 0 ? new object[m_ParamsCount] : Array.Empty<object>();
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            if (m_IsMismatch)
            {
                return;
            }

            var ownedObject = GetParent().GetValue();
            rect = EditorStyles.inspectorDefaultMargins.RevertMargins(rect);
            GUILayout.BeginArea(rect, EditorStyles.inspectorDefaultMargins);
            CustomInspectorLayout.BeginLayout(true);
            if (InvokeMethod(ownedObject, rect, label))
            {
                GetParent().SetValue(ownedObject);
                EditorUtility.SetDirty(GetUnityObject());
            }
            CustomInspectorLayout.EndLayout();
            GUILayout.EndArea();
        }

        public override float GetHeight()
        {
            CustomInspectorLayout.BeginLayout(false);
            var ownedObject = GetParent().GetValue();
            InvokeMethod(ownedObject, default, label);
            return CustomInspectorLayout.EndLayout();
        }

        private bool InvokeMethod(object ownedObject, Rect rect, GUIContent label)
        {
            if (m_ParamsCount >= 1)
            {
                m_ParamsCache[0] = rect;
            }
            if (m_ParamsCount >= 2)
            {
                m_ParamsCache[1] = label;
            }
            var result = m_MethodInfo.Invoke(ownedObject, m_ParamsCache);
            if (m_MethodInfo.ReturnType == typeof(bool) && (bool)result)
            {
                return true;
            }

            return false;
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
    }
}
#endif