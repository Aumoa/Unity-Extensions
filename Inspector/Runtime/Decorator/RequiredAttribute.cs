using System.Runtime.CompilerServices;

namespace Avalon.Inspector.Decorator
{
    public class RequiredAttribute : ErrorBoxIfAttribute
    {
        public RequiredAttribute([CallerMemberName] string name = null) : base("Required member is not bound.", name, null)
        {
        }
    }
}