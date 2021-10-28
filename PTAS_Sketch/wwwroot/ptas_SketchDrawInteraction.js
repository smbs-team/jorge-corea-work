// ptas_SketchDrawInteraction.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Interactions for the Draw mode
*/
class SketchDrawInteraction {
  /**
   * Updates the cursor's position in Draw mode
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Input event
   * @returns {boolean} - Whether the cursor is repositioned.
   */
  performMove(sketch, event) {
    let reposition = false;
    const position = event
      ? sketch.inverseTopLevelTransform.transform(
        new paper.Point(event.offsetX, event.offsetY)
      )
      : new paper.Point(sketch.cursor.x, sketch.cursor.y);
    this.updateCursor = true;
    if (this.editing) {
      if (this.editing.center) {
        this.updateCursor = false;
        if (this.history === undefined) {
          this.history = sketch.createHistory();
        }
        let deltaX = position.x - this.midX;
        let deltaY = position.y - this.midY;
        const projection = deltaX * this.directionX + deltaY * this.directionY;
        const toPointX = this.midX + projection * this.directionX;
        const toPointY = this.midY + projection * this.directionY;
        position.x = toPointX;
        position.y = toPointY;
        const startToPointDeltaX = toPointX - this.startX;
        const startToPointDeltaY = toPointY - this.startY;
        const endToPointDeltaX = toPointX - this.endX;
        const endToPointDeltaY = toPointY - this.endY;
        const angleCosine =
          (startToPointDeltaX * endToPointDeltaX +
            startToPointDeltaY * endToPointDeltaY) /
          (Math.sqrt(
            startToPointDeltaX * startToPointDeltaX +
            startToPointDeltaY * startToPointDeltaY
          ) *
            Math.sqrt(
              endToPointDeltaX * endToPointDeltaX +
              endToPointDeltaY * endToPointDeltaY
            ));
        let angle = Math.PI - Math.acos(angleCosine);
        if (projection < 0) {
          angle = -angle;
        }
        angle = Math.max(Math.min(Math.PI / 2, angle), -Math.PI / 2);
        this.editing.element.angle = Math.round((2 * angle * 180) / Math.PI);
        SketchUtils.createCurveDataIn(
          sketch.editing,
          this.startX,
          this.startY,
          this.endX,
          this.endY,
          this.editing.element.angle
        );
        if (sketch.editingValuesChanged !== undefined) {
          sketch.editingValuesChanged(sketch.editing);
        }
        reposition = true;
      }
    } else {
      let adjusted = false;
      if (sketch.snapToLine) {
        let lastPoint = undefined;
        if (sketch.sketchPathToEdit !== undefined) {
          if (sketch.sketchPathToEditPoint === 0) {
            lastPoint = sketch.sketchPathToEdit.start;
          } else {
            lastPoint =
              sketch.sketchPathToEdit.elements[
              sketch.sketchPathToEditPoint - 1
              ];
          }
        }
        let closest = {};
        sketch.closestPointTo(position, closest, lastPoint);
        if (
          closest.distanceSquared !== undefined &&
          closest.distanceSquared < 1
        ) {
          position.x = closest.position.x;
          position.y = closest.position.y;
          adjusted = true;
        }
      }
      if (!adjusted && sketch.sketchPathToEditPoint) {
        if (sketch.sketchPathToEdit !== undefined) {
          let lastPoint = undefined;
          if (sketch.sketchPathToEditPoint === 0) {
            lastPoint = sketch.sketchPathToEdit.start;
          } else {
            lastPoint =
              sketch.sketchPathToEdit.elements[
              sketch.sketchPathToEditPoint - 1
              ];
          }
          if (lastPoint) {
            const deltaX = position.x - lastPoint.x;
            const deltaY = position.y - lastPoint.y;
            sketch.addToPosition(position, lastPoint, deltaX, deltaY);
            adjusted = true;
          }
        }
      }
      if (!adjusted) {
        sketch.adjustPosition(position);
      }
      if (sketch.penDown && sketch.sketchPathToEdit) {
        sketch.relocateToJumpTo(position);
        if (sketch.sketchPathToEditPoint === 0) {
          if (this.history === undefined) {
            this.history = sketch.createHistory();
          }
          sketch.sketchPathToEdit.start.x = position.x;
          sketch.sketchPathToEdit.start.y = position.y;
          reposition = true;
        } else {
          const tooClose = sketch.isTooCloseToPath(
            position,
            sketch.sketchPathToEdit
          );
          if (tooClose.closestPoint && !tooClose.tooClose) {
            if (this.history === undefined) {
              this.history = sketch.createHistory();
            }
            sketch.sketchPathToEdit.elements[
              sketch.sketchPathToEditPoint - 1
            ].x = position.x;
            sketch.sketchPathToEdit.elements[
              sketch.sketchPathToEditPoint - 1
            ].y = position.y;
            sketch.sketchPathToEdit.closed = false;
            reposition = true;
          } else if (
            tooClose.closestPoint === 0 &&
            sketch.sketchPathToEdit.elements !== undefined &&
            sketch.sketchPathToEdit.elements.length >= 2
          ) {
            if (this.history === undefined) {
              this.history = sketch.createHistory();
            }
            sketch.sketchPathToEdit.elements[
              sketch.sketchPathToEditPoint - 1
            ] = {
              x: sketch.sketchPathToEdit.start.x,
              y: sketch.sketchPathToEdit.start.y
            };
            sketch.sketchPathToEdit.closed = true;
            reposition = true;
            if (sketch.drawNegativeArea) {
              sketch.sketchPathToEdit.negative = true;
              sketch.lastAction = "negative pen";
            } else {
              sketch.sketchPathToEdit.negative = false;
              sketch.lastAction = "closed area";
            }
          }
        }
      } else {
        reposition = true;
      }
    }
    if (reposition) {
      if (this.updateCursor) {
        if (sketch.penDown) {
          if (sketch.cursor) {
            sketch.cursor.x = position.x;
            sketch.cursor.y = position.y;
          } else {
            sketch.cursor = { x: position.x, y: position.y };
          }
        }
      }
      return true;
    }
  }

  /**
   * Restores the initial state after mouse release.
   *
   * @param {SketchControl} sketch - Control instance
   * @returns {void}
   */
  performUp(sketch) {
    this.isDown = false;
    sketch.isDown = false;
    this.editing = undefined;
    sketch.editing = undefined;
    this.startX = undefined;
    this.startY = undefined;
    this.endX = undefined;
    this.endY = undefined;
    this.midX = undefined;
    this.midY = undefined;
    this.directionX = undefined;
    this.directionY = undefined;
    if (
      sketch.sketchPathToEdit !== undefined &&
      sketch.sketchPathToEdit.closed
    ) {
      sketch.sketchPathToEdit = undefined;
      sketch.sketchObjectToEdit = undefined;
    }
    if (this.history !== undefined) {
      sketch.addToUndoHistory(this.history);
      this.history = undefined;
    }
  }

  /**
   * Mouse down event handler.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Input event
   * @param {boolean} pob - Whether it's Pen Down
   * @returns {void}
   */
  down(sketch, event, pob = false) {
    this.isDown = true;
    sketch.isDown = true;
    let position = undefined;
    let reposition = false;
    let candidates = [];
    sketch.findPathSelectionCandidates(
      event.offsetX,
      event.offsetY,
      candidates,
      false,
      true
    );
    if (!sketch.penDown) {
      reposition = true;
      for (let i = 0; i < candidates.length; i++) {
        if (candidates[i].center &&
          candidates[i].layer.uniqueIdentifier == sketch.sketchLayerToEdit ||
          (candidates[i].layer.name == 'Scratchpad' && !sketch.sketchLayerToEdit)) {
          this.editing = candidates[i];
          let startX = this.editing.path.start.x;
          let startY = this.editing.path.start.y;
          for (let i = 0; i < this.editing.path.elements.length; i++) {
            const element = this.editing.path.elements[i];
            const endX = element.x;
            const endY = element.y;
            if (element === this.editing.element) {
              this.startX = startX;
              this.startY = startY;
              this.endX = endX;
              this.endY = endY;
              this.midX = (startX + endX) / 2;
              this.midY = (startY + endY) / 2;
              let deltaX = endX - startX;
              let deltaY = endY - startY;
              const length = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
              this.directionX = -deltaY / length;
              this.directionY = deltaX / length;
              let angle = 0;
              if (element.angle) {
                angle = element.angle;
              }
              SketchUtils.createCurveDataIn(
                this.editing,
                startX,
                startY,
                endX,
                endY,
                angle
              );
              if (sketch.editingValuesChanged) {
                sketch.editingValuesChanged(this.editing);
              }
              break;
            }
            startX = endX;
            startY = endY;
          }
          sketch.editing = this.editing;
          position = sketch.inverseTopLevelTransform.transform(this.editing.center);
          reposition = false;
          break;
        }
      }
      sketch.stopPenCursor()
    } else {
      sketch.editing = undefined;
      if (pob) position = new paper.Point(event.offsetX, event.offsetY);
      else
        position = sketch.inverseTopLevelTransform.transform(
          new paper.Point(event.offsetX, event.offsetY)
        );
      let adjusted = false;
      if (sketch.snapToLine) {
        let lastPoint = undefined;
        if (sketch.sketchPathToEdit !== undefined) {
          if (sketch.sketchPathToEditPoint === 0) {
            lastPoint = sketch.sketchPathToEdit.start;
          } else {
            lastPoint =
              sketch.sketchPathToEdit.elements[
              sketch.sketchPathToEdit.elements.length - 1
              ];
          }
        }
        let closest = {};
        sketch.closestPointTo(position, closest, lastPoint);
        if (
          closest.distanceSquared !== undefined &&
          closest.distanceSquared < 1
        ) {
          position.x = closest.position.x;
          position.y = closest.position.y;
          adjusted = true;
        }
      }
      if (!adjusted) {
        if (sketch.sketchPathToEdit !== undefined) {
          let lastPoint = undefined;
          if (sketch.sketchPathToEditPoint === 0) {
            lastPoint = sketch.sketchPathToEdit.start;
          } else {
            lastPoint =
              sketch.sketchPathToEdit.elements[
              sketch.sketchPathToEdit.elements.length - 1
              ];
          }
          const deltaX = position.x - lastPoint.x;
          const deltaY = position.y - lastPoint.y;
          sketch.addToPosition(position, lastPoint, deltaX, deltaY);
          adjusted = true;
        }
      }
      if (!adjusted) {
        sketch.adjustPosition(position);
      }
      if (sketch.penDown) {
        const history = sketch.createHistory();
        if (sketch.addPoint(position)) {
          if (this.history === undefined) {
            this.history = history;
          }
        }
        reposition = true;
        if (sketch.drawNegativeArea) {
          sketch.sketchPathToEdit.negative = true;
          sketch.lastAction = "negative pen";
        } else {
          sketch.sketchPathToEdit.negative = false;
          sketch.lastAction = "open area";
        }
      } else {
        sketch.stopPenCursor();
        reposition = true;
      }
    }
    if (reposition) {
      if (sketch.penDown) {
        if (!sketch.cursor) {
          sketch.cursor = { x: position.x, y: position.y };
        } else {
          sketch.cursor.x = position.x;
          sketch.cursor.y = position.y;
        }
        sketch.showContextBar("line");
      } else {
        sketch.cursor = undefined;
        sketch.penUpCursor = undefined;
        sketch.createPenUpCursor(
          event.offsetX,
          event.offsetY,
          sketch.penUpCursor
        );
      }
      if (pob) {
        this.performUp(sketch, event);
      }
      if (candidates.length < 1) {
        sketch.findPathSelectionCandidates(
          event.offsetX,
          event.offsetY,
          candidates,
          false,
          true
        );
        this.editing = candidates[0];
        if (this.editing && this.editing.path && this.editing.path.elements) {
          const length = this.editing.path.elements.length;
          let startX =
            length > 1
              ? this.editing.path.elements[length - 2].x
              : this.editing.path.start.x;
          let startY =
            length > 1
              ? this.editing.path.elements[length - 2].y
              : this.editing.path.start.y;
          const element = this.editing.path.elements[length - 1];
          const endX = element.x;
          const endY = element.y;
          if (element) {
            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;
            let angle = 0;
            SketchUtils.createCurveDataIn(
              this.editing,
              startX,
              startY,
              endX,
              endY,
              angle
            );
            if (sketch.editingValuesChanged !== undefined) {
              sketch.editingValuesChanged(this.editing);
            }
          }
          sketch.selected = [this.editing];
        }
      }
    }
  }

  /**
   * Mouse move event handler.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Input event
   * @returns {void}
   */
  move(sketch, event) {
    if (sketch.penDown) {
      sketch.cursor = undefined;
      if (!event) {
        event = {
          offsetX: window.innerWidth / 2,
          offsetY: window.innerHeight / 2
        };
      }
      else {
        sketch.lastEvent = (event.type)
      }
      sketch.createCursor(event.offsetX, event.offsetY, sketch.cursor);
    } else if (sketch.movingPenCursor == "moving") {
      let cursor;
      sketch.penUpCursor = undefined;
      if (!event) {
        cursor = sketch.topLevelTransform.translate(
          new paper.Point(sketch.cursor.x, sketch.cursor.y)
        );
      }
      sketch.createPenUpCursor(
        event ? event.offsetX : cursor.x,
        event ? event.offsetY : cursor.y,
        sketch.penUpCursor
      );
      sketch.cursor = undefined;
    }
    if (sketch.isDown) {
      this.performMove(sketch, event);
    }
    sketch.lastEvent = event.type
    sketch.draw();

    // For some reason, this method gets called in Safari if pressing the Shift key, either if the mouse was moved or not (WTF?!?):
  }

  /**
   * Mouse up event handler.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Input event
   * @returns {void}
   */
  up(sketch, event) {
    if (this.isDown) {
      this.performMove(sketch, event);
      this.performUp(sketch, event);
    }
  }
}
