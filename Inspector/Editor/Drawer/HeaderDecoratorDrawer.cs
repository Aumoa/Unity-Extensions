using UnityEditor;
using UnityEngine;
using HeaderAttribute = Ayla.Inspector.Decorator.HeaderAttribute;

namespace Ayla.Inspector.Editor.Drawer
{
    [CustomPropertyDrawer(typeof(HeaderAttribute))]
    public class HeaderDecoratorDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            if (attribute is HeaderAttribute headerAttribute)
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