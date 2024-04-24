using System;
using System.Collections.Generic;
using System.Reflection;
using Ayla.Inspector.Drawer;
using Ayla.Inspector.Drawer.Editor;
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector.Utilities
{
    internal static class ScriptAttributeUtility
    {
        private static bool s_Initialized;

        private static readonly Dictionary<Type, (Type drawerType, bool useForChildren)> s_PropertyDrawerCache = new();
        private static readonly Dictionary<Type, (Type drawerType, bool useForChildren)> s_NativePropertyDrawerCache = new();
        private static readonly Dictionary<Type, (Type drawerType, bool useForChildren)> s_DecoratorDrawerCache = new();

        private static Action<DecoratorDrawer, PropertyAttribute> s_DecoratorDrawerAssign;

        private static void InitializeComponents()
        {
            if (s_Initialized)
            {
                return;
            }

            lock (s_PropertyDrawerCache)
            lock (s_NativePropertyDrawerCache)
            lock (s_DecoratorDrawerCache)
            {
                if (s_Initialized)
                {
                    return;
                }

                s_PropertyDrawerCache.Clear();
                s_NativePropertyDrawerCache.Clear();
                s_DecoratorDrawerCache.Clear();

                const BindingFlags k_BindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

                var m_Type = ReflectionUtility.GetField<CustomPropertyDrawer, Type>("m_Type", k_BindingFlags);
                var m_UseForChildren = ReflectionUtility.GetField<CustomPropertyDrawer, bool>("m_UseForChildren", k_BindingFlags);

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        try
                        {
                            if (type == typeof(PropertyDrawer) || type.IsSubclassOf(typeof(PropertyDrawer)))
                            {
                                var isDrawer = type.GetCustomAttributes<CustomPropertyDrawer>();
                                if (isDrawer != null)
                                {
                                    foreach (var drawerAttr in isDrawer)
                                    {
                                        var targetType = m_Type(drawerAttr);
                                        var useForChildren = m_UseForChildren(drawerAttr);
                                        s_PropertyDrawerCache.TryAdd(targetType, (type, useForChildren));
                                    }
                                }
                            }
                            else if (type == typeof(NativePropertyDrawer) || type.IsSubclassOf(typeof(NativePropertyDrawer)))
                            {
                                var isDrawer = type.GetCustomAttributes<CustomNativePropertyDrawerAttribute>(false);
                                if (isDrawer != null)
                                {
                                    foreach (var drawerAttr in isDrawer)
                                    {
                                        var targetType = drawerAttr.targetType;
                                        var useForChildren = drawerAttr.useForChildren;
                                        s_NativePropertyDrawerCache.TryAdd(targetType, (type, useForChildren));
                                    }
                                }
                            }
                            else if (type == typeof(DecoratorDrawer) || type.IsSubclassOf(typeof(DecoratorDrawer)))
                            {
                                var isDrawer = type.GetCustomAttributes<CustomPropertyDrawer>();
                                if (isDrawer != null)
                                {
                                    foreach (var drawerAttr in isDrawer)
                                    {
                                        var targetType = m_Type(drawerAttr);
                                        var useForChildren = m_UseForChildren(drawerAttr);
                                        s_DecoratorDrawerCache.TryAdd(targetType, (type, useForChildren));
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // ignore exception.
                        }
                    }
                }

                s_DecoratorDrawerAssign = ReflectionUtility.SetField<DecoratorDrawer, PropertyAttribute>("m_Attribute", k_BindingFlags);

#if UNITY_EDITOR
                AssemblyReloadEvents.beforeAssemblyReload += () =>
                {
                    lock (s_PropertyDrawerCache)
                    lock (s_NativePropertyDrawerCache)
                    lock (s_DecoratorDrawerCache)
                    {
                        s_PropertyDrawerCache.Clear();
                        s_NativePropertyDrawerCache.Clear();
                        s_Initialized = false;
                    }
                };
#endif

                s_Initialized = true;
            }
        }

        public static NativePropertyDrawer InstantiateNativePropertyDrawerForChain(Type targetType, IEnumerable<PropertyAttribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                if (InternalInstantiateDrawer(in s_NativePropertyDrawerCache, attribute.GetType()) is NativePropertyDrawer drawer)
                {
                    return drawer;
                }
            }

            {
                if (InternalInstantiateDrawer(in s_NativePropertyDrawerCache, targetType) is NativePropertyDrawer drawer)
                {
                    return drawer;
                }
            }

            return new NativePropertyDrawer();
        }

        public static DecoratorDrawer InstantiateDecoratorDrawer(PropertyAttribute propertyAttribute)
        {
            var drawer = InternalInstantiateDrawer(in s_DecoratorDrawerCache, propertyAttribute.GetType()) as DecoratorDrawer;
            if (drawer != null)
            {
                s_DecoratorDrawerAssign(drawer, propertyAttribute);
            }
            return drawer;
        }

        private static object InternalInstantiateDrawer(in Dictionary<Type, (Type drawerType, bool useForChildren)> drawerCache, Type targetType)
        {
            InitializeComponents();

            lock (drawerCache)
            {
                if (drawerCache.TryGetValue(targetType, out var cachedInfo) == false)
                {
                    cachedInfo = FindPropertyDrawerForChildren(drawerCache, targetType.BaseType);
                    drawerCache.Add(targetType, cachedInfo);
                }

                if (cachedInfo.drawerType == null)
                {
                    return null;
                }

                var ctor = cachedInfo.drawerType.GetConstructor(Array.Empty<Type>());
                if (ctor == null)
                {
                    drawerCache.Remove(targetType);
                    return null;
                }

                return ctor.Invoke(Array.Empty<object>());
            }
        }

        private static (Type drawerType, bool useForChildren) FindPropertyDrawerForChildren(Dictionary<Type, (Type drawerType, bool useForChildren)> drawerCache, Type targetType)
        {
            if (targetType == null)
            {
                return (null, false);
            }

            if (drawerCache.TryGetValue(targetType, out var cachedInfo) && cachedInfo.useForChildren)
            {
                return cachedInfo;
            }

            return FindPropertyDrawerForChildren(drawerCache, targetType.BaseType);
        }
    }
}
