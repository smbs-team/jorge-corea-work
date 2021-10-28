using System;
using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchCircle : SketchItemWithLineAttributes
    {
        public SketchPoint center;

        public double radius;

        public SketchCircle CreateCopy()
        {
            var copy = new SketchCircle
            {
                radius = radius
            };
            CopyLineAttributesTo(copy);
            if (center != null)
            {
                copy.center = new SketchPoint(center);
            }
            return copy;
        }

        public void GetBounds(SketchBounds bounds)
        {
            if (center != null)
            {
                if (bounds.started)
                {
                    bounds.min.X = Math.Min(bounds.min.X, center.X - radius);
                    bounds.min.Y = Math.Min(bounds.min.Y, center.Y - radius);
                    bounds.max.X = Math.Max(bounds.max.X, center.X + radius);
                    bounds.max.Y = Math.Max(bounds.max.Y, center.Y + radius);
                }
                else
                {
                    bounds.min.X = center.X - radius;
                    bounds.min.Y = center.Y - radius;
                    bounds.max.X = center.X + radius;
                    bounds.max.Y = center.Y + radius;
                    bounds.started = true;
                }
            }
        }
    }
}
