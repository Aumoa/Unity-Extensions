using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public class OrderAttribute : MetaAttribute
    {
    }
}
