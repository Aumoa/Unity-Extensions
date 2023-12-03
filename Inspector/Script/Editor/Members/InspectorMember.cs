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
        private readonly InspectorMember parent;
        private readonly Func<object> currentObject;
        private readonly MemberInfo memberInfo;

        private GUIContent cachedLabel;

        protected InspectorMember(InspectorMember parent, Func<object> currentObject, MemberInfo memberInfo, string pathName)
        {
            this.parent = parent;
            this.currentObject = currentObject;
            this.memberInfo = memberInfo;
            if (parent == null)
            {
                PropertyPath = pathName;
            }
            else
            {
                PropertyPath = parent.PropertyPath + '.' + pathName;
            }
        }

        public override string ToString()
        {
            return $"{PropertyPath} ({currentObject})";
        }

        public abstract void OnGUI(Rect rect, GUIContent label);

        public abstract float GetHeight();

        public abstract IEnumerable<InspectorMember> GetChildren();

        public abstract ReorderableList GenerateReorderableList();

        public MemberInfo GetMemberInfo()
        {
            return memberInfo;
        }

        public InspectorMember GetParent()
        {
            return parent;
        }

        public object GetValue()
        {
            return currentObject?.Invoke();
        }

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
                if (GetCustomAttribute<ReadOnlyAttribute>() != null)
                {
                    return true;
                }

                var parent = GetParent();
                if (parent == null)
                {
                    return false;
                }

                var siblings = parent.GetChildren()
                    .GroupBy(p => p.Name)
                    .ToDictionary(p => p.Key, p => p.First());
                foreach (var disableIf in GetCustomAttributes<DisableIfAttribute>())
                {
                    if (siblings.TryGetValue(disableIf.Name, out var sibling))
                    {
                        switch (disableIf.Comparison)
                        {
                            case Comparison.Equals:
                                return sibling.GetValue()?.Equals(disableIf.Value) == true;
                        }
                    }
                }

                return false;
            }
        }

        public GUIContent Label
        {
            get
            {
                cachedLabel ??= new GUIContent(DisplayName);
                return cachedLabel;
            }
        }
    }
}
