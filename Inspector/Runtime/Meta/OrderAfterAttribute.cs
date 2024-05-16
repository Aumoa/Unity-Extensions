namespace Avalon.Inspector.Meta
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
