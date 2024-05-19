using System.Runtime.CompilerServices;
using Ayla.Numerics;

namespace Ayla.SlateCore
{
    public struct SlateLayoutTransform
    {
        public Scale2D scale
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public Translate2D translation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public override readonly string ToString()
        {
            return $"Translation: {translation}, Scale: {scale}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T TransformPoint<T>(in T rhs) where T : struct, IVector2
        {
            return Vector2.Cast<T>.Do(translation.TransformPoint(scale.TransformPoint(rhs)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Matrix3x2 ToMatrix()
        {
            return Matrix3x2.Make(
                scale.x, 0,
                0, scale.y,
                translation.x, translation.y
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly SlateLayoutTransform Concatenate(in SlateLayoutTransform rhs)
        {
            return Make(
                scale.Concatenate(rhs.scale),
                rhs.TransformPoint(translation)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly SlateRenderTransform Concatenate(in SlateRenderTransform rhs)
        {
            return new SlateRenderTransform(ToMatrix()).Concatenate(rhs);
        }

        public static SlateLayoutTransform Make<TScale, TTranslate>(in TScale scale, in TTranslate translate) where TScale : IVector2 where TTranslate : IVector2
        {
            return new SlateLayoutTransform
            {
                scale = Vector2.Cast<Scale2D>.Do(scale),
                translation = Vector2.Cast<Translate2D>.Do(translate)
            };
        }
    }
}