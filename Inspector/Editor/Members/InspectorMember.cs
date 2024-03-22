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

        private Dictionary<string, InspectorMember> cachedSiblings;
        private GUIContent cachedLabel;

        protected InspectorMember(InspectorMember parent, Func<object> currentObject, MemberInfo memberInfo, string pathName)
        {
            this.parent = parent;
            this.currentObject = currentObject;
            this.memberInfo = memberInfo;
            if (parent == null)
            {
                propertyPath = pathName;
            }
            else
            {
                propertyPath = parent.propertyPath + '.' + pathName;
            }
        }

        public override string ToString()
        {
            return $"{propertyPath} ({currentObject})";
        }

        public abstract void OnGUI(Rect rect, GUIContent label, bool isLayout);

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

        public abstract string name { get; }

        public virtual string displayName
        {
            get
            {
                var attribute = GetCustomAttribute<LabelAttribute>();
                if (attribute != null)
                {
                    return attribute.labelName;
                }

                return ObjectNames.NicifyVariableName(name);
            }
        }

        public virtual string propertyPath { get; }

        public abstract bool isEditable { get; }

        public abstract bool isExpanded { get; }

        public abstract bool isList { get; }

        public virtual bool isEnabled
        {
            get
            {
                if (GetCustomAttribute<ReadOnlyAttribute>() != null)
                {
                    return false;
                }

                var parent = GetParent();
                if (parent == null)
                {
                    return false;
                }

                var activationAttrs = GetCustomAttributes<ActivationIfAttribute>();
                if (activationAttrs.Length == 0)
                {
                    return true;
                }

                foreach (var attr in activationAttrs)
                {
                    if (IsConditionalPass(siblings, attr) == !attr.inverted)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public virtual bool isVisible
        {
            get
            {
                if (GetCustomAttribute<HideInInspector>() != null)
                {
                    return false;
                }

                var visibleAttrs = GetCustomAttributes<VisibilityIfAttribute>();
                if (visibleAttrs.Any() == false)
                {
                    return true;
                }

                var parent = GetParent();
                if (parent == null)
                {
                    return false;
                }

                foreach (var attr in visibleAttrs)
                {
                    if (IsConditionalPass(siblings, attr) == !attr.inverted)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private static bool IsConditionalPass(Dictionary<string, InspectorMember> siblings, MetaIfAttribute ifAttribute)
        {
            if (siblings == null)
            {
                return true;
            }

            if (siblings.TryGetValue(ifAttribute.name, out var sibling))
            {
                object lValue = sibling.GetValue();
                object rValue = ifAttribute.value;

                if (lValue is IComparable comparable)
                {
                    switch (ifAttribute.comparison)
                    {
                        case Comparison.Equals:
                            if (comparable.CompareTo(rValue) == 0)
                            {
                                return true;
                            }
                            break;
                        case Comparison.NotEquals:
                            if (comparable.CompareTo(rValue) != 0)
                            {
                                return true;
                            }
                            break;
                        case Comparison.Greater:
                            if (comparable.CompareTo(rValue) > 0)
                            {
                                return true;
                            }
                            break;
                        case Comparison.GreaterEquals:
                            if (comparable.CompareTo(rValue) >= 0)
                            {
                                return true;
                            }
                            break;
                        case Comparison.Less:
                            if (comparable.CompareTo(rValue) < 0)
                            {
                                return true;
                            }
                            break;
                        case Comparison.LessEquals:
                            if (comparable.CompareTo(rValue) <= 0)
                            {
                                return true;
                            }
                            break;
                    }
                }
                else
                {
                    switch (ifAttribute.comparison)
                    {
                        case Comparison.Equals:
                            if (lValue?.Equals(rValue) == true)
                            {
                                return true;
                            }
                            break;
                        case Comparison.NotEquals:
                            if (lValue?.Equals(rValue) == false)
                            {
                                return true;
                            }
                            break;
                    }
                }

                if (lValue is IConvertible lConv && rValue is IConvertible rConv)
                {
                    switch (ifAttribute.comparison)
                    {
                        case Comparison.FlagContains:
                            if ((lConv.ToInt32(null) & rConv.ToInt32(null)) > 0)
                            {
                                return true;
                            }
                            break;
                        case Comparison.FlagNotContains:
                            if ((lConv.ToInt32(null) & rConv.ToInt32(null)) == 0)
                            {
                                return true;
                            }
                            break;
                    }
                }
            }

            return false;
        }

        public GUIContent label
        {
            get
            {
                cachedLabel ??= new GUIContent(displayName);
                return cachedLabel;
            }
        }

        public Dictionary<string, InspectorMember> siblings
        {
            get
            {
                cachedSiblings ??= parent?.GetChildren()
                    .GroupBy(p => p.name)
                    .ToDictionary(p => p.Key, p => p.First());
                return cachedSiblings;
            }
        }
    }
}
