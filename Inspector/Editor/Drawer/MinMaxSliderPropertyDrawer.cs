#if UNITY_EDITOR
using Avalon.Inspector.Utilities;
using UnityEditor;
using UnityEngine;

namespace Avalon.Inspector.Drawer
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var minMaxSliderAttribute = (MinMaxSliderAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.Vector2 || property.propertyType == SerializedPropertyType.Vector2Int)
            {
                EditorGUI.BeginProperty(position, label, property);

                float labelWidth = EditorGUIUtility.labelWidth - InspectorLayoutExtensions.GetIndentSpace() + EditorGUIUtility.standardVerticalSpacing;
                float floatFieldWidth = EditorGUIUtility.fieldWidth;
                float sliderWidth = position.width - labelWidth - 2.0f * floatFieldWidth;
                float sliderPadding = 5.0f;

                Rect labelRect = new Rect(
                    position.x,
                    position.y,
                    labelWidth,
                    position.height);

                Rect sliderRect = new Rect(
                    position.x + labelWidth + floatFieldWidth + sliderPadding,
                    position.y,
                    sliderWidth - 2.0f * sliderPadding,
                    position.height);

                Rect minFloatFieldRect = new Rect(
                    position.x + labelWidth,
                    position.y,
                    floatFieldWidth + InspectorLayoutExtensions.GetIndentSpace(),
                    position.height);

                Rect maxFloatFieldRect = new Rect(
                    position.x + labelWidth + floatFieldWidth + sliderWidth - InspectorLayoutExtensions.GetIndentSpace(),
                    position.y,
                    floatFieldWidth + InspectorLayoutExtensions.GetIndentSpace(),
                    position.height);

                // Draw the label
                EditorGUI.LabelField(labelRect, label.text);

                // Draw the slider
                EditorGUI.BeginChangeCheck();

                if (property.propertyType == SerializedPropertyType.Vector2)
                {
                    Vector2 sliderValue = property.vector2Value;
                    EditorGUI.MinMaxSlider(sliderRect, ref sliderValue.x, ref sliderValue.y, minMaxSliderAttribute.minValue, minMaxSliderAttribute.maxValue);

                    sliderValue.x = EditorGUI.FloatField(minFloatFieldRect, sliderValue.x);
                    sliderValue.x = Mathf.Clamp(sliderValue.x, minMaxSliderAttribute.minValue, Mathf.Min(minMaxSliderAttribute.maxValue, sliderValue.y));

                    sliderValue.y = EditorGUI.FloatField(maxFloatFieldRect, sliderValue.y);
                    sliderValue.y = Mathf.Clamp(sliderValue.y, Mathf.Max(minMaxSliderAttribute.minValue, sliderValue.x), minMaxSliderAttribute.maxValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.vector2Value = sliderValue;
                    }
                }
                else if (property.propertyType == SerializedPropertyType.Vector2Int)
                {
                    Vector2Int sliderValue = property.vector2IntValue;
                    float xValue = sliderValue.x;
                    float yValue = sliderValue.y;
                    EditorGUI.MinMaxSlider(sliderRect, ref xValue, ref yValue, minMaxSliderAttribute.minValue, minMaxSliderAttribute.maxValue);

                    sliderValue.x = EditorGUI.IntField(minFloatFieldRect, (int)xValue);
                    sliderValue.x = (int)Mathf.Clamp(sliderValue.x, minMaxSliderAttribute.minValue, Mathf.Min(minMaxSliderAttribute.maxValue, sliderValue.y));

                    sliderValue.y = EditorGUI.IntField(maxFloatFieldRect, (int)yValue);
                    sliderValue.y = (int)Mathf.Clamp(sliderValue.y, Mathf.Max(minMaxSliderAttribute.minValue, sliderValue.x), minMaxSliderAttribute.maxValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.vector2IntValue = sliderValue;
                    }
                }

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.LabelField(position, $"{nameof(MinMaxSliderAttribute)} can be used only on Vector2 or Vector2Int fields");
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif