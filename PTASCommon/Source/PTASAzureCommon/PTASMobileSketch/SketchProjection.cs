using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchProjection
    {
        public SketchBounds bounds;

        public readonly SketchPoint center = new SketchPoint();

        public readonly List<SketchPoint> projected = new List<SketchPoint>();

        public int index;
    }
}
