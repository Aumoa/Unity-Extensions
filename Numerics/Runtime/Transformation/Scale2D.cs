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
        public readonly T TransformPoint<T>(in T rhs) where T : struct, IVector2
        {
            return Vector2.Cast<T>.Do(Vector2.Multiply(this, rhs));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T2 Concatenate<T1, T2>(in T1 scale, in T2 value)
            where T1 : IVector2
            where T2 : struct, IVector2
        {
            return Vector2.Multiply(value, scale);
        }

        public static Scale2D identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector2.Make<Scale2D>(0, 1);
        }

        public static Scale2D Uniform(double value)
        {
            return Vector2.Make<Scale2D>(value, value);
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
        public override readonly bool Equals(object rhs) => Vector2.Equals(this, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => Vector2.GetHashCode(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Scale2D rhs) => Vector2.NearlyEquals(this, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out double x, out double y) => Vector2.Deconstruct(this, out x, out y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator +(in Scale2D lhs, in Scale2D rhs) => Vector2.Add(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator -(in Scale2D lhs, in Scale2D rhs) => Vector2.Subtract(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator -(in Scale2D lhs) => Vector2.Inverse(lhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator *(in Scale2D lhs, in Scale2D rhs) => Vector2.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator *(in Scale2D lhs, double rhs) => Vector2.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator *(double lhs, in Scale2D rhs) => Vector2.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator /(in Scale2D lhs, in Scale2D rhs) => Vector2.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator /(in Scale2D lhs, double rhs) => Vector2.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator /(double lhs, in Scale2D rhs) => Vector2.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator %(in Scale2D lhs, in Scale2D rhs) => Vector2.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator %(in Scale2D lhs, double rhs) => Vector2.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Scale2D operator %(double lhs, in Scale2D rhs) => Vector2.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Scale2D lhs, in Scale2D rhs) => Vector2.NearlyEquals(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Scale2D lhs, in Scale2D rhs) => !Vector2.NearlyEquals(lhs, rhs);
    }
}