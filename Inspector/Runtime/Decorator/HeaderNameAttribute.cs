namespace Ayla.Inspector.Decorator
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