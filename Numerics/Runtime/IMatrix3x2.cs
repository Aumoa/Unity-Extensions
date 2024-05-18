namespace Ayla.Numerics
{
    public interface IMatrix3x2 : IMatrix2x2
    {
        double m20 { get; set; }
        double m21 { get; set; }
    }
}