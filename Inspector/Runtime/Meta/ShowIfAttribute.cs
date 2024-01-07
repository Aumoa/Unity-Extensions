using System;

namespace Ayla.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public class ShowIfAttribute : MetaIfAttribute
    {
        public ShowIfAttribute(string name, object value) : base(name, value)
        {
            Inverted = false;
        }
    }
}
