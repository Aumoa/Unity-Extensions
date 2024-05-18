using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    public static class Matrix2x2Utility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Make<T>(double p00, double p01, double p10, double p11)
            where T : struct, IMatrix2x2
        {
            var result = default(T);
            result.m00 = p00;
            result.m01 = p01;
            result.m10 = p10;
            result.m11 = p11;
            return result;
        }

        public static U Scale<T, U>(in T scale) where T : IVector2 where U : struct, IMatrix2x2
        {
            return Make<U>(
                scale.x, 0,
                0, scale.y
            );
        }

        public static Matrix2x2 Scale<T>(in T scale) where T : IVector2 => Scale<T, Matrix2x2>(scale);

        public static U Scale<U>(in Vector2 scale) where U : struct, IMatrix2x2 => Scale<Vector2, U>(scale);

        public static U Shear<T, U>(in T shear) where T : IVector2 where U : struct, IMatrix2x2
        {
            return Make<U>(
                1, shear.y,
                shear.x, 1
            );
        }

        public static Matrix2x2 Shear<T>(in T shear) where T : IVector2 => Shear<T, Matrix2x2>(shear);

        public static U Shear<U>(in Vector2 shear) where U : struct, IMatrix2x2 => Shear<Vector2, U>(shear);

        public static U Rotation<T, U>(in T complex) where T : IVector2 where U : struct, IMatrix2x2
        {
            return Make<U>
            (
                complex.x, complex.y,
                -complex.y, complex.x
            );
        }

        public static Matrix2x2 Rotation<T>(in T complex) where T : IVector2 => Rotation<T, Matrix2x2>(complex);

        public static U Rotation<U>(in Complex complex) where U : struct, IMatrix2x2 => Rotation<Complex, U>(complex);
    }
}