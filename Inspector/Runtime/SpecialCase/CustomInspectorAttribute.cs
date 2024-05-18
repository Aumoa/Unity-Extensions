using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomInspectorAttribute : SpecialCaseAttribute
    {
    }
}
