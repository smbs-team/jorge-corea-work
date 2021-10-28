// ptas_SketchStyleSet.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Style set for PaperJS elements.
*/
class SketchStyleSet {
  /**
    * Default style sets for canvas objects
    *
    * @returns void
    *
    */
  setAsDefault() {
    this.style = new SketchStyle();
    this.style.color = "black";
    this.style.width = 2;
    this.objectSelectionStyle = new SketchStyle();
    this.objectSelectionStyle.color = "#a5c727";
    this.objectSelectionStyle.width = 4;
    this.objectSelectionStyle.radius = 8;
    this.objectGroupSelectionStyle = new SketchStyle();
    this.objectGroupSelectionStyle.color = "#a5c727";
    this.objectGroupSelectionStyle.width = 4;
    this.objectGroupSelectionStyle.radius = 4;
    this.objectOutOfSelectionStyle = new SketchStyle();
    this.objectOutOfSelectionStyle.color = "black";
    this.objectOutOfSelectionStyle.width = 2;
    this.shapeSelectionStyle = new SketchStyle();
    this.shapeSelectionStyle.color = "#a5c727";
    this.shapeSelectionStyle.width = 6;
    this.shapeSelectionStyle.radius = 8;
    this.arrowheadStyle = new SketchStyle();
    this.arrowheadStyle.color = "black";
    this.arrowheadStyle.radius = 8;
    this.overlappingStyle = new SketchStyle();
    this.overlappingStyle.color = "black";
    this.overlappingStyle.width = 2;
    this.openObjectStyle = new SketchStyle();
    this.openObjectStyle.color = "black";
    this.openObjectStyle.width = 4;
    this.openObjectStyle.radius = 8;
    this.linearMeasurementStyle = new SketchStyle();
    this.linearMeasurementStyle.color = "black";
    this.linearMeasurementStyle.width = 2;
    this.areaMeasurementStyle = new SketchStyle();
    this.areaMeasurementStyle.color = "black";
    this.areaMeasurementStyle.width = 2;
    this.customTextStyle = new SketchStyle();
    this.customTextStyle.color = "black";
    this.customTextStyle.width = 2;
    this.textSelectionStyle = new SketchStyle();
    this.textSelectionStyle.color = "black";
    this.textSelectionStyle.fill = "white";
    this.textSelectionStyle.width = 2;
    this.textSelectionLineToRotateIconStyle = new SketchStyle();
    this.textSelectionLineToRotateIconStyle.color = "black";
    this.textSelectionLineToRotateIconStyle.width = 2;
    this.minorGridlineStyle = new SketchStyle();
    this.minorGridlineStyle.color = "#e0e0e0";
    this.majorGridlineStyle = new SketchStyle();
    this.majorGridlineStyle.color = "#c0c0c0";
    this.gridlineAxisStyle = new SketchStyle();
    this.gridlineAxisStyle.color = "#a0a0a0";
    this.penUpCursorStyle = new SketchStyle();
    this.penUpCursorStyle.color = "black";
    this.penUpCursorStyle.width = 2;
    this.penUpCursorStyle.pattern = [8, 4];
    this.penDownCursorStyle = new SketchStyle();
    this.penDownCursorStyle.color = "black";
    this.penDownCursorStyle.width = 2;
    this.lineToPenStyle = new SketchStyle();
    this.lineToPenStyle.color = "gray";
    this.lineToPenStyle.width = 1;
    this.lineFor90DegreesIndicatorStyle = new SketchStyle();
    this.lineFor90DegreesIndicatorStyle.color = "black";
    this.lineFor90DegreesIndicatorStyle.width = 1;
    this.innerAngleIndicatorStyle = new SketchStyle();
    this.innerAngleIndicatorStyle.fill = "#a0a0a0";
    this.outerAngleIndicatorStyle = new SketchStyle();
    this.outerAngleIndicatorStyle.fill = "#c0c0c0";
    this.riseAndRunIndicatorStyle = new SketchStyle();
    this.riseAndRunIndicatorStyle.fill = "black";
    this.riseAndRunIndicatorStyle.width = 2;
    this.jumpToIndicatorStyle = new SketchStyle();
    this.jumpToIndicatorStyle.color = "blue";
    this.jumpToIndicatorStyle.radius = 5;
    this.overIndicatorStyle = new SketchStyle();
    this.overIndicatorStyle.color = "blue";
    this.overIndicatorStyle.radius = 8;
  }
}
