using System.Runtime.CompilerServices;
using Ayla.Numerics;

namespace Ayla.SlateCore
{
    public static class TransformUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SlateRenderTransform Concatenate(this Translate2D translation, in SlateRenderTransform transform)
        {
            return new SlateRenderTransform(
                transform.matrix,
                transform.matrix.TransformPoint(translation).Concatenate(transform.translation)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SlateRenderTransform Concatenate(this SlateRenderTransform transform, in Translate2D translation)
        {
            return transform.Concatenate(new SlateRenderTransform(translation));
        }
    }
}