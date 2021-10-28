using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchStyleSet
    {
        public SketchStyle style; // Default, unselected element style
        public SketchStyle objectSelectionStyle;
        public SketchStyle objectGroupSelectionStyle;
        public SketchStyle objectOutOfSelectionStyle;
        public SketchStyle shapeSelectionStyle;
        public SketchStyle arrowheadStyle;
        public SketchStyle overlappingStyle;
        public SketchStyle openObjectStyle;
        public SketchStyle linearMeasurementStyle;
        public SketchStyle areaMeasurementStyle;
        public SketchStyle customTextStyle;
        public SketchStyle textSelectionStyle;
        public SketchStyle minorGridlineStyle;
        public SketchStyle majorGridlineStyle;
        public SketchStyle gridlineAxisStyle;
        public SketchStyle penUpCursorStyle;
        public SketchStyle penDownCursorStyle;
        public SketchStyle lineToPenStyle;
        public SketchStyle lineFor90DegreesIndicatorStyle;
        public SketchStyle innerAngleIndicatorStyle;
        public SketchStyle outerAngleIndicatorStyle;
        public SketchStyle riseAndRunIndicatorStyle;
        public SketchStyle jumpToIndicatorStyle;
        public SketchStyle jumpToBayIndicatorStyle;
        public SketchStyle hiddenDistanceStyle;
        public SketchStyle unselectedPointInLineStyle;
        public SketchStyle unselectedPointStyle;
        public SketchStyle overIndicatorStyle;

        public void SetAsDefault()
        {
            style = new SketchStyle();
            style.color = new SketchColor(0, 0, 0);
            style.width = 2;
            objectSelectionStyle = new SketchStyle();
            objectSelectionStyle.color = new SketchColor(0, 0, 0);
            objectSelectionStyle.width = 4;
            objectSelectionStyle.radius = 8;
            objectGroupSelectionStyle = new SketchStyle();
            objectGroupSelectionStyle.color = new SketchColor(0, 0, 0);
            objectGroupSelectionStyle.width = 4;
            objectGroupSelectionStyle.radius = 4;
            objectOutOfSelectionStyle = new SketchStyle();
            objectOutOfSelectionStyle.color = new SketchColor(0, 0, 0);
            objectOutOfSelectionStyle.width = 2;
            shapeSelectionStyle = new SketchStyle();
            shapeSelectionStyle.color = new SketchColor(0, 0, 0);
            shapeSelectionStyle.width = 6;
            shapeSelectionStyle.radius = 8;
            arrowheadStyle = new SketchStyle();
            arrowheadStyle.color = new SketchColor(0, 0, 0);
            arrowheadStyle.width = 8;
            overlappingStyle = new SketchStyle();
            overlappingStyle.color = new SketchColor(0, 0, 0);
            overlappingStyle.width = 2;
            openObjectStyle = new SketchStyle();
            openObjectStyle.color = new SketchColor(0, 0, 0);
            openObjectStyle.width = 4;
            openObjectStyle.radius = 8;
            linearMeasurementStyle = new SketchStyle();
            linearMeasurementStyle.color = new SketchColor(0, 0, 0);
            linearMeasurementStyle.width = 2;
            areaMeasurementStyle = new SketchStyle();
            areaMeasurementStyle.color = new SketchColor(0, 0, 0);
            areaMeasurementStyle.width = 2;
            customTextStyle = new SketchStyle();
            customTextStyle.color = new SketchColor(0, 0, 0);
            customTextStyle.width = 2;
            textSelectionStyle = new SketchStyle();
            textSelectionStyle.color = new SketchColor(0, 0, 0);
            textSelectionStyle.fill = new SketchColor(255, 255, 255);
            textSelectionStyle.width = 2;
            minorGridlineStyle = new SketchStyle();
            minorGridlineStyle.color = new SketchColor(224, 224, 224);
            majorGridlineStyle = new SketchStyle();
            majorGridlineStyle.color = new SketchColor(192, 192, 192);
            gridlineAxisStyle = new SketchStyle();
            gridlineAxisStyle.color = new SketchColor(160, 160, 160);
            penUpCursorStyle = new SketchStyle();
            penUpCursorStyle.color = new SketchColor(0, 0, 0);
            penUpCursorStyle.width = 2;
            penDownCursorStyle = new SketchStyle();
            penDownCursorStyle.color = new SketchColor(0, 0, 0);
            penDownCursorStyle.width = 2;
            lineToPenStyle = new SketchStyle();
            lineToPenStyle.color = new SketchColor(169, 169, 169);
            lineToPenStyle.width = 1;
            lineFor90DegreesIndicatorStyle = new SketchStyle();
            lineFor90DegreesIndicatorStyle.color = new SketchColor(0, 0, 0);
            lineFor90DegreesIndicatorStyle.width = 1;
            innerAngleIndicatorStyle = new SketchStyle();
            innerAngleIndicatorStyle.fill = new SketchColor(160, 160, 160);
            outerAngleIndicatorStyle = new SketchStyle();
            outerAngleIndicatorStyle.fill = new SketchColor(192, 192, 192);
            riseAndRunIndicatorStyle = new SketchStyle();
            riseAndRunIndicatorStyle.fill = new SketchColor(0, 0, 0);
            riseAndRunIndicatorStyle.width = 2;
            jumpToIndicatorStyle = new SketchStyle();
            jumpToIndicatorStyle.color = new SketchColor(0, 0, 0);
            jumpToIndicatorStyle.radius = 4;
            jumpToBayIndicatorStyle = new SketchStyle();
            jumpToBayIndicatorStyle.color = new SketchColor(0, 0, 0);
            jumpToBayIndicatorStyle.radius = 4;
            hiddenDistanceStyle = new SketchStyle();
            hiddenDistanceStyle.color = new SketchColor(128, 128, 128);
            hiddenDistanceStyle.width = 1;
            unselectedPointInLineStyle = new SketchStyle();
            unselectedPointInLineStyle.point = new SketchColor(0, 0, 0, 1);
            unselectedPointInLineStyle.radius = 8;
            unselectedPointStyle = new SketchStyle();
            unselectedPointStyle.point = new SketchColor(0, 0, 0, 1);
            unselectedPointStyle.radius = 8;
            overIndicatorStyle = new SketchStyle();
            overIndicatorStyle.color = new SketchColor(0, 0, 0);
            overIndicatorStyle.radius = 8;
        }
    }
}
