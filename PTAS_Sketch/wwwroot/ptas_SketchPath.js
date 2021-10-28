// ptas_SketchPath.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Paths are the polygons in a sketch, formed by points, lines (elements), distances, and areas.
*/
class SketchPath {
  get negative() {
    if (this._negative === undefined) {
      this._negative = false;
    }
    return this._negative;
  }

  set negative(value) {
    this._negative = value;
  }

  /**
   * Calculates the inner area of the current Path.
   *
   * @param {SketchControl} sketch - Control instance
   * @returns {number} - Square feet.
   */
  getArea(sketch) {
    if (this.fillPath && this.fillPath.area && sketch.resolution) {
      return (
        Math.abs(this.fillPath.area) / (sketch.resolution * sketch.resolution)
      );
    }
    return 0;
  }

  /**
   * Copies the current Path's properties into a new instance.
   *
   * @returns {SketchPath} - New copy.
   */
  createCopy() {
    let copy = new SketchPath();
    if (this.uniqueIdentifier) {
      copy.uniqueIdentifier = this.uniqueIdentifier;
    }
    if (this.start) {
      copy.start = { x: this.start.x, y: this.start.y };
    }
    if (this.elements) {
      copy.elements = [];
      for (let i = 0; i < this.elements.length; i++) {
        let element = {
          x: this.elements[i].x,
          y: this.elements[i].y,
          angle: this.elements[i].angle
        };
        SketchUtils.copyLineAttributes(this.elements[i], element);
        copy.elements.push(element);
      }
    }
    copy.closed = this.closed;
    copy.negative = this.negative;
    copy.label = this.label;
    copy.arrow = this.arrow;
    copy.layer = this.layer
    if (this.fillPath) {
      copy.fillPath = this.fillPath.clone();
    }
    return copy;
  }

  /**
   * Determines whether the current Path has a point selected.
   *
   * @param {object} point - Path point
   * @param {SketchControl} sketch - Control instance
   * @returns {boolean} - Selected found
   */
  pointIsSelected(point, sketch) {
    if (sketch.selected !== undefined) {
      for (let i = 0; i < sketch.selected.length; i++) {
        if (sketch.selected[i].point === point) {
          return true;
        }
      }
    }
    return false;
  }

  /**
   * Determines whether the current Path has a line selected.
   *
   * @param {object} element - Path line end point
   * @param {SketchControl} sketch - Control instance
   * @returns {boolean} - Selected found
   */
  elementIsSelected(element, sketch) {
    if (sketch.selected !== undefined) {
      for (let i = 0; i < sketch.selected.length; i++) {
        if (sketch.selected[i].element === element) {
          return true;
        }
      }
    }
    return false;
  }

  /**
   * Enter frame event handler. Updates the Path on the canvas.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Matrix} transform - Matrix for converting between canvas and offset positions
   * @param {SketchLayer} parentLayer - Layer containing the current Path.
   * @returns {void}
   */
  draw(sketch, transform, parentLayer) {
    if (this.drawCounter !== sketch.drawCounter) {
      this.drawCounter = sketch.drawCounter;
      this.pathCount = 0;
      this.pointCount = 0;
      this.centerPointCount = 0;
    }
    let coords = undefined;
    let withinBounds = false;
    if (this.start !== undefined) {
      let sourceCoords = [this.start.x, this.start.y];
      if (this.elements !== undefined) {
        for (let i = 0; i < this.elements.length; i++) {
          sourceCoords.push(this.elements[i].x);
          sourceCoords.push(this.elements[i].y);
        }
      }
      coords = [];
      transform.transform(sourceCoords, coords, sourceCoords.length / 2);
      let started = false;
      let minX = undefined;
      let maxX = undefined;
      let minY = undefined;
      let maxY = undefined;
      for (let i = 0; i < coords.length; i += 2) {
        if (started) {
          minX = Math.min(minX, coords[i]);
          maxX = Math.max(maxX, coords[i]);
          maxY = Math.max(maxY, coords[i + 1]);
          minY = Math.min(minY, coords[i + 1]);
        } else {
          minX = coords[i];
          maxX = coords[i];
          maxY = coords[i + 1];
          minY = coords[i + 1];
          started = true;
        }
      }
      withinBounds =
        minX < sketch.root.clientWidth &&
        maxX > 0 &&
        minY < sketch.root.clientHeight &&
        maxY > 0;
    }
    if (!withinBounds) {
      return;
    }
    let style = undefined;
    let isSelected = false;
    if (sketch.selected !== undefined) {
      for (let i = 0; i < sketch.selected.length; i++) {
        if (sketch.selected[i].path === this) {
          isSelected = true;
          break;
        }
      }
    }
    if (isSelected) {
      if (sketch.mode == SketchMode.MultipleSelect) {
        style = sketch.styleSet.objectGroupSelectionStyle;
      } else {
        style = sketch.styleSet.objectSelectionStyle;
      }
    } else if (sketch.selected !== undefined && sketch.selected.length > 0) {
      style = sketch.styleSet.objectOutOfSelectionStyle;
    } else if (this.closed) {
      style = sketch.styleSet.style;
    } else {
      style = sketch.styleSet.openObjectStyle;
    }
    if (coords !== undefined && coords.length >= 6) {
      let startX = coords[0];
      let startY = coords[1];
      if (this.fillPath === undefined) {
        this.fillPath = new paper.Path();
      } else {
        this.fillPath.removeSegments();
      }
      if (this.closed) {
        sketch.relocateInMainScopeActiveLayer(this.fillPath);
        style.applyFillTo(this.fillPath, this.negative);
      }
      this.fillPath.moveTo(startX, startY);
      let elementIndex = 0;
      for (let i = 2; i < coords.length; i += 2) {
        let finishX = coords[i];
        let finishY = coords[i + 1];
        SketchUtils.addLineTo(
          this.fillPath,
          startX,
          startY,
          finishX,
          finishY,
          this.elements[elementIndex]
        );
        startX = finishX;
        startY = finishY;
        elementIndex++;
      }
      this.fillPath.closed = true;
    } else {
      this.fillPath = undefined;
    }
    if (coords !== undefined && coords.length >= 4) {
      let startX = coords[0];
      let startY = coords[1];
      let previousStyle = undefined;
      let previousColor = undefined;
      let previousPattern = undefined;
      let previousWorldUnits = undefined;
      let previousWidth = undefined;
      let elementIndex = 0;
      for (let i = 2; i < coords.length; i += 2) {
        let finishX = coords[i];
        let finishY = coords[i + 1];
        let style = undefined;
        let color = undefined;
        let pattern = undefined;
        let worldUnits = undefined;
        let width = undefined;
        if (this.elementIsSelected(this.elements[elementIndex], sketch)) {
          style = sketch.styleSet.shapeSelectionStyle;
        } else if (isSelected) {
          if (sketch.mode == SketchMode.MultipleSelect) {
            style = sketch.styleSet.objectGroupSelectionStyle;
          } else {
            style = sketch.styleSet.objectSelectionStyle;
          }
        } else if (
          sketch.selected !== undefined &&
          sketch.selected.length > 0
        ) {
          style = sketch.styleSet.objectOutOfSelectionStyle;
          color = this.elements[elementIndex].color;
          pattern = this.elements[elementIndex].pattern;
          worldUnits = this.elements[elementIndex].worldUnits;
          width = this.elements[elementIndex].width;
        } else if (this.closed) {
          style = sketch.styleSet.style;
          color = this.elements[elementIndex].color;
          pattern = this.elements[elementIndex].pattern;
          worldUnits = this.elements[elementIndex].worldUnits;
          width = this.elements[elementIndex].width;
        } else {
          style = sketch.styleSet.openObjectStyle;
          color = this.elements[elementIndex].color;
          pattern = this.elements[elementIndex].pattern;
          worldUnits = this.elements[elementIndex].worldUnits;
          width = this.elements[elementIndex].width;
        }
        let path = undefined;
        if (
          previousStyle === style &&
          previousColor === color &&
          SketchUtils.patternsMatch(pattern, previousPattern) &&
          previousWorldUnits === worldUnits &&
          previousWidth === width
        ) {
          path = this.paths[this.pathCount - 1];
        } else {
          if (this.paths === undefined) {
            path = new paper.Path();
            this.paths = [path];
            this.pathCount = 1;
          } else if (this.pathCount >= this.paths.length) {
            path = new paper.Path();
            this.paths.push(path);
            this.pathCount = this.paths.length;
          } else {
            path = this.paths[this.pathCount];
            path.removeSegments();
            path.closed = false;
            this.pathCount++;
          }
          let layer = sketch.getLayer(parentLayer);
          if (!layer) {
            //Lost its parent layer
            layer = sketch.layers.find(layer => layer.name == "Scratchpad");
          }
          if (this.tile) {
            style.width = 1;
            style.color = "#888888";
            style.pattern = [1, 0];
          } else if (this.arrow && sketch.layerList[layer.name]) {
            style.width = sketch.layerList[layer.name].style.strokeWidth;
            style.color = "#000000";
            style.pattern = [1, 0];
          } else if (style !== sketch.styleSet.shapeSelectionStyle) {
            if (
              sketch.layerList[layer.name] &&
              sketch.layerList[layer.name].style
            ) {
              style.width = sketch.layerList[layer.name].style.strokeWidth;
              style.color = sketch.layerList[layer.name].style.strokeColor;
              style.pattern = sketch.layerList[layer.name].style.strokePattern;
            } else {
              style.width = 2;
              style.color = "#000000";
              style.pattern = [1, 0];
            }
          }
          sketch.relocateInMainScopeActiveLayer(path);
          style.color = style.color.substr(0, 7)
          if (parentLayer == sketch.sketchLayerToEdit) {
            this.locked = false;
          } else {
            this.locked = true;
            style.color += "90";
          }
          style.applyStrokeTo(path, this.tile);
          if (color !== undefined) {
            path.strokeColor = color;
          }
          if (width !== undefined) {
            path.strokeWidth = width;
          }
          if (pattern !== undefined) {
            path.dashArray = [];
            for (let j = 0; j < pattern.length; j++) {
              let entry = pattern[j];
              if (worldUnits) {
                path.dashArray.push((entry * sketch.resolution) / 12);
              } else {
                path.dashArray.push(entry * 96);
              }
            }
          }
          path.moveTo(startX, startY);
          if (this.arrow) {
            paper.project.activeLayer.insertChild(0, path);
          }
        }
        SketchUtils.addLineTo(
          path,
          startX,
          startY,
          finishX,
          finishY,
          this.elements[elementIndex]
        );
        previousStyle = style;
        previousColor = color;
        previousPattern = pattern;
        previousWorldUnits = worldUnits;
        previousWidth = width;
        startX = finishX;
        startY = finishY;
        elementIndex++;
      }
    }
    let firstPointIndex = this.pointCount;
    if (coords !== undefined && coords.length >= 2) {
      let startX = coords[0];
      let startY = coords[1];
      let position = new paper.Point(startX, startY);
      if (this.points === undefined) {
        let point = new paper.Path.Circle(position, 1);
        this.points = [point];
        this.pointCount = 1;
      } else if (this.pointCount >= this.points.length) {
        let point = new paper.Path.Circle(position, 1);
        this.points.push(point);
        this.pointCount = this.points.length;
      } else {
        let point = this.points[this.pointCount];
        point.position = position;
        this.pointCount++;
      }
      let elementIndex = 0;
      for (let i = 2; i < coords.length; i += 2) {
        let x = coords[i];
        let y = coords[i + 1];
        if (
          this.closed &&
          elementIndex === this.elements.length - 1 &&
          x === startX &&
          y === startY
        ) {
          break;
        }
        let position = new paper.Point(x, y);
        if (this.points === undefined) {
          let point = new paper.Path.Circle(position, 1);
          this.points = [point];
          this.pointCount = 1;
        } else if (this.pointCount >= this.points.length) {
          let point = new paper.Path.Circle(position, 1);
          this.points.push(point);
          this.pointCount = this.points.length;
        } else {
          let point = this.points[this.pointCount];
          point.position = position;
          this.pointCount++;
        }
      }
    }
    let firstSelectedPoint = -1;
    let secondSelectedPoint = -1;
    if (this.elements !== undefined) {
      for (let i = 0; i < this.elements.length; i++) {
        if (this.arrow) {
          style = sketch.styleSet.arrowheadStyle;
          const arrowhead = new paper.Path.RegularPolygon(
            new paper.Point(
              this.points[1].position.x,
              this.points[1].position.y + 2
            ),
            3,
            style.radius
          );
          style.applyArrowStyleTo(arrowhead);
          arrowhead.rotate(
            (Math.atan2(
              this.points[1].position.y - this.points[0].position.y,
              this.points[1].position.x - this.points[0].position.x
            ) *
              180) /
            Math.PI +
            90
          );
          sketch.relocateInMainScopeActiveLayer(arrowhead);
        } else if (this.elementIsSelected(this.elements[i], sketch)) {
          style = sketch.styleSet.shapeSelectionStyle;
          firstSelectedPoint = i;
          secondSelectedPoint = firstSelectedPoint + 1;
          if (secondSelectedPoint >= this.pointCount) {
            secondSelectedPoint = 0;
          }
          firstSelectedPoint += firstPointIndex;
          secondSelectedPoint += firstPointIndex;
          sketch.relocateInMainScopeActiveLayer(
            this.points[firstSelectedPoint]
          );
          style.applyPointStyleTo(this.points[firstSelectedPoint]);
          sketch.relocateInMainScopeActiveLayer(
            this.points[secondSelectedPoint]
          );
          style.applyPointStyleTo(this.points[secondSelectedPoint]);
          break;
        }
      }
    }
    let point0Selected = false;
    if (
      this.start !== undefined &&
      this.pointIsSelected(this.start, sketch) &&
      this.pointCount > firstPointIndex &&
      firstSelectedPoint !== firstPointIndex &&
      secondSelectedPoint !== firstPointIndex
    ) {
      style = sketch.styleSet.shapeSelectionStyle;
      sketch.relocateInMainScopeActiveLayer(this.points[firstPointIndex]);
      style.applyPointStyleTo(this.points[firstPointIndex]);
      point0Selected = true;
    }
    if (this.elements !== undefined) {
      for (let i = 0; i < this.elements.length; i++) {
        let pointIndex = i + 1;
        if (pointIndex === this.elements.length) {
          if (point0Selected) {
            break;
          }
          pointIndex = 0;
        }
        pointIndex += firstPointIndex;
        if (
          pointIndex !== firstSelectedPoint &&
          pointIndex !== secondSelectedPoint
        ) {
          style = undefined;
          if (this.pointIsSelected(this.elements[i], sketch)) {
            style = sketch.styleSet.shapeSelectionStyle;
          } else if (isSelected) {
            if (
              sketch.mode == SketchMode.MultipleSelect ||
              !sketch.selected[0].curveData
            ) {
              style = sketch.styleSet.objectGroupSelectionStyle;
            } else {
              style = sketch.styleSet.objectSelectionStyle;
            }
          } else if (
            sketch.selected !== undefined &&
            sketch.selected.length > 0
          ) {
            style = sketch.styleSet.objectOutOfSelectionStyle;
          }
          if (style !== undefined) {
            sketch.relocateInMainScopeActiveLayer(this.points[pointIndex]);
            style.applyPointStyleTo(this.points[pointIndex]);
          }
        }
      }
    }
    if (coords !== undefined && coords.length >= 4) {
      let startX = coords[0];
      let startY = coords[1];
      let elementIndex = 0;
      for (let i = 2; i < coords.length; i += 2) {
        const element = this.elements[elementIndex];
        let finishX = coords[i];
        let finishY = coords[i + 1];
        if (this.elementIsSelected(element, sketch)) {
          style = sketch.styleSet.shapeSelectionStyle;
        } else if (isSelected) {
          if (sketch.mode == SketchMode.MultipleSelect) {
            style = sketch.styleSet.objectGroupSelectionStyle;
          } else {
            style = sketch.styleSet.objectSelectionStyle;
          }
        } else if (
          sketch.selected !== undefined &&
          sketch.selected.length > 0
        ) {
          style = sketch.styleSet.objectOutOfSelectionStyle;
        } else if (this.closed) {
          style = sketch.styleSet.style;
        } else {
          style = sketch.styleSet.openObjectStyle;
        }
        let center = undefined;
        let angle = 0;
        if (element.angle !== undefined) {
          angle = element.angle;
        }
        if (angle !== 0) {
          center = SketchUtils.centerPointOfLine(
            startX,
            startY,
            finishX,
            finishY,
            angle
          );
        } else {
          center = { x: (startX + finishX) / 2, y: (startY + finishY) / 2 };
        }
        let position = new paper.Point(center.x, center.y);
        let point = undefined;
        if (this.centerPoints === undefined) {
          point = new paper.Path.Circle(position, 1);
          this.centerPoints = [point];
          this.centerPointCount = 1;
        } else if (this.centerPointCount >= this.centerPoints.length) {
          point = new paper.Path.Circle(position, 1);
          this.centerPoints.push(point);
          this.centerPointCount = this.centerPoints.length;
        } else {
          point = this.centerPoints[this.centerPointCount];
          point.position = position;
          this.centerPointCount++;
        }
        sketch.relocateInMainScopeActiveLayer(point);
        style.applyCenterStyleTo(point, finishX - startX, finishY - startY);
        startX = finishX;
        startY = finishY;
        elementIndex++;
      }
    }
  }

  /**
   * Determines whether the specified point is within a selectable distance.
   *
   * @param {number} deltaX - X position
   * @param {number} deltaY - Y position
   * @returns {boolean}
   */
  distanceWithinRange(deltaX, deltaY) {
    return deltaX * deltaX + deltaY * deltaY < 10 * 10;
  }

  /**
   * Returns a path selection candidate.
   *
   * @param {number} positionX
   * @param {number} positionY
   * @param {Matrix} transform - Matrix for converting between canvas and offset positions
   * @param {boolean} pathLevelOnly - Skip the point candidates if false.
   * @param {boolean} skipInsidePath - Skip the path candidate if true.
   * @returns {object} - Object with a path property
   */
  createSelectionCandidate(
    positionX,
    positionY,
    transform,
    pathLevelOnly,
    skipInsidePath
  ) {
    if (this.start !== undefined) {
      let point = new paper.Point(this.start.x, this.start.y);
      point = point.transform(transform);
      if (this.distanceWithinRange(point.x - positionX, point.y - positionY)) {
        if (pathLevelOnly) {
          return { path: this };
        } else {
          return { path: this, point: this.start };
        }
      }
    }
    if (this.elements !== undefined) {
      for (let i = 0; i < this.elements.length; i++) {
        let point = new paper.Point(this.elements[i].x, this.elements[i].y);
        point = point.transform(transform);
        if (
          this.distanceWithinRange(point.x - positionX, point.y - positionY)
        ) {
          if (pathLevelOnly) {
            return { path: this };
          } else {
            return { path: this, point: this.elements[i] };
          }
        }
      }
    }
    if (this.start !== undefined && this.elements !== undefined) {
      let start = new paper.Point(this.start.x, this.start.y);
      start = start.transform(transform);
      for (let i = 0; i < this.elements.length; i++) {
        let end = new paper.Point(this.elements[i].x, this.elements[i].y);
        end = end.transform(transform);
        let angle = 0;
        if (this.elements[i].angle !== undefined) {
          angle = this.elements[i].angle;
        }
        if (angle !== 0) {
          let inRadians = (Math.abs(angle) * Math.PI) / 180;
          let startXToUse = undefined;
          let startYToUse = undefined;
          let endXToUse = undefined;
          let endYToUse = undefined;
          if (angle < 0) {
            startXToUse = start.x;
            startYToUse = start.y;
            endXToUse = end.x;
            endYToUse = end.y;
          } else {
            startXToUse = end.x;
            startYToUse = end.y;
            endXToUse = start.x;
            endYToUse = start.y;
          }
          let deltaX = endXToUse - startXToUse;
          let deltaY = endYToUse - startYToUse;
          let distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
          if (distance > 0) {
            const halfDistance = distance / 2;
            const factor = Math.tan(Math.PI / 2 - inRadians / 2);
            const distanceToCenter = factor * halfDistance;
            const directionX = deltaY / distance;
            const directionY = -deltaX / distance;
            const centerX =
              (startXToUse + endXToUse) / 2 + directionX * distanceToCenter;
            const centerY =
              (startYToUse + endYToUse) / 2 + directionY * distanceToCenter;
            deltaX = start.x - centerX;
            deltaY = start.y - centerY;
            const radius = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
            deltaX = positionX - centerX;
            deltaY = positionY - centerY;
            distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
            if (Math.abs(distance - radius) < 10) {
              const delta1X = endXToUse - startXToUse;
              const delta1Y = endYToUse - startYToUse;
              const delta2X = positionX - centerX;
              const delta2Y = positionY - centerY;
              const determinant = -delta2X * delta1Y + delta1X * delta2Y;
              if (determinant !== 0) {
                const s =
                  (-delta1Y * (startXToUse - centerX) +
                    delta1X * (startYToUse - centerY)) /
                  determinant;
                const t =
                  (delta2X * (startYToUse - centerY) -
                    delta2Y * (startXToUse - centerX)) /
                  determinant;
                if (s >= 0 && s <= 1 && t >= 0 && t <= 1) {
                  if (pathLevelOnly) {
                    return { path: this };
                  } else {
                    const centerInCurveX = centerX - directionX * radius;
                    const centerInCurveY = centerY - directionY * radius;
                    if (
                      this.distanceWithinRange(
                        positionX - centerInCurveX,
                        positionY - centerInCurveY
                      )
                    ) {
                      return {
                        path: this,
                        element: this.elements[i],
                        center: { x: centerInCurveX, y: centerInCurveY }
                      };
                    } else {
                      return { path: this, element: this.elements[i] };
                    }
                  }
                }
              }
            }
          }
        } else {
          const segmentX = end.x - start.x;
          const segmentY = end.y - start.y;
          let length = Math.sqrt(segmentX * segmentX + segmentY * segmentY);
          if (length > 0) {
            const directionX = segmentX / length;
            const directionY = segmentY / length;
            let deltaX = positionX - start.x;
            let deltaY = positionY - start.y;
            const projection = deltaX * directionX + deltaY * directionY;
            if (projection >= 0 && projection < length) {
              const projectedX = start.x + projection * directionX;
              const projectedY = start.y + projection * directionY;
              const centerX = (start.x + end.x) / 2;
              const centerY = (start.y + end.y) / 2;
              if (
                this.distanceWithinRange(
                  positionX - projectedX,
                  positionY - projectedY
                )
              ) {
                if (pathLevelOnly) {
                  return { path: this };
                } else {
                  if (
                    this.distanceWithinRange(
                      positionX - centerX,
                      positionY - centerY
                    )
                  ) {
                    return {
                      path: this,
                      element: this.elements[i],
                      center: { x: centerX, y: centerY }
                    };
                  } else {
                    return { path: this, element: this.elements[i] };
                  }
                }
              }
            }
          }
        }
        start = end;
      }
    }
    if (
      !skipInsidePath &&
      this.closed &&
      this.fillPath &&
      this.fillPath.contains(new paper.Point(positionX, positionY))
    ) {
      return { path: this };
    }
  }

  /**
    * Fills up an object with the bounding box positions of a selection
    *
    * @param {object} bounds - object containing the bounding positions
    * @returns {void}
    *
    */
  getBounds(bounds) {
    if (this.elements && this.elements[this.elements.length - 1].x) {
      if (bounds.started) {
        bounds.min.x = Math.min(bounds.min.x, this.start.x);
        bounds.min.y = Math.min(bounds.min.y, this.start.y);
        bounds.max.x = Math.max(bounds.max.x, this.start.x);
        bounds.max.y = Math.max(bounds.max.y, this.start.y);
      } else {
        bounds.min = { x: this.start.x, y: this.start.y };
        bounds.max = { x: this.start.x, y: this.start.y };
        bounds.started = true;
      }
      for (let i = 0; i < this.elements.length; i++) {
        bounds.min.x = Math.min(bounds.min.x, this.elements[i].x);
        bounds.min.y = Math.min(bounds.min.y, this.elements[i].y);
        bounds.max.x = Math.max(bounds.max.x, this.elements[i].x);
        bounds.max.y = Math.max(bounds.max.y, this.elements[i].y);
      }
    }
  }

  /**
    * Fills up a projected array within the given object with a center-based position of the current Path instance
    *
    * @param {object} projection - object containing an array named 'projected'
    * @returns {void}
    *
    */
  findProjection(projection) {
    if (this.start !== undefined) {
      projection.projected.push({
        x: this.start.x - projection.center.x,
        y: this.start.y - projection.center.y
      });
      if (this.elements !== undefined) {
        for (let i = 0; i < this.elements.length; i++) {
          projection.projected.push({
            x: this.elements[i].x - projection.center.x,
            y: this.elements[i].y - projection.center.y
          });
        }
      }
    }
  }

  /**
    * Relocates the current Path instance in X based on the given projection
    *
    * @param {object} projection - object containing an array named 'projected' and a number named 'index'
    * @param {SketchLayer} layer - layer containing the current Path
    * @returns {void}
    *
    */
  flipHorizontally(projection, layer) {
    if (this.start) {
      let index = projection.index;
      this.start.x = projection.center.x - projection.projected[index].x;
      index++;
      if (this.elements) {
        for (let i = 0; i < this.elements.length; i++) {
          this.elements[i].x =
            projection.center.x - projection.projected[index].x;
          index++;
        }
        const label = layer.objects.find(
          object => object.objects[0].uniqueIdentifier == this.label
        );
        if (label) {
          const bounds = {};
          this.getBounds(bounds);
          parent.sketchControl.setPosition(
            label.objects[0],
            bounds.min.x + (bounds.max.x - bounds.min.x) / 2,
            bounds.min.y + (bounds.max.y - bounds.min.y) / 2
          );
        }
      }
      projection.index = index;
    }
  }

  /**
    * Relocates the current Path instance in Y based on the given projection
    *
    * @param {object} projection - object containing an array named 'projected' and a number named 'index'
    * @param {SketchLayer} layer - layer containing the current Path
    * @returns {void}
    *
    */
  flipVertically(projection, layer) {
    if (this.start) {
      let index = projection.index;
      this.start.y = projection.center.y - projection.projected[index].y;
      index++;
      if (this.elements) {
        for (let i = 0; i < this.elements.length; i++) {
          this.elements[i].y =
            projection.center.y - projection.projected[index].y;
          if (this.elements[i].angle !== undefined) {
            this.elements[i].angle = -this.elements[i].angle;
          }
          index++;
        }
        const label = layer.objects.find(
          object => object.objects[0].uniqueIdentifier == this.label
        );
        if (label) {
          const bounds = {};
          this.getBounds(bounds);
          parent.sketchControl.setPosition(
            label.objects[0],
            bounds.min.x + (bounds.max.x - bounds.min.x) / 2,
            bounds.min.y + (bounds.max.y - bounds.min.y) / 2
          );
        }
      }
      projection.index = index;
    }
  }

  /**
    * Rotates the current Path -90º.
    *
    * @param {object} projection - object containing an array named 'projected' and a number named 'index'
    * @param {SketchLayer} layer - layer containing the current Path
    * @returns {void}
    *
    */
  rotateLeft(projection, layer) {
    if (this.start) {
      let index = projection.index;
      this.start.x = projection.center.x - projection.projected[index].y;
      this.start.y = projection.center.y + projection.projected[index].x;
      index++;
      if (this.elements) {
        for (let i = 0; i < this.elements.length; i++) {
          this.elements[i].x =
            projection.center.x - projection.projected[index].y;
          this.elements[i].y =
            projection.center.y + projection.projected[index].x;
          index++;
        }
        const label = layer.objects.find(
          object => object.objects[0].uniqueIdentifier == this.label
        );
        if (label) {
          const bounds = {};
          this.getBounds(bounds);
          parent.sketchControl.setPosition(
            label.objects[0],
            bounds.min.x + (bounds.max.x - bounds.min.x) / 2,
            bounds.min.y + (bounds.max.y - bounds.min.y) / 2
          );
        }
      }
      projection.index = index;
    }
  }

  /**
    * Rotates the current Path 90º.
    *
    * @param {object} projection - object containing an array named 'projected' and a number named 'index'
    * @param {SketchLayer} layer - layer containing the current Path
    * @returns {void}
    *
    */
  rotateRight(projection, layer) {
    if (this.start) {
      let index = projection.index;
      this.start.x = projection.center.x + projection.projected[index].y;
      this.start.y = projection.center.y - projection.projected[index].x;
      index++;
      if (this.elements) {
        for (let i = 0; i < this.elements.length; i++) {
          this.elements[i].x =
            projection.center.x + projection.projected[index].y;
          this.elements[i].y =
            projection.center.y - projection.projected[index].x;
          index++;
        }
        const label = layer.objects.find(
          object => object.objects[0].uniqueIdentifier == this.label
        );
        if (label) {
          const bounds = {};
          this.getBounds(bounds);
          parent.sketchControl.setPosition(
            label.objects[0],
            bounds.min.x + (bounds.max.x - bounds.min.x) / 2,
            bounds.min.y + (bounds.max.y - bounds.min.y) / 2
          );
        }
      }
      projection.index = index;
    }
  }

  /**
   * Deletes the specified object.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {SketchObject} selected - Selected object
   * @param {SketchObject} parent - Object container
   * @returns {boolean} - Located the selected object
   */
  deleteSelection(sketch, selected, parent) {
    if (selected.path && selected.path === this) {
      if (this.paths) {
        for (let i = 0; i < this.paths.length; i++) {
          this.paths[i].remove();
        }
      }
      if (this.points) {
        for (let i = 0; i < this.points.length; i++) {
          this.points[i].remove();
        }
      }
      for (let i = 0; i < parent.objects.length; i++) {
        if (parent.objects[i] === this) {
          parent.objects.splice(i, 1);
          break;
        }
      }
      if (sketch.sketchPathToEdit === this) {
        sketch.sketchPathToEdit = undefined;
        sketch.sketchObjectToEdit = undefined;
      }
      return true;
    }
    return false;
  }

  /**
   * Fills the closest parameter with the closest point from the position parameter.
   *
   * @param {object} position - Reference point
   * @param {object} closest - Destination object
   * @param {object/Array} exclude - Can be a reference of a point that should be excluded,
   * or it could be an array of points (references) that should be excluded
   * @returns {void}
   */
  closestPointTo(position, closest, exclude) {
    if (
      this.start &&
      ((!Array.isArray(exclude) && this.start !== exclude) ||
        (Array.isArray(exclude) &&
          !SketchUtils.listContainsPoint(exclude, this.start)))
    ) {
      const deltaX = this.start.x - position.x;
      const deltaY = this.start.y - position.y;
      const distanceSquared = deltaX * deltaX + deltaY * deltaY;
      if (
        closest.distanceSquared === undefined ||
        closest.distanceSquared > distanceSquared
      ) {
        closest.position = this.start;
        closest.distanceSquared = distanceSquared;
      }
    }
    if (this.elements) {
      for (let i = 0; i < this.elements.length; i++) {
        if (
          (!Array.isArray(exclude) && this.elements[i] !== exclude) ||
          (Array.isArray(exclude) &&
            !SketchUtils.listContainsPoint(exclude, this.elements[i]))
        ) {
          const deltaX = this.elements[i].x - position.x;
          const deltaY = this.elements[i].y - position.y;
          const distanceSquared = deltaX * deltaX + deltaY * deltaY;
          if (
            (closest.distanceSquared === undefined || closest.distanceSquared > distanceSquared) && (!Array.isArray(exclude) ||
              Array.isArray(exclude) && !exclude.find(exc => exc.x == this.elements[i].x && exc.y == this.elements[i].y))
          ) {
            closest.position = this.elements[i];
            closest.distanceSquared = distanceSquared;
          }
        }
      }
    }
  }

  /**
   * Inverts the point/element order within the Path, setting the start point as the end point.
   *
   * @returns {void}
   */
  reverse() {
    if (this.elements && this.elements.length > 0) {
      let oldStart = this.start;
      let oldElements = this.elements;
      let index = oldElements.length - 1;
      this.start = { x: oldElements[index].x, y: oldElements[index].y };
      this.elements = [];
      while (index >= 0) {
        let element = {};
        if (oldElements[index].angle !== undefined) {
          element.angle = -oldElements[index].angle;
        }
        index--;
        if (index >= 0) {
          element.x = oldElements[index].x;
          element.y = oldElements[index].y;
        } else {
          element.x = oldStart.x;
          element.y = oldStart.y;
        }

        this.elements.push(element);
      }
      this.reverted = !this.reverted;
    }
  }
}
