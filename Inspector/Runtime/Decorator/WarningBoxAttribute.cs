namespace Avalon.Inspector.Decorator
{
    public class WarningBoxAttribute : HelpBoxAttribute
    {
        public WarningBoxAttribute(string text) : base(HelpBoxType.Warning, text)
        {
        }
    }
}