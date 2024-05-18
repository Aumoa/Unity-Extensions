using System;

namespace Ayla.Numerics
{
    public static class Mathd
    {
        public const double PI = Math.PI;

        private const double HalfPI = PI * 0.5;
        private const double InvPI = 1.0 / PI;

        public static void SinCos<T>(in T rad, out double sin, out double cos) where T : IRadians
        {
            // Map Value to y in [-pi,pi], x = 2*pi*quotient + remainder.
            double quotient = (InvPI * 0.5) * rad.radians;
            if (rad.radians >= 0.0)
            {
                quotient = (int)(quotient + 0.5);
            }
            else
            {
                quotient = (int)(quotient - 0.5);
            }
            double y = rad.radians - (2.0 * PI) * quotient;

            // Map y to [-pi/2,pi/2] with sin(y) = sin(Value).
            double sign;
            if (y > HalfPI)
            {
                y = PI - y;
                sign = -1.0;
            }
            else if (y < -HalfPI)
            {
                y = -PI - y;
                sign = -1.0;
            }
            else
            {
                sign = +1.0;
            }

            double y2 = y * y;

            // 11-degree minimax approximation
            sin = (((((-2.3889859e-08 * y2 + 2.7525562e-06) * y2 - 0.00019840874) * y2 + 0.0083333310) * y2 - 0.16666667) * y2 + 1.0) * y;

            // 10-degree minimax approximation
            double p = ((((-2.6051615e-07 * y2 + 2.4760495e-05) * y2 - 0.0013888378) * y2 + 0.041666638) * y2 - 0.5) * y2 + 1.0;
            cos = sign * p;
        }
    }
}