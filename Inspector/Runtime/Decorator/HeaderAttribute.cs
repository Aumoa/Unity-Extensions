namespace Ayla.Inspector.Decorator
{
    public class HeaderAttribute : DecoratorAttribute
    {
        public string name { get; }
        
        public HeaderAttribute(string name)
        {
            this.name = name;
        }
    }
}