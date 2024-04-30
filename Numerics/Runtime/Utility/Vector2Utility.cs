using System;
using System.Runtime.CompilerServices;

namespace Ayla.Numerics.Utility
{
    public static class Vector2Utility
    {
        public static class Cast<T> where T : struct, IVector2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T Do<U>(in U value)
                where U : IVector2
            {
                return Make<T>(value.x, value.y);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Make<T>(double x, double y)
            where T : struct, IVector2
        {
            var result = default(T);
            result.x = x;
            result.y = y;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Set<T>(ref this T value, double x, double y)
            where T : struct, IVector2
        {
            value.x = x;
            value.y = y;
            return ref value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Set<T>(this T value, double x, double y)
            where T : struct, IVector2
        {
            value.x = x;
            value.y = y;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(in T lhs, object rhs)
            where T : IVector2
        {
            if (rhs is IVector2 v)
            {
                return NearlyEquals(lhs, v);
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NearlyEquals<T1, T2>(in T1 lhs, in T2 rhs, double epsilon = 0.0001)
            where T1 : IVector2
            where T2 : IVector2
        {
            return Math.Abs(rhs.x - lhs.x) <= epsilon
                && Math.Abs(rhs.y - lhs.y) <= epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetHashCode<T>(in T value)
            where T : IVector2
        {
            return HashCode.Combine(value.x, value.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Deconstruct<T>(in T value, out double x, out double y)
        {
            x = value.x;
            y = value.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Cross<T1, T2>(in T1 lhs, in T2 rhs)
            where T1 : IVector2
            where T2 : IVector2
        {
            return lhs.x * rhs.y - lhs.y * rhs.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot<T1, T2>(in T1 lhs, in T2 rhs)
            where T1 : IVector2
            where T2 : IVector2
        {
            return lhs.x * rhs.x + lhs.y * rhs.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SqrMagnitude<T>(in T value)
            where T : IVector2
        {
            return value.x * value.x + value.y * value.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Magnitude<T>(in T value)
            where T : IVector2
        {
            return Math.Sqrt(SqrMagnitude(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SqrDistance<T1, T2>(in T1 lhs, T2 rhs)
            where T1 : IVector2
            where T2 : struct, IVector2
        {
            return SqrMagnitude(Subtract(rhs, lhs));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance<T1, T2>(in T1 lhs, T2 rhs)
            where T1 : IVector2
            where T2 : struct, IVector2
        {
            return Math.Sqrt(SqrDistance(lhs, rhs));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Add<T1, T2>(in T1 lhs, in T2 rhs)
            where T1 : struct, IVector2
            where T2 : IVector2
        {
            return Make<T1>(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Subtract<T1, T2>(in T1 lhs, in T2 rhs)
            where T1 : struct, IVector2
            where T2 : IVector2
        {
            return Make<T1>(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Inverse<T1>(in T1 lhs)
            where T1 : struct, IVector2
        {
            return Make<T1>(-lhs.x, -lhs.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Multiply<T1, T2>(in T1 lhs, in T2 rhs)
            where T1 : struct, IVector2
            where T2 : IVector2
        {
            return Make<T1>(lhs.x * rhs.x, lhs.y * rhs.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Multiply<T1>(in T1 lhs, double rhs)
            where T1 : struct, IVector2
        {
            return Make<T1>(lhs.x * rhs, lhs.y * rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Multiply<T1>(double lhs, in T1 rhs)
            where T1 : struct, IVector2
        {
            return Make<T1>(lhs * rhs.x, lhs * rhs.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Divide<T1, T2>(in T1 lhs, in T2 rhs)
            where T1 : struct, IVector2
            where T2 : IVector2
        {
            return Make<T1>(lhs.x / rhs.x, lhs.y / rhs.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Divide<T1>(in T1 lhs, double rhs)
            where T1 : struct, IVector2
        {
            return Make<T1>(lhs.x / rhs, lhs.y / rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Divide<T1>(double lhs, in T1 rhs)
            where T1 : struct, IVector2
        {
            return Make<T1>(lhs / rhs.x, lhs / rhs.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Mod<T1, T2>(in T1 lhs, in T2 rhs)
            where T1 : struct, IVector2
            where T2 : IVector2
        {
            return Make<T1>(lhs.x % rhs.x, lhs.y % rhs.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Mod<T1>(in T1 lhs, double rhs)
            where T1 : struct, IVector2
        {
            return Make<T1>(lhs.x % rhs, lhs.y % rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 Mod<T1>(double lhs, in T1 rhs)
            where T1 : struct, IVector2
        {
            return Make<T1>(lhs % rhs.x, lhs % rhs.y);
        }
    }
}