using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchPathElements : SketchItemWithLineAttributes
    {
        public SketchPoint point;

        public double? angle;

        public SketchPathElements CreateCopy()
        {
            var copy = new SketchPathElements
            {
                angle = angle
            };
            CopyLineAttributesTo(copy);
            if (point != null)
            {
                copy.point = new SketchPoint(point);
            }
            return copy;
        }
    }
}
