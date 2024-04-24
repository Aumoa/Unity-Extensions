using System;

namespace Ayla.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class IndentAttribute : MetaAttribute
    {
        public int indentLevel { get; }
        
        public IndentAttribute(int indentLevel = 1)
        {
            this.indentLevel = indentLevel;
        }
    }
}