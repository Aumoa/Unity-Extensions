#if UNITY_EDITOR
using System;
using Avalon.Inspector.Utilities;
using UnityEditor;
using UnityEngine;

namespace Avalon.Inspector.Drawer
{
    [CustomPropertyDrawer(typeof(ForceEnumAttribute))]
    public class ForceEnumPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is ForceEnumAttribute forceEnumAttribute)
            {
                using var scope = Scopes.ChangedScope();
                var enumValue = (Enum)Convert.ChangeType(property.intValue, forceEnumAttribute.enumType);
                var select = EditorGUI.EnumPopup(position, label, enumValue);
                if (GUI.changed)
                {
                    property.intValue = ((IConvertible)select).ToInt32(null);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}
#endif