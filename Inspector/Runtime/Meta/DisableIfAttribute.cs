using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public class DisableIfAttribute : ActivationIfAttribute
    {
        public DisableIfAttribute(string name, object value) : base(name, value)
        {
            inverted = true;
        }

        public DisableIfAttribute(string name) : this(name, true)
        {
        }
    }
}
