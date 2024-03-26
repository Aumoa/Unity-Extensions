using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using Ayla.Inspector.Editor.Members;
using Ayla.Inspector.Meta;
using Ayla.Inspector.SpecialCase;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Extensions
{
    public static class InspectorPropertyExtensions
    {
        private static string NormalizeUnityPath(this string path)
        {
            return path
                .Replace(".Array.data[", "[")
                .Replace(".Array", string.Empty);
        }

        public static string GetUniqueId(SerializedProperty serializedProperty)
        {
            string uniqueId = serializedProperty.serializedObject.targetObject.GetInstanceID() + serializedProperty.propertyPath;
            return uniqueId;
        }

        private static object GetTargetObject(this SerializedProperty serializedProperty)
        {
            string propertyPath = serializedProperty.propertyPath.NormalizeUnityPath();
            string[] elements = propertyPath.Split('.');
            object currentObject = serializedProperty.serializedObject.targetObject;
            var currentType = currentObject.GetType();

            foreach (var element in elements)
            {
                if (element[^1] == ']')
                {
                    var indexOf = element.IndexOf('[');
                    var name = element[..indexOf];
                    int.TryParse(element[(indexOf + 1)..(^1)], out int index);
                    var listMemberInfo = currentType.GetMember(name);
                    if (listMemberInfo.Length != 1)
                    {
                        return null;
                    }

                    var currentMember = listMemberInfo[0];
                    Type listType = currentMember.GetReturningType();
                    currentType = listType.GetElementType() ?? listType.GetGenericArguments()[0];
                    var list = (IList)currentMember.GetReturningValue(currentObject);
                    if (list.Count <= index)
                    {
                        return null;
                    }
                    currentObject = list[index];
                }
                else
                {
                    var member = currentType.GetMember(element);
                    if (member.Length != 1)
                    {
                        return null;
                    }

                    var memberType = member.First();
                    if (element[^1] == ']')
                    {
                        var returningType = memberType.GetReturningType();
                        if (returningType.IsArray)
                        {
                            memberType = returningType.GetElementType();
                        }
                        else
                        {
                            memberType = returningType.GetGenericArguments().FirstOrDefault();
                            if (memberType == null)
                            {
                                return null;
                            }
                        }
                    }

                    currentType = memberType.GetReturningType();
                    currentObject = memberType.GetReturningValue(currentObject);
                }
            }

            return currentObject;
        }

        public static MemberInfo GetMemberInfo(this SerializedProperty serializedProperty)
        {
            string propertyPath = serializedProperty.propertyPath.NormalizeUnityPath();
            string[] elements = propertyPath.Split('.');
            MemberInfo memberInfo = null;
            var currentType = serializedProperty.serializedObject.targetObject.GetType();

            foreach (var element in elements)
            {
                if (element[^1] == ']')
                {
                    var indexOf = element.IndexOf('[');
                    var name = element[..indexOf];
                    var listMemberInfo = currentType.GetMember(name);
                    if (listMemberInfo.Length != 1)
                    {
                        return null;
                    }

                    var currentMember = listMemberInfo[0];
                    Type listType = currentMember.GetReturningType();
                    currentType = listType.GetElementType() ?? listType.GetGenericArguments()[0];
                    memberInfo = currentMember.GetReturningType();
                }
                else
                {
                    var member = currentType.GetMember(element);
                    if (member.Length != 1)
                    {
                        return null;
                    }

                    var memberType = member.First();
                    if (element[^1] == ']')
                    {
                        var returningType = memberType.GetReturningType();
                        if (returningType.IsArray)
                        {
                            memberType = returningType.GetElementType();
                        }
                        else
                        {
                            memberType = returningType.GetGenericArguments().FirstOrDefault();
                            if (memberType == null)
                            {
                                return null;
                            }
                        }
                    }

                    currentType = memberType.GetReturningType();
                    memberInfo = memberType;
                }
            }

            return memberInfo;
        }

        private static Type GetReturningType(this MemberInfo memberInfo)
        {
            switch (memberInfo)
            {
                case FieldInfo fieldInfo:
                    return fieldInfo.FieldType;
                case PropertyInfo propertyInfo:
                    return propertyInfo.PropertyType;
                case MethodInfo methodInfo:
                    return methodInfo.ReturnType;
                case ConstructorInfo constructorInfo:
                    return constructorInfo.DeclaringType;
                case EventInfo:
                    return null;
                default:
                    throw new InvalidOperationException("MemberInfo is corrupted.");
            }
        }

        private static object GetReturningValue(this MemberInfo memberInfo, object target)
        {
            switch (memberInfo)
            {
                case FieldInfo fieldInfo:
                    return fieldInfo.GetValue(target);
                case PropertyInfo propertyInfo:
                    return propertyInfo.GetValue(target);
                case MethodInfo methodInfo:
                    return methodInfo.Invoke(target, Array.Empty<object>());
                case ConstructorInfo constructorInfo:
                    return constructorInfo.DeclaringType;
                case EventInfo:
                    return null;
                default:
                    throw new InvalidOperationException("MemberInfo is corrupted.");
            }
        }

        public static InspectorSerializedObjectMember GetInspector(this SerializedObject serializedObject, Object unityObject, InspectorMember parent, string pathName)
        {
            return new InspectorSerializedObjectMember(parent, unityObject, serializedObject, pathName);
        }

        public static IEnumerable<InspectorMember> GetInspectorChildren(this SerializedObject serializedObject, Object unityObject)
        {
            return GetInspectorChildren(serializedObject.targetObject, unityObject, null, serializedObject.GetChildren());
        }

        public static IEnumerable<InspectorMember> GetInspectorChildren(this object targetObject, Object unityObject, InspectorMember parent, IEnumerable<SerializedProperty> serializedProperties)
        {
            if (targetObject == null)
            {
                yield break;
            }

            var targetType = targetObject.GetType();
            if (targetObject is IList list)
            {
                using var enumerator = serializedProperties.GetEnumerator();

                for (int i = 0; i < list.Count; ++i)
                {
                    if (enumerator.MoveNext() == false)
                    {
                        Debug.Assert(false);
                    }

                    var lv = i;
                    yield return new InspectorSerializedFieldMember(
                        parent,
                        unityObject,
                        () => list[lv],
                        value => list[lv] = value,
                        enumerator.Current,
                        null,
                        $"[{lv}]"
                        );
                }
            }
            else
            {
                var dict = serializedProperties.ToDictionary(p => p.name, p => p);

                if (dict.TryGetValue("m_Script", out var scriptMember))
                {
                    yield return new InspectorScriptMember(parent, unityObject, scriptMember, "m_Script");
                }

                foreach (var memberInfo in targetType.ForEachMembers(null))
                {
                    if (dict.TryGetValue(memberInfo.Name, out var serializedProperty))
                    {
                        var fieldInfo = (FieldInfo)memberInfo;
                        yield return new InspectorSerializedFieldMember(
                            parent,
                            unityObject,
                            () => fieldInfo.GetValue(targetObject),
                            value => fieldInfo.SetValue(targetObject, value),
                            serializedProperty,
                            fieldInfo,
                            memberInfo.Name
                        );
                    }
                    else if (memberInfo.GetCustomAttribute<NonSerializeFieldAttribute>() != null)
                    {
                        if (memberInfo is FieldInfo fieldInfo)
                        {
                            yield return new InspectorNonSerializedFieldMember(
                                parent,
                                unityObject,
                                () => fieldInfo.GetValue(targetObject),
                                value => fieldInfo.SetValue(targetObject, value),
                                fieldInfo,
                                memberInfo.Name
                            );
                        }
                    }
                    else
                    {
                        var buttonAttribute = memberInfo.GetCustomAttribute<ButtonAttribute>();
                        if (buttonAttribute != null)
                        {
                            if (memberInfo is MethodInfo methodInfo)
                            {
                                yield return new InspectorButtonMember(
                                    parent,
                                    unityObject,
                                    methodInfo,
                                    memberInfo.Name,
                                    buttonAttribute
                                );
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<MemberInfo> ForEachMembers(this Type baseType, Action<Type> inTypeChangeCallback)
        {
            const BindingFlags bindingFlags
                = BindingFlags.Instance | BindingFlags.Static
                | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.GetField | BindingFlags.SetField
                | BindingFlags.GetProperty | BindingFlags.SetProperty
                | BindingFlags.InvokeMethod
                | BindingFlags.DeclaredOnly;

            while (baseType != typeof(object) && baseType != null)
            {
                inTypeChangeCallback?.Invoke(baseType);

                foreach (var member in baseType.GetMembers(bindingFlags))
                {
                    yield return member;
                }

                baseType = baseType.BaseType;
            }
        }

        public static IEnumerable<SerializedProperty> GetChildren(this SerializedObject serializedObject)
        {
            var iterator = serializedObject.GetIterator();
            if (iterator.Next(true) == false)
            {
                yield break;
            }

            do
            {
                yield return iterator.Copy();
            }
            while (iterator.Next(false));
        }

        public static bool IsList(this SerializedProperty serializedProperty)
        {
            return serializedProperty.isArray && serializedProperty.type != "string";
        }

        public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty serializedProperty)
        {
            if (serializedProperty.IsList())
            {
                for (int i = 0; i < serializedProperty.arraySize; ++i)
                {
                    yield return serializedProperty.GetArrayElementAtIndex(i).Copy();
                }
            }
            else
            {
                int level = serializedProperty.depth;
                var iterator = serializedProperty.Copy();
                if (iterator.Next(true) == false)
                {
                    yield break;
                }

                do
                {
                    if (level + 1 != iterator.depth)
                    {
                        yield break;
                    }

                    yield return iterator.Copy();
                }
                while (iterator.Next(false));
            }
        }
    }
}
