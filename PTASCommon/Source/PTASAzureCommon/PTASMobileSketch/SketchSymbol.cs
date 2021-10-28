using System;

namespace PTASMobileSketch
{
    public class SketchSymbol
    {
        public string symbol;

        public SketchPoint startPoint;

        public double? rotation;

        public SketchPoint scale;

        public SketchSymbol CreateCopy()
        {
            var copy = new SketchSymbol
            {
                rotation = rotation,
                symbol = symbol
            };
            if (scale != null)
            {
                copy.scale = new SketchPoint(scale);
            }
            if (startPoint != null)
            {
                copy.startPoint = new SketchPoint(startPoint);
            }
            return copy;
        }

        public void GetBounds(SketchBounds bounds, SketchControl sketch)
        {
            if (symbol != null && sketch != null && sketch.symbols != null)
            {
                var symbolBounds = new SketchBounds();
                foreach (var entry in sketch.symbols)
                {
                    if (entry.name != null && entry.name.Equals(symbol))
                    {
                        entry.GetBounds(symbolBounds, null);
                    }
                }
                if (symbolBounds.started)
                {
                    var topLeftX = symbolBounds.min.X;
                    var topLeftY = symbolBounds.min.Y;
                    var topRightX = symbolBounds.max.X;
                    var topRightY = symbolBounds.min.Y;
                    var bottomRightX = symbolBounds.max.X;
                    var bottomRightY = symbolBounds.max.Y;
                    var bottomLeftX = symbolBounds.min.X;
                    var bottomLeftY = symbolBounds.max.Y;
                    if (rotation != null && rotation.Value != 0)
                    {
                        var length = Math.Sqrt(topLeftX * topLeftX + topLeftY * topLeftY);
                        var angle = Math.Atan2(topLeftY, topLeftX);
                        angle += rotation.Value * Math.PI / 180;
                        topLeftX = length * Math.Cos(angle);
                        topLeftY = length * Math.Sin(angle);
                        length = Math.Sqrt(topRightX * topRightX + topRightY * topRightY);
                        angle = Math.Atan2(topRightY, topRightX);
                        angle += rotation.Value * Math.PI / 180;
                        topRightX = length * Math.Cos(angle);
                        topRightY = length * Math.Sin(angle);
                        length = Math.Sqrt(bottomRightX * bottomRightX + bottomRightY * bottomRightY);
                        angle = Math.Atan2(bottomRightY, bottomRightX);
                        angle += rotation.Value * Math.PI / 180;
                        bottomRightX = length * Math.Cos(angle);
                        bottomRightY = length * Math.Sin(angle);
                        length = Math.Sqrt(bottomLeftX * bottomLeftX + bottomLeftY * bottomLeftY);
                        angle = Math.Atan2(bottomLeftY, bottomLeftX);
                        angle += rotation.Value * Math.PI / 180;
                        bottomLeftX = length * Math.Cos(angle);
                        bottomLeftY = length * Math.Sin(angle);
                    }
                    if (scale != null)
                    {
                        topLeftX *= scale.X;
                        topLeftY *= scale.Y;
                        topRightX *= scale.X;
                        topRightY *= scale.Y;
                        bottomRightX *= scale.X;
                        bottomRightY *= scale.Y;
                        bottomLeftX *= scale.X;
                        bottomLeftY *= scale.Y;
                    }
                    if (startPoint != null)
                    {
                        topLeftX += startPoint.X;
                        topLeftY += startPoint.Y;
                        topRightX += startPoint.X;
                        topRightY += startPoint.Y;
                        bottomRightX += startPoint.X;
                        bottomRightY += startPoint.Y;
                        bottomLeftX += startPoint.X;
                        bottomLeftY += startPoint.Y;
                    }
                    bounds.min.X = Math.Min(Math.Min(Math.Min(Math.Min(bounds.min.X, topLeftX), topRightX), bottomRightX), bottomLeftX);
                    bounds.min.Y = Math.Min(Math.Min(Math.Min(Math.Min(bounds.min.Y, topLeftY), topRightY), bottomRightY), bottomLeftY);
                    bounds.max.X = Math.Max(Math.Max(Math.Max(Math.Max(bounds.max.X, topLeftX), topRightX), bottomRightX), bottomLeftX);
                    bounds.max.Y = Math.Max(Math.Max(Math.Max(Math.Max(bounds.max.Y, topLeftY), topRightY), bottomRightY), bottomLeftY);
                }
            }
        }
    }
}
