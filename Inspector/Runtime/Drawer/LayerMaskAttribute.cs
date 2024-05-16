using System;

namespace Avalon.Inspector.Drawer
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class LayerMaskAttribute : InspectorAttribute
    {
    }
}
