#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Avalon.Inspector.Decorator
{
    [CustomPropertyDrawer(typeof(SeparatorAttribute))]
    public class SeparatorDecoratorDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            if (attribute is SeparatorAttribute separatorAttribute)
            {
                position.y += separatorAttribute.margin;
                position.height = separatorAttribute.space;
                EditorGUI.DrawRect(position, new Color(0.5f, 0.5f, 0.5f, 1.0f));
            }
        }

        public override float GetHeight()
        {
            if (attribute is SeparatorAttribute separatorAttribute)
            {
                return separatorAttribute.space + separatorAttribute.margin * 2.0f;
            }

            return 0.0f;
        }
    }
}
#endif