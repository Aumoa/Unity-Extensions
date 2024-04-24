using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ayla.Inspector.Members.Editor;
using Ayla.Inspector.Meta;
using Ayla.Inspector.SpecialCase;
using Ayla.Inspector.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Members
{
    public abstract class InspectorMember
    {
        private readonly InspectorMember m_Parent;
        private readonly Object m_UnityObject;
        private readonly Func<object> m_Getter;
        private readonly Action<object> m_Setter;
        private readonly MemberInfo m_MemberInfo;

        private Dictionary<string, InspectorMember> m_CachedSiblings;
        private GUIContent m_CachedLabel;

        protected InspectorMember(InspectorMember parent, Object unityObject, Func<object> getter, Action<object> setter, MemberInfo memberInfo, string pathName)
        {
            m_Parent = parent;
            m_UnityObject = unityObject;
            m_Getter = getter;
            m_Setter = setter;
            m_MemberInfo = memberInfo;
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
            return $"{propertyPath} ({m_Getter?.Invoke()})";
        }

        public abstract void OnGUI(Rect rect, GUIContent label);

        public abstract float GetHeight();

        public abstract InspectorMember[] GetChildren();

        public abstract ReorderableList GenerateReorderableList();

        public abstract Type GetMemberType();

        public MemberInfo GetMemberInfo()
        {
            return m_MemberInfo;
        }

        public InspectorMember GetParent()
        {
            return m_Parent;
        }

        public InspectorMember GetChild(string name)
        {
            return GetChildren().FirstOrDefault(p => p.name == name);
        }

        public Object GetUnityObject()
        {
            return m_UnityObject;
        }

        public object GetValue()
        {
            return m_Getter?.Invoke();
        }

        public void SetValue(object value)
        {
            if (m_Setter != null)
            {
                m_Setter.Invoke(value);
            }
        }

        public virtual void HandleOnValueChanged()
        {
            var attr = GetCustomAttribute<OnValueChangedAttribute>();
            if (attr != null)
            {
                object ownedObject = GetParent().GetValue();
                if (ownedObject != null)
                {
                    const BindingFlags k_BindingFlags =
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Instance | BindingFlags.Static |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.InvokeMethod;

                    foreach (var methodInfo in ownedObject.GetType().GetMethods(k_BindingFlags))
                    {
                        if (methodInfo.Name == attr.methodName && methodInfo.GetParameters().Length == 0)
                        {
                            methodInfo.Invoke(ownedObject, Array.Empty<object>());
                            return;
                        }
                    }
                }

                Debug.LogWarningFormat("Suitable callback method not found. MemberPath: {0}", propertyPath);
            }
        }

        public virtual Attribute[] GetCustomAttributes(Type type, bool inherit = false)
        {
            return (Attribute[])(m_MemberInfo?.GetCustomAttributes(type) ?? Array.Empty<Attribute>());
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

        public virtual bool isInline => HasCustomAttribute<InlineAttribute>();

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
                    return true;
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

        public virtual int indentLevel
        {
            get
            {
                int sum = 0;
                foreach (var indentAttribute in GetCustomAttributes<IndentAttribute>())
                {
                    sum += indentAttribute.indentLevel;
                }

                foreach (var indentIfAttribute in GetCustomAttributes<IndentIfAttribute>())
                {
                    if (IsConditionalPass(indentIfAttribute))
                    {
                        sum += indentIfAttribute.indentLevel;
                    }
                }

                return sum;
            }
        }

        public bool IsConditionalPass<T>(T attribute) where T : IIfAttribute
        {
            return IsConditionalPass(siblings, attribute);
        }

        private static bool IsConditionalPass(Dictionary<string, InspectorMember> siblings, IIfAttribute ifAttribute)
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
                m_CachedLabel ??= new GUIContent(displayName);
                return m_CachedLabel;
            }
        }

        public Dictionary<string, InspectorMember> siblings
        {
            get
            {
                if (m_CachedSiblings == null)
                {
                    m_CachedSiblings = new Dictionary<string, InspectorMember>();
                    foreach (var child in m_Parent?.GetChildren() ?? Array.Empty<InspectorMember>())
                    {
                        if (child is InspectorGroup group)
                        {
                            foreach (var groupChild in group.GetChildren())
                            {
                                m_CachedSiblings.TryAdd(groupChild.name, groupChild);
                            }
                        }
                        else
                        {
                            m_CachedSiblings.TryAdd(child.name, child);
                        }
                    }
                }
                
                return m_CachedSiblings;
            }
        }
    }
}
