namespace Ayla.Inspector
{
    public class ErrorBoxAttribute : HelpBoxAttribute
    {
        public ErrorBoxAttribute(string text) : base(HelpBoxType.Error, text)
        {
        }
    }
}