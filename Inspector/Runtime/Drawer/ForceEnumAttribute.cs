using System;

namespace Ayla.Inspector.Drawer
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public class ForceEnumAttribute : InspectorAttribute
    {
        public Type enumType { get; }

        public ForceEnumAttribute(Type enumType)
        {
            this.enumType = enumType;
        }
    }
}