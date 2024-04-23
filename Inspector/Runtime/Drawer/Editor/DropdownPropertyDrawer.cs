using System.Collections.Generic;
using Ayla.Inspector.Editor;
using Ayla.Inspector.Utilities;
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector.Drawer.Editor
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var handled = InspectorDrawer.currentHandled;
            if (handled != null && attribute is DropdownAttribute dropdownAttribute && property.propertyType is SerializedPropertyType.Integer or SerializedPropertyType.String)
            {
                var dropdownList = handled.GetParent().GetChild(dropdownAttribute.dropdownList);
                if (dropdownList != null && dropdownList.GetValue() is IList<string> strs)
                {
                    int selectedIndex = property.propertyType switch
                    {
                        SerializedPropertyType.Integer => property.intValue,
                        SerializedPropertyType.String => strs.IndexOf(property.stringValue),
                        _ => 0
                    };

                    using (Scopes.ChangedScope())
                    {
                        selectedIndex = EditorGUI.Popup(position, selectedIndex, strs.AsArray());
                        if (GUI.changed)
                        {
                            switch (property.propertyType)
                            {
                                case SerializedPropertyType.Integer:
                                    property.intValue = selectedIndex;
                                    break;
                                case SerializedPropertyType.String:
                                    property.stringValue = strs.GetOrDefault(selectedIndex);
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property);
            }
        }
    }
}