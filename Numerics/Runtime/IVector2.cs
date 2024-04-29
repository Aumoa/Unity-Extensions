namespace Ayla.Numerics
{
    public interface IVector2
    {
        double x { get; set; }

        double y { get; set; }

        void Deconstruct(out double x, out double y);
    }
}