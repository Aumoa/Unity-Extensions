using System;

namespace Ayla.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public class ShowIfAttribute : VisibilityIfAttribute
    {
        public ShowIfAttribute(string name, object value) : base(name, value)
        {
            inverted = false;
        }
    }
}
