// ptas_SketchMultipleSelectInteraction.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Interactions for the Multiple Select mode
*/
class SketchMultipleSelectInteraction {
  /**
    * Verifies if there are selection candidates
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {MouseEvent} event - Input event
    * @returns {boolean} - Whether selection candidates were found at the cursor position
    *
    */
  performMultipleSelect(sketch, event) {
    let candidates = [];
    sketch.findPathSelectionCandidates(
      event.offsetX,
      event.offsetY,
      candidates,
      false,
      false
    );
    let current = -1;
    if (!sketch.selected) {
      sketch.selected = [];
    }
    if (sketch.selected.length > 0) {
      for (let i = 0; i < candidates.length; i++) {
        let candidate = candidates[i];
        if (candidate) {
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
    }
    if (current >= 0) {
      if (candidates[current].point) {
        this.addToSelection(sketch, candidates, "point", current);
      } else if (candidates[current].element) {
        if (candidates[current].layer.uniqueIdentifier == sketch.sketchLayerToEdit) {
          this.addToSelection(sketch, candidates, "element", current);
        } else if (current === candidates.length - 1) {
          if (candidates.length > 1) {
            sketch.clearSelection();
          } else {
            this.addToSelection(sketch, candidates, "element", current);
          }
        } else {
          this.addToSelection(sketch, candidates, "element", current + 1);
        }
      } else if (candidates[current].path) {
        this.addToSelection(sketch, candidates, 'area', current)
      }
    } else if (candidates.length > 0) {
      if (sketch.overrideSelection) {
        sketch.overrideSelection = false;
        sketch.selected.push(
          candidates.find(
            candidate =>
              candidate.layer.uniqueIdentifier == sketch.sketchLayerToEdit
          )
        );
      } else {
        sketch.selected.push(candidates[0]);
        this.added = true;
        if (candidates[0].point) {
          sketch.lastSelection = candidates[0];
        } else if (candidates[0].element) {
          sketch.selectedElements += 1;
        } else {
          sketch.selectedAreas += 1;
        }
      }
    } else if (candidates.length < 1) {
      sketch.findTextSelectionCandidates(
        sketch,
        event.offsetX,
        event.offsetY,
        candidates
      );
      if (candidates.length > 0 && !sketch.selected.find(candidate => candidate.textObject == candidates[0].textObject)) {
        sketch.selected.push(candidates[0])
        this.added = true;
      }
    } else {
      sketch.clearSelection();
      sketch.selectedElements = sketch.selectedAreas = 0
    }
    if (
      sketch.selected &&
      sketch.selected.length > 0 &&
      sketch.selected[0].path &&
      sketch.selected[0].path.closed &&
      !sketch.selected[0].element
    ) {
      let text = "";
      let feet = 0;
      let inches = 0;
      let area = 0;
      sketch.selected.map(selected => {
        selected.sketchObject.objects.map((object, index) => {
          if (index > 0 && object.text) {
            text = object.text.content;
            feet += parseInt(text.substring(0, text.indexOf("'")));
            inches +=
              text.indexOf('"') > 0
                ? parseInt(
                  text.substring(text.indexOf("'") + 2, text.indexOf('"'))
                )
                : 0;
          } else if (object.closed) {
            area += object.getArea(sketch);
          }
        });
      });
      feet += parseInt(inches / 12);
      inches %= 12;
      document.getElementById("selected-area").textContent =
        Math.round(area * 100) / 100 + " ⏍";
      document.getElementById("selected-perimeter").textContent =
        feet + "' " + inches + '"';
      sketch.showContextBar("area");
    }
    if (sketch.selected && sketch.selected.length > 0) {
      const selected = sketch.selected[sketch.selected.length - 1];
      sketch.sketchLayerToEdit = selected.layer.uniqueIdentifier;
      if (selected.path && selected.path.negative) {
        sketch.lastAction = "negative select";
      } else {
        sketch.lastAction = "multiple";
      }
    }
    return candidates.length > 0;
  }

  /**
    * Adds an object to the selection array
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {Array} candidates - The candidates found at the cursor position
    * @param {string} property - The object type of the selection: point, element, area
    * @param {number} current - The position in the candidates array
    * @returns {void}
    *
    */
  addToSelection(sketch, candidates, property, current) {
    let missing = true;
    for (let i = 0; i < sketch.selected.length; i++) {
      if (sketch.selected[i][property] && sketch.selected[i][property] == candidates[current][property]) {
        missing = false;
        break;
      }
    }
    if (missing) {
      if (property == 'area') {
        sketch.selectedAreas++;
        for (let i = 0; i < sketch.selected.length; i++) {
          if (sketch.selected[i].path == candidates[current].path && (sketch.selected[i].point || sketch.selected[i].element)) {
            if (sketch.selected[i].element)
              sketch.selectedElements--;
            sketch.selected.splice(i, 1)
            i--;
          }
        }
      }
      else {
        if (property == 'element') {
          sketch.selectedElements++;
          for (let i = 0; i < sketch.selected.length; i++) {
            if (sketch.selected[i].path == candidates[current].path && !sketch.selected[i].point && !sketch.selected[i].element) {
              if (sketch.selected[i].element)
                sketch.selectedElements--;
              sketch.selected.splice(i, 1)
              break;
            }
          }
        }
      }
      sketch.selected.push(candidates[current]);
      this.added = true
    }
    sketch.lastSelection = candidates[current];
  }

  /**
    * Removes an object from the selection array
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {Array} candidates - The candidates found at the cursor position
    * @param {string} property - The object type of the selection: point, element, area
    * @param {number} current - The position in the candidates array
    * @returns {void}
    *
    */
  removeFromSelection(sketch, candidates, property, current) {
    for (let i = 0; i < sketch.selected.length; i++) {
      if (sketch.selected[i][property] && sketch.selected[i][property] == candidates[current][property]) {
        sketch.selected.splice(i, 1);
        switch (property) {
          case "element":
            sketch.selectedElements--;
            break;
          case "area":
            sketch.selectedAreas--;
        }
        break;
      }
    }
    sketch.lastSelection = candidates[current];
  }

  /**
    * Moves the selected objects
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {MouseEvent} event - Input event
    * @returns {void}
    *
    */
  performMove(sketch, event) {
    if (
      this.isMove ||
      this.startX !== event.offsetX ||
      this.startY !== event.offsetY
    ) {
      this.isMove = true;
      if (this.selection) {
        const MIN_OFFSET_LINE_DISCONNECT = 2;
        let continueLoopOverSelection = true;
        let snappingPoint;
        let snappingPointClosest;
        const position = sketch.inverseTopLevelTransform.transform(
          new paper.Point(event.offsetX, event.offsetY)
        );
        const start = sketch.inverseTopLevelTransform.transform(
          new paper.Point(this.startX, this.startY)
        );

        //Phase 1: find if there is an endpoint connected to another endpoint
        //or if one should snap to line
        if (this.selection.filter(index => index.point).length > 1) {
          for (
            let i = 0;
            i < this.selection.length && continueLoopOverSelection;
            i++
          ) {
            let selected = this.selection[i];
            let path = null;
            selected.sketchObject.objects.forEach(obj => {
              if (obj instanceof SketchPath) {
                path = obj;
              }
            });

            if (path && !path.closed) {
              if (sketch.snapToLine) {
                let pointsExcluded = this.getPointsFromSelection(this.selection);
                let closest = {};
                sketch.closestPointTo(path.start, closest, pointsExcluded);

                //Check if start should snap to line
                if (
                  closest.distanceSquared &&
                  closest.distanceSquared < 1
                ) {
                  if (!snappingPoint) {
                    //Only set if it was not set before
                    snappingPoint = path.start;
                    snappingPointClosest = closest;
                  }
                }

                if (!snappingPoint && path.elements) {
                  for (let j = 0; j < path.elements.length; j++) {
                    const element = path.elements[j];
                    closest = {};
                    sketch.closestPointTo(element, closest, pointsExcluded);

                    //Check if element should snap to line
                    if (
                      closest.distanceSquared &&
                      closest.distanceSquared < 1
                    ) {
                      if (!snappingPoint) {
                        //Only set if it was not set before
                        snappingPoint = element;
                        snappingPointClosest = closest;
                      }
                    }
                  }
                }
              }
            }
          }
        }

        //Phase 2: set positions of every selected point
        if (snappingPoint) {
          //Snap to line
          let offsetDifferenceX =
            snappingPointClosest.position.x - snappingPoint.x;
          let offsetDifferenceY =
            snappingPointClosest.position.y - snappingPoint.y;

          let index = 0;
          for (let i = 0; i < this.selection.length; i++) {
            let selected = this.selection[i];
            for (let j = 0; j < selected.sketchObject.objects.length; j++) {
              if (selected.sketchObject.objects[j] instanceof SketchPath) {
                if (this.history === undefined) {
                  this.history = sketch.createHistory();
                }

                let path = selected.sketchObject.objects[j];
                path.start.x = path.start.x + offsetDifferenceX;
                path.start.y = path.start.y + offsetDifferenceY;
                this.selectedPointsX[index] = path.start.x;
                this.selectedPointsY[index] = path.start.y;
                index++;

                for (let k = 0; k < path.elements.length; k++) {
                  path.elements[k].x = path.elements[k].x + offsetDifferenceX;
                  path.elements[k].y = path.elements[k].y + offsetDifferenceY;
                  this.selectedPointsX[index] = path.elements[k].x;
                  this.selectedPointsY[index] = path.elements[k].y;
                  index++;
                }
              }
            }
          }

          this.startX = event.offsetX;
          this.startY = event.offsetY;
        } else {
          //Regular move without disconnecting from or snapping to a line
          let index = 0;
          let textIndex = 0;
          if (
            sketch.selectedElements < 2 &&
            sketch.selectedAreas < 1 &&
            sketch.lastSelection &&
            sketch.lastSelection.point
          ) {
            const path = sketch.lastSelection.path;
            if (!this.history) {
              this.history = sketch.createHistory();
            }
            if (
              path.start.x == sketch.lastSelection.point.x &&
              path.start.y == sketch.lastSelection.point.y &&
              path.closed
            ) {
              sketch.setPosition(path.start, position.x, position.y);
              sketch.setPosition(path.elements[path.elements.length - 1], position.x, position.y);
            } else {
              const e = path.elements.find(
                element =>
                  element.x == sketch.lastSelection.point.x &&
                  element.y == sketch.lastSelection.point.y
              );
              sketch.setPosition(e ?? path.start, position.x, position.y);
            }
          } else {
            const paths = []
            for (let i = 0; i < this.selection.length; i++) {
              let selected = this.selection[i];
              if (selected.path && !paths.find(p => p == selected.path)) {
                paths.push(selected.path)
                for (let j = 0; j < selected.sketchObject.objects.length; j++) {
                  const path = selected.sketchObject.objects[j];
                  if (path instanceof SketchPath) {
                    if (!this.history) {
                      this.history = sketch.createHistory();
                    }
                    let offsetX = position.x - start.x;
                    let offsetY = position.y - start.y;
                    const labelOffsetX = path.start.x;
                    const labelOffsetY = path.start.y;
                    const label = selected.layer.objects;
                    sketch.setPosition(
                      path.start,
                      this.selectedPointsX[index] + offsetX,
                      this.selectedPointsY[index] + offsetY
                    );
                    offsetX = path.start.x - this.selectedPointsX[index];
                    offsetY = path.start.y - this.selectedPointsY[index];
                    index++;
                    if (path.elements) {
                      for (let k = 0; k < path.elements.length; k++) {
                        path.elements[k].x = this.selectedPointsX[index] + offsetX;
                        path.elements[k].y = this.selectedPointsY[index] + offsetY;
                        index++;
                      }
                    }
                    for (let i = 0; i < label.length; i++) {
                      const linkedLabel = label[i].objects.find(
                        object => object.uniqueIdentifier == path.label
                      );
                      if (linkedLabel) {
                        sketch.setPosition(
                          linkedLabel,
                          path.start.x + (linkedLabel.x - labelOffsetX),
                          path.start.y + (linkedLabel.y - labelOffsetY)
                        );
                      }
                    }
                  }
                  else if (selected.sketchObject.objects[j] instanceof SketchCustomText &&
                    !selected.sketchObject.objects[j].isLabel || selected.sketchObject.objects[j].manuallyMoved) {
                    const text = selected.sketchObject.objects[j];
                    let offsetX = position.x - start.x;
                    let offsetY = position.y - start.y;
                    sketch.setPosition(
                      text,
                      this.selectedTexts[textIndex].x + offsetX,
                      this.selectedTexts[textIndex].y + offsetY
                    );
                    textIndex++;
                  }
                }
              }
              else {
                if (selected.path)
                  index += selected.path.elements.length + 1;
              }
            }
          }
        }
      }

      if (sketch.selected) {
        for (let i = 0; i < sketch.selected.length; i++) {
          let bounds = {};
          let object = sketch.selected[i].layer.objects.find(
            obj =>
              obj.objects && obj.objects.length > 0 && obj.objects[0].isLabel
          );
          if (object) {
            sketch.selected[i].layer.getBounds(bounds);
            if (bounds.max) {
              object.objects[0].x = (bounds.max.x + bounds.min.x) / 2;
              object.objects[0].y = (bounds.max.y + bounds.min.y) / 2;
            }
          }
          sketch.draw();
        }
      }
    }
  }

  /**
    * Called on mouse release
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {MouseEvent} event - Input event
    * @returns {void}
    *
    */
  performUp(sketch, event) {
    if (this.selection) {
      let point = false;
      let element = false;
      let area = false;
      let single = this.selection.length < 3;

      let candidates = [];
      sketch.findPathSelectionCandidates(
        event.offsetX,
        event.offsetY,
        candidates,
        false,
        false
      );
      if (candidates.length > 0 && !this.added && !this.isMove) {
        const candidate = this.selection.find(cand => candidates[0].path && candidates[0].path == cand.path)
        if (candidates[0].point) {
          this.removeFromSelection(sketch, candidates, 'point', 0);
        } else if (candidates[0].element) {
          this.removeFromSelection(sketch, candidates, "element", 0);
        } else if (candidates[0].path) {
          this.removeFromSelection(sketch, candidates, "path", 0);
        }
      }

      if (single) {
        for (let i = 0; i < this.selection.length; i++) {
          if (this.selection[i].point) {
            if (point) {
              single = false;
              break;
            } else {
              point = true;
            }
          }
          else if (this.selection[i].element) {
            if (element) {
              single = false;
              break;
            } else {
              element = true;
            }
          }
          else if (this.selection[i].path) {
            if (area) {
              single = false;
              break;
            } else {
              area = true;
            }
          }
        }
      }
      if (this.selection.length > 0 &&
        !this.selection[this.selection.length - 1].textObject) {
        sketch.interaction = new SketchSingleSelectInteraction();
        sketch.interaction.performUp(sketch, event);
      }
    }
    this.added = false
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
    * Mouse down event handler
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {MouseEvent} event - Input event
    * @returns {void}
    *
    */
  down(sketch, event) {
    if (!sketch.selected || sketch.selected.length < 1) {
      sketch.interaction = new SketchSingleSelectInteraction();
      sketch.interaction.down(sketch, event);
    }
    else {
      let found = this.performMultipleSelect(sketch, event);
      this.isDown = true;
      if (found && sketch.selected && sketch.selected.length > 0) {
        this.selection = sketch.selected;
        this.selectedPointsX = [];
        this.selectedPointsY = [];
        this.selectedTexts = [];
        for (let i = 0; i < this.selection.length; i++) {
          let selected = this.selection[i];
          for (let j = 0; j < selected.sketchObject.objects.length; j++) {
            if (selected.sketchObject.objects[j] instanceof SketchPath) {
              let path = selected.sketchObject.objects[j];
              this.selectedPointsX.push(path.start.x);
              this.selectedPointsY.push(path.start.y);
              if (path.elements) {
                path.elements.map(element => {
                  this.selectedPointsX.push(element.x);
                  this.selectedPointsY.push(element.y);
                });
              }
            }
            else if (selected.sketchObject.objects[j] instanceof SketchCustomText &&
              !selected.sketchObject.objects[j].isLabel || selected.sketchObject.objects[j].manuallyMoved) {
              this.selectedTexts.push({ x: selected.sketchObject.objects[j].x, y: selected.sketchObject.objects[j].y });
            }
          }
        }
        if (this.selection[0].path && this.selection[0].path.closed && this.selection[0].path.negative) {
          sketch.negativeAreaSelection(true);
        }
      } else {
        sketch.clearSelection();
        sketch.selectedElements = sketch.selectedAreas = 0;
        sketch.showContextBar("none");
      }
      this.startX = event.offsetX;
      this.startY = event.offsetY;
      sketch.draw();
    }
  }

  /**
    * Mouse move event handler
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
    * Mouse up event handler
    *
    * @param {SketchControl} sketch - The Sketch Control instance
    * @param {MouseEvent} event - Input event
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
    * Adds an object to the selection array
    *
    * @param {Array} selection - Selected objects
    * @returns {Array} - Points
    *
    */
  getPointsFromSelection(selection) {
    const points = [];

    if (selection && selection.length > 0) {
      const checkedPaths = []
      for (let i = 0; i < selection.length; i++) {
        let selected = selection[i];
        if (selected.path && selected.path.start && !checkedPaths.find(checked => checked == selected.path)) {
          checkedPaths.push(selected.path)
          points.push(selected.path.start);

          if (selected.path.elements && selected.path.elements.length > 0) {
            selected.path.elements.forEach(p => {
              points.push(p);
            });
          }
        }
      }
    }

    return points;
  }
}
