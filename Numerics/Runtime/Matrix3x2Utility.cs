using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    public static class Matrix3x2Utility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Make<T>(double p00, double p01, double p10, double p11, double p20, double p21)
            where T : struct, IMatrix3x2
        {
            var result = default(T);
            result.m00 = p00;
            result.m01 = p01;
            result.m10 = p10;
            result.m11 = p11;
            result.m20 = p20;
            result.m21 = p21;
            return result;
        }
    }
}