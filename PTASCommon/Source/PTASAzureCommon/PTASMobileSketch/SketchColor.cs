using System;

namespace PTASMobileSketch
{
    public struct SketchColor
    {
        public double R;

        public double G;

        public double B;

        public double A;

        public SketchColor(double r, double g, double b, double a = 1)
        {
            R = Math.Min(1, Math.Max(0, r));
            G = Math.Min(1, Math.Max(0, g));
            B = Math.Min(1, Math.Max(0, b));
            A = Math.Min(1, Math.Max(0, a));
        }

        public SketchColor(SketchColor color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }
    }
}
