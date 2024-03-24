namespace Ayla.Inspector.Meta
{
    public abstract class InvertableIfAttribute : MetaIfAttribute
    {
        public bool inverted { get; protected set; }

        public InvertableIfAttribute(string name, object value) : base(name, value)
        {
        }
    }
}
