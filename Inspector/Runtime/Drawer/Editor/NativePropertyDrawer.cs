using System;
using Ayla.Inspector.Editor.Members;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Drawer.Editor
{
    public class NativePropertyDrawer
    {
        private static GUIContent s_NullContent;

        internal Attribute m_Attribute;

        protected Attribute attribute => m_Attribute;

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

            var value = property.GetValue();
            var valueType = property.GetMemberType();
            if (valueType == null)
            {
                return;
            }

            if (value == null)
            {
                if (valueType.IsSubclassOf(typeof(Object)))
                {
                    EditorGUI.ObjectField(position, label, null, valueType, allowSceneObjects: true);
                }
                else
                {
                    s_NullContent ??= new GUIContent("null");
                    EditorGUI.LabelField(position, label, s_NullContent);
                }
            }
            else
            {
                TypeCode typeCode = Type.GetTypeCode(valueType);
                var conv = value as IConvertible;
                bool handled = false;
                int? intValue = null;
                long? longValue = null;
                float? floatValue = null;
                double? doubleValue = null;
                string stringValue = null;
                Vector2? vector2Value = null;
                Vector2Int? vector2IntValue = null;
                Vector3? vector3Value = null;
                Vector3Int? vector3IntValue = null;
                Enum enumValue = null;

                if (valueType.IsEnum)
                {
                    enumValue = EditorGUI.EnumPopup(position, label, (Enum)value);
                    handled = true;
                }
                else
                {
                    switch (typeCode)
                    {
                        case TypeCode.Byte:
                        case TypeCode.SByte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                            intValue = EditorGUI.IntField(position, label, conv.ToInt32(null));
                            handled = true;
                            break;
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                            longValue = EditorGUI.LongField(position, label, conv.ToInt64(null));
                            handled = true;
                            break;
                        case TypeCode.Single:
                            floatValue = EditorGUI.FloatField(position, label, conv.ToSingle(null));
                            handled = true;
                            break;
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            doubleValue = EditorGUI.DoubleField(position, label, conv.ToDouble(null));
                            handled = true;
                            break;
                        case TypeCode.String:
                            stringValue = EditorGUI.TextField(position, label, value as string);
                            handled = true;
                            break;
                    }
                }

                if (handled == false)
                {
                    if (valueType == typeof(Vector2))
                    {
                        vector2Value = EditorGUI.Vector2Field(position, label, (Vector2)value);
                        handled = true;
                    }
                    else if (valueType == typeof(Vector2Int))
                    {
                        vector2IntValue = EditorGUI.Vector2IntField(position, label, (Vector2Int)value);
                        handled = true;
                    }
                    else if (valueType == typeof(Vector3))
                    {
                        vector3Value = EditorGUI.Vector3Field(position, label, (Vector3)value);
                        handled = true;
                    }
                    else if (valueType == typeof(Vector3Int))
                    {
                        vector3IntValue = EditorGUI.Vector3IntField(position, label, (Vector3Int)value);
                        handled = true;
                    }
                }

                if (GUI.changed && handled)
                {
                    var unityObject = property.GetUnityObject();
                    Undo.RecordObject(unityObject, "Native Property");


                    if (enumValue != null)
                    {
                        property.SetValue(enumValue);
                    }
                    else
                    {
                        switch (typeCode)
                        {
                            case TypeCode.Byte:
                                property.SetValue((byte)intValue.Value);
                                break;
                            case TypeCode.SByte:
                                property.SetValue((sbyte)intValue.Value);
                                break;
                            case TypeCode.Int16:
                                property.SetValue((short)intValue.Value);
                                break;
                            case TypeCode.UInt16:
                                property.SetValue((ushort)intValue.Value);
                                break;
                            case TypeCode.Int32:
                                property.SetValue(intValue.Value);
                                break;
                            case TypeCode.UInt32:
                                property.SetValue((uint)intValue.Value);
                                break;
                            case TypeCode.Int64:
                                property.SetValue(longValue.Value);
                                break;
                            case TypeCode.UInt64:
                                property.SetValue((ulong)longValue.Value);
                                break;
                            case TypeCode.Single:
                                property.SetValue(floatValue.Value);
                                break;
                            case TypeCode.Double:
                                property.SetValue(doubleValue.Value);
                                break;
                            case TypeCode.Decimal:
                                property.SetValue((decimal)doubleValue.Value);
                                break;
                            case TypeCode.String:
                                property.SetValue(stringValue);
                                break;
                            default:
                                if (valueType == typeof(Vector2))
                                {
                                    property.SetValue(vector2Value.Value);
                                }
                                else if (valueType == typeof(Vector2Int))
                                {
                                    property.SetValue(vector2IntValue.Value);
                                }
                                else if (valueType == typeof(Vector3))
                                {
                                    property.SetValue(vector3Value.Value);
                                }
                                else if (valueType == typeof(Vector3Int))
                                {
                                    property.SetValue(vector3IntValue.Value);
                                }
                                break;
                        }
                    }

                    EditorUtility.SetDirty(property.GetUnityObject());
                }
            }
        }

        public virtual float GetPropertyHeight(InspectorMember property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
