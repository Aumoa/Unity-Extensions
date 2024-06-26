﻿using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public class LabelAttribute : MetaAttribute
    {
        public string labelName { get; private set; }

        public LabelAttribute(string name = null)
        {
            labelName = name;
        }
    }
}
