namespace Ayla.Inspector.SpecialCase
{
    public interface IIfAttribute
    {
        string name { get; }
        
        object value { get; }
        
        Comparison comparison { get; }
    }
}