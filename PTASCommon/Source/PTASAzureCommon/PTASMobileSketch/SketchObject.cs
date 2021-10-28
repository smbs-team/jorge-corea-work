using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchObject
    {
        public List<SketchDistance> distanceList;

        public List<SketchCustomText> customTextList;

        public List<SketchPath> pathList;

        public List<SketchCircle> circleList;

        public List<SketchSymbol> symbolList;

        public SketchObject CreateCopy()
        {
            var copy = new SketchObject();
            if (pathList != null)
            {
                copy.pathList = new List<SketchPath>();
                foreach (var path in pathList)
                {
                    var child = path.CreateCopy();
                    copy.pathList.Add(child);
                }
            }
            if (circleList != null)
            {
                copy.circleList = new List<SketchCircle>();
                foreach (var circle in circleList)
                {
                    var child = circle.CreateCopy();
                    copy.circleList.Add(child);
                }
            }
            if (customTextList != null)
            {
                copy.customTextList = new List<SketchCustomText>();
                foreach (var customText in customTextList)
                {
                    var child = customText.CreateCopy();
                    copy.customTextList.Add(child);
                }
            }
            if (distanceList != null)
            {
                copy.distanceList = new List<SketchDistance>();
                foreach (var distance in distanceList)
                {
                    var child = distance.CreateCopy();
                    copy.distanceList.Add(child);
                }
            }
            if (symbolList != null)
            {
                copy.symbolList = new List<SketchSymbol>();
                foreach (var symbol in symbolList)
                {
                    var child = symbol.CreateCopy();
                    copy.symbolList.Add(child);
                }
            }
            return copy;
        }

        public SketchParentObject FindParent(object item)
        {
            if (circleList != null && circleList.Count > 0)
            {
                for (var i = 0; i < circleList.Count; i++)
                {
                    if (ReferenceEquals(circleList[i], item))
                    {
                        return new SketchParentObject { parent = this, index = i };
                    }
                }
            }
            if (customTextList != null && customTextList.Count > 0)
            {
                for (var i = 0; i < customTextList.Count; i++)
                {
                    if (ReferenceEquals(customTextList[i], item))
                    {
                        return new SketchParentObject { parent = this, index = i };
                    }
                }
            }
            if (distanceList != null && distanceList.Count > 0)
            {
                for (var i = 0; i < distanceList.Count; i++)
                {
                    if (ReferenceEquals(distanceList[i], item))
                    {
                        return new SketchParentObject { parent = this, index = i };
                    }
                }
            }
            if (pathList != null && pathList.Count > 0)
            {
                for (var i = 0; i < pathList.Count; i++)
                {
                    if (ReferenceEquals(pathList[i], item))
                    {
                        return new SketchParentObject { parent = this, index = i };
                    }
                }
            }
            if (symbolList != null && symbolList.Count > 0)
            {
                for (var i = 0; i < symbolList.Count; i++)
                {
                    if (ReferenceEquals(symbolList[i], item))
                    {
                        return new SketchParentObject { parent = this, index = i };
                    }
                }
            }
            return null;
        }

        public void GetBounds(SketchBounds bounds, SketchControl sketch)
        {
            if (circleList != null)
            {
                foreach (var circle in circleList)
                {
                    circle.GetBounds(bounds);
                }
            }
            if (customTextList != null)
            {
                foreach (var customText in customTextList)
                {
                    customText.GetBounds(bounds);
                }
            }
            if (distanceList != null)
            {
                foreach (var distance in distanceList)
                {
                    distance.GetBounds(bounds, this);
                }
            }
            if (pathList != null)
            {
                foreach (var path in pathList)
                {
                    path.GetBounds(bounds);
                }
            }
            if (symbolList != null)
            {
                foreach (var symbol in symbolList)
                {
                    symbol.GetBounds(bounds, sketch);
                }
            }
        }

        public SketchPath FindRelatedPath(SketchPath path, SketchObject relatedSketchObject)
        {
            if (pathList != null && relatedSketchObject.pathList != null)
            {
                for (var i = 0; i < pathList.Count && i < relatedSketchObject.pathList.Count; i++)
                {
                    if (ReferenceEquals(pathList[i], path))
                    {
                        return relatedSketchObject.pathList[i];
                    }
                }
            }
            return null;
        }

        public void ClosestPointTo(SketchPoint position, SketchPoint exclude, ref SketchClosestPoint closest)
        {
            if (pathList != null)
            {
                foreach (var path in pathList)
                {
                    path.ClosestPointTo(position, exclude, ref closest);
                }
            }
        }

        public bool IsEmpty(SketchObject sketchObject)
        {
            var count = 0;
            if (sketchObject.circleList != null)
            {
                count += sketchObject.circleList.Count;
            }
            if (sketchObject.customTextList != null)
            {
                count += sketchObject.customTextList.Count;
            }
            if (sketchObject.distanceList != null)
            {
                count += sketchObject.distanceList.Count;
            }
            if (sketchObject.pathList != null)
            {
                count += sketchObject.pathList.Count;
            }
            if (sketchObject.symbolList != null)
            {
                count += sketchObject.symbolList.Count;
            }
            return (count > 0);
        }
    }
}
