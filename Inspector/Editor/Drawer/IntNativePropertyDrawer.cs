using System;
using Ayla.Inspector.Editor.Members;
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector.Editor.Drawer
{
    [CustomNativePropertyDrawer(typeof(int))]
    [CustomNativePropertyDrawer(typeof(uint))]
    [CustomNativePropertyDrawer(typeof(short))]
    [CustomNativePropertyDrawer(typeof(ushort))]
    [CustomNativePropertyDrawer(typeof(sbyte))]
    [CustomNativePropertyDrawer(typeof(byte))]
    public class IntNativePropertyDrawer : NativePropertyDrawer
    {
        public override void OnGUI(Rect position, InspectorMember property, GUIContent label)
        {
            var value = (IConvertible)property.GetValue();
            var intValue = value.ToInt64(null);
            GUI.changed = false;

            unchecked
            {
                intValue = EditorGUI.IntField(position, label, (int)intValue);

                if (GUI.changed)
                {
                    switch (Type.GetTypeCode(value.GetType()))
                    {
                        case TypeCode.Int32:
                            property.SetValue((int)intValue);
                            break;
                        case TypeCode.UInt32:
                            property.SetValue((uint)intValue);
                            break;
                        case TypeCode.Int16:
                            property.SetValue((short)intValue);
                            break;
                        case TypeCode.UInt16:
                            property.SetValue((ushort)intValue);
                            break;
                        case TypeCode.SByte:
                            property.SetValue((sbyte)intValue);
                            break;
                        case TypeCode.Byte:
                            property.SetValue((byte)intValue);
                            break;
                    }

                    EditorUtility.SetDirty(property.GetUnityObject());
                    GUI.changed = false;
                }
            }
        }
    }
}
