namespace Ayla.Inspector.Meta
{
    public class IndentIfAttribute : MetaIfAttribute
    {
        public int indentLevel { get; }
        
        public IndentIfAttribute(string name, object value, int indentLevel = 1) : base(name, value)
        {
            this.indentLevel = indentLevel;
        }

        public IndentIfAttribute(string name, int indentLevel = 1) : this(name, true, indentLevel)
        {
        }
    }
}