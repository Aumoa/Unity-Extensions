using Ayla.Numerics;

namespace Ayla.SlateCore
{
    public struct RenderTransform
    {
        private Matrix2x2 m;
        private Translate2D translation;

        public RenderTransform(in Translate2D translation)
        {
            m = Matrix2x2.identity;
            this.translation = translation;
        }

        public RenderTransform(in double uniformScale, in Translate2D translation)
            : this(Scale2D.Uniform(uniformScale), translation)
        {
        }

        public RenderTransform(in Scale2D scale, in Translate2D translation)
        {
            m = Matrix2x2Utility.Scale(scale);
            this.translation = translation;
        }

        public RenderTransform(in Shear2D shear, in Translate2D translation)
        {
            m = Matrix2x2Utility.Shear(shear);
            this.translation = translation;
        }

        public RenderTransform(in Complex rotation, in Translate2D translation)
        {
            m = Matrix2x2Utility.Rotation(rotation);
            this.translation = translation;
        }

        public RenderTransform(in Matrix2x2 transform, in Translate2D translation)
        {
            m = transform;
            this.translation = translation;
        }
    }
}