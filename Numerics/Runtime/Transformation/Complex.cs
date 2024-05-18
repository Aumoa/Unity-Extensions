using System;
using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    [Serializable]
    public struct Complex : IVector2, IEquatable<Complex>
    {
        public double x;
        public double y;

        public static Complex identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector2Utility.Make<Complex>(0, 1);
        }

        double IVector2.x
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => x;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => x = value;
        }

        double IVector2.y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => y;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => y = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object rhs) => Vector2Utility.Equals(this, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => Vector2Utility.GetHashCode(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Complex rhs) => Vector2Utility.NearlyEquals(this, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out double x, out double y) => Vector2Utility.Deconstruct(this, out x, out y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(in Complex lhs, in Complex rhs) => Vector2Utility.Add(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(in Complex lhs, in Complex rhs) => Vector2Utility.Subtract(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(in Complex lhs) => Vector2Utility.Inverse(lhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(in Complex lhs, in Complex rhs) => Vector2Utility.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(in Complex lhs, double rhs) => Vector2Utility.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(double lhs, in Complex rhs) => Vector2Utility.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(in Complex lhs, in Complex rhs) => Vector2Utility.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(in Complex lhs, double rhs) => Vector2Utility.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(double lhs, in Complex rhs) => Vector2Utility.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator %(in Complex lhs, in Complex rhs) => Vector2Utility.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator %(in Complex lhs, double rhs) => Vector2Utility.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator %(double lhs, in Complex rhs) => Vector2Utility.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Complex lhs, in Complex rhs) => Vector2Utility.NearlyEquals(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Complex lhs, in Complex rhs) => !Vector2Utility.NearlyEquals(lhs, rhs);
    }
}