using System;
using System.Runtime.CompilerServices;
using Ayla.Numerics.Utility;

namespace Ayla.Numerics
{
    [Serializable]
    public struct Vector2D : IVector2, IEquatable<Vector2D>
    {
        public double x;
        public double y;

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
        public override readonly bool Equals(object rhs)
        {
            if (rhs is IVector2 v)
            {
                return Vector2Utility.NearlyEquals(this, v);
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Vector2D rhs)
        {
            return Vector2Utility.NearlyEquals(this, rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out double x, out double y)
        {
            x = this.x;
            y = this.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator +(in Vector2D lhs, in Vector2D rhs)
        {
            return Vector2Utility.Add(lhs, rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator -(in Vector2D lhs, in Vector2D rhs)
        {
            return Vector2Utility.Subtract(lhs, rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator -(in Vector2D lhs)
        {
            return Vector2Utility.Inverse(lhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator *(in Vector2D lhs, in Vector2D rhs)
        {
            return Vector2Utility.Multiply(lhs, rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator /(in Vector2D lhs, in Vector2D rhs)
        {
            return Vector2Utility.Divide(lhs, rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2D operator %(in Vector2D lhs, in Vector2D rhs)
        {
            return Vector2Utility.Mod(lhs, rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Vector2D lhs, in Vector2D rhs)
        {
            return Vector2Utility.NearlyEquals(lhs, rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Vector2D lhs, in Vector2D rhs)
        {
            return !Vector2Utility.NearlyEquals(lhs, rhs);
        }
    }
}