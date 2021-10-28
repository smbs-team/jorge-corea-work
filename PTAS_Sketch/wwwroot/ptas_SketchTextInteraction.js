// ptas_SketchTextInteraction.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Interactions for the Text mode
*/
class SketchTextInteraction {
  constructor() {
    this.shouldPerformRotateCount = 0;
    this.foundArray = [];
  }

  /**
   * Selects the CustomText based on the cursor position.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Input event
   * @returns {boolean}
   */
  performTextSelect(sketch, event) {
    const candidates = [];
    sketch.findTextSelectionCandidates(
      sketch,
      event.offsetX,
      event.offsetY,
      candidates
    );
    if (candidates.length < 1 && sketch.mode == SketchMode.Text) {
      sketch.interaction = new SketchSingleSelectInteraction();
      sketch.down(event);
    } else {
      let current = -1;
      if (sketch.selected && sketch.selected.length > 0) {
        for (let i = 0; i < candidates.length; i++) {
          let candidate = candidates[i];
          for (let j = 0; j < sketch.selected.length; j++) {
            let selection = sketch.selected[j];
            if (
              candidate.sketchObject === selection.sketchObject &&
              candidate.textObject === selection.textObject
            ) {
              current = i;
              break;
            }
          }
        }
      }
      this.foundArray = candidates;
      if (current >= 0) {
        if (current === candidates.length - 1) {
          if (candidates.length > 1) {
            sketch.clearSelection();
          }
        } else {
          sketch.selected = [candidates[current + 1]];
        }
      } else if (candidates.length > 0) {
        sketch.selected = [candidates[0]];
      }
    }
    return candidates.length > 0;
  }

  /**
   * Determines the rail direction based on the given vertices.
   *
   * @param {object} p1 - First point
   * @param {object} p2 - Second point
   * @param {object} p3 - Third point
   * @returns {number}
   */
  sign(p1, p2, p3) {
    return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
  }

  /**
   * Determines whether the cursor is within the given vertices.
   *
   * @param {object} pt - Cursor position
   * @param {object} v1 - First point
   * @param {object} v2 - Second point
   * @param {object} v3 - Third point
   * @returns {boolean}
   */
  inTriangle(pt, v1, v2, v3) {
    let d1, d2, d3, has_neg, has_pos;

    d1 = this.sign(pt, v1, v2);
    d2 = this.sign(pt, v2, v3);
    this.offsetY = d3 = this.sign(pt, v3, v1);

    has_neg = d1 < 0 || d2 < 0 || d3 < 0;
    has_pos = d1 > 0 || d2 > 0 || d3 > 0;
    return !(has_neg && has_pos);
  }

  /**
   * Updates the cursor's position in Text mode
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Input event
   * @returns {void}
   */
  performMove(sketch, event) {
    if (
      this.isMove ||
      this.startX !== event.offsetX ||
      this.startY !== event.offsetY
    ) {
      this.isMove = true;
      if (this.selection) {
        if (this.history === undefined) {
          this.history = sketch.createHistory();
        }
        const position = sketch.inverseTopLevelTransform.transform(
          new paper.Point(event.offsetX, event.offsetY)
        );
        const start = sketch.inverseTopLevelTransform.transform(
          new paper.Point(this.startX, this.startY)
        );
        if (this.selection.textObject.offset) {
          let forwardX = this.lineEndX - this.lineStartX;
          let forwardY = this.lineEndY - this.lineStartY;
          let cursorDeltaX = position.x - this.lineStartX;
          let cursorDeltaY = position.y - this.lineStartY;
          let length = Math.sqrt(forwardX * forwardX + forwardY * forwardY);
          if (length > 0) {
            forwardX = forwardX / length;
            forwardY = forwardY / length;
            let projection = forwardX * cursorDeltaX + forwardY * cursorDeltaY;
            if (projection < 0) {
              projection = 0;
            } else if (projection > length) {
              projection = length;
            }
            this.selection.textObject.offset.x = projection - length / 2;

            let v2;
            let triangleSide;
            let minX = Math.min(this.lineStartX, this.lineEndX);
            let maxX = Math.max(this.lineStartX, this.lineEndX);
            let minY = Math.min(this.lineStartY, this.lineEndY);
            let maxY = Math.max(this.lineStartY, this.lineEndY);
            let newPosition = { x: position.x, y: position.y };
            if (Math.abs(forwardX) > Math.abs(forwardY)) {
              if (this.lineStartX > this.lineEndX) {
                if (this.lineStartY > this.lineEndY) {
                  v2 = { x: this.lineEndX, y: this.lineStartY };
                  triangleSide = 0;
                } else {
                  v2 = { x: this.lineStartX, y: this.lineEndY };
                  triangleSide = 1;
                }
              } else {
                if (this.lineStartY > this.lineEndY) {
                  v2 = { x: this.lineStartX, y: this.lineEndY };
                  triangleSide = 2;
                } else {
                  v2 = { x: this.lineEndX, y: this.lineStartY };
                  triangleSide = 3;
                }
              }
            } else {
              if (this.lineStartX > this.lineEndX) {
                if (this.lineStartY > this.lineEndY) {
                  v2 = { x: this.lineEndX, y: this.lineStartY };
                  triangleSide = 4;
                } else {
                  v2 = { x: this.lineStartX, y: this.lineEndY };
                  triangleSide = 5;
                }
              } else {
                if (this.lineStartY > this.lineEndY) {
                  v2 = { x: this.lineStartX, y: this.lineEndY };
                  triangleSide = 6;
                } else {
                  v2 = { x: this.lineEndX, y: this.lineStartY };
                  triangleSide = 7;
                }
              }
            }

            if (
              newPosition.x < minX &&
              triangleSide !== 5 &&
              triangleSide !== 7
            ) {
              newPosition.x = minX;
            } else if (
              newPosition.x > maxX &&
              triangleSide !== 6 &&
              triangleSide !== 4
            ) {
              newPosition.x = maxX;
            }
            if (
              newPosition.y < minY &&
              triangleSide !== 0 &&
              triangleSide !== 1
            ) {
              newPosition.y = minY;
            } else if (
              newPosition.y > maxY &&
              triangleSide !== 2 &&
              triangleSide !== 3
            ) {
              newPosition.y = maxY;
            }

            if (
              this.inTriangle(
                newPosition,
                { x: this.lineStartX, y: this.lineStartY },
                v2,
                { x: this.lineEndX, y: this.lineEndY }
              )
            ) {
              if (this.offsetY >= 0) {
                this.offsetY = this.sign(
                  position,
                  { x: this.lineEndX, y: this.lineEndY },
                  { x: this.lineStartX, y: this.lineStartY }
                );
              }
              this.selection.textObject.offset.y = Math.max(
                -this.offsetY / 6,
                -2
              );
            } else {
              this.selection.textObject.offset.y = Math.min(
                -this.offsetY / 6,
                2
              );
            }
          }
        } else {
          const offsetX = position.x - start.x;
          const offsetY = position.y - start.y;
          sketch.setPosition(
            this.selection.textObject,
            this.selectedPointX + offsetX,
            this.selectedPointY + offsetY
          );
        }
        this.selection.textObject.manuallyMoved = true;
        this.selection.textObject.xDistance = event.clientX;
        this.selection.textObject.yDistance = event.clientY;
        if (this.selection.textObject.arrow) {
          const arrow = this.selection.layer.objects.find(
            object =>
              object.objects &&
              object.objects.length > 0 &&
              object.objects[0].uniqueIdentifier ==
              this.selection.textObject.arrow
          );
          if (arrow) {
            arrow.objects[0].start = {
              x: this.selection.textObject.x,
              y: this.selection.textObject.y
            };
          }
        }
        sketch.draw();
      }
    }
  }

  /**
   * Rotates the text based on the cursor position.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Mouse event
   * @returns {void}
   */
  performRotate(sketch, event) {
    if (this.shouldPerformRotateCount % 3 != 0) {
      this.shouldPerformRotateCount++;
      return;
    }
    this.shouldPerformRotateCount++;

    if (this.history === undefined) {
      this.history = sketch.createHistory();
    }
    const position = sketch.inverseTopLevelTransform.transform(
      new paper.Point(event.offsetX, event.offsetY)
    );
    let textPosition = isNaN(sketch.selected[0].textObject.x)
      ? {
        x: sketch.selected[0].textObject.text.position.x,
        y: sketch.selected[0].textObject.text.position.y
      }
      : {
        x: sketch.selected[0].textObject.x,
        y: sketch.selected[0].textObject.y
      };
    let text = new paper.PointText(textPosition);
    text.content = sketch.selected[0].textObject;

    if (isNaN(sketch.selected[0].textObject.x)) {
      textPosition = sketch.inverseTopLevelTransform.transform(
        new paper.Point(textPosition.x, textPosition.y)
      );
    }

    let deltaX = position.x - textPosition.x;
    let deltaY = position.y - textPosition.y;
    let angle = Math.atan2(deltaY, deltaX);
    angle = angle * (180 / Math.PI) - 90;
    sketch.selected[0].textObject.rotation = angle * -1;
    sketch.draw();
  }

  /**
   * Restores the initial state after button release.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Input event
   * @returns {void}
   */
  performUp(sketch, event) {
    if (sketch.mode !== SketchMode.Text) {
      if (sketch.mode == SketchMode.SingleSelect)
        sketch.interaction = new SketchSingleSelectInteraction();
      else
        sketch.interaction = new SketchMultipleSelectInteraction();
    }
    if (
      (this.isMove === undefined || !this.isMove) &&
      this.selection === undefined &&
      !sketch.isRotating &&
      !sketch.selected &&
      sketch.mode == SketchMode.Text
    ) {
      if (sketch.textInputElement === undefined) {
        sketch.textInputElement = document.createElement("input");
        sketch.textInputElement.setAttribute("type", "text");
        sketch.textInputElement.style.position = "absolute";
        sketch.textInputElement.setAttribute(
          "oninput",
          sketch.textInputElementTextChangedName + "()"
        );
        sketch.textInputElement.setAttribute(
          "onkeyup",
          sketch.textInputElementKeyUpName + "(event)"
        );
        sketch.root.appendChild(sketch.textInputElement);
      }
      sketch.textInputElement.style.left = event.offsetX + "px";
      sketch.textInputElement.style.top = event.offsetY + "px";
      const style = sketch.styleSet.customTextStyle;
      if (style !== undefined && style.fontFamily !== undefined) {
        sketch.textInputElement.style.fontFamily = style.fontFamily;
      } else {
        sketch.textInputElement.style.fontFamily =
          sketch.canvas.style.fontFamily;
      }
      if (style !== undefined && style.fontSize !== undefined) {
        sketch.textInputElement.style.fontSize = style.fontSize;
      } else {
        sketch.textInputElement.style.fontSize = sketch.canvas.style.fontSize;
      }
      if (style !== undefined && style.color !== undefined) {
        sketch.textInputElement.style.color = style.color;
      } else {
        sketch.textInputElement.style.color = sketch.canvas.style.color;
      }
      sketch.textInputElement.style.visibility = "visible";
      sketch.textInputElement.value = "";
      sketch.textInputElement.setAttribute("size", "10");
      sketch.textInputElement.focus();
      sketch.textInputElementPosition = { x: this.startX, y: this.startY };
    } else {
      sketch.dismissTextInput();
      if (sketch.customTextEditing) {
        sketch.customTextEditing.editingText = false;
        sketch.customTextEditing = null;
      }
      parent.sketchControl.autoSave();
      sketch.isRotating = false;
    }

    sketch.isRotating = false;
    this.isDown = sketch.isDown = false;
    this.isMove = false;
    this.selection = undefined;
    this.startX = undefined;
    this.startY = undefined;
    this.selectedPointX = undefined;
    this.selectedPointY = undefined;
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
   * @param {boolean} isRotating - Whether it's rotating
   * @returns {void}
   */
  down(sketch, event, isRotating) {
    if (!isRotating) {
      let foundArray =
        this.foundArray.length > 0 ? this.foundArray : sketch.selected;
      let found = this.performTextSelect(sketch, event);
      this.isDown = true;
      if (!sketch.selected || !sketch.selected[0].path) {
        if (
          foundArray &&
          foundArray.length > 0 &&
          sketch.selected &&
          sketch.selected.length > 0 &&
          foundArray[0].textObject == sketch.selected[0].textObject &&
          Math.abs(
            foundArray[0].textObject.rotateIconRaster.position.x - event.clientX
          ) < 8 &&
          Math.abs(
            foundArray[0].textObject.rotateIconRaster.position.y - event.clientY
          ) < 8
        ) {
          sketch.isRotating = true;
        } else {
          document
            .getElementById("distance-buttons")
            .classList.remove("distance-buttons-visible");
          if (
            found &&
            sketch.selected !== undefined &&
            sketch.selected.length > 0
          ) {
            this.selection = sketch.selected[0];
            let position = isNaN(this.selection.textObject.x)
              ? this.selection.textObject.text.position
              : this.selection.textObject;
            this.selectedPointX = position.x;
            this.selectedPointY = position.y;
          }
        }
      }
    }

    this.startX = event.offsetX;
    this.startY = event.offsetY;

    if (this.selection && this.selection.textObject.offset) {
      if (this.selection.textObject.start == 0) {
        this.lineStartX = this.selection.textObject.sketchPath.start.x;
        this.lineStartY = this.selection.textObject.sketchPath.start.y;
      } else {
        this.lineStartX = this.selection.textObject.sketchPath.elements[
          this.selection.textObject.start - 1
        ].x;
        this.lineStartY = this.selection.textObject.sketchPath.elements[
          this.selection.textObject.start - 1
        ].y;
      }
      if (this.selection.textObject.finish == 0) {
        this.lineEndX = this.selection.textObject.sketchPath.start.x;
        this.lineEndY = this.selection.textObject.sketchPath.start.y;
      } else {
        this.lineEndX = this.selection.textObject.sketchPath.elements[
          this.selection.textObject.finish - 1
        ].x;
        this.lineEndY = this.selection.textObject.sketchPath.elements[
          this.selection.textObject.finish - 1
        ].y;
      }
    }
    if (this.selection && this.selection.textObject instanceof SketchDistance) {
      sketch.lastAction = "distance";
    } else {
      sketch.lastAction = "text";
    }

    sketch.draw();
  }

  /**
   * Mouse move event handler.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Input event
   * @param {boolean} isRotating - Whether it's rotating
   * @returns {void}
   */
  move(sketch, event, isRotating) {
    if (isRotating) {
      this.performRotate(sketch, event);
    } else if (this.isDown || sketch.isDown) {
      this.performMove(sketch, event);
    }
  }

  /**
   * Mouse up event handler.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Input event
   * @returns {void}
   */
  up(sketch, event) {
    if (this.isDown || sketch.isDown) {
      this.performMove(sketch, event);
      this.performUp(sketch, event);
    } else if (sketch.isRotating) {
      this.performUp(sketch, event);
    }
  }

  /**
   * Resizes the text field in order to fit the content.
   *
   * @param {SketchControl} sketch - Control instance
   * @returns {void}
   */
  textChanged(sketch) {
    if (sketch.textInputElement && sketch.textInputElement.style.visibility === "visible") {
      let width = sketch.textInputElement.value.length + 3;
      sketch.textInputElement.setAttribute("size", Math.max(width, 10));
    }
  }

  /**
   * Key up event handler.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Mouse event
   * @returns {void}
   */
  keyUp(sketch, event) {
    if (
      sketch.textInputElement !== undefined &&
      sketch.textInputElement.style.visibility === "visible"
    ) {
      if (sketch.textInputElement.value != "" && event.key === "Enter") {
        const history = sketch.createHistory();

        if (sketch.customTextEditing) {
          sketch.customTextEditing.customText = sketch.textInputElement.value;
          sketch.customTextEditing.editingText = false;
          sketch.customTextEditing = null;
        } else {
          let customTextObject = new SketchObject();
          let layerToEdit = sketch.sketchLayerToEdit
            ? sketch.sketchLayerToEdit
            : sketch.defaultSketchLayerToEdit;
          sketch.customTextLayer = sketch.getLayer(layerToEdit);
          if (sketch.customTextLayer === undefined) {
            sketch.customTextLayer = new SketchLayer();
            sketch.customTextLayer.visible = true;
            sketch.customTextLayer.name = "Custom text";
            sketch.customTextLayer.uniqueIdentifier = SketchControl.uuidv4();
            if (sketch.layers === undefined) {
              sketch.layers = [sketch.customTextLayer];
            } else {
              parent.sketchControl.layers.push(sketch.customTextLayer);
            }
          }
          if (sketch.customTextLayer.objects) {
            sketch.customTextLayer.objects.push(customTextObject);
          } else {
            sketch.customTextLayer.objects = [customTextObject];
          }
          const style = sketch.textInputElement.style;
          sketch.textInputElementPosition.x = parseInt(
            style.left.slice(0, style.left.length - 2)
          );
          sketch.textInputElementPosition.y = parseInt(
            style.top.slice(0, style.top.length - 2)
          );
          let customText = new SketchCustomText();
          customText.uniqueIdentifier = SketchControl.uuidv4();
          customText.customText = sketch.textInputElement.value;
          const position = sketch.inverseTopLevelTransform.transform(
            new paper.Point(
              sketch.textInputElementPosition.x,
              sketch.textInputElementPosition.y
            )
          );
          customText.x = position.x;
          customText.y = position.y;
          customText.rotation = 0;
          if (customTextObject.objects === undefined) {
            customTextObject.objects = [customText];
          } else {
            customTextObject.objects.push(customText);
          }
        }
        sketch.addToUndoHistory(history);
        sketch.autoSave();
      }
      if (event.key === "Enter" || event.key === "Escape") {
        sketch.dismissTextInput();

        if (sketch.customTextEditing) {
          sketch.customTextEditing.editingText = false;
          sketch.customTextEditing = null;
        }
      }
    }
  }

  /**
   * Double click event handler.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Event} event - Mouse event
   * @returns {void}
   */
  doubleClick(sketch, event) {
    sketch.isRotating = false;
    let found = this.performTextSelect(sketch, event);
    if (found && sketch.selected && sketch.selected.length > 0) {
      this.selection = sketch.selected[0];
      let position = isNaN(this.selection.textObject.x)
        ? this.selection.textObject.text.position
        : this.selection.textObject;
      this.selectedPointX = position.x;
      this.selectedPointY = position.y;
      this.selection.textObject.editingText = true;
    }
    sketch.draw();
  }
}
