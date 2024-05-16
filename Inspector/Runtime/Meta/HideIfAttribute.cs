using System;

namespace Avalon.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public class HideIfAttribute : VisibilityIfAttribute
    {
        public HideIfAttribute(string name, object value) : base(name, value)
        {
            inverted = true;
        }
        
        public HideIfAttribute(string name) : this(name, true)
        {
        }
    }
}
