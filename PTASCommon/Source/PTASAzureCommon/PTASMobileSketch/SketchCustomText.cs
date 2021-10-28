using System;
using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchCustomText : SketchItemWithTextAttributes
    {
        public string uniqueIdentifier;

        public string customText;

        public SketchPoint start;

        public string arrow;

        public Dictionary<string, object> bag;

        public SketchCustomText CreateCopy()
        {
            var copy = new SketchCustomText
            {
                uniqueIdentifier = uniqueIdentifier,
                customText = customText,
                arrow = arrow
            };
            CopyTextAttributesTo(copy);
            if (start != null)
            {
                copy.start = new SketchPoint(start);
            }
            return copy;
        }

        public void GetBounds(SketchBounds bounds)
        {
            if (start != null)
            {
                if (bounds.started)
                {
                    bounds.min.X = Math.Min(bounds.min.X, start.X);
                    bounds.min.Y = Math.Min(bounds.min.Y, start.Y);
                    bounds.max.X = Math.Max(bounds.max.X, start.X);
                    bounds.max.Y = Math.Max(bounds.max.Y, start.Y);
                }
                else
                {
                    bounds.min.X = start.X;
                    bounds.min.Y = start.Y;
                    bounds.max.X = start.X;
                    bounds.max.Y = start.Y;
                    bounds.started = true;
                }
            }
        }
    }
}
