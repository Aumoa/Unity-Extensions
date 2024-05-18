using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class LayerMaskAttribute : InspectorAttribute
    {
    }
}
