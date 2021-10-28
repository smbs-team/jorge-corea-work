using System;
using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchPath
    {
        public string uniqueIdentifier;

        public SketchPoint startPoint;

        public bool? closed;

        public bool? negative;

        public List<SketchPathElements> elements;

        public Dictionary<string, object> bag;

        public double area;

        public SketchColor? fill;

        public string label;

        public bool? arrow;

        public SketchPath CreateCopy()
        {
            var copy = new SketchPath
            {
                uniqueIdentifier = uniqueIdentifier,
                closed = closed,
                negative = negative,
                area = area,
                label = label,
                arrow = arrow
            };
            if (startPoint != null)
            {
                copy.startPoint = new SketchPoint(startPoint);
            }
            if (elements != null)
            {
                copy.elements = new List<SketchPathElements>();
                foreach (var element in elements)
                {
                    var child = element.CreateCopy();
                    copy.elements.Add(child);
                }
            }
            if (fill != null)
            {
                copy.fill = new SketchColor(fill.Value);
            }
            return copy;
        }

        public void GetBounds(SketchBounds bounds)
        {
            if (startPoint != null)
            {
                if (bounds.started)
                {
                    bounds.min.X = Math.Min(bounds.min.X, startPoint.X);
                    bounds.min.Y = Math.Min(bounds.min.Y, startPoint.Y);
                    bounds.max.X = Math.Max(bounds.max.X, startPoint.X);
                    bounds.max.Y = Math.Max(bounds.max.Y, startPoint.Y);
                }
                else
                {
                    bounds.min.X = startPoint.X;
                    bounds.min.Y = startPoint.Y;
                    bounds.max.X = startPoint.X;
                    bounds.max.Y = startPoint.Y;
                    bounds.started = true;
                }
            }
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    if (element.point != null)
                    {
                        if (bounds.started)
                        {
                            bounds.min.X = Math.Min(bounds.min.X, element.point.X);
                            bounds.min.Y = Math.Min(bounds.min.Y, element.point.Y);
                            bounds.max.X = Math.Max(bounds.max.X, element.point.X);
                            bounds.max.Y = Math.Max(bounds.max.Y, element.point.Y);
                        }
                        else
                        {
                            bounds.min.X = element.point.X;
                            bounds.min.Y = element.point.Y;
                            bounds.max.X = element.point.X;
                            bounds.max.Y = element.point.Y;
                        }
                    }
                }
            }
        }

        public void FindProjection(ref SketchProjection projection)
        {
            if (startPoint != null)
            {
                projection.projected.Add(new SketchPoint(startPoint.X - projection.center.X, startPoint.Y - projection.center.Y));
            }
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    if (element.point != null)
                    {
                        projection.projected.Add(new SketchPoint(element.point.X - projection.center.X, element.point.Y - projection.center.Y));
                    }
                }
            }
        }

        public void FlipHorizontally(SketchProjection projection)
        {
            if (startPoint != null)
            {
                startPoint.X = projection.center.X - projection.projected[projection.index].X;
                projection.index++;
            }
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    if (element.point != null)
                    {
                        element.point.X = projection.center.X - projection.projected[projection.index].X;
                        if (element.angle != null)
                        {
                            element.angle = -element.angle;
                        }
                        projection.index++;
                    }
                }
            }
        }

        public void FlipVertically(SketchProjection projection)
        {
            if (startPoint != null)
            {
                startPoint.Y = projection.center.Y - projection.projected[projection.index].Y;
                projection.index++;
            }
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    if (element.point != null)
                    {
                        element.point.Y = projection.center.Y - projection.projected[projection.index].Y;
                        if (element.angle != null)
                        {
                            element.angle = -element.angle;
                        }
                        projection.index++;
                    }
                }
            }
        }

        public void RotateLeft(SketchProjection projection)
        {
            if (startPoint != null)
            {
                startPoint.X = projection.center.X - projection.projected[projection.index].Y;
                startPoint.Y = projection.center.Y + projection.projected[projection.index].X;
                projection.index++;
            }
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    if (element.point != null)
                    {
                        element.point.X = projection.center.X - projection.projected[projection.index].Y;
                        element.point.Y = projection.center.Y + projection.projected[projection.index].X;
                        projection.index++;
                    }
                }
            }
        }

        public void RotateRight(SketchProjection projection)
        {
            if (startPoint != null)
            {
                startPoint.X = projection.center.X + projection.projected[projection.index].Y;
                startPoint.Y = projection.center.Y - projection.projected[projection.index].X;
                projection.index++;
            }
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    if (element.point != null)
                    {
                        element.point.X = projection.center.X + projection.projected[projection.index].Y;
                        element.point.Y = projection.center.Y - projection.projected[projection.index].X;
                        projection.index++;
                    }
                }
            }
        }

        public void ClosestPointTo(SketchPoint position, SketchPoint exclude, ref SketchClosestPoint closest)
        {
            var excludeToCheck = exclude;
            var skipLastElementPoint = false;
            if (startPoint != null && elements != null && elements.Count > 0 && elements[elements.Count - 1].point != null)
            {
                if (ReferenceEquals(startPoint, exclude) || ReferenceEquals(elements[elements.Count - 1].point, exclude))
                {
                    if (Math.Abs(startPoint.X - elements[elements.Count - 1].point.X) < 1e-5 && Math.Abs(startPoint.Y - elements[elements.Count - 1].point.Y) < 1e-5)
                    {
                        skipLastElementPoint = true;
                        excludeToCheck = startPoint;
                    }
                }
            }
            if (startPoint != null && !ReferenceEquals(startPoint, excludeToCheck))
            {
                var deltaX = startPoint.X - position.X;
                var deltaY = startPoint.Y - position.Y;
                var distanceSquared = deltaX * deltaX + deltaY * deltaY;
                if (closest.distanceSquared == null || closest.distanceSquared.Value > distanceSquared)
                {
                    closest.position = startPoint;
                    closest.distanceSquared = distanceSquared;
                }
            }
            if (elements != null)
            {
                var count = elements.Count;
                if (skipLastElementPoint)
                {
                    count--;
                }
                for (var i = 0; i < count; i++)
                {
                    if (!ReferenceEquals(elements[i].point, excludeToCheck))
                    {
                        var deltaX = elements[i].point.X - position.X;
                        var deltaY = elements[i].point.Y - position.Y;
                        var distanceSquared = deltaX * deltaX + deltaY * deltaY;
                        if (closest.distanceSquared == null || closest.distanceSquared.Value > distanceSquared)
                        {
                            closest.position = elements[i].point;
                            closest.distanceSquared = distanceSquared;
                        }
                    }
                }
            }
        }

        public void Reverse()
        {
            if (elements != null && elements.Count > 0)
            {
                var oldStart = startPoint;
                var oldElements = elements;
                var index = oldElements.Count - 1;
                startPoint = new SketchPoint(oldElements[index].point);
                elements = new List<SketchPathElements>();
                while (index >= 0)
                {
                    var element = new SketchPathElements();
                    if (oldElements[index].angle != null)
                    {
                        element.angle = -oldElements[index].angle;
                    }
                    index--;
                    if (index >= 0)
                    {
                        element.point = new SketchPoint(oldElements[index].point);
                    }
                    else
                    {
                        element.point = new SketchPoint(oldStart);
                    }
                    elements.Add(element);
                }
            }
        }

        public SketchPathElements FindRelatedElement(SketchPathElements element, SketchPath relatedPath)
        {
            if (elements != null && relatedPath.elements != null)
            {
                for (var i = 0; i < elements.Count && i < relatedPath.elements.Count; i++)
                {
                    if (ReferenceEquals(elements[i], element))
                    {
                        return relatedPath.elements[i];
                    }
                }
            }
            return null;
        }

        public SketchPoint FindRelatedPoint(SketchPoint point, SketchPath relatedPath)
        {
            if (ReferenceEquals(startPoint, point))
            {
                return relatedPath.startPoint;
            }
            if (elements != null && relatedPath.elements != null)
            {
                for (var i = 0; i < elements.Count && i < relatedPath.elements.Count; i++)
                {
                    if (ReferenceEquals(elements[i].point, point))
                    {
                        return relatedPath.elements[i].point;
                    }
                }
            }
            return null;
        }
    }
}
