using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchLayer
    {
        public string uniqueIdentifier;

        public string name;

        public List<SketchObject> objects;

        public bool? visible;

        public int? indexForSorting;

        public double netArea;

        public double grossArea;

        public string customTextForLabel;

        public bool? labelMoved;

        public SketchLayer CreateCopy()
        {
            var copy = new SketchLayer
            {
                uniqueIdentifier = uniqueIdentifier,
                name = name,
                indexForSorting = indexForSorting,
                visible = visible,
                netArea = netArea,
                grossArea = grossArea,
                customTextForLabel = customTextForLabel,
                labelMoved = labelMoved
            };
            if (objects != null)
            {
                copy.objects = new List<SketchObject>();
                foreach (var sketchObject in objects)
                {
                    var child = sketchObject.CreateCopy();
                    copy.objects.Add(child);
                }
            }
            return copy;
        }

        public SketchParentObject FindParent(object item)
        {
            if (objects != null && objects.Count > 0)
            {
                for (var i = 0; i < objects.Count; i++)
                {
                    if (ReferenceEquals(objects[i], item))
                    {
                        return new SketchParentObject { parent = this, index = i };
                    }
                }
                for (var i = 0; i < objects.Count; i++)
                {
                    var parent = objects[i].FindParent(item);
                    if (parent != null)
                    {
                        return parent;
                    }
                }
            }
            return null;
        }

        public void GetBounds(SketchBounds bounds, SketchControl sketch)
        {
            if (objects != null)
            {
                foreach (var sketchObject in objects)
                {
                    sketchObject.GetBounds(bounds, sketch);
                }
            }
        }

        public SketchObject FindRelatedSketchObject(SketchObject sketchObject, SketchLayer relatedLayer)
        {
            if (objects != null && relatedLayer.objects != null)
            {
                for (var i = 0; i < objects.Count && i < relatedLayer.objects.Count; i++)
                {
                    if (ReferenceEquals(objects[i], sketchObject))
                    {
                        return relatedLayer.objects[i];
                    }
                }
            }
            return null;
        }

        public void ClosestPointTo(SketchPoint position, SketchPoint exclude, ref SketchClosestPoint closest)
        {
            if (visible != null && visible.Value && objects != null)
            {
                foreach (var sketchObject in objects)
                {
                    sketchObject.ClosestPointTo(position, exclude, ref closest);
                }
            }
        }
    }
}
