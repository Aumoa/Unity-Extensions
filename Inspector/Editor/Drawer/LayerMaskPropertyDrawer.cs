#if UNITY_EDITOR
using Avalon.Inspector.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Avalon.Inspector.Drawer
{
    [CustomPropertyDrawer(typeof(LayerMaskAttribute))]
    public class LayerMaskPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                DrawPropertyForInt(position, property, label, GetLayers());
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }

            EditorGUI.EndProperty();
        }

        private static string[] GetLayers()
        {
            return InternalEditorUtility.layers;
        }

        private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label, string[] layers)
        {
            var layerMask = property.intValue;

            int fastInt = 0;
            int scopeMask = 0;
            for (int i = 0; i < 32; ++i)
            {
                int currentMask = 1 << i;
                if ((layerMask & currentMask) > 0)
                {
                    string layerName = LayerMask.LayerToName(i);
                    // 알려진 레이어 정렬과 전체 레이어 정렬이 같은 점을 이용하여 빠르게 비교합니다.
                    for (int j = fastInt; j < layers.Length; ++j)
                    {
                        if (layerName == layers[j])
                        {
                            scopeMask |= 1 << j;
                            fastInt = j;
                            break;
                        }
                    }
                }
            }

            using (Scopes.ChangedScope())
            {
                scopeMask = EditorGUI.MaskField(rect, label, scopeMask, layers);
                if (GUI.changed)
                {
                    layerMask = 0;
                    for (int i = 0; i < layers.Length; ++i)
                    {
                        int currentMask = 1 << i;
                        if ((scopeMask & currentMask) > 0)
                        {
                            layerMask |= 1 << LayerMask.NameToLayer(layers[i]);
                        }
                    }

                    property.intValue = layerMask;
                }
            }
        }
    }
}
#endif