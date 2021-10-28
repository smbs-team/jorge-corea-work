// ptas_SketchCircle.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Points are shown as Circles.
*/
class SketchCircle {
  /**
    * Creates a Circle with the same properties as itself
    *
    * @returns {SketchCircle}
    *
    */
  createCopy() {
    let copy = new SketchCircle();
    copy.center = { x: this.center.x, y: this.center.y };
    copy.radius = this.radius;
    SketchUtils.copyLineAttributes(this, copy);
    return copy;
  }

  /**
   * Enter frame event handler. Updates the Circle on the canvas.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Matrix} transform - Matrix for converting between canvas and offset positions
   * @returns {void}
   */
  draw(sketch, transform) {
    if (this.drawCounter !== sketch.drawCounter) {
      this.drawCounter = sketch.drawCounter;
      this.fillPathCount = 0;
      this.pathCount = 0;
    }
    let position = new paper.Point(this.center.x, this.center.y);
    position = position.transform(transform);
    let style = undefined;
    if (sketch.selected !== undefined && sketch.selected.length > 0) {
      style = sketch.styleSet.objectOutOfSelectionStyle;
    } else {
      style = sketch.styleSet.style;
    }
    let fillPath = undefined;
    if (this.fillPaths === undefined) {
      fillPath = new paper.Path.Circle(position, 1);
      this.fillPaths = [fillPath];
      this.fillPathCount = 1;
    } else if (this.fillPathCount >= this.fillPaths.length) {
      fillPath = new paper.Path.Circle(position, 1);
      this.fillPaths.push(fillPath);
      this.fillPathCount = this.fillPaths.length;
    } else {
      fillPath = this.fillPaths[this.fillPathCount];
      fillPath.position = position;
      this.fillPathCount++;
    }
    sketch.relocateInMainScopeActiveLayer(fillPath);
    style.applyFillTo(fillPath);
    const bounds = fillPath.bounds;
    const radius = this.radius * sketch.resolution;
    fillPath.scale(radius / (bounds.width / 2), radius / (bounds.height / 2));
    let path = undefined;
    if (this.paths === undefined) {
      path = new paper.Path.Circle(position, 1);
      this.paths = [path];
      this.pathCount = 1;
    } else if (this.pathCount >= this.paths.length) {
      path = new paper.Path.Circle(position, 1);
      this.paths.push(path);
      this.pathCount = this.paths.length;
    } else {
      path = this.paths[this.pathCount];
      path.position = position;
      this.pathCount++;
    }
    sketch.relocateInMainScopeActiveLayer(path);
    style.applyStrokeTo(path);
    if (this.color !== undefined) {
      path.strokeColor = this.color;
    }
    if (this.width !== undefined) {
      path.strokeWidth = this.width;
    }
    if (this.pattern !== undefined) {
      path.dashArray = [];
      for (let i = 0; i < this.pattern.length; i++) {
        let entry = this.pattern[i];
        if (this.worldUnits) {
          path.dashArray.push((entry * sketch.resolution) / 12);
        } else {
          path.dashArray.push(entry * 96);
        }
      }
    }
    path.scale(radius / (bounds.width / 2), radius / (bounds.height / 2));
  }

  /**
    * Fills up an object with the bounding box positions of a selection
    *
    * @param {object} bounds - object containing the bounding positions
    * @returns {void}
    *
    */
  getBounds(bounds) {
    if (bounds.started) {
      bounds.min.x = Math.min(bounds.min.x, this.center.x - this.radius);
      bounds.min.y = Math.min(bounds.min.y, this.center.y - this.radius);
      bounds.max.x = Math.max(bounds.max.x, this.center.x + this.radius);
      bounds.max.y = Math.max(bounds.max.y, this.center.y + this.radius);
    } else {
      bounds.min = {
        x: this.center.x - this.radius,
        y: this.center.y - this.radius
      };
      bounds.max = {
        x: this.center.x + this.radius,
        y: this.center.y + this.radius
      };
      bounds.started = true;
    }
  }

  /**
    * Fills up a projected array within the given object with a center-based position of the current Circle instance
    *
    * @param {object} projection - object containing an array named 'projected'
    * @returns {void}
    *
    */
  findProjection(projection) {
    projection.projected.push({
      x: this.center.x - projection.center.x,
      y: this.center.y - projection.center.y
    });
  }

  /**
    * Relocates the current Circle instance in Y based on the given projection
    *
    * @param {object} projection - object containing an array named 'projected' and a number named 'index'
    * @returns {void}
    *
    */
  flipVertically(projection) {
    let index = projection.index;
    this.center.y = projection.center.y - projection.projected[index].y;
    projection.index = index + 1;
  }
}
