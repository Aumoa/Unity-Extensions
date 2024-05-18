using System.Runtime.CompilerServices;
using Ayla.Numerics;

namespace Ayla.SlateCore
{
    public struct LayoutTransform
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

        public readonly Vector2 TransformPoint<T>(in T rhs) where T : struct, IVector2
        {
            return translation.TransformPoint(scale.TransformPoint(rhs));
        }

        public readonly Matrix3x2 ToMatrix()
        {
            return Matrix3x2Utility.Make<Matrix3x2>(
                scale.x, 0,
                0, scale.y,
                translation.x, translation.y
            );
        }

        public readonly LayoutTransform Concatenate(in LayoutTransform rhs)
        {
            return Make(
                scale.Concatenate(rhs.scale),
                rhs.TransformPoint(translation)
            );
        }

        public static LayoutTransform Make<TScale, TTranslate>(in TScale scale, in TTranslate translate) where TScale : IVector2 where TTranslate : IVector2
        {
            return new LayoutTransform
            {
                scale = Vector2Utility.Cast<TScale, Scale2D>(scale),
                translation = Vector2Utility.Cast<TTranslate, Translate2D>(translate)
            };
        }
    }
}