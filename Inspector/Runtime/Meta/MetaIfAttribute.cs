namespace Ayla.Inspector.Meta
{
    public abstract class MetaIfAttribute : MetaAttribute
    {
        public string name { get; set; }

        public object value { get; set; }

        public Comparison comparison { get; set; }

        public bool inverted { get; protected set; }

        public MetaIfAttribute(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
