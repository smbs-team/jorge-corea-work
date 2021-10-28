using System;
using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchDistance : SketchItemWithTextAttributes
    {
        public string path;

        public int element;

        public SketchPoint offset;

        public Dictionary<string, object> bag;

        public SketchDistance CreateCopy()
        {
            var copy = new SketchDistance
            {
                path = path,
                element = element
            };
            CopyTextAttributesTo(copy);
            if (offset != null)
            {
                copy.offset = new SketchPoint(offset);
            }
            return copy;
        }

        public void GetBounds(SketchBounds bounds, SketchObject sketchObject)
        {
            if (element >= 0 && offset != null && sketchObject != null && sketchObject.pathList != null)
            {
                foreach (var path in sketchObject.pathList)
                {
                    if (!string.IsNullOrWhiteSpace(path.uniqueIdentifier) && path.uniqueIdentifier.Equals(this.path))
                    {
                        if (path.elements != null && element < path.elements.Count)
                        {
                            double startX;
                            double startY;
                            if (element == 0)
                            {
                                startX = path.startPoint.X;
                                startY = path.startPoint.Y;
                            }
                            else
                            {
                                startX = path.elements[element - 1].point.X;
                                startY = path.elements[element - 1].point.Y;
                            }
                            var endX = path.elements[element].point.X;
                            var endY = path.elements[element].point.Y;
                            var curveData = new SketchCurveData();
                            curveData.CenterPointOfLine(startX, startY, endX, endY, path.elements[element].angle);
                            var positionX = curveData.centerInCurveX + offset.X;
                            var positionY = curveData.centerInCurveY + offset.Y;
                            if (bounds.started)
                            {
                                bounds.min.X = Math.Min(bounds.min.X, positionX);
                                bounds.min.Y = Math.Min(bounds.min.Y, positionY);
                                bounds.max.X = Math.Max(bounds.max.X, positionX);
                                bounds.max.Y = Math.Max(bounds.max.Y, positionY);
                            }
                            else
                            {
                                bounds.min.X = positionX;
                                bounds.min.Y = positionY;
                                bounds.max.X = positionX;
                                bounds.max.Y = positionY;
                                bounds.started = true;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
