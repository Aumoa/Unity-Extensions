using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public class ShowNativeMemberAttribute : MetaAttribute
    {
    }
}
