using System;

namespace Ayla.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public class LogicalGroupAttribute : MetaAttribute
    {
        public LogicalGroupAttribute()
        {
        }
    }
}
