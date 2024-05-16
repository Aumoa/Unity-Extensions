namespace Avalon.Inspector.Decorator
{
    public class ErrorBoxIfAttribute : HelpBoxIfAttribute
    {
        public ErrorBoxIfAttribute(string text, string name, object value) : base(HelpBoxType.Error, text, name, value)
        {
        }

        public ErrorBoxIfAttribute(string text, string name) : this(text, name, true)
        {
        }
    }
}