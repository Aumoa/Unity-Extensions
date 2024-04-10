using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ayla.Inspector.Editor.Drawer;
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector.Editor.Utilities
{
    public static class ScriptAttributeUtility
    {
        private static bool s_Initialized;
        private static readonly Dictionary<Type, (Type drawerType, bool useForChildren)> s_PropertyDrawerCache = new();
        private static readonly Dictionary<Type, (Type drawerType, bool useForChildren)> s_NativePropertyDrawerCache = new();

        private static void InitializeComponents()
        {
            if (s_Initialized)
            {
                return;
            }

            lock (s_PropertyDrawerCache)
            lock (s_NativePropertyDrawerCache)
            {
                if (s_Initialized)
                {
                    return;
                }

                s_PropertyDrawerCache.Clear();
                s_NativePropertyDrawerCache.Clear();

                FieldInfo m_Type = typeof(CustomPropertyDrawer).GetField("m_Type", BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo m_UseForChildren = typeof(CustomPropertyDrawer).GetField("m_UseForChildren", BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        try
                        {
                            if (type == typeof(PropertyDrawer) || type.IsSubclassOf(typeof(PropertyDrawer)))
                            {
                                var isDrawer = type.GetCustomAttributes<CustomPropertyDrawer>();
                                if (isDrawer?.Any() == true)
                                {
                                    foreach (var drawerAttr in isDrawer)
                                    {
                                        var targetType = (Type)m_Type.GetValue(drawerAttr);
                                        var useForChildren = (bool)m_UseForChildren.GetValue(drawerAttr);
                                        s_PropertyDrawerCache.TryAdd(targetType, (type, useForChildren));
                                    }
                                }
                            }
                            else if (type == typeof(NativePropertyDrawer) || type.IsSubclassOf(typeof(NativePropertyDrawer)))
                            {
                                var isDrawer = type.GetCustomAttributes<CustomNativePropertyDrawerAttribute>(false);
                                if (isDrawer?.Any() == true)
                                {
                                    foreach (var drawerAttr in isDrawer)
                                    {
                                        var targetType = drawerAttr.targetType;
                                        var useForChildren = drawerAttr.useForChildren;
                                        s_NativePropertyDrawerCache.TryAdd(targetType, (type, useForChildren));
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }

#if UNITY_EDITOR
                AssemblyReloadEvents.beforeAssemblyReload += () =>
                {
                    lock (s_PropertyDrawerCache)
                    lock (s_NativePropertyDrawerCache)
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

        public static PropertyDrawer InstantiatePropertyDrawer(Type targetType)
            => InternalInstantiateDrawer(in s_PropertyDrawerCache, targetType) as PropertyDrawer;

        public static NativePropertyDrawer InstantiateNativePropertyDrawer(Type targetType)
            => InternalInstantiateDrawer(in s_NativePropertyDrawerCache, targetType) as NativePropertyDrawer;

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
