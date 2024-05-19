using System;
using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    [Serializable]
    public struct Shear2D : IVector2, IEquatable<Shear2D>
    {
        public double x;
        public double y;

        public static Shear2D identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector2.Make<Shear2D>(0, 1);
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
        public readonly bool Equals(Shear2D rhs) => Vector2.NearlyEquals(this, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out double x, out double y) => Vector2.Deconstruct(this, out x, out y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator +(in Shear2D lhs, in Shear2D rhs) => Vector2.Add(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator -(in Shear2D lhs, in Shear2D rhs) => Vector2.Subtract(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator -(in Shear2D lhs) => Vector2.Inverse(lhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator *(in Shear2D lhs, in Shear2D rhs) => Vector2.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator *(in Shear2D lhs, double rhs) => Vector2.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator *(double lhs, in Shear2D rhs) => Vector2.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator /(in Shear2D lhs, in Shear2D rhs) => Vector2.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator /(in Shear2D lhs, double rhs) => Vector2.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator /(double lhs, in Shear2D rhs) => Vector2.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator %(in Shear2D lhs, in Shear2D rhs) => Vector2.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator %(in Shear2D lhs, double rhs) => Vector2.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Shear2D operator %(double lhs, in Shear2D rhs) => Vector2.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Shear2D lhs, in Shear2D rhs) => Vector2.NearlyEquals(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Shear2D lhs, in Shear2D rhs) => !Vector2.NearlyEquals(lhs, rhs);
    }
}