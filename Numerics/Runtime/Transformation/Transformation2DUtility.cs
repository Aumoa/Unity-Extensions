using System.Runtime.CompilerServices;

namespace Ayla.Numerics
{
    public static class Transformation2DUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Concatenate<T>(in Translate2D lhs, in T rhs)
            where T : struct, IVector2
        {
            var result = default(T);
            result.x = lhs.x + rhs.x;
            result.y = lhs.y + rhs.y;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Concatenate<T>(in Scale2D lhs, in T rhs)
            where T : struct, IVector2
        {
            var result = default(T);
            result.x = lhs.x * rhs.x;
            result.y = lhs.y * rhs.y;
            return result;
        }
    }
}