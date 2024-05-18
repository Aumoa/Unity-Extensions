using System;
using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    [Serializable]
    public struct Translate2D : IVector2, IEquatable<Translate2D>
    {
        public double x;
        public double y;

        public static Translate2D identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector2Utility.Make<Translate2D>(0, 1);
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
        public readonly bool Equals(Translate2D rhs) => Vector2Utility.NearlyEquals(this, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out double x, out double y) => Vector2Utility.Deconstruct(this, out x, out y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator +(in Translate2D lhs, in Translate2D rhs) => Vector2Utility.Add(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator -(in Translate2D lhs, in Translate2D rhs) => Vector2Utility.Subtract(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator -(in Translate2D lhs) => Vector2Utility.Inverse(lhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator *(in Translate2D lhs, in Translate2D rhs) => Vector2Utility.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator *(in Translate2D lhs, double rhs) => Vector2Utility.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator *(double lhs, in Translate2D rhs) => Vector2Utility.Multiply(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator /(in Translate2D lhs, in Translate2D rhs) => Vector2Utility.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator /(in Translate2D lhs, double rhs) => Vector2Utility.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator /(double lhs, in Translate2D rhs) => Vector2Utility.Divide(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator %(in Translate2D lhs, in Translate2D rhs) => Vector2Utility.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator %(in Translate2D lhs, double rhs) => Vector2Utility.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Translate2D operator %(double lhs, in Translate2D rhs) => Vector2Utility.Mod(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Translate2D lhs, in Translate2D rhs) => Vector2Utility.NearlyEquals(lhs, rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Translate2D lhs, in Translate2D rhs) => !Vector2Utility.NearlyEquals(lhs, rhs);
    }
}