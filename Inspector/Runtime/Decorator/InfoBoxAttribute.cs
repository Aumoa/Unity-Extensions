namespace Ayla.Inspector
{
    public class InfoBoxAttribute : HelpBoxAttribute
    {
        public InfoBoxAttribute(string text) : base(HelpBoxType.Info, text)
        {
        }
    }
}