using System;

namespace Ayla.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class ActivationIfAttribute : MetaIfAttribute
    {
        public ActivationIfAttribute(string name, object value) : base(name, value)
        {
        }
    }
}
