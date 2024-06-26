﻿using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class ActivationIfAttribute : InvertableIfAttribute
    {
        public ActivationIfAttribute(string name, object value) : base(name, value)
        {
        }
    }
}
