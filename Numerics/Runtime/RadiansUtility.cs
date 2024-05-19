namespace Ayla.Numerics
{
    public static class RadiansUtility
    {
        public static Complex ToComplex<T>(this T rad) where T : IRadians
        {
            Mathd.SinCos(rad, out var s, out var c);
            return Vector2.Make<Complex>(c, -s);
        }
    }
}