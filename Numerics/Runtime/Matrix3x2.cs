using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    public struct Matrix3x2 : IMatrix3x2
    {
        public double m00;
        public double m01;
        public double m10;
        public double m11;
        public double m20;
        public double m21;

        public Translate2D translation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector2.Make<Translate2D>(m20, m21);
        }

        public static Matrix3x2 identity => Identity<Matrix3x2>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 Scale<T>(in T scale) where T : IVector2
        {
            return Matrix2x2.Cast<Matrix3x2>.Do(Matrix2x2.Scale(scale));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 Shear<T>(in T shear) where T : IVector2
        {
            return Matrix2x2.Cast<Matrix3x2>.Do(Matrix2x2.Shear(shear));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 Rotation(in Complex complex)
        {
            return Matrix2x2.Cast<Matrix3x2>.Do(Matrix2x2.Rotation(complex));
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

        double IMatrix3x2.m20
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => m20;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => m20 = value;
        }

        double IMatrix3x2.m21
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => m21;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => m21 = value;
        }

        void IMatrix2x2.SetIdentity()
        {
            m00 = 1.0;
            m01 = 0.0;
            m10 = 0.0;
            m11 = 1.0;
            m20 = 0.0;
            m21 = 0.0;
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 Make(double p00, double p01, double p10, double p11, double p20, double p21)
        {
            return Make<Matrix3x2>(p00, p01, p10, p11, p20, p21);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Identity<T>() where T : struct, IMatrix3x2
        {
            return Matrix2x2.Identity<T>();
        }
    }
}