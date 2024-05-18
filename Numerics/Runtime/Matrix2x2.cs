using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    public struct Matrix2x2 : IMatrix2x2
    {
        public double m00;
        public double m01;
        public double m10;
        public double m11;

        public static Matrix2x2 identity => Matrix2x2Utility.Make<Matrix2x2>(1, 0, 0, 1);

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
    }
}