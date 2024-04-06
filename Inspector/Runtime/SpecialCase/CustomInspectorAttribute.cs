using System;

namespace Ayla.Inspector.SpecialCase
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomInspectorAttribute : SpecialCaseAttribute
    {
        public readonly string heightMemberName;

        public CustomInspectorAttribute(string heightMemberName)
        {
            this.heightMemberName = heightMemberName;
        }
    }
}
