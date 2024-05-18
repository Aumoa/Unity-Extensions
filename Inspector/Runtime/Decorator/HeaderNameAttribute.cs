namespace Ayla.Inspector
{
    public class HeaderNameAttribute : DecoratorAttribute
    {
        public string name { get; }
        
        public HeaderNameAttribute(string name)
        {
            this.name = name;
        }
    }
}