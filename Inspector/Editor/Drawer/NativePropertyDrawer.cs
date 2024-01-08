// Copyright 2020-2023 Aumoa.lib. All right reserved.

using Ayla.Inspector.Editor.Members;

using UnityEditor;

using UnityEngine;

namespace Ayla.Inspector.Editor.Drawer
{
    [CustomNativePropertyDrawer(typeof(object), useForChildren: true)]
    public class NativePropertyDrawer
    {
        private GUIContent cachedContent;

        public NativePropertyDrawer()
        {
        }

        public virtual void OnGUI(Rect position, InspectorMember property, GUIContent label)
        {
            cachedContent ??= new GUIContent("No GUI Implemented (NativePropertyDrawer)");
            EditorGUI.LabelField(position, label, cachedContent);
        }

        public virtual float GetPropertyHeight(InspectorMember property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
