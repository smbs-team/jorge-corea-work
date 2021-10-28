using System;
using System.Globalization;

namespace PTASMobileSketch
{
    public static class SketchUtils
    {
        public static double AdjustAmount(double amount)
        {
            var amountToUse = amount;
            var nearestIntegerAmountToUse = Math.Round(amount);
            if (Math.Abs(amountToUse - nearestIntegerAmountToUse) < 1e-5)
            {
                return nearestIntegerAmountToUse;
            }
            return amountToUse;
        }

        public static string FeetTextFor(double amount)
        {
            var amountToUse = AdjustAmount(amount);
            return Math.Floor(amountToUse).ToString();
        }

        public static string InchesTextFor(double amount)
        {
            var amountToUse = AdjustAmount(amount);
            var feet = Math.Floor(amountToUse);
            return Math.Floor((amountToUse - feet) * 12).ToString();
        }

        public static string DistanceTextFor(double amount)
        {
            var amountToUse = AdjustAmount(amount);
            var feet = Math.Floor(amountToUse);
            var inches = Math.Floor((amountToUse - feet) * 12);
            if (inches == 0)
            {
                return feet.ToString(CultureInfo.InvariantCulture) + "'";
            }
            return feet.ToString(CultureInfo.InvariantCulture) + "' " + inches.ToString(CultureInfo.InvariantCulture) + '"';
        }

        public static SketchColor ParseColor(string color)
        {
            if (SketchNamedColorTable.colors.TryGetValue(color, out var entry))
            {
                color = entry;
            }
            if (color.Length == 7 && color[0] == '#')
            {
                return new SketchColor(int.Parse(color.Substring(1, 2), NumberStyles.HexNumber) / 255.0, int.Parse(color.Substring(3, 2), NumberStyles.HexNumber) / 255.0, int.Parse(color.Substring(5, 2), NumberStyles.HexNumber) / 255.0);
            }
            else if (color.Length == 9 && color[0] == '#')
            {
                return new SketchColor(int.Parse(color.Substring(1, 2), NumberStyles.HexNumber) / 255.0, int.Parse(color.Substring(3, 2), NumberStyles.HexNumber) / 255.0, int.Parse(color.Substring(5, 2), NumberStyles.HexNumber) / 255.0, int.Parse(color.Substring(7, 2), NumberStyles.HexNumber) / 255.0);
            }
            return new SketchColor();
        }
    }
}
