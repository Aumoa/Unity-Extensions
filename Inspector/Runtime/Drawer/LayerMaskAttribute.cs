using System;

namespace Ayla.Inspector.Drawer
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class LayerMaskAttribute : InspectorAttribute
    {
    }
}