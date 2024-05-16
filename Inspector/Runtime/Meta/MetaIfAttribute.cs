using Avalon.Inspector.SpecialCase;

namespace Avalon.Inspector.Meta
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
