namespace Ayla.Inspector.Meta
{
    public class MetaIfAttribute : MetaAttribute
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public Comparison Comparison { get; set; }

        public bool Inverted { get; protected set; }

        public MetaIfAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
