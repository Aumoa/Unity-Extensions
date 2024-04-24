using System;

namespace Ayla.Inspector.Drawer
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public class InlineObjectAttribute : InspectorAttribute
    {
        public bool allowInlineObjects { get; }

        public InlineObjectAttribute(bool allowInlineObjects = true)
        {
            this.allowInlineObjects = allowInlineObjects;
        }
    }
}