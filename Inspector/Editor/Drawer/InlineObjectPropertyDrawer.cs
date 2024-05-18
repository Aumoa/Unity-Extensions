#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector
{
    [CustomPropertyDrawer(typeof(InlineObjectAttribute))]
    public class InlineObjectPropertyDrawer : PropertyDrawer
    {
        private GUIStyle m_CachedStyle;
        private bool m_Cached;
        private InspectorSerializedObjectMember m_ObjectMember;
        private bool m_Foldout;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float spacing = EditorGUIUtility.singleLineHeight;

            if (m_Foldout)
            {
                if (CacheInspector(property))
                {
                    using var scope = Scopes.IndentLevelScope(1);
                    spacing += InspectorDrawer.GetHeight_Element(m_ObjectMember) + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            CacheStyle();
            spacing += m_CachedStyle.margin.vertical + m_CachedStyle.padding.vertical;

            return spacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CacheStyle();
            position = EditorStyles.inspectorDefaultMargins.RevertMarginsLeft(position);
            position = m_CachedStyle.RevertMarginsLeft(position);
            GUI.Box(position, GUIContent.none, m_CachedStyle);
            position = m_CachedStyle.ApplyMargins(position);
            position = EditorStyles.inspectorDefaultMargins.ApplyMarginsLeft(position);

            position.height = EditorGUIUtility.singleLineHeight;
            var foldoutPosition = position;
            foldoutPosition.width = EditorGUIUtility.labelWidth;
            m_Foldout = EditorGUI.Foldout(foldoutPosition, m_Foldout, label);

            if (attribute is InlineObjectAttribute inlineObjectAttribute)
            {
                using var scoped = Scopes.ChangedScope();
                var objectPosition = position.GetFieldPosition();
                objectPosition.width -= m_CachedStyle.GetRightSpace();
                var newValue = EditorGUI.ObjectField(objectPosition, property.objectReferenceValue, InspectorDrawer.currentHandled.GetMemberType(), inlineObjectAttribute.allowInlineObjects);
                if (GUI.changed)
                {
                    property.objectReferenceValue = newValue;
                    m_Cached = false;
                }
            }

            if (m_Foldout)
            {
                if (CacheInspector(property))
                {
                    using var scope = Scopes.IndentLevelScope(1);
                    position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                    position.height = InspectorDrawer.GetHeight_Element(m_ObjectMember);
                    InspectorDrawer.OnGUI_Element(m_ObjectMember, position, false);
                }
            }
        }

        private void CacheStyle()
        {
            if (m_CachedStyle == null)
            {
                m_CachedStyle = new GUIStyle(EditorStyles.helpBox);
                m_CachedStyle.padding.bottom = m_CachedStyle.padding.top;
            }
        }

        private bool CacheInspector(SerializedProperty property)
        {
            if (m_Cached == false)
            {
                if (property.boxedValue is Object innerObject && InspectorDrawer.currentHandled != null)
                {
                    m_ObjectMember = new SerializedObject(innerObject).GetInspector(innerObject, null, string.Empty);
                }

                m_Cached = true;
            }

            return m_ObjectMember != null;
        }
    }
}
#endif