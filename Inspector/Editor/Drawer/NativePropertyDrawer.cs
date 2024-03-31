using System.Linq;
using Ayla.Inspector.Editor.Members;
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector.Editor.Drawer
{
    [CustomNativePropertyDrawer(typeof(object), useForChildren: true)]
    public class NativePropertyDrawer
    {
        public NativePropertyDrawer()
        {
        }

        public virtual void OnGUI(Rect position, InspectorMember property, GUIContent label)
        {
            if (property.isExpandable)
            {
                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
            }
            else
            {
                EditorGUI.LabelField(position, label);
            }
        }

        public virtual float GetPropertyHeight(InspectorMember property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
