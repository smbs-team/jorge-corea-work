using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchControl
    {
        public List<SketchLevel> levels;

        public List<SketchLayer> layers;

        public List<SketchLayer> symbols;

        public Dictionary<string, string> metadata;

        public SketchParentObject FindParent(object item)
        {
            if (layers != null && layers.Count > 0)
            {
                for (var i = 0; i < layers.Count; i++)
                {
                    if (ReferenceEquals(layers[i], item))
                    {
                        return new SketchParentObject { parent = this, index = i };
                    }
                }
                for (var i = 0; i < layers.Count; i++)
                {
                    var parent = layers[i].FindParent(item);
                    if (parent != null)
                    {
                        return parent;
                    }
                }
            }
            return null;
        }

        public void GetBounds(SketchBounds bounds)
        {
            if (layers != null)
            {
                foreach (var layer in layers)
                {
                    layer.GetBounds(bounds, this);
                }
            }
        }

        public void ClosestPointTo(SketchPoint position, SketchPoint exclude, ref SketchClosestPoint closest)
        {
            if (layers != null)
            {
                foreach (var layer in layers)
                {
                    layer.ClosestPointTo(position, exclude, ref closest);
                }
            }
        }
    }
}
