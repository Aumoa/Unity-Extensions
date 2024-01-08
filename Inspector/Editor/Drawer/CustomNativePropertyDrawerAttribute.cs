// Copyright 2020-2023 Aumoa.lib. All right reserved.

using System;

namespace Ayla.Inspector.Editor.Drawer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CustomNativePropertyDrawerAttribute : Attribute
    {
        public CustomNativePropertyDrawerAttribute(Type targetType, bool useForChildren)
        {
            this.targetType = targetType;
            this.useForChildren = useForChildren;
        }

        public Type targetType { get; private set; }

        public bool useForChildren { get; private set; }
    }
}
