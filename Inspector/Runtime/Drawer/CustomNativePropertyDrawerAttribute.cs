using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CustomNativePropertyDrawerAttribute : Attribute
    {
        public CustomNativePropertyDrawerAttribute(Type targetType, bool useForChildren = false)
        {
            this.targetType = targetType;
            this.useForChildren = useForChildren;
        }

        public Type targetType { get; private set; }

        public bool useForChildren { get; private set; }
    }
}
