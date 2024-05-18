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

        public static Matrix2x2 identity => Matrix2x2Utility.Make<Matrix2x2>(1, 0, 0, 1);

        public static Matrix2x2 Scale<T>(in T scale) where T : IVector2
        {
            return Matrix2x2Utility.Make<Matrix2x2>(
                scale.x, 0,
                0, scale.y
            );
        }

        public static Matrix2x2 Shear<T>(in T shear) where T : IVector2
        {
            return Matrix2x2Utility.Make<Matrix2x2>(
                1, shear.y,
                shear.x, 1
            );
        }

        public static Matrix2x2 Rotation(in Radians rad)
        {
            Mathd.SinCos(rad, out double s, out double c);

            return Matrix2x2Utility.Make<Matrix2x2>
            (
                c, -s,
                s, c
            );
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
    }
}