using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Ayla.Inspector.Meta;
using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace Ayla.Inspector.Editor.Members
{
    public abstract class InspectorMember
    {
        private readonly MemberInfo memberInfo;

        protected InspectorMember(MemberInfo memberInfo, string propertyPath)
        {
            this.memberInfo = memberInfo;
            PropertyPath = propertyPath;
        }

        public abstract void OnGUI(Rect rect, GUIContent label);

        public abstract float GetHeight();

        public abstract IEnumerable<InspectorMember> GetChildren();

        public abstract ReorderableList GenerateReorderableList();

        public Attribute[] GetCustomAttributes(Type type)
        {
            return (Attribute[])(memberInfo?.GetCustomAttributes(type) ?? Array.Empty<Attribute>());
        }

        public T GetCustomAttribute<T>() where T : Attribute
        {
            return memberInfo?.GetCustomAttribute<T>();
        }

        public T[] GetCustomAttributes<T>() where T : Attribute
        {
            return memberInfo?.GetCustomAttributes<T>().ToArray() ?? Array.Empty<T>();
        }

        public abstract string Name { get; }

        public virtual string DisplayName => ObjectNames.NicifyVariableName(Name);

        public virtual string PropertyPath { get; }

        public abstract bool IsEditable { get; }

        public abstract bool IsExpanded { get; }

        public abstract bool IsList { get; }

        public virtual bool IsDisabled
        {
            get
            {
                var groups = GetCustomAttributes<LogicalGroupAttribute>()
                    .ToDictionary(p => p.GroupId, p => p);
                if (groups.ContainsKey(int.MinValue))
                {
                    throw new InvalidOperationException($"GroupId {int.MinValue} is not allowed.");
                }

                Dictionary<int, List<DisableIfAttribute>> groupAttributes = new();

                var attributes = GetCustomAttributes<DisableIfAttribute>();
                foreach (var attribute in attributes)
                {
                }
                return true;
            }
        }
    }
}
