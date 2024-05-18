using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public class EnableIfAttribute : ActivationIfAttribute
    {
        public EnableIfAttribute(string name, object value) : base(name, value)
        {
            inverted = false;
        }

        public EnableIfAttribute(string name) : this(name, true)
        {
        }
    }
}
