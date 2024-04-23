using System;

namespace Ayla.Inspector.Drawer
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class MinMaxSliderAttribute : InspectorAttribute
    {
        public float minValue { get; }
        
        public float maxValue { get; }
        
        public MinMaxSliderAttribute(float minValue, float maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }
}