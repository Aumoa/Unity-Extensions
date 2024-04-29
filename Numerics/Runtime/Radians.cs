using System;
using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    [Serializable]
    public struct Radians
    {
        private const double k_AngleHalf = 3.1415926535892932384626433832795028841971963;
        private const double k_AngleMax = k_AngleHalf * 2.0;

        public double value;

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

        public Radians(double value)
        {
            this.value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
        {
            return $"{value}rad";
        }

        public static implicit operator Radians(double value)
        {
            return new Radians(value);
        }

        public static implicit operator double(Radians value)
        {
            return value.value;
        }
    }
}