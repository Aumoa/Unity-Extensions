using System;

namespace Avalon.Inspector.Decorator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class DecoratorAttribute : InspectorAttribute
    {
    }
}