using System;
using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    [Serializable]
    public struct Scale2D : IVector2, IEquatable<Scale2D>
    {
        public double x;
        public double y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T2 Concatenate<T1, T2>(in T1 scale, in T2 value)
            where T1 : IVector2
            where T2 : struct, IVector2
        {
            return Vector2Utility.Multiply(value, scale);
        }

        public static Scale2D identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector2Utility.Make<Scale2D>(0, 1);
        }

        public static Scale2D Uniform(double value)
        {
            return Vector2Utility.Make<Scale2D>(value, value);
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
        public readonly bool Equals(Scale2D rhs) => Vector2Utility.NearlyEquals(this, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out double x, out double y) => Vector2Utility.Deconstruct(this, out x, out y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator +(in Scale2D lhs, in Scale2D rhs) => Vector2Utility.Add(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator -(in Scale2D lhs, in Scale2D rhs) => Vector2Utility.Subtract(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator -(in Scale2D lhs) => Vector2Utility.Inverse(lhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator *(in Scale2D lhs, in Scale2D rhs) => Vector2Utility.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator *(in Scale2D lhs, double rhs) => Vector2Utility.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator *(double lhs, in Scale2D rhs) => Vector2Utility.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator /(in Scale2D lhs, in Scale2D rhs) => Vector2Utility.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator /(in Scale2D lhs, double rhs) => Vector2Utility.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator /(double lhs, in Scale2D rhs) => Vector2Utility.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator %(in Scale2D lhs, in Scale2D rhs) => Vector2Utility.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator %(in Scale2D lhs, double rhs) => Vector2Utility.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator %(double lhs, in Scale2D rhs) => Vector2Utility.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Scale2D lhs, in Scale2D rhs) => Vector2Utility.NearlyEquals(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Scale2D lhs, in Scale2D rhs) => !Vector2Utility.NearlyEquals(lhs, rhs);
    }
}