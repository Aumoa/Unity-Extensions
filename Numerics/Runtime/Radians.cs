using System;
using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    [Serializable]
    public struct Radians : IRadians, IDegrees
    {
        private const double k_AngleHalf = Math.PI;
        private const double k_AngleMax = k_AngleHalf * 2.0;
        private const double k_180dPI = 180 / Math.PI;

        public double value;

        public Radians(double value)
        {
            this.value = value;
        }

        public readonly double radians
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => value;
        }

        public readonly double degrees
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => value * k_180dPI;
        }

        public readonly Radians normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var v = clamped;
                if (v.value > k_AngleHalf)
                {
                    return v.value - k_AngleMax;
                }
                else
                {
                    return v;
                }
            }
        }

        public readonly Radians clamped
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => value % k_AngleMax;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
        {
            return $"{value}rad";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NearlyEquals<T>(in T radians)
            where T : IRadians
        {
            return Math.Abs(radians.radians - value) <= double.Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Radians(double value)
        {
            return new Radians(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double(Radians value)
        {
            return value.value;
        }
    }
}