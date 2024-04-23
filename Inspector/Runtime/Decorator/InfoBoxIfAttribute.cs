namespace Ayla.Inspector.Decorator
{
    public class InfoBoxIfAttribute : HelpBoxIfAttribute
    {
        public InfoBoxIfAttribute(string text, string name, object value) : base(HelpBoxType.Info, text, name, value)
        {
        }
        
        public InfoBoxIfAttribute(string text, string name) : base(HelpBoxType.Info, text, name, true)
        {
        }
    }
}