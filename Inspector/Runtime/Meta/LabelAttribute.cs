// Copyright 2020-2023 Aumoa.lib. All right reserved.

using System;

namespace Ayla.Inspector.Meta
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public class LabelAttribute : MetaAttribute
    {
        public string labelName { get; private set; }

        public LabelAttribute(string name)
        {
            labelName = name;
        }
    }
}
