namespace Ayla.Inspector.Decorator
{
    public class SeparatorAttribute : DecoratorAttribute, IInheritableDecorator
    {
        public float space { get; }
        
        public float margin { get; }
        
        public bool forDerived { get; }
        
        public SeparatorAttribute(float space = 1, float margin = 1, bool forDerived = false, bool forHasAnyMember = false)
        {
            this.space = space;
            this.margin = margin;
            this.forDerived = forDerived;
        }
    }
}