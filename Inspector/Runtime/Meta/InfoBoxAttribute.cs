namespace Ayla.Inspector.Meta
{
    public class InfoBoxAttribute : MetaAttribute
    {
        public string message { get; }

        public InfoBoxAttribute(string message)
        {
            this.message = message;
        }
    }
}
