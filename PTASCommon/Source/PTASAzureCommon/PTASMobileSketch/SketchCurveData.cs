using System;

namespace PTASMobileSketch
{
	public class SketchCurveData
	{
		public double startX;

		public double startY;

        public double endX;

        public double endY;

        public double centerX;

        public double centerY;

        public double radius;

        public double centerInCurveX;

        public double centerInCurveY;

        public double length;

        public double rise;

        public double run;

        public double angle;

        public double chord;

        public double height;

        public void CenterPointOfLine(double startX, double startY, double endX, double endY, double? angle)
        {
            var angleToUse = 0.0;
            if (angle != null)
            {
                angleToUse = angle.Value;
            }
            if (Math.Abs(angleToUse) < 1e-5)
            {
                centerInCurveX = (startX + endX) / 2;
                centerInCurveY = (startY + endY) / 2;
                var deltaX = endX - startX;
                var deltaY = endY - startY;
                var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                length = distance;
                chord = distance;
            }
            else
            {
                var inRadians = Math.Abs(angleToUse) * Math.PI / 180;
                double startXToUse;
                double startYToUse;
                double endXToUse;
                double endYToUse;
                if (angle < 0)
                {
                    startXToUse = endX;
                    startYToUse = endY;
                    endXToUse = startX;
                    endYToUse = startY;
                }
                else
                {
                    startXToUse = startX;
                    startYToUse = startY;
                    endXToUse = endX;
                    endYToUse = endY;
                }
                var deltaX = endXToUse - startXToUse;
                var deltaY = endYToUse - startYToUse;
                var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                var halfDistance = distance / 2;
                var factor = Math.Tan(Math.PI / 2 - inRadians / 2);
                var distanceToCenter = factor * halfDistance;
                var directionX = deltaY / distance;
                var directionY = -deltaX / distance;
                centerX = (startXToUse + endXToUse) / 2 + directionX * distanceToCenter;
                centerY = (startYToUse + endYToUse) / 2 + directionY * distanceToCenter;
                deltaX = startXToUse - centerX;
                deltaY = startYToUse - centerY;
                radius = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                centerInCurveX = centerX - directionX * radius;
                centerInCurveY = centerY - directionY * radius;
                length = radius * inRadians;
                chord = distance;
            }
            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;
            this.angle = angleToUse;
        }

        public void Create(double startX, double startY, double endX, double endY, double? angle)
        {
            CenterPointOfLine(startX, startY, endX, endY, angle);
            rise = startY - endY;
            run = endX - startX;
            var deltaX = centerInCurveX - (startX + endX) / 2;
            var deltaY = centerInCurveY - (startY + endY) / 2;
            height = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
