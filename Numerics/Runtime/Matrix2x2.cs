using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    public struct Matrix2x2 : IMatrix2x2
    {
        public double m00;
        public double m01;
        public double m10;
        public double m11;

        public static Matrix2x2 identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Identity<Matrix2x2>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T TransformVector<T>(in T v) where T : struct, IVector2
        {
            return Multiply(this, v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T TransformPoint<T>(in T v) where T : struct, IVector2
        {
            return TransformVector(v);
        }

        double IMatrix2x2.m00
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => m00;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => m00 = value;
        }

        double IMatrix2x2.m01
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => m01;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => m01 = value;
        }

        double IMatrix2x2.m10
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => m10;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => m10 = value;
        }

        double IMatrix2x2.m11
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => m11;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => m11 = value;
        }

        void IMatrix2x2.SetIdentity()
        {
            m00 = 1.0;
            m01 = 0.0;
            m10 = 0.0;
            m11 = 1.0;
        }

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

        public static class Cast<T> where T : struct, IMatrix2x2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T Do<U>(in U value) where U : IMatrix2x2
            {
                return Make<T>(
                    value.m00, value.m01,
                    value.m10, value.m11
                );
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Identity<T>() where T : struct, IMatrix2x2
        {
            return Make<T>(1, 0, 0, 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static U Scale<T, U>(in T scale) where T : IVector2 where U : struct, IMatrix2x2
        {
            return Make<U>(
                scale.x, 0,
                0, scale.y
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 Scale<T>(in T scale) where T : IVector2 => Scale<T, Matrix2x2>(scale);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static U Scale<U>(in Vector2 scale) where U : struct, IMatrix2x2 => Scale<Vector2, U>(scale);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static U Shear<T, U>(in T shear) where T : IVector2 where U : struct, IMatrix2x2
        {
            return Make<U>(
                1, shear.y,
                shear.x, 1
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 Shear<T>(in T shear) where T : IVector2 => Shear<T, Matrix2x2>(shear);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static U Shear<U>(in Vector2 shear) where U : struct, IMatrix2x2 => Shear<Vector2, U>(shear);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static U Rotation<T, U>(in T complex) where T : IVector2 where U : struct, IMatrix2x2
        {
            return Make<U>
            (
                complex.x, complex.y,
                -complex.y, complex.x
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix2x2 Rotation<T>(in T complex) where T : IVector2 => Rotation<T, Matrix2x2>(complex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static U Rotation<U>(in Complex complex) where U : struct, IMatrix2x2 => Rotation<Complex, U>(complex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static U Multiply<T, U>(in T lhs, in U rhs) where T : IMatrix2x2 where U : struct, IMatrix2x2
        {
            return Make<U>(
                lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10, lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11,
                lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10, lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static U Multiply<T, U>(in T matrix, U vector) where T : IMatrix2x2 where U : struct, IVector2
        {
            return Vector2.Make<U>(
                matrix.m00 * vector.x + matrix.m01 * vector.y,
                matrix.m10 * vector.x + matrix.m11 * vector.y
            );
        }
    }
}