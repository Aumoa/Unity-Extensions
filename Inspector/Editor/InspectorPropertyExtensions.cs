#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Ayla.Inspector
{
    public static class InspectorPropertyExtensions
    {
        public enum InspectorVisibility
        {
            None,
            NonSerializedField,
            PropertyField,
            Method
        }

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

        private static void SortInspectorMembers(List<InspectorMember> inoutMembers)
        {
            using (ListPool<(InspectorMember, OrderAttribute)>.Get(out var orders))
            {
                foreach (var member in inoutMembers)
                {
                    var attribute = member.GetCustomAttribute<OrderAttribute>();
                    if (attribute != null)
                    {
                        orders.Add((member, attribute));
                    }
                }

                foreach (var (member, order) in orders)
                {
                    if (order is OrderBeforeAttribute orderBefore)
                    {
                        inoutMembers.Remove(member);
                        int indexOf = inoutMembers.FindIndex(p => p.name == orderBefore.memberName);
                        if (indexOf != -1)
                        {
                            inoutMembers.Insert(indexOf, member);
                        }
                    }
                    else if (order is OrderAfterAttribute orderAfter)
                    {
                        inoutMembers.Remove(member);
                        int indexOf = inoutMembers.FindIndex(p => p.name == orderAfter.memberName);
                        if (indexOf != -1)
                        {
                            inoutMembers.Insert(indexOf + 1, member);
                        }
                    }
                }
            }
        }

        public static InspectorMember[] GetInspectorChildren(this object targetObject, Object unityObject, InspectorMember parent, IEnumerable<SerializedProperty> serializedProperties)
        {
            using (ListPool<InspectorMember>.Get(out var list))
            using (ListPool<(string name, List<InspectorMember> list)>.Get(out var groups))
            {
                InternalGetInspectorChildren(targetObject, unityObject, parent, serializedProperties, list);

                for (int i = 0; i < list.Count; ++i)
                {
                    var child = list[i];
                    var groupBoxAttr = child.GetCustomAttribute<BoxGroupAttribute>();
                    if (groupBoxAttr != null)
                    {
                        var indexOf = groups.FindIndex(p => p.name == groupBoxAttr.groupName);
                        if (indexOf == -1)
                        {
                            indexOf = groups.Count;
                            groups.Add((groupBoxAttr.groupName, ListPool<InspectorMember>.Get()));
                        }

                        var value = groups[indexOf];
                        value.list.Add(child);

                        list.RemoveAt(i--);
                    }
                }

                int insertIndex = 1;
                foreach (var group in groups)
                {
                    SortInspectorMembers(group.list);
                    var inspectorGroup = new InspectorGroup(parent, unityObject, group.name, group.name, group.list.ToArray());
                    list.Insert(insertIndex++, inspectorGroup);
                    ListPool<InspectorMember>.Release(group.list);
                }

                SortInspectorMembers(list);
                return list.ToArray();
            }
        }

        private static void InternalGetInspectorChildren(this object targetObject, Object unityObject, InspectorMember parent, IEnumerable<SerializedProperty> serializedProperties, List<InspectorMember> output)
        {
            if (targetObject == null)
            {
                return;
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
                    output.Add(new InspectorSerializedFieldMember(
                        parent,
                        unityObject,
                        () => list[lv],
                        value => list[lv] = value,
                        enumerator.Current,
                        null,
                        $"[{lv}]"
                    ));
                }
            }
            else
            {
                Dictionary<string, SerializedProperty> dict = new();
                foreach (var property in serializedProperties)
                {
                    dict.TryAdd(property.name, property);
                }

                if (dict.TryGetValue("m_Script", out var scriptMember))
                {
                    output.Add(new InspectorScriptMember(parent, unityObject, scriptMember, "m_Script"));
                }

                void HandleForMember(MemberInfo memberInfo)
                {
                    if (dict.TryGetValue(memberInfo.Name, out var serializedProperty))
                    {
                        var fieldInfo = (FieldInfo)memberInfo;
                        output.Add(new InspectorSerializedFieldMember(
                            parent,
                            unityObject,
                            () => fieldInfo.GetValue(targetObject),
                            value => fieldInfo.SetValue(targetObject, value),
                            serializedProperty,
                            fieldInfo,
                            memberInfo.Name
                        ));

                        return;
                    }

                    var buttonAttribute = memberInfo.GetCustomAttribute<ButtonAttribute>();
                    if (buttonAttribute != null)
                    {
                        if (memberInfo is MethodInfo methodInfo)
                        {
                            output.Add(new InspectorButtonMember(
                                parent,
                                unityObject,
                                methodInfo,
                                memberInfo.Name,
                                buttonAttribute
                            ));

                            return;
                        }
                    }

                    var customAttribute = memberInfo.GetCustomAttribute<CustomInspectorAttribute>();
                    if (customAttribute != null)
                    {
                        if (memberInfo is MethodInfo methodInfo)
                        {
                            output.Add(new InspectorCustomMember(
                                parent,
                                unityObject,
                                methodInfo,
                                methodInfo.Name
                            ));

                            return;
                        }
                    }

                    var inspectorVisibility = GetInspectorVisibility(memberInfo);
                    if (inspectorVisibility == InspectorVisibility.NonSerializedField)
                    {
                        if (memberInfo is FieldInfo fieldInfo)
                        {
                            output.Add(new InspectorNonSerializedFieldMember(
                                parent,
                                unityObject,
                                () => fieldInfo.GetValue(targetObject),
                                value => fieldInfo.SetValue(targetObject, value),
                                fieldInfo,
                                memberInfo.Name
                            ));
                        }
                    }

                    if (inspectorVisibility == InspectorVisibility.PropertyField)
                    {
                        if (memberInfo is PropertyInfo propertyInfo)
                        {
                            output.Add(new InspectorNativePropertyMember(
                                parent,
                                unityObject,
                                () => propertyInfo.GetValue(targetObject),
                                value => propertyInfo.SetValue(targetObject, value),
                                propertyInfo,
                                memberInfo.Name
                            ));
                        }
                    }

                    if (inspectorVisibility == InspectorVisibility.Method)
                    {
                        if (memberInfo is MethodInfo methodInfo)
                        {
                            output.Add(new InspectorNativeMethodMember(
                                parent,
                                unityObject,
                                () => methodInfo.Invoke(targetObject, Array.Empty<object>()),
                                methodInfo,
                                memberInfo.Name
                            ));
                        }
                    }
                }

                void HandleForClass(Type @class)
                {
                    output.Add(new InspectorClass(parent, unityObject, @class));
                }

                targetType.ForEachMembers(HandleForMember, HandleForClass);
            }
        }

        public static void ForEachMembers(this Type baseType, Action<MemberInfo> memberCallback, Action<Type> inTypeChangeCallback)
        {
            const BindingFlags bindingFlags
                = BindingFlags.Instance | BindingFlags.Static
                | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.GetField | BindingFlags.SetField
                | BindingFlags.GetProperty | BindingFlags.SetProperty
                | BindingFlags.InvokeMethod
                | BindingFlags.DeclaredOnly;

            bool IsNotInstrictTypeOrNull(Type type)
            {
                return type != null
                    && type != typeof(Object)
                    && type != typeof(object)
                    && type != typeof(ScriptableObject)
                    && type != typeof(Component)
                    && type != typeof(MonoBehaviour);
            }

            using (ListPool<Type>.Get(out var stack))
            {
                while (IsNotInstrictTypeOrNull(baseType))
                {
                    stack.Add(baseType);
                    baseType = baseType!.BaseType;
                }

                while (ListUtility.TryPop(stack, out var pop))
                {
                    inTypeChangeCallback?.Invoke(pop);
                    foreach (var member in pop.GetMembers(bindingFlags))
                    {
                        memberCallback.Invoke(member);
                    }
                }
            }
        }

        public static void GetChildren(this SerializedObject serializedObject, List<SerializedProperty> output)
        {
            var iterator = serializedObject.GetIterator();
            if (iterator.Next(true) == false)
            {
                return;
            }

            do
            {
                output.Add(iterator.Copy());
            }
            while (iterator.Next(false));
        }

        public static bool IsList(this SerializedProperty serializedProperty)
        {
            return serializedProperty.isArray && serializedProperty.type != "string";
        }

        public static void GetChildren(this SerializedProperty serializedProperty, List<SerializedProperty> output)
        {
            if (serializedProperty.IsList())
            {
                for (int i = 0; i < serializedProperty.arraySize; ++i)
                {
                    output.Add(serializedProperty.GetArrayElementAtIndex(i).Copy());
                }
            }
            else
            {
                int level = serializedProperty.depth;
                var iterator = serializedProperty.Copy();
                if (iterator.Next(true) == false)
                {
                    return;
                }

                do
                {
                    if (level + 1 != iterator.depth)
                    {
                        return;
                    }

                    output.Add(iterator.Copy());
                }
                while (iterator.Next(false));
            }
        }

        private static InspectorVisibility GetInspectorVisibility(MemberInfo memberInfo)
        {
            // If there is a SerializeField property but no SerializedProperty, the member is NonSerialized.
            if (memberInfo.GetCustomAttribute<SerializeField>() != null)
            {
                return InspectorVisibility.NonSerializedField;
            }

            if (memberInfo is PropertyInfo)
            {
                return InspectorVisibility.PropertyField;
            }

            if (memberInfo is MethodInfo methodInfo)
            {
                if (methodInfo.ReturnType != typeof(void) && methodInfo.GetParameters().Length == 0)
                {
                    return InspectorVisibility.Method;
                }
            }

            if (memberInfo.GetCustomAttribute<ShowNativeMemberAttribute>() != null)
            {
                return InspectorVisibility.NonSerializedField;
            }

            if (memberInfo is FieldInfo fieldInfo && fieldInfo.IsPublic && !fieldInfo.IsStatic && !fieldInfo.IsLiteral)
            {
                switch (Type.GetTypeCode(fieldInfo.FieldType))
                {
                    case TypeCode.Boolean:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.Int16:
                    case TypeCode.UInt32:
                    case TypeCode.Int32:
                    case TypeCode.UInt64:
                    case TypeCode.Int64:
                    case TypeCode.String:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return InspectorVisibility.NonSerializedField;
                }

                if (fieldInfo.FieldType.GetCustomAttribute<SerializableAttribute>() != null)
                {
                    return InspectorVisibility.NonSerializedField;
                }
            }

            return InspectorVisibility.None;
        }

        public static bool IsExpandable(this Type fieldType)
        {
            switch (Type.GetTypeCode(fieldType))
            {
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.Int16:
                case TypeCode.UInt32:
                case TypeCode.Int32:
                case TypeCode.UInt64:
                case TypeCode.Int64:
                case TypeCode.String:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return false;
            }

            return fieldType.GetCustomAttribute<SerializableAttribute>() != null;
        }
    }
}
#endif