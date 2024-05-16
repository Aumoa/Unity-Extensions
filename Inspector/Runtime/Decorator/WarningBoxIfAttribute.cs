namespace Avalon.Inspector.Decorator
{
    public class WarningBoxIfAttribute : HelpBoxIfAttribute
    {
        public WarningBoxIfAttribute(string text, string name, object value) : base(HelpBoxType.Warning, text, name, value)
        {
        }
        
        public WarningBoxIfAttribute(string text, string name) : base(HelpBoxType.Warning, text, name, true)
        {
        }
    }
}