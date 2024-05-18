namespace Ayla.Inspector
{
    public class HelpBoxIfAttribute : DecoratorAttribute, IIfAttribute
    {
        public HelpBoxType infoBoxType { get; }
        
        public string text { get; }
        
        public string name { get; }
        
        public object value { get; }
        
        public Comparison comparison { get; set; }

        public HelpBoxIfAttribute(HelpBoxType infoBoxType, string text, string name, object value)
        {
            this.infoBoxType = infoBoxType;
            this.text = text;
            this.name = name;
            this.value = value;
        }

        public HelpBoxIfAttribute(HelpBoxType infoBoxType, string text, string name) : this(infoBoxType, text, name, true)
        {
        }
    }
}