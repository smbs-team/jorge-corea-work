// ptas_SketchSingleSelectInteraction.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Interactions for the Single Select mode
*/
class SketchSingleSelectInteraction {
  /**
    * Verifies if there are selection candidates
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {Event} event - Input event
    * @returns {boolean} - Whether selection candidates were found at the cursor position
    *
    */
  performSingleSelect(sketch, event) {
    let candidates = [];
    sketch.findPathSelectionCandidates(
      event.offsetX,
      event.offsetY,
      candidates,
      false,
      false
    );
    if (sketch.mode == SketchMode.Text) {
      const candidate = candidates.find(candidate => candidate.path.arrow);
      candidates = candidate ? [candidate] : [];
    }
    let current = -1;
    if (sketch.selected && sketch.selected.length > 0) {
      for (let i = 0; i < candidates.length; i++) {
        let candidate = candidates[i];
        for (let j = 0; j < sketch.selected.length; j++) {
          let selection = sketch.selected[j];
          if (
            candidate.sketchObject === selection.sketchObject &&
            candidate.path === selection.path
          ) {
            current = i;
            break;
          }
        }
      }
    }
    if (
      sketch.selected &&
      sketch.mode == SketchMode.MultipleSelect &&
      current >= 0 &&
      ((sketch.selected[0].element &&
        candidates[current].element &&
        sketch.selected[0].element !== candidates[current].element) ||
        (sketch.selected[0].point &&
          candidates[current].point &&
          sketch.selected[0].point !== candidates[current].point))
    ) {
      sketch.interaction = new SketchMultipleSelectInteraction();
      sketch.interaction.performMultipleSelect(sketch, event);
    } else {
      if (current >= 0) {
        if (candidates[current].point === undefined) {
          if (
            candidates[current].element &&
            candidates[current].layer.uniqueIdentifier ==
            sketch.sketchLayerToEdit
          ) {
            sketch.selected = [candidates[current]];
          } else if (current === candidates.length - 1) {
            if (candidates.length > 1) {
              sketch.clearSelection();
            } else {
              sketch.selected = [candidates[current]];
            }
          } else {
            sketch.selected = [candidates[current + 1]];
          }
        } else {
          sketch.selected = [candidates[current]];
        }
      } else if (candidates.length > 0) {
        if (sketch.overrideSelection) {
          sketch.overrideSelection = false;
          sketch.selected = [
            candidates.find(
              candidate =>
                candidate.layer.uniqueIdentifier == sketch.sketchLayerToEdit
            )
          ];
          if (!sketch.selected[0]) {
            sketch.selected = [candidates[0]];
          }
        } else {
          sketch.selected = [candidates[0]];
        }
      } else {
        sketch.clearSelection()
      }
      if (
        sketch.selected &&
        sketch.selected.length > 0 &&
        !sketch.selected[0].element &&
        sketch.selected[0].path.closed
      ) {
        let text = "";
        let feet = 0;
        let inches = 0;
        let area = 0;
        sketch.selected[0].sketchObject.objects.map((object, index) => {
          if (index > 0 && object.text) {
            text = object.text.content;
            feet += parseInt(text.substring(0, text.indexOf("'")));
            inches +=
              text.indexOf('"') > 0
                ? parseInt(
                  text.substring(text.indexOf("'") + 2, text.indexOf('"'))
                )
                : 0;
          } else if (object instanceof SketchPath) {
            area = object.getArea(sketch);
          }
        });
        feet += parseInt(inches / 12);
        inches %= 12;
        document.getElementById("selected-area").textContent =
          Math.round(area * 100) / 100 + " ⏍";
        document.getElementById("selected-perimeter").textContent =
          feet + "' " + inches + '"';
        sketch.showContextBar("area");
      }
      if (sketch.selected) {
        sketch.sketchLayerToEdit = sketch.selected[0].layer.uniqueIdentifier;
        if (sketch.selected[0].path && sketch.selected[0].path.negative) {
          sketch.lastAction = "negative select";
        } else {
          sketch.lastAction = "single";
        }
      }
      if (candidates.length < 1 && sketch.mode !== SketchMode.Text) {
        sketch.interaction = new SketchTextInteraction();
        sketch.down(event);
      }
      return candidates.length > 0;
    }
  }

  /**
    * Moves the selected object
    *
    * @param {sketchControl} sketch - The Sketch Control instance
    * @param {Event} event - Input event
    * @param {sketchObject} selection - Selected object
    * @returns {void}
    *
    */
  performMove(sketch, event, selection) {
    if (
      this.isMove ||
      this.startX !== event.offsetX ||
      this.startY !== event.offsetY
    ) {
      this.isMove = true;
      const start = sketch.inverseTopLevelTransform.transform(
        new paper.Point(this.startX, this.startY)
      );
      const position = sketch.inverseTopLevelTransform.transform(
        new paper.Point(event.offsetX, event.offsetY)
      );
      const prevBounds = {};
      if (selection) {
        this.selection = selection;
      }
      if (this.selection) {
        const MIN_OFFSET_LINE_DISCONNECT = 2;

        if (this.selection.point) {
          if (this.history === undefined) {
            this.history = sketch.createHistory();
          }
          let adjusted = false;
          if (sketch.snapToLine) {
            let closest = {};
            sketch.closestPointTo(position, closest, this.selection.point);
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
            sketch.adjustPosition(position);
          }

          this.selection.point.x = position.x;
          this.selection.point.y = position.y;
          this.createCurveData(sketch, this.selection);
          if (
            this.selection.path.closed &&
            this.selection.path.elements.length > 0
          ) {
            if (this.selection.path.start === this.selection.point) {
              this.selection.path.elements[
                this.selection.path.elements.length - 1
              ].x = position.x;
              this.selection.path.elements[
                this.selection.path.elements.length - 1
              ].y = position.y;
            } else if (
              this.selection.path.elements[
              this.selection.path.elements.length - 1
              ] === this.selection.point
            ) {
              this.selection.path.start.x = position.x;
              this.selection.path.start.y = position.y;
            }
          }
        } else if (
          this.selection.element &&
          this.selection.center &&
          !this.selection.path.arrow
        ) {
          if (this.history === undefined) {
            this.history = sketch.createHistory();
          }
          let deltaX = position.x - this.selectedMidX;
          let deltaY = position.y - this.selectedMidY;
          const projection =
            deltaX * this.directionX + deltaY * this.directionY;
          const toPointX = this.selectedMidX + projection * this.directionX;
          const toPointY = this.selectedMidY + projection * this.directionY;
          position.x = toPointX;
          position.y = toPointY;
          const startToPointDeltaX = toPointX - this.selectedStartX;
          const startToPointDeltaY = toPointY - this.selectedStartY;
          const endToPointDeltaX = toPointX - this.selectedEndX;
          const endToPointDeltaY = toPointY - this.selectedEndY;
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
          this.selection.element.angle = Math.round(
            (2 * angle * 180) / Math.PI
          );
          this.createCurveData(sketch, this.selection);
        } else if (this.selection.element) {
          let path = this.selection.path;
          let offsetX = position.x - start.x;
          let offsetY = position.y - start.y;

          if (this.selectedStartIndex < 0) {
            //Moving start (or single) line
            if (this.history === undefined) {
              this.history = sketch.createHistory();
            }

            let connectedPoint = null;
            let snappingPoint = null;
            let snappingPointClosest = null;

            //Phase 1: find if there is an endpoint connected to another endpoint
            //or if one should snap to line
            if (sketch.snapToLine) {
              let pointsExcluded = this.getPointsFromSelection(this.selection);
              let closest = {};
              sketch.closestPointTo(path.start, closest, pointsExcluded);

              //Check if start should be disconnected from a line
              if (
                closest.position &&
                path.start.x === closest.position.x &&
                path.start.y === closest.position.y
              ) {
                connectedPoint = path.start;
              }

              //Check if start should snap to line
              if (
                !connectedPoint &&
                closest.distanceSquared !== undefined &&
                closest.distanceSquared < 1
              ) {
                snappingPoint = path.start;
                snappingPointClosest = closest;
              }

              if (!connectedPoint) {
                const element = this.selection.element;
                closest = {};
                sketch.closestPointTo(element, closest, pointsExcluded);

                //Check if element should be disconnected from a line
                if (
                  closest.position &&
                  element.x === closest.position.x &&
                  element.y === closest.position.y
                ) {
                  connectedPoint = element;
                }

                //Check if element should snap to line
                if (
                  !connectedPoint &&
                  !snappingPoint &&
                  closest.distanceSquared !== undefined &&
                  closest.distanceSquared < 1
                ) {
                  snappingPoint = element;
                  snappingPointClosest = closest;
                }
              }
            }

            //Phase 2: set positions of every selected point
            if (connectedPoint) {
              //Disconnect from line
              let offsetDifferenceX = position.x - start.x;
              let offsetDifferenceY = position.y - start.y;

              //If the move was enough to disconnect the endpoints, adjust positions
              if (
                Math.abs(offsetDifferenceX) > MIN_OFFSET_LINE_DISCONNECT ||
                Math.abs(offsetDifferenceY) > MIN_OFFSET_LINE_DISCONNECT
              ) {
                if (this.history === undefined) {
                  this.history = sketch.createHistory();
                }

                offsetDifferenceX =
                  position.x - this.selectedStartOffsetX - path.start.x;
                offsetDifferenceY =
                  position.y - this.selectedStartOffsetY - path.start.y;
                path.start.x = path.start.x + offsetDifferenceX;
                path.start.y = path.start.y + offsetDifferenceY;
                this.selection.element.x =
                  this.selection.element.x + offsetDifferenceX;
                this.selection.element.y =
                  this.selection.element.y + offsetDifferenceY;
                if (path.closed) {
                  path.elements[path.elements.length - 1].x = path.start.x;
                  path.elements[path.elements.length - 1].y = path.start.y;
                }

                this.selectedStartX = path.start.x;
                this.selectedStartY = path.start.y;
                this.selectedEndX = this.selection.element.x;
                this.selectedEndY = this.selection.element.y;
                this.startX = event.offsetX;
                this.startY = event.offsetY;
              }
            } else if (snappingPoint) {
              //Snap to line
              if (this.history === undefined) {
                this.history = sketch.createHistory();
              }

              let offsetDifferenceX =
                snappingPointClosest.position.x - snappingPoint.x;
              let offsetDifferenceY =
                snappingPointClosest.position.y - snappingPoint.y;
              path.start.x = path.start.x + offsetDifferenceX;
              path.start.y = path.start.y + offsetDifferenceY;
              this.selection.element.x =
                this.selection.element.x + offsetDifferenceX;
              this.selection.element.y =
                this.selection.element.y + offsetDifferenceY;
              if (path.closed) {
                path.elements[path.elements.length - 1].x = path.start.x;
                path.elements[path.elements.length - 1].y = path.start.y;
              }

              this.selectedStartX = path.start.x;
              this.selectedStartY = path.start.y;
              this.selectedEndX = this.selection.element.x;
              this.selectedEndY = this.selection.element.y;
              this.startX = event.offsetX;
              this.startY = event.offsetY;
            } else {
              //Regular move
              const newStartX = this.selectedStartX + offsetX;
              const newStartY = this.selectedStartY + offsetY;

              sketch.setPosition(
                this.selection.path.start,
                newStartX,
                newStartY
              );
              if (this.selection.path.closed) {
                sketch.setPosition(
                  this.selection.path.elements[
                  this.selection.path.elements.length - 1
                  ],
                  newStartX,
                  newStartY
                );
              }
              offsetX = this.selection.path.start.x - this.selectedStartX;
              offsetY = this.selection.path.start.y - this.selectedStartY;

              const newFinishX = this.selectedEndX + offsetX;
              const newFinishY = this.selectedEndY + offsetY;
              this.selection.element.x = newFinishX;
              this.selection.element.y = newFinishY;

              //Check if adjusted (by snapToGrid) position fell into another point
              let pointsExcluded = this.getPointsFromSelection(this.selection);
              let closest = {};
              sketch.closestPointTo(path.start, closest, pointsExcluded);
              if (
                closest.position &&
                path.start.x === closest.position.x &&
                path.start.y === closest.position.y
              ) {
                this.startX = event.offsetX;
                this.startY = event.offsetY;
              } else {
                let closest = {};
                sketch.closestPointTo(
                  this.selection.element,
                  closest,
                  pointsExcluded
                );
                if (
                  closest.position &&
                  this.selection.element.x === closest.position.x &&
                  this.selection.element.y === closest.position.y
                ) {
                  this.startX = event.offsetX;
                  this.startY = event.offsetY;
                }
              }
            }
          } //Moving an element different to start
          else {
            if (this.history === undefined) {
              this.history = sketch.createHistory();
            }

            let connectedPoint = null;
            let snappingPoint = null;
            let snappingPointClosest = null;
            let firstElement = this.selection.path.elements[
              this.selectedStartIndex
            ];
            let secondElement = this.selection.element;

            //Phase 1: find if there is an endpoint connected to another endpoint
            if (sketch.snapToLine) {
              let pointsExcluded = this.getPointsFromSelection(this.selection);
              let closest = {};
              sketch.closestPointTo(firstElement, closest, pointsExcluded);

              //Check if some element should be disconnected from a line
              if (
                closest.position &&
                firstElement.x === closest.position.x &&
                firstElement.y === closest.position.y
              ) {
                connectedPoint = firstElement;
              }
              if (!connectedPoint) {
                closest = {};
                sketch.closestPointTo(secondElement, closest, pointsExcluded);
                if (
                  closest.position &&
                  secondElement.x === closest.position.x &&
                  secondElement.y === closest.position.y
                ) {
                  connectedPoint = secondElement;
                }
              }

              //Check if some element should snap to line
              if (!connectedPoint) {
                closest = {};
                sketch.closestPointTo(firstElement, closest, pointsExcluded);

                if (
                  closest.distanceSquared !== undefined &&
                  closest.distanceSquared < 1
                ) {
                  snappingPoint = firstElement;
                  snappingPointClosest = closest;
                }

                if (!snappingPoint) {
                  //Only set if it was not set before
                  closest = {};
                  sketch.closestPointTo(secondElement, closest, pointsExcluded);

                  if (
                    closest.distanceSquared !== undefined &&
                    closest.distanceSquared < 1
                  ) {
                    snappingPoint = secondElement;
                    snappingPointClosest = closest;
                  }
                }
              }
            }

            //Phase 2: set positions of both points
            if (connectedPoint) {
              //Disconnect from line
              let offsetDifferenceX = position.x - start.x;
              let offsetDifferenceY = position.y - start.y;

              //If the move was enough to disconnect the endpoints, adjust positions
              if (
                Math.abs(offsetDifferenceX) > MIN_OFFSET_LINE_DISCONNECT ||
                Math.abs(offsetDifferenceY) > MIN_OFFSET_LINE_DISCONNECT
              ) {
                offsetDifferenceX =
                  position.x - this.selectedStartOffsetX - firstElement.x;
                offsetDifferenceY =
                  position.y - this.selectedStartOffsetY - firstElement.y;
                firstElement.x = firstElement.x + offsetDifferenceX;
                firstElement.y = firstElement.y + offsetDifferenceY;
                secondElement.x = secondElement.x + offsetDifferenceX;
                secondElement.y = secondElement.y + offsetDifferenceY;
                if (
                  this.selectedEndIndex ===
                  this.selection.path.elements.length - 1 &&
                  this.selection.path.closed
                ) {
                  this.selection.path.start.x = secondElement.x;
                  this.selection.path.start.y = secondElement.y;
                }

                this.selectedStartX = firstElement.x;
                this.selectedStartY = firstElement.y;
                this.selectedEndX = secondElement.x;
                this.selectedEndY = secondElement.y;
                this.startX = event.offsetX;
                this.startY = event.offsetY;
              }
            } else if (snappingPoint) {
              let offsetDifferenceX =
                snappingPointClosest.position.x - snappingPoint.x;
              let offsetDifferenceY =
                snappingPointClosest.position.y - snappingPoint.y;

              firstElement.x = firstElement.x + offsetDifferenceX;
              firstElement.y = firstElement.y + offsetDifferenceY;
              secondElement.x = secondElement.x + offsetDifferenceX;
              secondElement.y = secondElement.y + offsetDifferenceY;
              if (
                this.selectedEndIndex ===
                this.selection.path.elements.length - 1 &&
                this.selection.path.closed
              ) {
                this.selection.path.start.x = secondElement.x;
                this.selection.path.start.y = secondElement.y;
              }

              this.selectedStartX = firstElement.x;
              this.selectedStartY = firstElement.y;
              this.selectedEndX = secondElement.x;
              this.selectedEndY = secondElement.y;
              this.startX = event.offsetX;
              this.startY = event.offsetY;
            } else {
              //Regular move
              const newStartX = this.selectedStartX + offsetX;
              const newStartY = this.selectedStartY + offsetY;
              sketch.setPosition(
                this.selection.path.elements[this.selectedStartIndex],
                newStartX,
                newStartY
              );
              offsetX =
                this.selection.path.elements[this.selectedStartIndex].x -
                this.selectedStartX;
              offsetY =
                this.selection.path.elements[this.selectedStartIndex].y -
                this.selectedStartY;

              const newFinishX = this.selectedEndX + offsetX;
              const newFinishY = this.selectedEndY + offsetY;
              this.selection.element.x = newFinishX;
              this.selection.element.y = newFinishY;

              if (
                this.selectedEndIndex ===
                this.selection.path.elements.length - 1 &&
                this.selection.path.closed
              ) {
                this.selection.path.start.x = newFinishX;
                this.selection.path.start.y = newFinishY;
              }

              //Check if adjusted (by snapToGrid) position fell into another point
              let pointsExcluded = this.getPointsFromSelection(this.selection);
              let closest = {};
              sketch.closestPointTo(
                this.selection.path.elements[this.selectedStartIndex],
                closest,
                pointsExcluded
              );
              if (
                closest.position &&
                this.selection.path.elements[this.selectedStartIndex].x ===
                closest.position.x &&
                this.selection.path.elements[this.selectedStartIndex].y ===
                closest.position.y
              ) {
                this.startX = event.offsetX;
                this.startY = event.offsetY;
              } else {
                let closest = {};
                sketch.closestPointTo(
                  this.selection.element,
                  closest,
                  pointsExcluded
                );
                if (
                  closest.position &&
                  this.selection.element.x === closest.position.x &&
                  this.selection.element.y === closest.position.y
                ) {
                  this.startX = event.offsetX;
                  this.startY = event.offsetY;
                }
              }
            }
          }
        } //Moving a whole figure/shape
        else {
          for (let i = 0; i < this.selection.sketchObject.objects.length; i++) {
            if (this.selection.sketchObject.objects[i] instanceof SketchPath) {
              let path = this.selection.sketchObject.objects[i];
              let connectedPoint = null;
              let snappingPoint = null;
              let snappingPointClosest = null;
              path.getBounds(prevBounds);

              //Phase 1: find if there is an endpoint connected to another endpoint
              //or if one should snap to line
              if (sketch.snapToLine) {
                let pointsExcluded = this.getPointsFromSelection(
                  this.selection
                );
                let closest = {};
                sketch.closestPointTo(path.start, closest, pointsExcluded);

                //Check if start should be disconnected from a line
                if (
                  closest.position &&
                  path.start.x === closest.position.x &&
                  path.start.y === closest.position.y
                ) {
                  connectedPoint = path.start;
                }
                //Check if start should snap to line
                if (
                  !connectedPoint &&
                  closest.distanceSquared !== undefined &&
                  closest.distanceSquared < 1
                ) {
                  snappingPoint = path.start;
                  snappingPointClosest = closest;
                }

                if (!connectedPoint && !snappingPoint) {
                  for (let j = 0; j < path.elements.length; j++) {
                    const element = path.elements[j];
                    closest = {};
                    sketch.closestPointTo(element, closest, pointsExcluded);

                    //Check if element should be disconnected from a line
                    if (
                      closest.position &&
                      element.x === closest.position.x &&
                      element.y === closest.position.y
                    ) {
                      connectedPoint = element;
                      break;
                    }
                    //Check if element should snap to line
                    if (
                      !connectedPoint &&
                      closest.distanceSquared !== undefined &&
                      closest.distanceSquared < 1
                    ) {
                      snappingPoint = element;
                      snappingPointClosest = closest;
                    }
                  }
                }
              }

              //Phase 2: set positions of every selected point
              if (connectedPoint) {
                //Disconnect from line
                let offsetDifferenceX = position.x - start.x;
                let offsetDifferenceY = position.y - start.y;

                //If the move was enough to disconnect the endpoints, adjust positions
                if (
                  Math.abs(offsetDifferenceX) > MIN_OFFSET_LINE_DISCONNECT ||
                  Math.abs(offsetDifferenceY) > MIN_OFFSET_LINE_DISCONNECT
                ) {
                  if (this.history === undefined) {
                    this.history = sketch.createHistory();
                  }

                  path.start.x = path.start.x + offsetDifferenceX;
                  path.start.y = path.start.y + offsetDifferenceY;
                  this.selectedPointsX = [path.start.x];
                  this.selectedPointsY = [path.start.y];

                  for (let k = 0; k < path.elements.length; k++) {
                    path.elements[k].x = path.elements[k].x + offsetDifferenceX;
                    path.elements[k].y = path.elements[k].y + offsetDifferenceY;
                    this.selectedPointsX.push(path.elements[k].x);
                    this.selectedPointsY.push(path.elements[k].y);
                  }
                  this.startX = event.offsetX;
                  this.startY = event.offsetY;
                }
              } else if (snappingPoint) {
                //Snap to line
                if (this.history === undefined) {
                  this.history = sketch.createHistory();
                }

                let offsetDifferenceX =
                  snappingPointClosest.position.x - snappingPoint.x;
                let offsetDifferenceY =
                  snappingPointClosest.position.y - snappingPoint.y;
                path.start.x = path.start.x + offsetDifferenceX;
                path.start.y = path.start.y + offsetDifferenceY;
                this.selectedPointsX = [path.start.x];
                this.selectedPointsY = [path.start.y];

                for (let k = 0; k < path.elements.length; k++) {
                  path.elements[k].x = path.elements[k].x + offsetDifferenceX;
                  path.elements[k].y = path.elements[k].y + offsetDifferenceY;
                  this.selectedPointsX.push(path.elements[k].x);
                  this.selectedPointsY.push(path.elements[k].y);
                }

                this.startX = event.offsetX;
                this.startY = event.offsetY;
              } //Regular move without disconnecting from or snapping to a line
              else {
                if (this.history === undefined) {
                  this.history = sketch.createHistory();
                }

                let index = 0;
                let offsetX = position.x - start.x;
                let offsetY = position.y - start.y;
                sketch.setPosition(
                  path.start,
                  this.selectedPointsX[index] + offsetX,
                  this.selectedPointsY[index] + offsetY
                );
                offsetX = path.start.x - this.selectedPointsX[index];
                offsetY = path.start.y - this.selectedPointsY[index];

                //Check if adjusted (by snapToGrid) position fell into another point
                let pointsExcluded = this.getPointsFromSelection(
                  this.selection
                );
                let closest = {};
                sketch.closestPointTo(path.start, closest, pointsExcluded);
                if (
                  closest.position &&
                  path.start.x === closest.position.x &&
                  path.start.y === closest.position.y
                ) {
                  this.startX = event.offsetX;
                  this.startY = event.offsetY;
                }

                index++;
                for (let j = 0; j < path.elements.length; j++) {
                  path.elements[j].x = this.selectedPointsX[index] + offsetX;
                  path.elements[j].y = this.selectedPointsY[index] + offsetY;
                  index++;

                  closest = {};
                  sketch.closestPointTo(
                    path.elements[j],
                    closest,
                    pointsExcluded
                  );
                  if (
                    closest.position &&
                    path.elements[j].x === closest.position.x &&
                    path.elements[j].y === closest.position.y
                  ) {
                    this.startX = event.offsetX;
                    this.startY = event.offsetY;
                  }
                }
              }
            }
          }
        }
      }

      if (
        sketch.selected &&
        sketch.selected[0].path &&
        !sketch.selected[0].point
      ) {
        const bounds = {};
        const object = sketch.selected[0].layer.objects.find(
          obj =>
            obj.objects &&
            obj.objects.length > 0 &&
            obj.objects[0].isLabel &&
            obj.objects[0].uniqueIdentifier == sketch.selected[0].path.label
        );
        if (object && !sketch.selected[0].element) {
          sketch.selected[0].path.getBounds(bounds);
          if (!object.objects[0].manuallyMoved) {
            object.objects[0].x = (bounds.max.x + bounds.min.x) / 2;
            object.objects[0].y = (bounds.max.y + bounds.min.y) / 2;
          } else {
            object.objects[0].x += bounds.min.x - prevBounds.min.x;
            object.objects[0].y += bounds.min.y - prevBounds.min.y;
          }
          if (object.objects[0].arrow) {
            const arrow = sketch.selected[0].layer.objects.find(
              obj =>
                obj.objects &&
                obj.objects.length > 0 &&
                obj.objects[0].uniqueIdentifier == object.objects[0].arrow
            );
            arrow.objects[0].start = {
              x: object.objects[0].x,
              y: object.objects[0].y
            };
          }
        }
        if (sketch.selected[0].path.arrow) {
          const label = sketch.selected[0].layer.objects.find(
            object =>
              object.objects &&
              object.objects.length > 0 &&
              object.objects[0].arrow &&
              sketch.selected[0].path.uniqueIdentifier
          );
          label.objects[0].x = sketch.selected[0].path.start.x;
          label.objects[0].y = sketch.selected[0].path.start.y;
        }
      }
      sketch.draw();
    }
  }

  /**
    * Called on mouse release
    *
    * @param {sketchControl} sketch - The Sketch Control instance
    * @param {Event} event - Input event
    * @returns {void}
    *
    */
  performUp(sketch, event) {
    if (sketch.mode == SketchMode.Text) {
      sketch.interaction = new SketchTextInteraction();
      sketch.interaction.performUp(sketch, event);
    } else {
      if (sketch.mode == SketchMode.MultipleSelect) {
        sketch.interaction = new SketchMultipleSelectInteraction();
        if (!this.selection && sketch.selected)
          this.selection = sketch.selected[sketch.selected.length - 1]
      }
      if (
        sketch.layers &&
        this.selection &&
        this.selection.path &&
        !this.selection.path.closed
      ) {
        const layer = sketch.getLayer(sketch.sketchLayerToEdit);
        if (
          this.selection.path.start.x ==
          this.selection.path.elements[this.selection.path.elements.length - 1]
            .x &&
          this.selection.path.start.y ==
          this.selection.path.elements[this.selection.path.elements.length - 1]
            .y
        ) {
          this.selection.path.closed = true;
          sketch.sketchPathToEdit = this.selection.path;
          layer.addLabel(sketch.sketchPathToEdit);
          sketch.alignDistances(layer);
        } else {
          let candidates = [];
          let startPoint = sketch.topLevelTransform.transform(
            new paper.Point(
              this.selection.path.start.x,
              this.selection.path.start.y
            )
          );
          sketch.findPathSelectionCandidates(
            startPoint.x,
            startPoint.y,
            candidates,
            true,
            true
          );
          if (candidates.length < 2) {
            candidates = [];
            startPoint = sketch.topLevelTransform.transform(
              new paper.Point(
                this.selection.path.elements[
                  this.selection.path.elements.length - 1
                ].x,
                this.selection.path.elements[
                  this.selection.path.elements.length - 1
                ].y
              )
            );
            sketch.findPathSelectionCandidates(
              startPoint.x,
              startPoint.y,
              candidates,
              true,
              true
            );
          }
          let candidate = candidates.find(
            candidate =>
              candidate.layer.uniqueIdentifier == sketch.sketchLayerToEdit &&
              candidate.path !== this.selection.path
          );
          if (candidate) {
            let position = sketch.inverseTopLevelTransform.transform(
              new paper.Point(event.offsetX, event.offsetY)
            );
            let closest = {};
            sketch.closestPointTo(position, closest, this.selection.point);
            if (
              this.selection.path.start.x == closest.position.x &&
              this.selection.path.start.y == closest.position.y
            ) {
              this.selection.path.elements.unshift(this.selection.path.start);
              this.selection.path.start = this.selection.path.elements.pop();
              this.selection.path.elements.reverse();
              this.selection.sketchObject.objects.push(
                this.selection.sketchObject.objects.shift()
              );
              this.selection.sketchObject.objects.reverse();
            }
            let selectionPoint = this.selection.point ?? this.selection.element;
            if (
              selectionPoint.x ==
              candidate.path.elements[candidate.path.elements.length - 1].x &&
              selectionPoint.y ==
              candidate.path.elements[candidate.path.elements.length - 1].y
            ) {
              candidate.path.elements.pop();
              candidate.path.elements.reverse();
              candidate.path.elements.push(candidate.path.start);
            }
            candidate.path.elements.map(element => {
              this.selection.path.elements.push({
                x: element.x,
                y: element.y,
                angle: element.angle
              });
            });
            candidate.sketchObject.objects.map((object, index) => {
              if (object instanceof SketchDistance) {
                let copy = object.createCopy();
                copy.sketchPath = this.selection.sketchObject.objects[0];
                copy.start = index + 1;
                copy.finish = index + 2;
                this.selection.sketchObject.objects.push(copy);
              }
            });
            for (let i = 0; i < layer.objects.length; i++) {
              if (layer.objects[i].objects[0] == candidate.path) {
                layer.objects.splice(i, 1);
                break;
              }
            }
          }
        }
      }
    }
    this.isDown = false;
    this.selection = undefined;
    this.startX = undefined;
    this.startY = undefined;
    this.selectedPointX = undefined;
    this.selectedPointY = undefined;
    this.selectedStartX = undefined;
    this.selectedStartY = undefined;
    this.selectedStartIndex = undefined;
    this.selectedEndX = undefined;
    this.selectedEndY = undefined;
    this.selectedEndIndex = undefined;
    this.selectedPointsX = undefined;
    this.selectedPointsY = undefined;
    this.selectedMidX = undefined;
    this.selectedMidY = undefined;
    this.directionX = undefined;
    this.directionY = undefined;
    if (this.isMove) {
      this.isMove = false;
      if (this.history) {
        sketch.addToUndoHistory(this.history);
        this.history = undefined;
      }
      sketch.autoSave();
    }
  }

  /**
    * Sets a curve angle on the selected object.
    *
    * @param {sketchControl} sketch - The Sketch Control instance
    * @param {SketchObject} selection - Input event
    * @returns {void}
    *
    */
  createCurveData(sketch, selection) {
    let pointIndex = 0;
    let elements = selection.path.elements;
    selection.path.elements.map((value, index) => {
      if (value == selection.point || value == selection.element) {
        pointIndex = index;
      }
    });
    if (!selection.path.elements[pointIndex].angle)
      selection.path.elements[pointIndex].angle = 0;
    if (pointIndex == 0) {
      if (
        selection.path.closed &&
        JSON.stringify(this.selection.path.start) ==
        JSON.stringify(this.selection.point)
      ) {
        SketchUtils.createCurveDataIn(
          selection,
          elements[elements.length - 2].x,
          elements[elements.length - 2].y,
          elements[elements.length - 1].x,
          elements[elements.length - 1].y,
          elements[elements.length - 1].angle
        );
        pointIndex = elements.length - 2;
      } else {
        SketchUtils.createCurveDataIn(
          selection,
          selection.path.start.x,
          selection.path.start.y,
          elements[pointIndex].x,
          elements[pointIndex].y,
          elements[pointIndex].angle
        );
      }
    } else {
      SketchUtils.createCurveDataIn(
        selection,
        elements[pointIndex - 1].x,
        elements[pointIndex - 1].y,
        elements[pointIndex].x,
        elements[pointIndex].y,
        elements[pointIndex].angle
      );
    }
    if (elements[pointIndex].angle == 0) {
      sketch.showContextBar("line");
    } else {
      sketch.showContextBar("curve");
    }
    sketch.editingValuesChanged(selection);
  }

  /**
    * Mouse down event handler
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {Event} event - Input event
    * @returns {void}
    *
    */
  down(sketch, event) {
    let found = this.performSingleSelect(sketch, event);
    sketch.negativeAreaSelection(false);
    this.isDown = true;
    if (found && sketch.selected !== undefined && sketch.selected.length > 0) {
      this.selection = sketch.selected[0];
      if (this.selection.point !== undefined) {
        this.selectedPointX = this.selection.point.x;
        this.selectedPointY = this.selection.point.y;
        let candidates = [];
        sketch.findPathSelectionCandidates(
          event.offsetX,
          event.offsetY,
          candidates,
          false,
          true
        );
        if (candidates.length > 0) {
          this.createCurveData(sketch, this.selection);
        }

        if (this.selection.path.closed && this.selection.path.negative) {
          sketch.negativeAreaSelection(true);
        }
      } else if (this.selection.element !== undefined) {
        for (let i = 0; i < this.selection.path.elements.length; i++) {
          if (this.selection.path.elements[i] === this.selection.element) {
            let candidates = [];
            sketch.findPathSelectionCandidates(
              event.offsetX,
              event.offsetY,
              candidates,
              false,
              true
            );
            if (i === 0) {
              this.selectedStartX = this.selection.path.start.x;
              this.selectedStartY = this.selection.path.start.y;
            } else {
              this.selectedStartX = this.selection.path.elements[i - 1].x;
              this.selectedStartY = this.selection.path.elements[i - 1].y;
            }
            this.selectedStartIndex = i - 1;
            this.selectedEndX = this.selection.element.x;
            this.selectedEndY = this.selection.element.y;
            this.selectedEndIndex = i;
            this.selectedMidX = (this.selectedStartX + this.selectedEndX) / 2;
            this.selectedMidY = (this.selectedStartY + this.selectedEndY) / 2;
            let deltaX = this.selectedEndX - this.selectedStartX;
            let deltaY = this.selectedEndY - this.selectedStartY;
            const length = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
            this.directionX = -deltaY / length;
            this.directionY = deltaX / length;

            if (this.selection) {
              this.createCurveData(sketch, this.selection);
            }

            if (this.selection.path.closed && this.selection.path.negative) {
              sketch.negativeAreaSelection(true);
            }

            break;
          }
        }
        if (this.selection.path) {
          let position = sketch.inverseTopLevelTransform.transform(
            new paper.Point(event.offsetX, event.offsetY)
          );
          this.selectedStartOffsetX = position.x - this.selectedStartX;
          this.selectedStartOffsetY = position.y - this.selectedStartY;
        }
      } else {
        this.selectedPointsX = [];
        this.selectedPointsY = [];
        for (let i = 0; i < this.selection.sketchObject.objects.length; i++) {
          if (this.selection.sketchObject.objects[i] instanceof SketchPath) {
            let path = this.selection.sketchObject.objects[i];
            this.selectedPointsX.push(path.start.x);
            this.selectedPointsY.push(path.start.y);
            for (let j = 0; j < path.elements.length; j++) {
              this.selectedPointsX.push(path.elements[j].x);
              this.selectedPointsY.push(path.elements[j].y);
            }

            if (path.closed && path.negative) {
              sketch.negativeAreaSelection(true);
            }
          }
        }
      }
      sketch.draw();
    } else {
      sketch.showContextBar("none");
    }
    this.startX = event.offsetX;
    this.startY = event.offsetY;
  }

  /**
    * Mouse move event handler.
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {Event} event - Input event
    * @returns {void}
    *
    */
  move(sketch, event) {
    if (this.isDown) {
      this.performMove(sketch, event);
    }
  }

  /**
    * Mouse up event handler.
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {Event} event - Input event
    * @returns {void}
    *
    */
  up(sketch, event) {
    if (this.isDown) {
      this.performMove(sketch, event);
      this.performUp(sketch, event);
    }
  }

  /**
    * Mouse down event handler
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {Event} event - Input event
    * @returns {void}
    *
    */
  enter(sketch, event) {
    if (this.isDown && event.buttons != 1) {
      this.performUp(sketch, event);
    }
  }

  /**
    * Adds an object to the selection array
    *
    * @param {Array} selection - Selected objects
    * @returns {Array} - Points
    *
    */
  getPointsFromSelection(selection) {
    const points = [];

    if (selection && selection.path && selection.path.start) {
      points.push(selection.path.start);

      if (selection.path.elements && selection.path.elements.length > 0) {
        selection.path.elements.forEach(p => {
          points.push(p);
        });
      }
    }

    return points;
  }

  /**
    * Double click event handler
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {MouseEvent} event - Input event
    * @returns {void}
    *
    */
  doubleClick(sketch, event) {
    if (
      (sketch.interaction instanceof SketchTextInteraction,
        sketch.selected && sketch.selected[0].textObject)
    ) {
      textClicked();
      sketch.interaction.doubleClick(sketch, event);
    }
  }
}
