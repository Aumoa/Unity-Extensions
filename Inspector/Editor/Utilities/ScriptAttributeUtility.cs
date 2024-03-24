using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ayla.Inspector.Editor.Drawer;
using UnityEditor;

namespace Ayla.Inspector.Editor.Utilities
{
    public static class ScriptAttributeUtility
    {
        private static readonly Dictionary<Type, (Type drawerType, bool useForChildren)> s_PropertyDrawerCache = new();
        private static readonly Dictionary<Type, (Type drawerType, bool useForChildren)> s_NativePropertyDrawerCache = new();

        [InitializeOnLoadMethod]
        private static void InitializeComponents()
        {
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
                            var isDrawer = type.GetCustomAttributes<CustomNativePropertyDrawerAttribute>();
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
        }

        public static PropertyDrawer InstantiatePropertyDrawer(Type targetType)
            => InternalInstantiateDrawer(s_PropertyDrawerCache, targetType) as PropertyDrawer;

        public static NativePropertyDrawer InstantiateNativePropertyDrawer(Type targetType)
            => InternalInstantiateDrawer(s_NativePropertyDrawerCache, targetType) as NativePropertyDrawer;

        private static object InternalInstantiateDrawer(Dictionary<Type, (Type drawerType, bool useForChildren)> drawerCache, Type targetType)
        {
            lock (drawerCache)
            {
                if (drawerCache.TryGetValue(targetType, out var cachedInfo) == false)
                {
                    Type drawerType = FindPropertyDrawerForChildren(drawerCache, targetType.BaseType);
                    cachedInfo.drawerType = drawerType;
                    cachedInfo.useForChildren = true;
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

        private static Type FindPropertyDrawerForChildren(Dictionary<Type, (Type drawerType, bool useForChildren)> drawerCache, Type targetType)
        {
            if (targetType == null)
            {
                return null;
            }

            if (drawerCache.TryGetValue(targetType, out var cachedInfo) == false)
            {
                return FindPropertyDrawerForChildren(drawerCache, targetType.BaseType);
            }

            return cachedInfo.drawerType;
        }
    }
}
