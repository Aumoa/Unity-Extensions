using System.Runtime.CompilerServices;
using Ayla.Numerics;

namespace Ayla.SlateCore
{
    public struct SlateRenderTransform
    {
        public Matrix2x2 matrix;
        public Translate2D translation;

        public SlateRenderTransform(in Translate2D translation)
        {
            matrix = Matrix2x2.identity;
            this.translation = translation;
        }

        public SlateRenderTransform(in double uniformScale, in Translate2D translation)
            : this(Scale2D.Uniform(uniformScale), translation)
        {
        }

        public SlateRenderTransform(in Scale2D scale, in Translate2D translation)
        {
            matrix = Matrix2x2.Scale(scale);
            this.translation = translation;
        }

        public SlateRenderTransform(in Shear2D shear, in Translate2D translation)
        {
            matrix = Matrix2x2.Shear(shear);
            this.translation = translation;
        }

        public SlateRenderTransform(in Complex rotation, in Translate2D translation)
        {
            matrix = Matrix2x2.Rotation(rotation);
            this.translation = translation;
        }

        public SlateRenderTransform(in Matrix2x2 transform, in Translate2D translation)
        {
            matrix = transform;
            this.translation = translation;
        }

        public SlateRenderTransform(in Matrix3x2 transform)
        {
            matrix = Matrix2x2.Cast<Matrix2x2>.Do(transform);
            translation = transform.translation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly SlateRenderTransform Concatenate(in SlateRenderTransform rhs)
        {
            return new SlateRenderTransform(
                Matrix2x2.Multiply(matrix, rhs.matrix),
                rhs.matrix.TransformVector(translation).Concatenate(rhs.translation)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly SlateRenderTransform Concatenate(in SlateLayoutTransform rhs)
        {
            return new SlateRenderTransform(
                Matrix2x2.Multiply(matrix, Matrix2x2.Scale(rhs.scale)),
                rhs.TransformPoint(translation)
            );
        }
    }
}