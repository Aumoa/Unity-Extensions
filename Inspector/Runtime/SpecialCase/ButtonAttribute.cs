using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : SpecialCaseAttribute
    {
        public readonly string[] bindings;

        public float buttonHeight;
        
        public ButtonAttribute(params string[] bindings)
        {
            this.bindings = bindings;
        }
    }
}