using System;

namespace Ayla.Inspector.SpecialCase
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomInspectorAttribute : SpecialCaseAttribute
    {
    }
}
