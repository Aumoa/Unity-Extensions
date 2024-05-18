namespace Ayla.Inspector
{
    public class OrderAfterAttribute : OrderAttribute
    {
        public readonly string memberName;

        public OrderAfterAttribute(string memberName)
        {
            this.memberName = memberName;
        }
    }
}
