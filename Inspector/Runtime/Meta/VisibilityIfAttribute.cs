﻿using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class VisibilityIfAttribute : InvertableIfAttribute
    {
        public VisibilityIfAttribute(string name, object value) : base(name, value)
        {
        }
    }
}
