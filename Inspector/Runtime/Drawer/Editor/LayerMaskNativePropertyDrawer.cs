using System;
using Ayla.Inspector.Editor.Members;
using Ayla.Inspector.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Ayla.Inspector.Drawer.Editor
{
    [CustomNativePropertyDrawer(typeof(LayerMaskAttribute))]
    public class LayerMaskNativePropertyDrawer : NativePropertyDrawer
    {
        public override float GetPropertyHeight(InspectorMember property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, InspectorMember property, GUIContent label)
        {
            var typeCode = Type.GetTypeCode(property.GetMemberType());
            switch (typeCode)
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    DrawPropertyForInt(position, property, label, GetLayers());
                    break;
                default:
                    base.OnGUI(position, property, label);
                    break;
            }
        }

        private static string[] GetLayers()
        {
            return InternalEditorUtility.layers;
        }

        private static void DrawPropertyForInt(Rect rect, InspectorMember property, GUIContent label, string[] layers)
        {
            int layerMask = Convert.ToInt32(property.GetValue());

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

                    property.SetValue(Convert.ChangeType(layerMask, property.GetMemberType()));
                }
            }
        }
    }
}