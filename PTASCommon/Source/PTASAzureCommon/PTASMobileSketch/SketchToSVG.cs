using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace PTASMobileSketch
{
    public class ElementWithAngle
    {
        public SketchPoint point;
        public double? angle;
    }

    public class ElementsPerStyle
    {
        public string color;
        public SketchColor? fill;
        public List<double> pattern;
        public bool? worldUnits;
        public double? width;
        public SketchPoint start;
        public List<ElementWithAngle> elements;
    }

    public class SketchToSVG
    {
        public static readonly Dictionary<string, SketchStyle> LayerDesigns = new Dictionary<string, SketchStyle>();

        static SketchToSVG()
        {
            LayerDesigns.Add("1st floor", new SketchStyle { color = new SketchColor(0, 0, 0, 1), width = 4, fill = new SketchColor(204.0 / 255.0, 204.0 / 255.0, 204.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("2nd floor", new SketchStyle { color = new SketchColor(0, 126.0 / 255.0, 143.0 / 255.0, 1), width = 4, pattern = new List<double> { 8, 8 }, fill = new SketchColor(0, 126.0 / 255.0, 143.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Upper floor", new SketchStyle { color = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 1), width = 4, pattern = new List<double> { 8, 8 }, fill = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Half floor", new SketchStyle { color = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 1), width = 4, pattern = new List<double> { 4, 4 }, fill = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Unfinished half floor", new SketchStyle { color = new SketchColor(37.0 / 255.0, 0, 143.0 / 255.0, 1), width = 4, pattern = new List<double> { 8, 8 }, fill = new SketchColor(37.0 / 255.0, 0, 143.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Unfinished full floor", new SketchStyle { color = new SketchColor(37.0 / 255.0, 0, 143.0 / 255.0, 1), width = 4, pattern = new List<double> { 8, 8 }, fill = new SketchColor(37.0 / 255.0, 0, 143.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Basement - total", new SketchStyle { color = new SketchColor(143.0 / 255.0, 0, 126.0 / 255.0, 1), width = 3, pattern = new List<double> { 4, 4 }, fill = new SketchColor(37.0 / 255.0, 0, 143.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Basement - finished", new SketchStyle { color = new SketchColor(143.0 / 255.0, 0, 126.0 / 255.0, 1), width = 3, pattern = new List<double> { 4, 4 }, fill = new SketchColor(143.0 / 255.0, 0, 126.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Garage - basement", new SketchStyle { color = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 1), width = 4, pattern = new List<double> { 4, 4 }, fill = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Garage - attached", new SketchStyle { color = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 1), width = 4, pattern = new List<double> { 4, 4 }, fill = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Porch - open", new SketchStyle { color = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 1), width = 2, pattern = new List<double> { 4, 4 }, fill = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Porch - enclosed", new SketchStyle { color = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 1), width = 2, pattern = new List<double> { 4, 4 }, fill = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Deck", new SketchStyle { color = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 1), width = 1, pattern = new List<double> { 2, 2 }, fill = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Garage - detached", new SketchStyle { color = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 1), width = 4, pattern = new List<double> { 4, 4 }, fill = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Carport", new SketchStyle { color = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 1), width = 4, pattern = new List<double> { 4, 4 }, fill = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Pavement - concrete", new SketchStyle { color = new SketchColor(0, 0, 0, 1), width = 2, fill = new SketchColor(204.0 / 255.0, 204.0 / 255.0, 204.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Pool - concrete", new SketchStyle { color = new SketchColor(0, 0, 0, 1), width = 2, fill = new SketchColor(204.0 / 255.0, 204.0 / 255.0, 204.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Pool - fiberglass/plastic", new SketchStyle { color = new SketchColor(0, 0, 0, 1), width = 2, fill = new SketchColor(204.0 / 255.0, 204.0 / 255.0, 204.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Misc. improvement", new SketchStyle { color = new SketchColor(0, 0, 0, 1), width = 2, fill = new SketchColor(204.0 / 255.0, 204.0 / 255.0, 204.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Mobile home", new SketchStyle { color = new SketchColor(0, 0, 0, 1), width = 4, fill = new SketchColor(204.0 / 255.0, 204.0 / 255.0, 204.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Room addition", new SketchStyle { color = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 1), width = 4, pattern = new List<double> { 4, 4 }, fill = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Tip out area", new SketchStyle { color = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 1), width = 2, pattern = new List<double> { 4, 4 }, fill = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 63.0 / 255.0) });
            LayerDesigns.Add("Mezzanine - display", new SketchStyle { color = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 1), width = 4, pattern = new List<double> { 2, 2 }, fill = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Mezzanine - office", new SketchStyle { color = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 1), width = 4, pattern = new List<double> { 4, 4 }, fill = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Mezzanine - storage", new SketchStyle { color = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 1), width = 4, pattern = new List<double> { 8, 8 }, fill = new SketchColor(0, 143.0 / 255.0, 6.0 / 255.0, 25.5 / 255.0) });
            LayerDesigns.Add("Balcony", new SketchStyle { color = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 1), width = 2, pattern = new List<double> { 4, 4 }, fill = new SketchColor(136.0 / 255.0, 136.0 / 255.0, 136.0 / 255.0, 63.0 / 255.0) });
        }

        public static void WritePath(ElementsPerStyle item, SketchColor? fill, XmlDocument document, XmlElement metadata, XmlElement path)
        {
            var d = new StringBuilder();
            d.Append("M ");
            d.Append(Convert.ToString(item.start.X, CultureInfo.InvariantCulture));
            d.Append(" ");
            d.Append(Convert.ToString(item.start.Y, CultureInfo.InvariantCulture));
            d.Append(" ");
            var previousX = item.start.X;
            var previousY = item.start.Y;
            foreach (var element in item.elements)
            {
                if (element.angle != null && element.angle.Value != 0)
                {
                    var angle = element.angle.Value * Math.PI / 180;
                    var deltaX = element.point.X - previousX;
                    var deltaY = element.point.Y - previousY;
                    var length = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                    if (length > 0)
                    {
                        var distanceToCenter = Math.Tan(Math.PI / 2 - angle / 2) * length / 2;
                        var centerX = previousX + deltaX / 2 - deltaY * distanceToCenter / length;
                        var centerY = previousY + deltaY / 2 + deltaX * distanceToCenter / length;
                        var deltaRadiusX = previousX - centerX;
                        var deltaRadiusY = previousY - centerY;
                        var radius = Math.Sqrt(deltaRadiusX * deltaRadiusX + deltaRadiusY * deltaRadiusY);
                        d.Append("A ");
                        d.Append(Convert.ToString(radius, CultureInfo.InvariantCulture));
                        d.Append(" ");
                        d.Append(Convert.ToString(radius, CultureInfo.InvariantCulture));
                        d.Append(" 0 0 ");
                        d.Append(Convert.ToString((element.angle.Value > 0 ? 0 : 1), CultureInfo.InvariantCulture));
                        d.Append(" ");
                        d.Append(Convert.ToString(element.point.X, CultureInfo.InvariantCulture));
                        d.Append(" ");
                        d.Append(Convert.ToString(element.point.Y, CultureInfo.InvariantCulture));
                        d.Append(" ");
                    }
                }
                else if (previousY == element.point.Y)
                {
                    d.Append("H ");
                    d.Append(Convert.ToString(element.point.X, CultureInfo.InvariantCulture));
                    d.Append(" ");
                }
                else if (previousX == element.point.X)
                {
                    d.Append("V ");
                    d.Append(Convert.ToString(element.point.Y, CultureInfo.InvariantCulture));
                    d.Append(" ");
                }
                else
                {
                    d.Append("L ");
                    d.Append(Convert.ToString(element.point.X, CultureInfo.InvariantCulture));
                    d.Append(" ");
                    d.Append(Convert.ToString(element.point.Y, CultureInfo.InvariantCulture));
                    d.Append(" ");
                }
                previousX = element.point.X;
                previousY = element.point.Y;
            }
            d.Remove(d.Length - 1, 1);
            path.SetAttribute("d", d.ToString());
            var includeIsColorPresent = false;
            var includeIsWidthPresent = false;
            if (item.color != null)
            {
                path.SetAttribute("stroke", item.color);
                includeIsColorPresent = true;
            }
            else
            {
                path.SetAttribute("stroke", "black");
            }
            if (item.pattern != null)
            {
                var pattern = new StringBuilder();
                foreach (var entry in item.pattern)
                {
                    pattern.Append(Convert.ToString(entry, CultureInfo.InvariantCulture));
                    if (item.worldUnits == null || !item.worldUnits.Value)
                    {
                        pattern.Append("in");
                    }
                    else
                    {
                        pattern.Append("px");
                    }
                    pattern.Append(" ");
                }
                pattern.Remove(pattern.Length - 1, 1);
                path.SetAttribute("stroke-dasharray", pattern.ToString());
            }
            if (item.width != null)
            {
                path.SetAttribute("stroke-width", Convert.ToString(item.width.Value, CultureInfo.InvariantCulture) + "px");
                includeIsWidthPresent = true;
            }
            else
            {
                path.SetAttribute("stroke-width", "1px");
            }
            if (fill != null)
            {
                var r = (int)Math.Floor(fill.Value.R * 255);
                var g = (int)Math.Floor(fill.Value.G * 255);
                var b = (int)Math.Floor(fill.Value.B * 255);
                var a = (int)Math.Floor(fill.Value.A * 255);
                path.SetAttribute("fill", $"#{r:X2}{g:X2}{b:X2}{a:X2}");
            }
            else
            {
                path.SetAttribute("fill", "transparent");
            }
            if (includeIsColorPresent || includeIsWidthPresent)
            {
                if (metadata == null)
                {
                    metadata = document.CreateElement("metadata");
                    path.AppendChild(metadata);
                }
                if (includeIsColorPresent)
                {
                    var isColorPresent = document.CreateElement("isColorPresent");
                    metadata.AppendChild(isColorPresent);
                }
                if (includeIsWidthPresent)
                {
                    var isWidthPresent = document.CreateElement("isWidthPresent");
                    metadata.AppendChild(isWidthPresent);
                }
            }
            path.SetAttribute("vector-effect", "non-scaling-stroke");
        }

        public static void WriteTextAttributes(SketchItemWithTextAttributes entry, XmlElement text, XmlDocument document, XmlElement metadata)
        {
            if (entry.fontFamily != null)
            {
                text.SetAttribute("font-family", entry.fontFamily);
                var fontFamily = document.CreateElement("fontFamily");
                fontFamily.InnerText = entry.fontFamily;
                metadata.AppendChild(fontFamily);
            }
            else
            {
                text.SetAttribute("font-family", "Open Sans, sans-serif");
            }
            if (entry.fontSize != null)
            {
                var size = entry.fontSize.Value;
                if (entry.worldUnits == null || !entry.worldUnits.Value)
                {
                    size /= 8;
                }
                text.SetAttribute("font-size", Convert.ToString(size, CultureInfo.InvariantCulture) + "px");
                var fontSize = document.CreateElement("fontSize");
                fontSize.InnerText = Convert.ToString(entry.fontSize, CultureInfo.InvariantCulture);
                metadata.AppendChild(fontSize);
            }
            else
            {
                text.SetAttribute("font-size", "2px");
            }
            if (entry.worldUnits != null && entry.worldUnits.Value)
            {
                var worldUnits = document.CreateElement("worldUnits");
                metadata.AppendChild(worldUnits);
            }
            if (entry.fontColor != null)
            {
                text.SetAttribute("fill", entry.fontColor);
                var fontColor = document.CreateElement("fontColor");
                fontColor.InnerText = entry.fontColor;
                metadata.AppendChild(fontColor);
            }
            if (entry.fontWeight != null)
            {
                text.SetAttribute("font-weight", Convert.ToString(entry.fontWeight, CultureInfo.InvariantCulture));
                var fontWeight = document.CreateElement("fontWeight");
                fontWeight.InnerText = Convert.ToString(entry.fontWeight, CultureInfo.InvariantCulture);
                metadata.AppendChild(fontWeight);
            }
            if (entry.horizontalAlign != null)
            {
                var horizontalAlign = document.CreateElement("horizontalAlign");
                horizontalAlign.InnerText = entry.horizontalAlign.Value.ToString();
                metadata.AppendChild(horizontalAlign);
                if (entry.horizontalAlign.Value == SketchTextHorizontalAlign.Center)
                {
                    text.SetAttribute("text-anchor", "middle");
                }
                else if (entry.horizontalAlign.Value == SketchTextHorizontalAlign.Right)
                {
                    text.SetAttribute("text-anchor", "right");
                }
            }
            if (entry.verticalAlign != null)
            {
                var verticalAlign = document.CreateElement("verticalAlign");
                verticalAlign.InnerText = entry.verticalAlign.Value.ToString();
                metadata.AppendChild(verticalAlign);
            }
        }

        public static void WriteDistance(SketchPath childObject, SketchDistance entry, XmlDocument document, XmlElement distance)
        {
            var text = document.CreateElement("text");
            var finishX = childObject.elements[entry.element].point.X;
            var finishY = childObject.elements[entry.element].point.Y;
            double startX;
            double startY;
            if (entry.element == 0)
            {
                startX = childObject.startPoint.X;
                startY = childObject.startPoint.Y;
            }
            else
            {
                startX = childObject.elements[entry.element - 1].point.X;
                startY = childObject.elements[entry.element - 1].point.Y;
            }
            var deltaX = finishX - startX;
            var deltaY = finishY - startY;
            var length = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            text.InnerText = SketchUtils.DistanceTextFor(length);
            var metadata = document.CreateElement("metadata");
            var isSketchDistance = document.CreateElement("isSketchDistance");
            metadata.AppendChild(isSketchDistance);
            var sketchPath = document.CreateElement("sketchPath");
            sketchPath.InnerText = childObject.uniqueIdentifier;
            metadata.AppendChild(sketchPath);
            var element = document.CreateElement("element");
            element.InnerText = Convert.ToString(entry.element, CultureInfo.InvariantCulture);
            metadata.AppendChild(element);
            var offsetX = document.CreateElement("offsetX");
            offsetX.InnerText = Convert.ToString(entry.offset.X, CultureInfo.InvariantCulture);
            metadata.AppendChild(offsetX);
            var offsetY = document.CreateElement("offsetY");
            offsetY.InnerText = Convert.ToString(entry.offset.Y, CultureInfo.InvariantCulture);
            metadata.AppendChild(offsetY);
            var forwardX = deltaX * entry.offset.X / length;
            var forwardY = deltaY * entry.offset.X / length;
            var sideX = -deltaY * entry.offset.Y / length;
            var sideY = deltaX * entry.offset.Y / length;
            var centerX = (startX + finishX) / 2;
            var centerY = (startY + finishY) / 2;
            var pointX = Convert.ToString(centerX + forwardX + sideX, CultureInfo.InvariantCulture);
            var pointY = Convert.ToString(centerY + forwardY + sideY, CultureInfo.InvariantCulture);
            distance.SetAttribute("transform", "translate(" + pointX + " " + pointY + ") scale(1 -1)");
            WriteTextAttributes(entry, text, document, metadata);
            if (entry.rotation != null)
            {
                var rotation = document.CreateElement("rotation");
                rotation.InnerText = Convert.ToString(entry.rotation.Value, CultureInfo.InvariantCulture);
                metadata.AppendChild(rotation);
                var transform = "rotate(";
                transform += Convert.ToString(entry.rotation.Value, CultureInfo.InvariantCulture);
                transform += " ";
                transform += Convert.ToString(pointX, CultureInfo.InvariantCulture);
                transform += " ";
                transform += Convert.ToString(pointY, CultureInfo.InvariantCulture);
                transform += ")";
                text.SetAttribute("transform", transform);
            }
            distance.AppendChild(text);
            distance.AppendChild(metadata);
        }

        public static void WriteCustomText(SketchCustomText entry, XmlDocument document, XmlElement customText)
        {
            var text = document.CreateElement("text");
            var metadata = document.CreateElement("metadata");
            var isSketchCustomText = document.CreateElement("isSketchCustomText");
            metadata.AppendChild(isSketchCustomText);
            var uniqueIdentifier = document.CreateElement("uniqueIdentifier");
            uniqueIdentifier.InnerText = entry.uniqueIdentifier;
            metadata.AppendChild(uniqueIdentifier);
            var x = document.CreateElement("x");
            x.InnerText = Convert.ToString(entry.start.X, CultureInfo.InvariantCulture);
            metadata.AppendChild(x);
            var y = document.CreateElement("y");
            y.InnerText = Convert.ToString(entry.start.Y, CultureInfo.InvariantCulture);
            metadata.AppendChild(y);
            customText.SetAttribute("transform", "translate(" + x.InnerText + " " + y.InnerText + ") scale(1 -1)");
            WriteTextAttributes(entry, text, document, metadata);
            if (entry.rotation != null)
            {
                var rotation = document.CreateElement("rotation");
                rotation.InnerText = Convert.ToString(entry.rotation.Value, CultureInfo.InvariantCulture);
                metadata.AppendChild(rotation);
                var transform = "rotate(";
                transform += Convert.ToString(entry.rotation.Value, CultureInfo.InvariantCulture);
                transform += ")";
                text.SetAttribute("transform", transform);
            }
            var lines = entry.customText.Split(new char[] { '\n' });
            if (lines.Length == 1)
            {
                var lineToWrite = new StringBuilder();
                foreach (var c in entry.customText)
                {
                    if (c == ' ')
                    {
                        lineToWrite.Append('\u00a0');
                    }
                    else
                    {
                        lineToWrite.Append(c);
                    }
                }
                text.InnerText = lineToWrite.ToString();
            }
            else
            {
                for (var i = 0; i < lines.Length; i++)
                {
                    var lineToWrite = new StringBuilder();
                    foreach (var c in lines[i])
                    {
                        if (c == ' ')
                        {
                            lineToWrite.Append('\u00a0');
                        }
                        else
                        {
                            lineToWrite.Append(c);
                        }
                    }
                    var tspan = document.CreateElement("tspan");
                    tspan.SetAttribute("x", "0");
                    if (i > 0)
                    {
                        tspan.SetAttribute("dy", "1em");
                    }
                    tspan.InnerText = lineToWrite.ToString();
                    text.AppendChild(tspan);
                }
            }
            var rawCustomText = document.CreateElement("customText");
            var rawCustomTextCData = document.CreateCDataSection(entry.customText);
            rawCustomText.AppendChild(rawCustomTextCData);
            if (!string.IsNullOrWhiteSpace(entry.arrow))
            {
                var arrow = document.CreateElement("arrow");
                arrow.InnerText = entry.arrow;
                metadata.AppendChild(arrow);
            }
            metadata.AppendChild(rawCustomText);
            customText.AppendChild(text);
            customText.AppendChild(metadata);
        }

        public static void WriteCircle(SketchCircle entry, XmlDocument document, XmlElement circle)
        {
            circle.SetAttribute("cx", Convert.ToString(entry.center.X, CultureInfo.InvariantCulture));
            circle.SetAttribute("cy", Convert.ToString(entry.center.Y, CultureInfo.InvariantCulture));
            circle.SetAttribute("r", Convert.ToString(entry.radius, CultureInfo.InvariantCulture));
            var metadata = document.CreateElement("metadata");
            if (entry.color != null)
            {
                circle.SetAttribute("stroke", entry.color);
                var isColorPresent = document.CreateElement("isColorPresent");
                metadata.AppendChild(isColorPresent);
            }
            else
            {
                circle.SetAttribute("stroke", "black");
            }
            if (entry.pattern != null)
            {
                var pattern = new StringBuilder();
                foreach (var patternEntry in entry.pattern)
                {
                    pattern.Append(Convert.ToString(patternEntry, CultureInfo.InvariantCulture));
                    if (entry.worldUnits == null || !entry.worldUnits.Value)
                    {
                        pattern.Append("in");
                    }
                    else
                    {
                        pattern.Append("px");
                    }
                    pattern.Append(" ");
                }
                pattern.Remove(pattern.Length - 1, 1);
                circle.SetAttribute("stroke-dasharray", pattern.ToString());
            }
            if (entry.width != null)
            {
                circle.SetAttribute("stroke-width", Convert.ToString(entry.width.Value, CultureInfo.InvariantCulture) + "px");
                var isWidthPresent = document.CreateElement("isWidthPresent");
                metadata.AppendChild(isWidthPresent);
            }
            else
            {
                circle.SetAttribute("stroke-width", "1px");
            }
            circle.SetAttribute("fill", "transparent");
            circle.AppendChild(metadata);
            circle.SetAttribute("vector-effect", "non-scaling-stroke");
        }

        public static void WriteSymbol(SketchSymbol symbol, XmlDocument document, XmlElement use)
        {
            use.SetAttribute("href", "#" + symbol.symbol);
            use.SetAttribute("x", Convert.ToString(symbol.startPoint.X, CultureInfo.InvariantCulture));
            use.SetAttribute("y", Convert.ToString(symbol.startPoint.Y, CultureInfo.InvariantCulture));
            var metadata = document.CreateElement("metadata");
            var isSketchSymbol = document.CreateElement("isSketchSymbol");
            metadata.AppendChild(isSketchSymbol);
            var transform = "";
            if (symbol.rotation != null)
            {
                var rotation = document.CreateElement("rotation");
                rotation.InnerText = Convert.ToString(symbol.rotation.Value, CultureInfo.InvariantCulture);
                metadata.AppendChild(rotation);
                transform += "rotate(";
                transform += Convert.ToString(symbol.rotation.Value, CultureInfo.InvariantCulture);
                transform += " ";
                transform += Convert.ToString(symbol.startPoint.X, CultureInfo.InvariantCulture);
                transform += " ";
                transform += Convert.ToString(symbol.startPoint.Y, CultureInfo.InvariantCulture);
                transform += ")";
            }
            if (symbol.scale != null)
            {
                var scaleX = document.CreateElement("scaleX");
                scaleX.InnerText = Convert.ToString(symbol.scale.X, CultureInfo.InvariantCulture);
                metadata.AppendChild(scaleX);
                var scaleY = document.CreateElement("scaleY");
                scaleY.InnerText = Convert.ToString(symbol.scale.Y, CultureInfo.InvariantCulture);
                metadata.AppendChild(scaleY);
                transform += "scale(";
                transform += Convert.ToString(symbol.scale.X, CultureInfo.InvariantCulture);
                transform += " ";
                transform += Convert.ToString(symbol.scale.Y, CultureInfo.InvariantCulture);
                transform += ")";
            }
            if (transform != "")
            {
                use.SetAttribute("transform", transform);
            }
            use.AppendChild(metadata);
        }

        public static void FillElementsPerStyleWithLayer(SketchLayer layer, List<ElementsPerStyle> elementsPerStyle)
        {
            if (layer == null)
            {
                return;
            }
            string name = layer.name;
            if (!string.IsNullOrWhiteSpace(name) && LayerDesigns.TryGetValue(name, out var style))
            {
                var r = (int)(style.color.R * 255.0);
                var g = (int)(style.color.G * 255.0);
                var b = (int)(style.color.B * 255.0);
                var a = (int)(style.color.A * 255.0);
                elementsPerStyle.Add(new ElementsPerStyle
                {
                    color = $"#{r:X2}{g:X2}{b:X2}{a:X2}",
                    fill = style.fill,
                    width = style.width,
                    pattern = style.pattern,
                    worldUnits = true
                });
            }
        }

        public static void WriteObjectChildren(SketchLayer layer, SketchObject sketchObject, XmlDocument document, XmlElement g)
        {
            if (sketchObject.pathList != null)
            {
                foreach (var entry in sketchObject.pathList)
                {
                    var elementsPerStyle = new List<ElementsPerStyle>();
                    var previousX = entry.startPoint.X;
                    var previousY = entry.startPoint.Y;
                    FillElementsPerStyleWithLayer(layer, elementsPerStyle);
                    if (elementsPerStyle.Count > 0)
                    {
                        if (entry.elements != null)
                        {
                            foreach (var element in entry.elements)
                            {
                                var itemToEdit = elementsPerStyle[elementsPerStyle.Count - 1];
                                if (itemToEdit.elements == null)
                                {
                                    itemToEdit.start = new SketchPoint { X = previousX, Y = previousY };
                                    itemToEdit.elements = new List<ElementWithAngle>();
                                }
                                var elementWithAngle = new ElementWithAngle { point = new SketchPoint { X = element.point.X, Y = element.point.Y }, angle = element.angle };
                                itemToEdit.elements.Add(elementWithAngle);
                                previousX = element.point.X;
                                previousY = element.point.Y;
                            }
                        }
                    }
                    else
                    {
                        if (entry.elements != null)
                        {
                            foreach (var element in entry.elements)
                            {
                                var match = false;
                                if (elementsPerStyle.Count > 0)
                                {
                                    var item = elementsPerStyle[elementsPerStyle.Count - 1];
                                    var colorsMatch = ((item.color != null) == (element.color != null));
                                    if (item.color != null && element.color != null)
                                    {
                                        colorsMatch = item.color.Equals(element.color);
                                    }
                                    if (colorsMatch)
                                    {
                                        var patternsMatch = ((item.pattern != null) == (element.pattern != null));
                                        if (item.pattern != null && element.pattern != null)
                                        {
                                            patternsMatch = (item.pattern.Count == element.pattern.Count);
                                            if (patternsMatch)
                                            {
                                                for (var i = 0; i < item.pattern.Count; i++)
                                                {
                                                    if (item.pattern[i] != element.pattern[i])
                                                    {
                                                        patternsMatch = false;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        if (patternsMatch)
                                        {
                                            var worldUnitsMatch = ((item.worldUnits != null) == (element.worldUnits != null));
                                            if (item.worldUnits != null && element.worldUnits != null)
                                            {
                                                worldUnitsMatch = (item.worldUnits.Value == element.worldUnits.Value);
                                            }
                                            if (worldUnitsMatch)
                                            {
                                                var widthsMatch = ((item.width != null) == (element.width != null));
                                                if (item.width != null && element.width != null)
                                                {
                                                    widthsMatch = (item.worldUnits.Value == element.worldUnits.Value);
                                                }
                                                match = widthsMatch;
                                            }
                                        }
                                    }
                                }
                                if (!match)
                                {
                                    var item = new ElementsPerStyle { color = element.color, pattern = element.pattern, worldUnits = element.worldUnits, width = element.width };
                                    elementsPerStyle.Add(item);
                                }
                                if (elementsPerStyle.Count > 0)
                                {
                                    var itemToEdit = elementsPerStyle[elementsPerStyle.Count - 1];
                                    if (itemToEdit.elements == null)
                                    {
                                        itemToEdit.start = new SketchPoint { X = previousX, Y = previousY };
                                        itemToEdit.elements = new List<ElementWithAngle>();
                                    }
                                    var elementWithAngle = new ElementWithAngle { point = new SketchPoint { X = element.point.X, Y = element.point.Y }, angle = element.angle };
                                    itemToEdit.elements.Add(elementWithAngle);
                                }
                                previousX = element.point.X;
                                previousY = element.point.Y;
                            }
                        }
                    }
                    var metadata = document.CreateElement("metadata");
                    var isSketchPath = document.CreateElement("isSketchPath");
                    metadata.AppendChild(isSketchPath);
                    var uniqueIdentifier = document.CreateElement("uniqueIdentifier");
                    uniqueIdentifier.InnerText = entry.uniqueIdentifier;
                    metadata.AppendChild(uniqueIdentifier);
                    if (entry.area != 0)
                    {
                        var area = document.CreateElement("area");
                        area.InnerText = Convert.ToString(entry.area, CultureInfo.InvariantCulture);
                        metadata.AppendChild(area);
                    }
                    XmlElement outputElement = null;
                    if (elementsPerStyle.Count == 1 && (entry.arrow == null || !entry.arrow.Value))
                    {
                        if (elementsPerStyle[0].start != null && elementsPerStyle[0].elements != null && elementsPerStyle[0].elements.Count > 0)
                        {
                            outputElement = document.CreateElement("path");
                            var fill = elementsPerStyle[0].fill;
                            if (fill == null)
                            {
                                fill = entry.fill;
                            }
                            WritePath(elementsPerStyle[0], fill, document, metadata, outputElement);
                        }
                    }
                    else if (elementsPerStyle.Count > 0 || (entry.arrow != null && entry.arrow.Value))
                    {
                        outputElement = document.CreateElement("g");
                        foreach (var item in elementsPerStyle)
                        {
                            if (item.start != null && item.elements != null && item.elements.Count > 0)
                            {
                                var path = document.CreateElement("path");
                                var fill = item.fill;
                                if (fill == null)
                                {
                                    fill = entry.fill;
                                }
                                WritePath(item, fill, document, null, path);
                                outputElement.AppendChild(path);
                            }
                        }
                        if (entry.arrow != null && entry.arrow.Value && elementsPerStyle.Count > 0)
                        {
                            var item = elementsPerStyle[elementsPerStyle.Count - 1];
                            if (item.elements.Count > 0)
                            {
                                var path = document.CreateElement("path");
                                var endPoint = item.elements[item.elements.Count - 1].point;
                                SketchPoint startPoint;
                                if (item.elements.Count == 1)
                                {
                                    startPoint = entry.startPoint;
                                }
                                else
                                {
                                    startPoint = item.elements[item.elements.Count - 2].point;
                                }
                                var deltaX = endPoint.X - startPoint.X;
                                var deltaY = endPoint.Y - startPoint.Y;
                                var lengthSquared = deltaX * deltaX + deltaY * deltaY;
                                if (lengthSquared > 0)
                                {
                                    var length = Math.Sqrt(lengthSquared);
                                    var forwardX = deltaX / length;
                                    var forwardY = deltaY / length;
                                    var rightX = forwardY;
                                    var rightY = -forwardX;
                                    var d = new StringBuilder();
                                    d.Append("M ");
                                    d.Append(Convert.ToString(endPoint.X - 0.5 * forwardX - 0.5 * rightX, CultureInfo.InvariantCulture));
                                    d.Append(" ");
                                    d.Append(Convert.ToString(endPoint.Y - 0.5 * forwardY - 0.5 * rightY, CultureInfo.InvariantCulture));
                                    d.Append(" L ");
                                    d.Append(Convert.ToString(endPoint.X + 0.5 * forwardX, CultureInfo.InvariantCulture));
                                    d.Append(" ");
                                    d.Append(Convert.ToString(endPoint.Y + 0.5 * forwardY, CultureInfo.InvariantCulture));
                                    d.Append(" L ");
                                    d.Append(Convert.ToString(endPoint.X - 0.5 * forwardX + 0.5 * rightX, CultureInfo.InvariantCulture));
                                    d.Append(" ");
                                    d.Append(Convert.ToString(endPoint.Y - 0.5 * forwardY + 0.5 * rightY, CultureInfo.InvariantCulture));
                                    d.Append(" Z");
                                    path.SetAttribute("d", d.ToString());
                                    if (item.color != null)
                                    {
                                        path.SetAttribute("fill", item.color);
                                    }
                                    else
                                    {
                                        path.SetAttribute("fill", "black");
                                    }
                                }
                                outputElement.AppendChild(path);
                            }
                        }
                    }
                    if (entry.closed != null && entry.closed.Value)
                    {
                        var isClosed = document.CreateElement("isClosed");
                        metadata.AppendChild(isClosed);
                    }
                    if (!string.IsNullOrWhiteSpace(entry.label))
                    {
                        var customTextForLabel = document.CreateElement("customTextForLabel");
                        customTextForLabel.InnerText = entry.label;
                        metadata.AppendChild(customTextForLabel);
                    }
                    if (entry.arrow != null && entry.arrow.Value)
                    {
                        var isArrow = document.CreateElement("isArrow");
                        metadata.AppendChild(isArrow);
                    }
                    if (outputElement != null)
                    {
                        outputElement.AppendChild(metadata);
                        g.AppendChild(outputElement);
                    }
                }
            }
            if (sketchObject.distanceList != null)
            {
                foreach (var entry in sketchObject.distanceList)
                {
                    if (sketchObject.pathList != null)
                    {
                        foreach (var childObject in sketchObject.pathList)
                        {
                            if (!string.IsNullOrWhiteSpace(childObject.uniqueIdentifier) && childObject.uniqueIdentifier.Equals(entry.path))
                            {
                                if (childObject.elements != null && entry.element >= 0 && entry.element < childObject.elements.Count)
                                {
                                    var distance = document.CreateElement("g");
                                    WriteDistance(childObject, entry, document, distance);
                                    g.AppendChild(distance);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            if (sketchObject.customTextList != null)
            {
                foreach (var entry in sketchObject.customTextList)
                {
                    if (entry.start != null)
                    {
                        var customText = document.CreateElement("g");
                        WriteCustomText(entry, document, customText);
                        g.AppendChild(customText);
                    }
                }
            }
            if (sketchObject.circleList != null)
            {
                foreach (var entry in sketchObject.circleList)
                {
                    if (entry.center != null)
                    {
                        var circle = document.CreateElement("circle");
                        WriteCircle(entry, document, circle);
                        g.AppendChild(circle);
                    }
                }
            }
            if (sketchObject.symbolList != null)
            {
                foreach (var entry in sketchObject.symbolList)
                {
                    var use = document.CreateElement("use");
                    WriteSymbol(entry, document, use);
                    g.AppendChild(use);
                }
            }
        }

        public static void WriteObjects(SketchLayer layer, XmlDocument document, XmlElement g)
        {
            if (layer.objects == null)
            {
                return;
            }
            foreach (var sketchObject in layer.objects)
            {
                var child = document.CreateElement("g");
                WriteObjectChildren(layer, sketchObject, document, child);
                var metadata = document.CreateElement("metadata");
                var isSketchObject = document.CreateElement("isSketchObject");
                metadata.AppendChild(isSketchObject);
                child.AppendChild(metadata);
                g.AppendChild(child);
            }
        }

        public static void WriteLayers(SketchControl sketch, XmlDocument document, XmlElement svg)
        {
            if (sketch.layers == null)
            {
                return;
            }
            foreach (var layer in sketch.layers)
            {
                var g = document.CreateElement("g");
                if (layer.visible == null && !layer.visible.Value)
                {
                    g.SetAttribute("visibility", "hidden");
                }
                WriteObjects(layer, document, g);
                var metadata = document.CreateElement("metadata");
                var isSketchLayer = document.CreateElement("isSketchLayer");
                metadata.AppendChild(isSketchLayer);
                var uniqueIdentifier = document.CreateElement("uniqueIdentifier");
                uniqueIdentifier.InnerText = layer.uniqueIdentifier;
                metadata.AppendChild(uniqueIdentifier);
                var name = document.CreateElement("name");
                name.InnerText = layer.name;
                metadata.AppendChild(name);
                if (layer.netArea != 0)
                {
                    var netArea = document.CreateElement("netArea");
                    netArea.InnerText = Convert.ToString(layer.netArea, CultureInfo.InvariantCulture);
                    metadata.AppendChild(netArea);
                }
                if (layer.grossArea != 0)
                {
                    var grossArea = document.CreateElement("grossArea");
                    grossArea.InnerText = Convert.ToString(layer.grossArea, CultureInfo.InvariantCulture);
                    metadata.AppendChild(grossArea);
                }
                if (!string.IsNullOrWhiteSpace(layer.customTextForLabel))
                {
                    var customTextForLabel = document.CreateElement("customTextForLabel");
                    customTextForLabel.InnerText = layer.customTextForLabel;
                    metadata.AppendChild(customTextForLabel);
                }
                if (layer.labelMoved != null && layer.labelMoved.Value)
                {
                    var labelMoved = document.CreateElement("labelMoved");
                    metadata.AppendChild(labelMoved);
                }
                g.AppendChild(metadata);
                svg.AppendChild(g);
            }
        }

        public static void WriteSymbols(SketchControl sketch, XmlDocument document, XmlElement svg)
        {
            if (sketch.symbols == null)
            {
                return;
            }
            foreach (var layer in sketch.symbols)
            {
                var symbol = document.CreateElement("symbol");
                symbol.SetAttribute("id", layer.name);
                symbol.SetAttribute("overflow", "visible");
                WriteObjects(layer, document, symbol);
                var metadata = document.CreateElement("metadata");
                var isSymbolAsSketchLayer = document.CreateElement("isSymbolAsSketchLayer");
                metadata.AppendChild(isSymbolAsSketchLayer);
                var uniqueIdentifier = document.CreateElement("uniqueIdentifier");
                uniqueIdentifier.InnerText = layer.uniqueIdentifier;
                metadata.AppendChild(uniqueIdentifier);
                var name = document.CreateElement("name");
                name.InnerText = layer.name;
                metadata.AppendChild(name);
                symbol.AppendChild(metadata);
                svg.AppendChild(symbol);
            }
        }

        public static string Write(SketchControl sketch)
        {
            var document = new XmlDocument();
            var svg = document.CreateElement("svg");
            svg.SetAttribute("xmlns", "http://www.w3.org/2000/svg");
            var bounds = new SketchBounds();
            sketch.GetBounds(bounds);
            if (bounds.started)
            {
                var width = bounds.max.X - bounds.min.X;
                var height = bounds.max.Y - bounds.min.Y;
                var size = Math.Max(width, height);
                var viewBoxWidth = Convert.ToString(width + size * 0.25, CultureInfo.InvariantCulture);
                var viewBoxHeight = Convert.ToString(height + size * 0.25, CultureInfo.InvariantCulture);
                svg.SetAttribute("viewBox", "0 0 " + viewBoxWidth + " " + viewBoxHeight);
                var topLevelGroup = document.CreateElement("g");
                var topLevelX = Convert.ToString(-bounds.min.X + size * 0.125, CultureInfo.InvariantCulture);
                var topLevelY = Convert.ToString(bounds.max.Y + size * 0.125, CultureInfo.InvariantCulture);
                topLevelGroup.SetAttribute("transform", "translate(" + topLevelX + " " + topLevelY + ") scale(1 -1)");
                WriteLayers(sketch, document, topLevelGroup);
                WriteSymbols(sketch, document, topLevelGroup);
                svg.AppendChild(topLevelGroup);
            }
            var metadata = document.CreateElement("metadata");
            var isSketch = document.CreateElement("isSketch");
            metadata.AppendChild(isSketch);
            if (sketch.levels != null)
            {
                foreach (var entry in sketch.levels)
                {
                    var level = document.CreateElement("level");
                    var uniqueIdentifier = document.CreateElement("uniqueIdentifier");
                    uniqueIdentifier.InnerText = entry.uniqueIdentifier;
                    level.AppendChild(uniqueIdentifier);
                    var name = document.CreateElement("name");
                    name.InnerText = entry.name;
                    level.AppendChild(name);
                    if (entry.layers != null)
                    {
                        foreach (var item in entry.layers)
                        {
                            var layer = document.CreateElement("layer");
                            layer.InnerText = item.uniqueIdentifier;
                            level.AppendChild(layer);
                        }
                    }
                    metadata.AppendChild(level);
                }
            }
            svg.AppendChild(metadata);
            document.AppendChild(svg);
            using (var writer = new StringWriter())
            {
                document.Save(writer);
                return writer.ToString();
            }
        }
    }
}
