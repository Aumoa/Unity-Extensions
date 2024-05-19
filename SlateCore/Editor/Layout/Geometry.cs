using Ayla.Numerics;

namespace Ayla.SlateCore
{
    public struct Geometry
    {
        private Vector2 m_Size;
        private Scale2D m_Scale;
        private Translate2D m_AbsolutePosition;
        private Translate2D m_Position;
        private SlateRenderTransform m_AccumulatedRenderTransform;
        private bool m_HasRenderTransform;

        public Geometry(
            in Vector2 localSize,
            in SlateLayoutTransform localLayoutTransform,
            in SlateRenderTransform localRenderTransform,
            in Translate2D localRenderTransformPivot,
            in SlateLayoutTransform parentAccumulatedLayoutTransform,
            in SlateRenderTransform parentAccumulatedRenderTransform)
        {
            var accumulatedLayoutTransform = localLayoutTransform.Concatenate(parentAccumulatedLayoutTransform);
            var translation = accumulatedLayoutTransform.translation;
            var localSizeScale = Vector2.Cast<Scale2D>.Do(localSize);

            m_Size = localSize;
            m_Scale = accumulatedLayoutTransform.scale;
            m_AbsolutePosition = translation;
            m_Position = localLayoutTransform.translation;
            m_AccumulatedRenderTransform =
                localSizeScale.TransformPoint(localRenderTransformPivot).inverse
                .Concatenate(localRenderTransform)
                .Concatenate(localSizeScale.TransformPoint(localRenderTransformPivot))
                .Concatenate(localLayoutTransform)
                .Concatenate(parentAccumulatedRenderTransform);
            m_HasRenderTransform = true;
        }

        public Geometry(
            in Vector2 localSize,
            in SlateLayoutTransform localLayoutTransform,
            in SlateLayoutTransform parentAccumulatedLayoutTransform,
            in SlateRenderTransform parentAccumulatedRenderTransform,
            bool parentHasRenderTransform)
        {
            var accumulatedLayoutTransform = localLayoutTransform.Concatenate(parentAccumulatedLayoutTransform);
            var translation = accumulatedLayoutTransform.translation;
            m_Size = localSize;
            m_Scale = accumulatedLayoutTransform.scale;
            m_AbsolutePosition = translation;
            m_AccumulatedRenderTransform = localLayoutTransform.Concatenate(parentAccumulatedRenderTransform);
            m_Position = localLayoutTransform.translation;
            m_HasRenderTransform = parentHasRenderTransform;
        }
    }
}