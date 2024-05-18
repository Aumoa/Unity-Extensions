using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class DecoratorAttribute : InspectorAttribute
    {
    }
}