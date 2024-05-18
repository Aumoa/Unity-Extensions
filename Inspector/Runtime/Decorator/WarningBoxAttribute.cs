namespace Ayla.Inspector
{
    public class WarningBoxAttribute : HelpBoxAttribute
    {
        public WarningBoxAttribute(string text) : base(HelpBoxType.Warning, text)
        {
        }
    }
}