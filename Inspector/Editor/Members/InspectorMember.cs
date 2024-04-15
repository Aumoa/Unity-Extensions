using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ayla.Inspector.Meta;
using Ayla.Inspector.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Members
{
    public abstract class InspectorMember
    {
        private readonly InspectorMember parent;
        private readonly Object unityObject;
        private readonly Func<object> getter;
        private readonly Action<object> setter;
        private readonly MemberInfo memberInfo;

        private Dictionary<string, InspectorMember> cachedSiblings;
        private GUIContent cachedLabel;

        protected InspectorMember(InspectorMember parent, Object unityObject, Func<object> getter, Action<object> setter, MemberInfo memberInfo, string pathName)
        {
            this.parent = parent;
            this.unityObject = unityObject;
            this.getter = getter;
            this.setter = setter;
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
            return $"{propertyPath} ({getter?.Invoke()})";
        }

        public abstract void OnGUI(Rect rect, GUIContent label);

        public abstract float GetHeight();

        public abstract IEnumerable<InspectorMember> GetChildren();

        public abstract ReorderableList GenerateReorderableList();

        public abstract Type GetMemberType();

        public MemberInfo GetMemberInfo()
        {
            return memberInfo;
        }

        public InspectorMember GetParent()
        {
            return parent;
        }

        public InspectorMember GetChild(string name)
        {
            return GetChildren().FirstOrDefault(p => p.name == name);
        }

        public Object GetUnityObject()
        {
            return unityObject;
        }

        public object GetValue()
        {
            return getter?.Invoke();
        }

        public void SetValue(object value)
        {
            if (setter != null)
            {
                setter.Invoke(value);
            }
        }

        public virtual Attribute[] GetCustomAttributes(Type type, bool inherit = false)
        {
            return (Attribute[])(memberInfo?.GetCustomAttributes(type) ?? Array.Empty<Attribute>());
        }

        public T GetCustomAttribute<T>(bool inherit = false) where T : Attribute
        {
            return GetCustomAttributes(typeof(T), inherit).OfType<T>().FirstOrDefault();
        }

        public T[] GetCustomAttributes<T>(bool inherit = false) where T : Attribute
        {
            return GetCustomAttributes(typeof(T), inherit).OfType<T>().ToArray();
        }

        public bool HasCustomAttribute<T>(bool inherit = false) where T : Attribute
        {
            return GetCustomAttribute<T>(inherit) != null;
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

        public abstract bool isExpanded { get; set; }

        public abstract bool isExpandable { get; }

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

        private static bool IsConditionalPass(Dictionary<string, InspectorMember> siblings, InvertableIfAttribute ifAttribute)
        {
            if (siblings == null)
            {
                return true;
            }

            object lValue;
            if (KnownPropertyNames.TryGetKnownProperty(ifAttribute.name, out lValue) == false)
            {
                if (siblings.TryGetValue(ifAttribute.name, out var sibling) == false)
                {
                    return false;
                }
                lValue = sibling.GetValue();
            }

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
