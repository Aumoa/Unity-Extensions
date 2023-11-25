using System;

namespace Ayla.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public class DisableIfAttribute : MetaIfAttribute
    {
        public DisableIfAttribute(string name, object value) : base(name, value)
        {
            Inverted = true;
        }
    }
}
