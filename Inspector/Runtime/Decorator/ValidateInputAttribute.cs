namespace Ayla.Inspector
{
    public class ValidateInputAttribute : ErrorBoxIfAttribute
    {
        public ValidateInputAttribute(string name, string text) : base(text, name, false)
        {
        }
    }
}