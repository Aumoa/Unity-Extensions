using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
    public class BoxGroupAttribute : MetaAttribute
    {
        public string groupName { get; }
        
        public BoxGroupAttribute(string groupName)
        {
            this.groupName = groupName;
        }
    }
}