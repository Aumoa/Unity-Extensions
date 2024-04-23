namespace Ayla.Inspector.Decorator
{
    public class ErrorBoxAttribute : HelpBoxAttribute
    {
        public ErrorBoxAttribute(string text) : base(HelpBoxType.Error, text)
        {
        }
    }
}