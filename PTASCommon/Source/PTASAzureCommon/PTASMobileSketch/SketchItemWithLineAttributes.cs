using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchItemWithLineAttributes
    {
        public string color;

        public List<double> pattern;

        public bool? worldUnits;

        public double? width;

        public void CopyLineAttributesTo(SketchItemWithLineAttributes target)
        {
            target.color = color;
            if (pattern != null)
            {
                target.pattern = new List<double>();
                target.pattern.AddRange(pattern);
            }
            else
            {
                target.pattern = null;
            }
            target.worldUnits = worldUnits;
            target.width = width;
        }
    }
}
