using System;

namespace Avalon.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class OnValueChangedAttribute : MetaAttribute
    {
        public string methodName { get; }

        public OnValueChangedAttribute(string methodName)
        {
            this.methodName = methodName;
        }
    }
}