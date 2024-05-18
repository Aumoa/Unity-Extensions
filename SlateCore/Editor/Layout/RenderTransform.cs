using Ayla.Numerics;

namespace Ayla.SlateCore
{
    public struct RenderTransform
    {
        private Matrix2x2 m_Matrix;
        private Translate2D m_Translate;

        public RenderTransform(in Translate2D translation)
        {
            m_Matrix = Matrix2x2.identity;
            this.m_Translate = translation;
        }

        public RenderTransform(in double uniformScale, in Translate2D translation)
            : this(Scale2D.Uniform(uniformScale), translation)
        {
        }

        public RenderTransform(in Scale2D scale, in Translate2D translation)
        {
            m_Matrix = Matrix2x2Utility.Scale(scale);
            this.m_Translate = translation;
        }

        public RenderTransform(in Shear2D shear, in Translate2D translation)
        {
            m_Matrix = Matrix2x2Utility.Shear(shear);
            this.m_Translate = translation;
        }

        public RenderTransform(in Complex rotation, in Translate2D translation)
        {
            m_Matrix = Matrix2x2Utility.Rotation(rotation);
            this.m_Translate = translation;
        }

        public RenderTransform(in Matrix2x2 transform, in Translate2D translation)
        {
            m_Matrix = transform;
            this.m_Translate = translation;
        }
    }
}