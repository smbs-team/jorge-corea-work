// ptas_SketchStyle.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Style applied to PaperJS elements.
*/
class SketchStyle {
  applyStrokeTo(item, noFill) {
    item.strokeColor = this.color;
    if (!noFill)
      item.fillColor = this.color.substr(0, 7) + '08';
    item.strokeWidth = this.width;
    item.dashArray = this.pattern;
  }

  applyFillTo(item, negative = false) {
    item.fillColor = negative ? this.negativeFill : this.fill;
  }

  applyPointStyleTo(point) {
    if (this.point !== undefined) {
      point.fillColor = this.point;
    } else if (this.color !== undefined) {
      point.fillColor = this.color;
    } else {
      point.fillColor = undefined;
    }
    let radius = 1;
    if (this.radius !== undefined) {
      radius = this.radius;
    }
    const bounds = point.bounds;
    point.scale(radius / (bounds.width / 2), radius / (bounds.height / 2));
    point.strokeColor = 'black';
    point.strokeWidth = 1;
  }

  applyArrowStyleTo(arrow) {
    arrow.fillColor = this.color;
  }

  applyExpandTo(bounds) {
    if (this.expand !== undefined) {
      return bounds.expand(this.expand);
    }
    return bounds;
  }

  applyCenterStyleTo(point, deltaX, deltaY) {
    point.applyMatrix = false;
    if (this.center !== undefined) {
      point.fillColor = this.center;
    } else {
      point.fillColor = undefined;
    }
    let radius = 1;
    if (this.centerRadius !== undefined) {
      radius = this.centerRadius;
    }
    point.rotation = 0;
    point.bounds.width = 5;
    point.bounds.height = point.bounds.width * 4;
    point.rotate((Math.atan2(deltaY, deltaX) * 180) / Math.PI);
    point.strokeColor = 'black';
    if (radius > 1) {
      point.strokeWidth = 0.1;
    } else {
      point.strokeWidth = 0;
    }
  }
}
