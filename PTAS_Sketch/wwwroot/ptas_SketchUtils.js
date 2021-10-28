// ptas_SketchUtils.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
 * Miscellaneous functions.
 */
class SketchUtils {
  /**
   * Gets the center point of a line
   *
   * @param {number} startX - starting X position
   * @param {number} startY - starting Y position
   * @param {number} finishX - ending X position
   * @param {number} finishY - ending Y position
   * @param {number} angle - line angle
   * @returns {object} - Point
   *
   */
  static centerPointOfLine(startX, startY, finishX, finishY, angle) {
    let inRadians = (Math.abs(angle) * Math.PI) / 180;
    let startXToUse = undefined;
    let startYToUse = undefined;
    let finishXToUse = undefined;
    let finishYToUse = undefined;
    if (angle < 0) {
      startXToUse = startX;
      startYToUse = startY;
      finishXToUse = finishX;
      finishYToUse = finishY;
    } else {
      startXToUse = finishX;
      startYToUse = finishY;
      finishXToUse = startX;
      finishYToUse = startY;
    }
    let deltaX = finishXToUse - startXToUse;
    let deltaY = finishYToUse - startYToUse;
    const distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
    const halfDistance = distance / 2;
    const factor = Math.tan(Math.PI / 2 - inRadians / 2);
    const distanceToCenter = factor * halfDistance;
    const directionX = deltaY / distance;
    const directionY = -deltaX / distance;
    const centerX =
      (startXToUse + finishXToUse) / 2 + directionX * distanceToCenter;
    const centerY =
      (startYToUse + finishYToUse) / 2 + directionY * distanceToCenter;
    deltaX = startXToUse - centerX;
    deltaY = startYToUse - centerY;
    const radius = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
    return {
      x: centerX - directionX * radius,
      y: centerY - directionY * radius
    };
  }

  /**
   * Adds the curve data to the specified object
   *
   * @param {object} editing - destination
   * @param {number} startX - starting X position
   * @param {number} startY - starting Y position
   * @param {number} finishX - ending X position
   * @param {number} finishY - ending Y position
   * @param {number} angle - line angle
   * @returns {void}
   *
   */
  static createCurveDataIn(editing, startX, startY, finishX, finishY, angle) {
    let inRadians = (Math.abs(angle) * Math.PI) / 180;
    let startXToUse = undefined;
    let startYToUse = undefined;
    let finishXToUse = undefined;
    let finishYToUse = undefined;
    if (angle < 0) {
      startXToUse = startX;
      startYToUse = startY;
      finishXToUse = finishX;
      finishYToUse = finishY;
    } else {
      startXToUse = finishX;
      startYToUse = finishY;
      finishXToUse = startX;
      finishYToUse = startY;
    }
    let deltaX = finishXToUse - startXToUse;
    let deltaY = finishYToUse - startYToUse;
    const distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
    const halfDistance = distance / 2;
    const factor = Math.tan(Math.PI / 2 - inRadians / 2);
    const distanceToCenter = factor * halfDistance;
    const directionX = deltaY / distance;
    const directionY = -deltaX / distance;
    const centerX =
      (startXToUse + finishXToUse) / 2 + directionX * distanceToCenter;
    const centerY =
      (startYToUse + finishYToUse) / 2 + directionY * distanceToCenter;
    deltaX = startXToUse - centerX;
    deltaY = startYToUse - centerY;
    const radius = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
    const centerInCurveX = centerX - directionX * radius;
    const centerInCurveY = centerY - directionY * radius;
    deltaX = centerInCurveX - (finishXToUse + startXToUse) / 2;
    deltaY = centerInCurveY - (finishYToUse + startYToUse) / 2;
    const height = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
    if (editing.curveData === undefined) {
      editing.curveData = {};
    }
    editing.curveData.startX = startX;
    editing.curveData.startY = startY;
    editing.curveData.finishX = finishX;
    editing.curveData.finishY = finishY;
    if (Math.abs(angle) < 1e-5) {
      editing.curveData.length = distance;
    } else {
      editing.curveData.length = radius * inRadians;
    }
    editing.curveData.rise = startY - finishY;
    editing.curveData.run = finishX - startX;
    editing.curveData.angle = angle;
    editing.curveData.chord = distance;
    editing.curveData.height = height;
  }

  /**
   * Adds a line element to the specified Path
   *
   * @param {SketchPath} path - destination
   * @param {number} startX - starting X position
   * @param {number} startY - starting Y position
   * @param {number} finishX - ending X position
   * @param {number} finishY - ending Y position
   * @param {object} element - source line element
   * @returns {void}
   *
   */
  static addLineTo(path, startX, startY, endX, endY, element) {
    let angle = 0;
    if (element.angle !== undefined) {
      angle = element.angle;
    }
    if (angle !== 0) {
      const center = this.centerPointOfLine(startX, startY, endX, endY, angle);
      path.arcTo(center.x, center.y, endX, endY);
    } else {
      path.lineTo(endX, endY);
    }
  }

  /**
   * Adjusts the line length
   *
   * @param {number} amount - source length
   * @returns {number} - destination line length
   *
   */
  static adjustAmount(amount) {
    const amountToUse = amount;
    const nearestIntegerAmountToUse = Math.round(amount);
    if (Math.abs(amountToUse - nearestIntegerAmountToUse) < 1e-5) {
      return nearestIntegerAmountToUse;
    }
    return amountToUse;
  }

  /**
   * Gets the feet length
   *
   * @param {number} amount - source length
   * @returns {string} - destination feet length
   *
   */
  static feetTextFor(amount) {
    const amountToUse = this.adjustAmount(amount);
    return Math.floor(amountToUse).toString();
  }

  /**
   * Gets the inches length
   *
   * @param {number} amount - source length
   * @returns {string} - destination inches length
   *
   */
  static inchesTextFor(amount) {
    const amountToUse = this.adjustAmount(amount);
    const feet = Math.floor(amountToUse);
    return Math.floor((amountToUse - feet) * 12).toString();
  }

  /**
   * Converts the length to string
   *
   * @param {number} amount - source length
   * @returns {string} - destination distance
   *
   */
  static distanceTextFor(amount) {
    const amountToUse = this.adjustAmount(amount);
    const feet = Math.floor(amountToUse);
    const inches = Math.floor((amountToUse - feet) * 12);
    if (inches === 0) {
      return feet.toString() + "'";
    }
    return feet.toString() + "' " + inches.toString() + '"';
  }

  /**
   * Draw a text field
   *
   * @param {SketchControl} sketch - Control instance
   * @param {PointText} text - text field
   * @param {object} position - text position
   * @param {number} content - content text
   * @param {number} background - Rectangle
   * @param {SketchStyle} style - non-CSS style set
   * @param {SketchDistance} attributes - Distance instance
   * @returns {void}
   *
   */
  static drawTextWithBackground(
    sketch,
    text,
    position,
    content,
    background,
    style,
    attributes
  ) {
    sketch.relocateInMainScopeActiveLayer(background);
    sketch.relocateInMainScopeActiveLayer(text);
    sketch.applyTextStyleWithAttributes(text, style, attributes);
    text.content = this.distanceTextFor(content);
    const bounds = text.bounds;
    text.position = position;
    const currentBounds = background.bounds;
    background.position = position;
    background.scale(
      bounds.width / currentBounds.width,
      bounds.height / currentBounds.height
    );
  }

  /**
   * Copies the format of a line to another
   *
   * @param {SketchObject} source - source line
   * @param {SketchObject} destination - destination line
   * @returns {void}
   *
   */
  static copyLineAttributes(source, target) {
    if (source.color !== undefined) {
      target.color = source.color;
    }
    if (source.pattern !== undefined) {
      target.pattern = [];
      for (let i = 0; i < source.pattern.length; i++) {
        target.pattern.push(source.pattern[i]);
      }
    }
    if (source.worldUnits !== undefined) {
      target.worldUnits = source.worldUnits;
    }
    if (source.width !== undefined) {
      target.width = source.width;
    }
  }

  /**
   * Copies the format of a text to another
   *
   * @param {SketchCustomText} source - source text
   * @param {SketchCustomText} destination - destination text
   * @returns {void}
   *
   */
  static copyTextAttributes(source, target) {
    if (source.fontFamily !== undefined) {
      target.fontFamily = source.fontFamily;
    }
    if (source.fontSize !== undefined) {
      target.fontSize = source.fontSize;
    }
    if (source.worldUnits !== undefined) {
      target.worldUnits = source.worldUnits;
    }
    if (source.horizontalAlign !== undefined) {
      target.horizontalAlign = source.horizontalAlign;
    }
    if (source.verticalAlign !== undefined) {
      target.verticalAlign = source.verticalAlign;
    }
    if (source.rotation !== undefined) {
      target.rotation = source.rotation;
    }
    if (source.fontColor !== undefined) {
      target.fontColor = source.fontColor;
    }
    if (source.fontWeight !== undefined) {
      target.fontWeight = source.fontWeight;
    }
    if (source.isLabel) {
      target.isLabel = source.isLabel;
    }
    if (source.path) {
      target.path = source.path;
    }
  }

  /**
   * Compares two line patterns
   *
   * @param {Array} firstPattern - first array
   * @param {Array} secondPattern - second array
   * @returns {boolean}
   *
   */
  static patternsMatch(firstPattern, secondPattern) {
    let match = (firstPattern !== undefined) === (secondPattern !== undefined);
    if (
      firstPattern !== undefined &&
      secondPattern !== undefined &&
      firstPattern.length === secondPattern.length
    ) {
      match = true;
      for (let j = 0; j < firstPattern.length; j++) {
        if (firstPattern[j] !== secondPattern[j]) {
          match = false;
          break;
        }
      }
    }
    return match;
  }

  /**
   * Finds a point within a list
   *
   * @param {Array} pointList - list
   * @param {object} point - point to find
   * @returns {boolean}
   *
   */
  static listContainsPoint(pointList, point) {
    if (pointList && pointList.length > 0 && point) {
      for (let p of pointList) {
        if (p === point) {
          return true;
        }
      }
    }
    return false;
  }
}
