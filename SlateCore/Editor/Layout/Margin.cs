using System;
using Ayla.Numerics;

namespace Ayla.SlateCore
{
    public struct Margin
    {
        public double left;
        public double top;
        public double right;
        public double bottom;

        public Margin(double left, double top, double right, double bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public Margin(double horizontal, double vertical)
        {
            left = horizontal;
            top = vertical;
            right = horizontal;
            bottom = vertical;
        }

        public Margin(double scalar)
        {
            left = scalar;
            top = scalar;
            right = scalar;
            bottom = scalar;
        }

        public readonly double GetTotalSpaceAlong(Orientation orientation)
        {
            return orientation switch
            {
                Orientation.Horizontal => left + right,
                Orientation.Vertical => top + bottom,
                _ => throw new ArgumentException()
            };
        }

        public readonly Vector2 GetTotalSpaceAlong()
        {
            return new Vector2(left + right, top + bottom);
        }
    }
}