﻿namespace Ayla.Inspector
{
    public abstract class MetaIfAttribute : MetaAttribute, IIfAttribute
    {
        public string name { get; set; }

        public object value { get; set; }

        public Comparison comparison { get; set; }

        public MetaIfAttribute(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
