using System;

namespace Avalon.Inspector.Drawer
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DropdownAttribute : InspectorAttribute
    {
        public string dropdownList { get; }

        public DropdownAttribute(string dropdownList)
        {
            this.dropdownList = dropdownList;
        }
    }
}
