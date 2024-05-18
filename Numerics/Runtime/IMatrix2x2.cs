namespace Ayla.Numerics
{
    public interface IMatrix2x2
    {
        double m00 { get; set; }
        double m01 { get; set; }
        double m10 { get; set; }
        double m11 { get; set; }

        void SetIdentity();
    }
}