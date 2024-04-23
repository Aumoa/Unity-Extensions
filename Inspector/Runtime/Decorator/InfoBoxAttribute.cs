namespace Ayla.Inspector.Decorator
{
    public class InfoBoxAttribute : HelpBoxAttribute
    {
        public InfoBoxAttribute(string text) : base(HelpBoxType.Info, text)
        {
        }
    }
}