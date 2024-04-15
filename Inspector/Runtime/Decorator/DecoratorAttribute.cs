using System;

namespace Ayla.Inspector.Decorator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public class DecoratorAttribute : InspectorAttribute
    {
    }
}