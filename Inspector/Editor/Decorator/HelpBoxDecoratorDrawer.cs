#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Avalon.Inspector.Decorator
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute), useForChildren: true)]
    public class HelpBoxDecoratorDrawer : DecoratorDrawer
    {
        private GUIContent m_CachedContent;
        
        public override float GetHeight()
        {
            if (attribute is HelpBoxAttribute helpBoxAttribute)
            {
                m_CachedContent ??= new GUIContent(helpBoxAttribute.text);
                float minHeight = EditorGUIUtility.singleLineHeight * 2.0f;
                float desiredHeight = GUI.skin.box.CalcHeight(m_CachedContent, EditorGUIUtility.currentViewWidth);
                return Mathf.Max(minHeight, desiredHeight);
            }

            return 0;
        }

        public override void OnGUI(Rect position)
        {
            if (attribute is HelpBoxAttribute helpBoxAttribute)
            {
                EditorGUI.HelpBox(position, helpBoxAttribute.text, helpBoxAttribute.infoBoxType switch
                {
                    HelpBoxType.Info => MessageType.Info,
                    HelpBoxType.Warning => MessageType.Warning,
                    HelpBoxType.Error => MessageType.Error,
                    _ => throw new ArgumentException("Invalid help box type detected.")
                });
            }
        }
    }
}
#endif