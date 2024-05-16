using System;

namespace Avalon.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public class LabelAttribute : MetaAttribute
    {
        public string labelName { get; private set; }

        public LabelAttribute(string name = null)
        {
            labelName = name;
        }
    }
}
