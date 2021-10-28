namespace PTASMobileSketch
{
    public class SketchPoint
    {
        public double X;

        public double Y;

        public SketchPoint()
        {
        }

        public SketchPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public SketchPoint(SketchPoint point)
        {
            X = point.X;
            Y = point.Y;
        }
    }
}
