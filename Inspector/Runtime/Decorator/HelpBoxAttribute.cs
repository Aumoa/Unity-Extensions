namespace Ayla.Inspector.Decorator
{
    public class HelpBoxAttribute : DecoratorAttribute
    {
        public HelpBoxType infoBoxType { get; }
        
        public string text { get; }

        public HelpBoxAttribute(HelpBoxType infoBoxType, string text)
        {
            this.infoBoxType = infoBoxType;
            this.text = text;
        }
    }
}