namespace PTASMobileSketch
{
    public class SketchItemWithTextAttributes
    {
        public string fontFamily;

        public double? fontSize;

        public bool? worldUnits;

        public SketchTextHorizontalAlign? horizontalAlign;

        public SketchTextVerticalAlign? verticalAlign;

        public double? rotation;

        public string fontColor;

        public int? fontWeight;

        public void CopyTextAttributesTo(SketchItemWithTextAttributes target)
        {
            target.fontFamily = fontFamily;
            target.fontSize = fontSize;
            target.worldUnits = worldUnits;
            target.horizontalAlign = horizontalAlign;
            target.verticalAlign = verticalAlign;
            target.rotation = rotation;
            target.fontColor = fontColor;
            target.fontWeight = fontWeight;
        }
    }
}
