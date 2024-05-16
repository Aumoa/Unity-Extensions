#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Avalon.Inspector.Decorator
{
    [CustomPropertyDrawer(typeof(HeaderNameAttribute))]
    public class HeaderNameDecoratorDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            if (attribute is HeaderNameAttribute headerAttribute)
            {
                GUI.Label(position, headerAttribute.name, EditorStyles.boldLabel);
            }
        }

        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif