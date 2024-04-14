using System;

namespace Ayla.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public class InlineAttribute : MetaAttribute
    {
        public InlineAttribute()
        {
        }
    }
}