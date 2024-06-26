using System;
using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    [Serializable]
    public struct Degrees : IDegrees, IRadians
    {
        private const double k_AngleHalf = 180.0;
        private const double k_AngleMax = k_AngleHalf * 2.0;
        private const double k_PId180 = Math.PI / 180;

        public double value;

        public Degrees(double value)
        {
            this.value = value;
        }

        public readonly double degrees
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => value;
        }

        public readonly double radians
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => value * k_PId180;
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
            get
            {
                return value % k_AngleMax;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
        {
            return $"{value}��";
        }

        public static implicit operator Degrees(double value)
        {
            return new Degrees(value);
        }

        public static implicit operator double(Degrees value)
        {
            return value.value;
        }
    }
}