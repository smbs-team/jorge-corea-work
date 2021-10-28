using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace PTASMobileSketch
{
    public class SketchFromVCADD
    {
        private static void ReadEntity(XmlElement entity, Dictionary<int, string> palette, Dictionary<int, List<double>> linePatterns, Dictionary<int, bool> linePatternsInWorldUnits, SketchLayer layer)
        {
            var name = entity.GetAttribute("Name");
            if (name == "Line")
            {
                var points = entity.SelectNodes("Points/Point");
                var startPoint = (XmlElement)points[0];
                var endPoint = (XmlElement)points[1];
                if (double.TryParse(startPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var startX) &&
                    double.TryParse(startPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var startY) &&
                    double.TryParse(endPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var endX) &&
                    double.TryParse(endPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var endY))
                {
                    var path = new SketchPath
                    {
                        uniqueIdentifier = Guid.NewGuid().ToString()
                    };
                    path.startPoint = new SketchPoint { X = startX / 12, Y = startY / 12 };
                    var element = new SketchPathElements { point = new SketchPoint { X = endX / 12, Y = endY / 12 } };
                    path.elements = new List<SketchPathElements> { element };
                    var propertySettings = entity.SelectSingleNode("PropertySettings");
                    if (propertySettings != null)
                    {
                        var ea = propertySettings.SelectSingleNode("EA");
                        if (ea != null)
                        {
                            var eattr = (XmlElement)ea.SelectSingleNode("EAttr");
                            if (eattr != null)
                            {
                                var colorAsText = eattr.GetAttribute("Color");
                                var colorValue = int.Parse(colorAsText, CultureInfo.InvariantCulture);
                                if (palette.TryGetValue(colorValue, out var color))
                                {
                                    element.color = color;
                                }
                                var typeAsText = eattr.GetAttribute("Type");
                                var typeValue = int.Parse(typeAsText, CultureInfo.InvariantCulture);
                                if (linePatterns.TryGetValue(typeValue, out var pattern))
                                {
                                    element.pattern = pattern;
                                    element.worldUnits = linePatternsInWorldUnits[typeValue];
                                }
                                var widthAsText = eattr.GetAttribute("Width");
                                var widthAsDouble = double.Parse(widthAsText, CultureInfo.InvariantCulture);
                                if (widthAsDouble == 0)
                                {
                                    widthAsDouble = 1;
                                }
                                element.width = widthAsDouble;
                            }
                        }
                    }
                    var sketchObject = new SketchObject();
                    if (sketchObject.pathList == null)
                    {
                        sketchObject.pathList = new List<SketchPath>();
                    }
                    sketchObject.pathList.Add(path);
                    if (layer.objects == null)
                    {
                        layer.objects = new List<SketchObject>();
                    }
                    layer.objects.Add(sketchObject);
                }
            }
            else if (name == "ContinuousLine")
            {
                var points = entity.SelectNodes("Points/Point");
                if (points.Count > 0)
                {
                    string color = null;
                    List<double> pattern = null;
                    bool? worldUnits = null;
                    double? width = null;
                    var propertySettings = entity.SelectSingleNode("PropertySettings");
                    if (propertySettings != null)
                    {
                        var ea = propertySettings.SelectSingleNode("EA");
                        if (ea != null)
                        {
                            var eattr = (XmlElement)ea.SelectSingleNode("EAttr");
                            if (eattr != null)
                            {
                                var colorAsText = eattr.GetAttribute("Color");
                                var colorValue = int.Parse(colorAsText, CultureInfo.InvariantCulture);
                                palette.TryGetValue(colorValue, out color);
                                var typeAsText = eattr.GetAttribute("Type");
                                var typeValue = int.Parse(typeAsText, CultureInfo.InvariantCulture);
                                linePatterns.TryGetValue(typeValue, out pattern);
                                linePatternsInWorldUnits.TryGetValue(typeValue, out var worldUnitsValue);
                                worldUnits = worldUnitsValue;
                                var widthAsText = eattr.GetAttribute("Width");
                                var widthAsDouble = double.Parse(widthAsText, CultureInfo.InvariantCulture);
                                if (widthAsDouble == 0)
                                {
                                    widthAsDouble = 1;
                                }
                                width = widthAsDouble;
                            }
                        }
                    }
                    var pathArea = 0.0;
                    var traits = entity.SelectSingleNode("Traits");
                    if (traits != null)
                    {
                        var area = traits.SelectSingleNode("Area");
                        if (area != null)
                        {
                            var areaInnerText = area.InnerText;
                            pathArea = double.Parse(areaInnerText, CultureInfo.InvariantCulture) / 144;
                        }
                    }
                    var path = new SketchPath
                    {
                        uniqueIdentifier = Guid.NewGuid().ToString(),
                        area = pathArea
                    };
                    var pointIndex = 0;
                    var firstX = 0.0;
                    var firstY = 0.0;
                    var x = 0.0;
                    var y = 0.0;
                    foreach (XmlElement point in points)
                    {
                        if (double.TryParse(point.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out x) &&
                            double.TryParse(point.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out y))
                        {
                            if (pointIndex == 0)
                            {
                                path.startPoint = new SketchPoint { X = x / 12, Y = y / 12 };
                                firstX = x;
                                firstY = y;
                            }
                            else
                            {
                                var element = new SketchPathElements { point = new SketchPoint { X = x / 12, Y = y / 12 } };
                                element.color = color;
                                element.pattern = pattern;
                                element.worldUnits = worldUnits;
                                element.width = width;
                                if (path.elements == null)
                                {
                                    path.elements = new List<SketchPathElements>();
                                }
                                path.elements.Add(element);
                            }
                            pointIndex++;
                        }
                    }
                    var continuousSettings = entity.SelectSingleNode("ContinuousSettings");
                    if (continuousSettings != null)
                    {
                        var closed = continuousSettings.SelectSingleNode("Closed");
                        if (closed != null)
                        {
                            var closedInnerText = closed.InnerText;
                            var closedValue = int.Parse(closedInnerText, CultureInfo.InvariantCulture);
                            if (closedValue != 0 && path.elements != null && path.elements.Count >= 2)
                            {
                                if (Math.Abs(x - firstX) > 1E-5 || Math.Abs(y - firstY) > 1E-5)
                                {
                                    var element = new SketchPathElements { point = new SketchPoint { X = firstX / 12, Y = firstY / 12 } };
                                    var previousElement = path.elements[path.elements.Count - 1];
                                    element.color = previousElement.color;
                                    element.pattern = previousElement.pattern;
                                    element.worldUnits = previousElement.worldUnits;
                                    element.width = previousElement.width;
                                    path.elements.Add(element);
                                }
                                path.closed = true;
                            }
                        }
                    }
                    var sketchObject = new SketchObject();
                    if (sketchObject.pathList == null)
                    {
                        sketchObject.pathList = new List<SketchPath>();
                    }
                    sketchObject.pathList.Add(path);
                    if (layer.objects == null)
                    {
                        layer.objects = new List<SketchObject>();
                    }
                    layer.objects.Add(sketchObject);
                }
            }
            else if (name == "Arc")
            {
                var points = entity.SelectNodes("Points/Point");
                var startPoint = (XmlElement)points[0];
                var throughPoint = (XmlElement)points[1];
                var endPoint = (XmlElement)points[2];
                if (double.TryParse(startPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var startX) &&
                    double.TryParse(startPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var startY) &&
                    double.TryParse(throughPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var throughX) &&
                    double.TryParse(throughPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var throughY) &&
                    double.TryParse(endPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var endX) &&
                    double.TryParse(endPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var endY))
                {
                    var path = new SketchPath
                    {
                        uniqueIdentifier = Guid.NewGuid().ToString()
                    };
                    path.startPoint = new SketchPoint { X = startX / 12, Y = startY / 12 };
                    var startDeltaX = throughX - startX;
                    var startDeltaY = throughY - startY;
                    var endDeltaX = throughX - endX;
                    var endDeltaY = throughY - endY;
                    var angleCosine = (startDeltaX * endDeltaX + startDeltaY * endDeltaY) / (Math.Sqrt(startDeltaX * startDeltaX + startDeltaY * startDeltaY) * Math.Sqrt(endDeltaX * endDeltaX + endDeltaY * endDeltaY));
                    var angle = Math.PI - Math.Acos(angleCosine);
                    var deltaX = endX - startX;
                    var deltaY = endY - startY;
                    var length = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                    var directionX = -deltaY / length;
                    var directionY = deltaX / length;
                    var midX = (startX + endX) / 2;
                    var midY = (startY + endY) / 2;
                    deltaX = throughX - midX;
                    deltaY = throughY - midY;
                    var projection = deltaX * directionX + deltaY * directionY;
                    if (projection < 0)
                    {
                        angle = -angle;
                    }
                    var element = new SketchPathElements { point = new SketchPoint { X = endX / 12, Y = endY / 12 }, angle = Math.Round(2 * angle * 180 / Math.PI) };
                    path.elements = new List<SketchPathElements> { element };
                    var propertySettings = entity.SelectSingleNode("PropertySettings");
                    if (propertySettings != null)
                    {
                        var ea = propertySettings.SelectSingleNode("EA");
                        if (ea != null)
                        {
                            var eattr = (XmlElement)ea.SelectSingleNode("EAttr");
                            if (eattr != null)
                            {
                                var colorAsText = eattr.GetAttribute("Color");
                                var colorValue = int.Parse(colorAsText, CultureInfo.InvariantCulture);
                                if (palette.TryGetValue(colorValue, out var color))
                                {
                                    element.color = color;
                                }
                                var typeAsText = eattr.GetAttribute("Type");
                                var typeValue = int.Parse(typeAsText, CultureInfo.InvariantCulture);
                                if (linePatterns.TryGetValue(typeValue, out var pattern))
                                {
                                    element.pattern = pattern;
                                    element.worldUnits = linePatternsInWorldUnits[typeValue];
                                }
                                var widthAsText = eattr.GetAttribute("Width");
                                var widthAsDouble = double.Parse(widthAsText, CultureInfo.InvariantCulture);
                                if (widthAsDouble == 0)
                                {
                                    widthAsDouble = 1;
                                }
                                element.width = widthAsDouble;
                            }
                        }
                    }
                    var sketchObject = new SketchObject();
                    if (sketchObject.pathList == null)
                    {
                        sketchObject.pathList = new List<SketchPath>();
                    }
                    sketchObject.pathList.Add(path);
                    if (layer.objects == null)
                    {
                        layer.objects = new List<SketchObject>();
                    }
                    layer.objects.Add(sketchObject);
                }
            }
            else if (name == "Circle")
            {
                var points = entity.SelectNodes("Points/Point");
                var centerPoint = (XmlElement)points[0];
                var throughPoint = (XmlElement)points[1];
                if (double.TryParse(centerPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var centerX) &&
                    double.TryParse(centerPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var centerY) &&
                    double.TryParse(throughPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var throughX) &&
                    double.TryParse(throughPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var throughY))
                {
                    var circle = new SketchCircle();
                    circle.center = new SketchPoint { X = centerX / 12, Y = centerY / 12 };
                    var deltaX = throughX - centerX;
                    var deltaY = throughY - centerY;
                    var radius = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                    circle.radius = radius / 12;
                    var propertySettings = entity.SelectSingleNode("PropertySettings");
                    if (propertySettings != null)
                    {
                        var ea = propertySettings.SelectSingleNode("EA");
                        if (ea != null)
                        {
                            var eattr = (XmlElement)ea.SelectSingleNode("EAttr");
                            if (eattr != null)
                            {
                                var colorAsText = eattr.GetAttribute("Color");
                                var colorValue = int.Parse(colorAsText, CultureInfo.InvariantCulture);
                                if (palette.TryGetValue(colorValue, out var color))
                                {
                                    circle.color = color;
                                }
                                var typeAsText = eattr.GetAttribute("Type");
                                var typeValue = int.Parse(typeAsText, CultureInfo.InvariantCulture);
                                if (linePatterns.TryGetValue(typeValue, out var pattern))
                                {
                                    circle.pattern = pattern;
                                    circle.worldUnits = linePatternsInWorldUnits[typeValue];
                                }
                                var widthAsText = eattr.GetAttribute("Width");
                                var widthAsDouble = double.Parse(widthAsText, CultureInfo.InvariantCulture);
                                if (widthAsDouble == 0)
                                {
                                    widthAsDouble = 1;
                                }
                                circle.width = widthAsDouble;
                            }
                        }
                    }
                    var sketchObject = new SketchObject();
                    if (sketchObject.circleList == null)
                    {
                        sketchObject.circleList = new List<SketchCircle>();
                    }
                    sketchObject.circleList.Add(circle);
                    if (layer.objects == null)
                    {
                        layer.objects = new List<SketchObject>();
                    }
                    layer.objects.Add(sketchObject);
                }
            }
            else if (name == "Text")
            {
                var point = (XmlElement)entity.SelectSingleNode("Points/Point");
                if (double.TryParse(point.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var x) &&
                    double.TryParse(point.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var y))
                {
                    var stringElement = entity.SelectSingleNode("String");
                    var text = stringElement.InnerText;
                    var customText = new SketchCustomText
                    {
                        uniqueIdentifier = Guid.NewGuid().ToString(),
                        start = new SketchPoint { X = x / 12, Y = y / 12 },
                        customText = text
                    };
                    var textSettings = entity.SelectSingleNode("TextSettings");
                    if (textSettings != null)
                    {
                        var font = textSettings.SelectSingleNode("Font");
                        if (font != null)
                        {
                            customText.fontFamily = font.InnerText;
                        }
                        var height = textSettings.SelectSingleNode("Height");
                        if (height != null)
                        {
                            var heightInnerText = height.InnerText;
                            customText.fontSize = double.Parse(heightInnerText, CultureInfo.InvariantCulture) / 12;
                            customText.worldUnits = true;
                        }
                        var justify = textSettings.SelectSingleNode("Justify");
                        if (justify != null)
                        {
                            var justifyInnerText = justify.InnerText;
                            if (justifyInnerText == "76")
                            {
                                customText.horizontalAlign = SketchTextHorizontalAlign.Left;
                            }
                            else if (justifyInnerText == "67")
                            {
                                customText.horizontalAlign = SketchTextHorizontalAlign.Center;
                            }
                            else if (justifyInnerText == "82")
                            {
                                customText.horizontalAlign = SketchTextHorizontalAlign.Right;
                            }
                            customText.verticalAlign = SketchTextVerticalAlign.Baseline;
                        }
                        var rotation = textSettings.SelectSingleNode("Rotation");
                        if (rotation != null)
                        {
                            var rotationInnerText = rotation.InnerText;
                            var rotationValue = double.Parse(rotationInnerText, CultureInfo.InvariantCulture);
                            if (rotationValue != 0)
                            {
                                customText.rotation = -rotationValue;
                            }
                        }
                        var textColor = textSettings.SelectSingleNode("TextColor");
                        if (textColor != null)
                        {
                            var textColorInnerText = textColor.InnerText;
                            var textColorValue = int.Parse(textColorInnerText, CultureInfo.InvariantCulture);
                            if (palette.TryGetValue(textColorValue, out var color))
                            {
                                customText.fontColor = color;
                            }
                        }
                        var weight = textSettings.SelectSingleNode("Weight");
                        if (weight != null)
                        {
                            var weightInnerText = weight.InnerText;
                            customText.fontWeight = int.Parse(weightInnerText, CultureInfo.InvariantCulture);
                        }
                    }
                    var sketchObject = new SketchObject();
                    if (sketchObject.customTextList == null)
                    {
                        sketchObject.customTextList = new List<SketchCustomText>();
                    }
                    sketchObject.customTextList.Add(customText);
                    if (layer.objects == null)
                    {
                        layer.objects = new List<SketchObject>();
                    }
                    layer.objects.Add(sketchObject);
                }
            }
            else if (name == "Symbol")
            {
                var point = (XmlElement)entity.SelectSingleNode("Points/Point");
                if (double.TryParse(point.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var x) &&
                    double.TryParse(point.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var y))
                {
                    var symbolSettings = entity.SelectSingleNode("SymbolSettings");
                    var symbolName = symbolSettings.SelectSingleNode("Name");
                    var symbolNameAsText = symbolName.InnerText;
                    var symbol = new SketchSymbol { startPoint = new SketchPoint { X = x / 12, Y = y / 12 }, symbol = symbolNameAsText };
                    var rotation = symbolSettings.SelectSingleNode("Rotation");
                    if (rotation != null)
                    {
                        var rotationAsText = rotation.InnerText;
                        var rotationValue = double.Parse(rotationAsText, CultureInfo.InvariantCulture);
                        if (rotationValue != 0)
                        {
                            symbol.rotation = rotationValue;
                        }
                    }
                    var scale = symbolSettings.SelectSingleNode("Scale");
                    if (scale != null)
                    {
                        point = (XmlElement)scale.SelectSingleNode("Point");
                        if (double.TryParse(point.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out x) &&
                            double.TryParse(point.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out y))
                        {
                            if (x != 1 || y != 1)
                            {
                                symbol.scale = new SketchPoint { X = x, Y = y };
                            }
                        }
                    }
                    var sketchObject = new SketchObject();
                    if (sketchObject.symbolList == null)
                    {
                        sketchObject.symbolList = new List<SketchSymbol>();
                    }
                    sketchObject.symbolList.Add(symbol);
                    if (layer.objects == null)
                    {
                        layer.objects = new List<SketchObject>();
                    }
                    layer.objects.Add(sketchObject);
                }
            }
        }

        private static bool ReadDimLinear(XmlElement entity, SketchLayer layer, Dictionary<int, string> palette, double startX, double startY, double endX, double endY, double textX, double textY, List<string> dimLinearPaths, List<int> dimLinearPathElements)
        {
            if (layer.objects == null)
            {
                return false;
            }
            foreach (var sketchObject in layer.objects)
            {
                if (sketchObject.pathList == null)
                {
                    continue;
                }
                foreach (var path in sketchObject.pathList)
                {
                    if (path.elements == null)
                    {
                        continue;
                    }
                    var startInPathX = path.startPoint.X;
                    var startInPathY = path.startPoint.Y;
                    for (var i = 0; i < path.elements.Count; i++)
                    {
                        var finishInPathX = path.elements[i].point.X;
                        var finishInPathY = path.elements[i].point.Y;
                        if ((Math.Abs(startInPathX - startX) < 1E-5 && Math.Abs(startInPathY - startY) < 1E-5 && Math.Abs(finishInPathX - endX) < 1E-5 && Math.Abs(finishInPathY - endY) < 1E-5) || (Math.Abs(startInPathX - endX) < 1E-5 && Math.Abs(startInPathY - endY) < 1E-5 && Math.Abs(finishInPathX - startX) < 1E-5 && Math.Abs(finishInPathY - startY) < 1E-5))
                        {
                            var alreadyPresent = false;
                            for (var j = 0; j < dimLinearPaths.Count; j++)
                            {
                                if (dimLinearPaths[j] == path.uniqueIdentifier && dimLinearPathElements[j] == i)
                                {
                                    alreadyPresent = true;
                                    break;
                                }
                            }
                            if (!alreadyPresent)
                            {
                                var segmentX = finishInPathX - startInPathX;
                                var segmentY = finishInPathY - startInPathY;
                                var length = Math.Sqrt(segmentX * segmentX + segmentY * segmentY);
                                var directionX = segmentX / length;
                                var directionY = segmentY / length;
                                var sideX = -directionY;
                                var sideY = directionX;
                                var midX = (startInPathX + finishInPathX) / 2;
                                var midY = (startInPathY + finishInPathY) / 2;
                                var deltaX = textX - midX;
                                var deltaY = textY - midY;
                                var projection = deltaX * directionX + deltaY * directionY;
                                var sideProjection = deltaX * sideX + deltaY * sideY;
                                var distance = new SketchDistance { path = path.uniqueIdentifier, element = i, offset = new SketchPoint { X = projection, Y = sideProjection } };
                                var dimTextSettings = entity.SelectSingleNode("DimTextSettings");
                                if (dimTextSettings != null)
                                {
                                    var fontName = dimTextSettings.SelectSingleNode("FontName");
                                    if (fontName != null)
                                    {
                                        distance.fontFamily = fontName.InnerText;
                                    }
                                    var height = dimTextSettings.SelectSingleNode("THeight");
                                    if (height != null)
                                    {
                                        var heightInnerText = height.InnerText;
                                        distance.fontSize = double.Parse(heightInnerText, CultureInfo.InvariantCulture) / 12;
                                        distance.worldUnits = true;
                                    }
                                    var weight = dimTextSettings.SelectSingleNode("Weight");
                                    if (weight != null)
                                    {
                                        var weightInnerText = weight.InnerText;
                                        distance.fontWeight = int.Parse(weightInnerText, CultureInfo.InvariantCulture);
                                    }
                                }
                                var dimDisplaySettings = entity.SelectSingleNode("DimDisplaySettings");
                                if (dimDisplaySettings != null)
                                {
                                    var dimText = (XmlElement)dimDisplaySettings.SelectSingleNode("DimText");
                                    if (dimText != null)
                                    {
                                        var colorAsText = dimText.GetAttribute("Color");
                                        var colorValue = int.Parse(colorAsText, CultureInfo.InvariantCulture);
                                        if (palette.TryGetValue(colorValue, out var color))
                                        {
                                            distance.fontColor = color;
                                        }
                                    }
                                }
                                distance.worldUnits = true;
                                if (sketchObject.distanceList == null)
                                {
                                    sketchObject.distanceList = new List<SketchDistance>();
                                }
                                sketchObject.distanceList.Add(distance);
                                dimLinearPaths.Add(path.uniqueIdentifier);
                                dimLinearPathElements.Add(i);
                                return true;
                            }
                        }
                        startInPathX = finishInPathX;
                        startInPathY = finishInPathY;
                    }
                }
            }
            return false;
        }

        public static SketchControl Read(string xml)
        {
            var firstXMLCharacter = xml.IndexOf('<');
            if (firstXMLCharacter > 0)
            {
                xml = xml.Remove(0, firstXMLCharacter);
            }
            var document = new XmlDocument();
            document.LoadXml(xml);
            var drawing = document.SelectSingleNode("Drawing");
            Dictionary<int, string> palette = new Dictionary<int, string>();
            var colors = drawing.SelectNodes("ColorPaletteSettings/Color");
            foreach (XmlElement color in colors)
            {
                var index = int.Parse(color.GetAttribute("Index"), CultureInfo.InvariantCulture);
                if (index == 15)
                {
                    palette[index] = "black";
                }
                else
                {
                    var r = int.Parse(color.GetAttribute("Red"), CultureInfo.InvariantCulture);
                    var g = int.Parse(color.GetAttribute("Green"), CultureInfo.InvariantCulture);
                    var b = int.Parse(color.GetAttribute("Blue"), CultureInfo.InvariantCulture);
                    palette[index] = $"#{r:X2}{g:X2}{b:X2}";
                }
            }
            Dictionary<int, List<double>> linePatterns = new Dictionary<int, List<double>>();
            Dictionary<int, bool> linePatternsInWorldUnits = new Dictionary<int, bool>();
            var linetypes = drawing.SelectNodes("Linetypes/Linetype");
            foreach (XmlElement linetype in linetypes)
            {
                var index = int.Parse(linetype.GetAttribute("Index"), CultureInfo.InvariantCulture);
                var linetypeRef = linetype.GetAttribute("Ref");
                linePatternsInWorldUnits[index] = (linetypeRef == "World");
                var pattern = new List<double>();
                linePatterns[index] = pattern;
                var line = linetype.SelectSingleNode("Line");
                if (line != null)
                {
                    foreach (XmlElement entry in line.ChildNodes)
                    {
                        var entryAsDouble = double.Parse(entry.InnerText, CultureInfo.InvariantCulture);
                        if (entryAsDouble == 0)
                        {
                            entryAsDouble = 0.05;
                        }
                        pattern.Add(entryAsDouble);
                    }
                }
            }
            var entities = drawing.SelectNodes("Entities/Entity");
            var defaultLayer = new SketchLayer
            {
                uniqueIdentifier = Guid.NewGuid().ToString(),
                name = "Layer"
            };
            SketchControl sketch = new SketchControl();
            SketchLayer layer = null;
            foreach (XmlElement entity in entities)
            {
                var name = entity.GetAttribute("Name");
                layer = null;
                var addLayer = false;
                if (name == "Line" || name == "ContinuousLine" || name == "Arc" || name == "Circle" || name == "Text" || name == "Symbol")
                {
                    var propertySettings = entity.SelectSingleNode("PropertySettings");
                    if (propertySettings != null)
                    {
                        var ea = propertySettings.SelectSingleNode("EA");
                        if (ea != null)
                        {
                            var eattr = (XmlElement)ea.SelectSingleNode("EAttr");
                            if (eattr != null)
                            {
                                var layerName = eattr.GetAttribute("Layer");
                                if (!string.IsNullOrWhiteSpace(layerName))
                                {
                                    var found = false;
                                    var newLayerName = "Layer " + layerName;
                                    if (sketch.layers != null)
                                    {
                                        foreach (var entry in sketch.layers)
                                        {
                                            if (entry.name == newLayerName)
                                            {
                                                found = true;
                                                layer = entry;
                                            }
                                        }
                                    }
                                    if (!found)
                                    {
                                        var layerIndex = int.Parse(layerName, CultureInfo.InvariantCulture);
                                        layer = new SketchLayer
                                        {
                                            uniqueIdentifier = Guid.NewGuid().ToString(),
                                            name = newLayerName,
                                            visible = true,
                                            indexForSorting = layerIndex
                                        };
                                        addLayer = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if (layer == null)
                {
                    layer = defaultLayer;
                }
                ReadEntity(entity, palette, linePatterns, linePatternsInWorldUnits, layer);
                if (addLayer)
                {
                    if (sketch.layers == null)
                    {
                        sketch.layers = new List<SketchLayer>();
                    }
                    sketch.layers.Add(layer);
                }
            }
            List<string> dimLinearPaths = new List<string>();
            List<int> dimLinearPathElements = new List<int>();
            foreach (XmlElement entity in entities)
            {
                var name = entity.GetAttribute("Name");
                if (name == "DimLinear")
                {
                    var points = entity.SelectNodes("Points/Point");
                    var startPoint = (XmlElement)points[0];
                    var endPoint = (XmlElement)points[1];
                    var textPoint = (XmlElement)points[2];
                    if (double.TryParse(startPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var startX) &&
                        double.TryParse(startPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var startY) &&
                        double.TryParse(endPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var endX) &&
                        double.TryParse(endPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var endY) &&
                        double.TryParse(textPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var textX) &&
                        double.TryParse(textPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var textY))
                    {
                        if (sketch.layers != null)
                        {
                            foreach (var layerInSketch in sketch.layers)
                            {
                                if (ReadDimLinear(entity, layerInSketch, palette, startX, startY, endX, endY, textX, textY, dimLinearPaths, dimLinearPathElements))
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (sketch.layers != null)
            {
                sketch.layers.Sort((first, second) => first.indexForSorting.Value.CompareTo(second.indexForSorting.Value));
                foreach (var layerToUpdate in sketch.layers)
                {
                    layerToUpdate.indexForSorting = null;
                    if (sketch.levels == null)
                    {
                        sketch.levels = new List<SketchLevel>();
                    }
                    if (sketch.levels.Count == 0)
                    {
                        sketch.levels.Add(new SketchLevel
                        {
                            uniqueIdentifier = Guid.NewGuid().ToString(),
                            name = "Level 1",
                            visible = true,
                            layers = new List<SketchLevelLayer>()
                        });
                    }
                    sketch.levels[0].layers.Add(new SketchLevelLayer
                    {
                        uniqueIdentifier = layerToUpdate.uniqueIdentifier
                    });
                }
            }
            var symbols = drawing.SelectNodes("Symbols/Symbol");
            foreach (XmlElement symbol in symbols)
            {
                var name = symbol.GetAttribute("Name");
                layer = new SketchLayer
                {
                    uniqueIdentifier = Guid.NewGuid().ToString(),
                    name = name,
                    visible = true
                };
                entities = symbol.SelectNodes("Entities/Entity");
                foreach (XmlElement entity in entities)
                {
                    ReadEntity(entity, palette, linePatterns, linePatternsInWorldUnits, layer);
                }
                dimLinearPaths = new List<string>();
                dimLinearPathElements = new List<int>();
                foreach (XmlElement entity in entities)
                {
                    name = entity.GetAttribute("Name");
                    if (name == "DimLinear")
                    {
                        var points = entity.SelectNodes("Points/Point");
                        var startPoint = (XmlElement)points[0];
                        var endPoint = (XmlElement)points[1];
                        var textPoint = (XmlElement)points[2];
                        if (double.TryParse(startPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var startX) &&
                            double.TryParse(startPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var startY) &&
                            double.TryParse(endPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var endX) &&
                            double.TryParse(endPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var endY) &&
                            double.TryParse(textPoint.GetAttribute("x"), NumberStyles.Any, CultureInfo.InvariantCulture, out var textX) &&
                            double.TryParse(textPoint.GetAttribute("y"), NumberStyles.Any, CultureInfo.InvariantCulture, out var textY))
                        {
                            ReadDimLinear(entity, layer, palette, startX, startY, endX, endY, textX, textY, dimLinearPaths, dimLinearPathElements);
                        }
                    }
                }
                if (sketch.symbols == null)
                {
                    sketch.symbols = new List<SketchLayer>();
                }
                sketch.symbols.Add(layer);
            }
            return sketch;
        }
    }
}
