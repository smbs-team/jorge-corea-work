// ptas_SketchDistance.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
 * Custom Text measuring a line in feet and inches.
 */
class SketchDistance {
  constructor(sketchPath, start, finish) {
    this.sketchPath = sketchPath;
    this.start = start;
    this.finish = finish;
    this.xDistance = -1;
    this.yDistance = -1;
    this.lineOffsetY = 20;
    this.lineOffsetX = 30;
  }

  /**
   * Copies the current distance's properties into a new one.
   *
   * @returns {SketchDistance}
   */
  createCopy() {
    let copy = new SketchDistance(this.sketchPath, this.start, this.finish);
    if (this.sketchPath && this.sketchPath.closed) {
      copy.offset = { x: this.offset.x, y: this.offset.y };
    } else {
      copy.offset = { x: 0, y: 0 };
    }
    SketchUtils.copyTextAttributes(this, copy);
    return copy;
  }

  /**
   * Removes itself from the sketch
   *
   * @returns {void}
   */
  delete() {
    if (this.background !== undefined) {
      this.background.remove();
    }
    if (this.text !== undefined) {
      this.text.remove();
    }
  }

  /**
   * Enter frame event handler. Updates the Distance on the canvas.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Matrix} transform - Matrix for converting between canvas and offset positions
   * @returns {void}
   */
  draw(sketch, transform) {
    if (!sketch.hideDistances) {
      if (
        sketch.selected &&
        sketch.selected.length > 0 &&
        sketch.selected[0].textObject &&
        sketch.selected[0].textObject == this
      ) {
        const hideDistance = document.getElementById("hideDistance");
        const showDistance = document.getElementById("showDistance");
        if (this.fontColor && this.fontColor.substr(0, 7) == "#b70f0a") {
          hideDistance.style.visibility = "inherit";
          showDistance.style.visibility = "collapse";
        } else {
          hideDistance.style.visibility = "collapse";
          showDistance.style.visibility = "inherit";
          this.fontColor = undefined;
        }
        if (sketch.mode == SketchMode.Text) {
          document
            .getElementById("distance-buttons")
            .classList.add("distance-buttons-visible");
        }
        let style = sketch.styleSet.textSelectionStyle;
        let internalBounds = this.text.internalBounds;
        let expandedInternalBounds = style.applyExpandTo(internalBounds);
        let rotation = 0;
        let rotationInRadians = 0;
        if (this.rotation !== undefined && this.rotation !== null) {
          rotation = this.rotation;
          rotationInRadians = rotation * (Math.PI / 180);
        }
        let rotationDiff = rotation - this.text.rotation;
        if (rotationDiff !== 0) {
          this.text.rotate(rotationDiff);
        }

        //Selection  frame
        if (this.selectionPath === undefined) {
          this.selectionPath = new paper.Path();
        } else {
          this.selectionPath.removeSegments();
        }
        sketch.relocateInMainScopeActiveLayer(this.selectionPath);
        const frameWidth = expandedInternalBounds.width;
        const frameHeight = expandedInternalBounds.height;
        this.selectionPath.moveTo(
          this.text.position.x - frameWidth / 2,
          this.text.position.y + frameHeight / 2
        );
        this.selectionPath.lineTo(
          this.text.position.x + frameWidth / 2,
          this.text.position.y + frameHeight / 2
        );
        this.selectionPath.lineTo(
          this.text.position.x + frameWidth / 2,
          this.text.position.y - frameHeight / 2
        );
        this.selectionPath.lineTo(
          this.text.position.x - frameWidth / 2,
          this.text.position.y - frameHeight / 2
        );
        this.selectionPath.lineTo(
          this.text.position.x - frameWidth / 2,
          this.text.position.y + frameHeight / 2
        );
        this.selectionPath.closed = true;
        style.applyStrokeTo(this.selectionPath);

        //Rotate icon
        let rotateIconPosition = { x: 0, y: 0 };
        const rotateIconImageSrc = sketch.isRotating
          ? sketch.rotateIconHighlighted
          : sketch.rotateIcon;
        const distanceToRotateIcon = 80;
        rotateIconPosition.x =
          Math.cos(rotationInRadians - Math.PI / 2) * distanceToRotateIcon +
          this.text.position.x;
        rotateIconPosition.y =
          Math.sin(rotationInRadians - Math.PI / 2) * distanceToRotateIcon +
          this.text.position.y;
        if (!this.rotateIconRaster) {
          this.rotateIconRaster = new paper.Raster({
            source: rotateIconImageSrc,
            position: { x: rotateIconPosition.x, y: rotateIconPosition.y }
          });
          this.rotateIconRaster.scale(0.5);
          this.rotateIconRaster.onMouseDown = event => {
            sketch.isRotating = true;
            this.rotateIconImage = rotateIconImageSrc;

            //Stopping propagation here seems to not have effect, because the down method of
            // text interaction is still executed. So, the flag isRotating was used to prevent
            // the interaction from 'de-selecting' the text object.
            event.stopPropagation();
          };
        } else {
          this.rotateIconRaster.position = {
            x: rotateIconPosition.x,
            y: rotateIconPosition.y
          };
          if (this.rotateIconImage != rotateIconImageSrc) {
            this.rotateIconRaster.source = rotateIconImageSrc;
            this.rotateIconImage = rotateIconImageSrc;
          }
        }
        sketch.relocateInMainScopeActiveLayer(this.rotateIconRaster);

        //Line from text frame to rotate icon
        let lineToRotateIconStyle =
          sketch.styleSet.textSelectionLineToRotateIconStyle;
        if (this.lineToRotateIcon === undefined) {
          this.lineToRotateIcon = new paper.Path();
        } else {
          this.lineToRotateIcon.removeSegments();
        }
        const lineStart = { x: 0, y: 0 };
        const lineEnd = { x: 0, y: 0 };
        lineStart.x =
          Math.cos(rotationInRadians - Math.PI / 2) * (frameHeight / 2) +
          this.text.position.x;
        lineStart.y =
          Math.sin(rotationInRadians - Math.PI / 2) * (frameHeight / 2) +
          this.text.position.y;
        lineEnd.x =
          Math.cos(rotationInRadians - Math.PI / 2) *
            (distanceToRotateIcon - 10) +
          this.text.position.x;
        lineEnd.y =
          Math.sin(rotationInRadians - Math.PI / 2) *
            (distanceToRotateIcon - 10) +
          this.text.position.y;
        this.lineToRotateIcon.moveTo(lineStart.x, lineStart.y);
        this.lineToRotateIcon.lineTo(lineEnd.x, lineEnd.y);
        lineToRotateIconStyle.applyStrokeTo(this.lineToRotateIcon);
        sketch.relocateInMainScopeActiveLayer(this.lineToRotateIcon);

        if (rotation !== 0) {
          this.selectionPath.rotate(rotation);
        }
      }

      if (this.finish > this.sketchPath.elements.length) {
        this.finish = this.sketchPath.elements.length;
        this.start = this.finish - 1;
      }

      if (
        this.sketchPath.start !== undefined &&
        this.sketchPath.elements !== undefined
      ) {
        let startX = undefined;
        let startY = undefined;
        if (this.start == 0) {
          startX = this.sketchPath.start.x;
          startY = this.sketchPath.start.y;
        } else {
          startX = this.sketchPath.elements[this.start - 1].x;
          startY = this.sketchPath.elements[this.start - 1].y;
        }
        let finishX = undefined;
        let finishY = undefined;
        if (this.finish == 0) {
          finishX = this.sketchPath.start.x;
          finishY = this.sketchPath.start.y;
        } else {
          finishX = this.sketchPath.elements[this.finish - 1].x;
          finishY = this.sketchPath.elements[this.finish - 1].y;
        }
        const deltaX = finishX - startX;
        const deltaY = finishY - startY;
        const length = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
        if (length > 0) {
          let angle = 0;
          if (this.finish == 0) {
            const angleFromElement = this.sketchPath.elements[
              this.sketchPath.elements.length - 1
            ].angle;
            if (angleFromElement !== undefined) {
              angle = angleFromElement;
            }
          } else {
            const angleFromElement = this.sketchPath.elements[this.finish - 1]
              .angle;
            if (angleFromElement !== undefined) {
              angle = angleFromElement;
            }
          }
          let positionX = undefined;
          let positionY = undefined;
          let lengthToDisplay = undefined;
          if (angle !== 0) {
            let inRadians = (Math.abs(angle) * Math.PI) / 180;
            let startXToUse = undefined;
            let startYToUse = undefined;
            let finishXToUse = undefined;
            let finishYToUse = undefined;
            if (angle > 0) {
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
            let segmentX = finishXToUse - startXToUse;
            let segmentY = finishYToUse - startYToUse;
            const distance = Math.sqrt(
              segmentX * segmentX + segmentY * segmentY
            );
            const halfDistance = distance / 2;
            const factor = Math.tan(Math.PI / 2 - inRadians / 2);
            const distanceToCenter = factor * halfDistance;
            const directionX = segmentY / distance;
            const directionY = -segmentX / distance;
            const centerX =
              (startXToUse + finishXToUse) / 2 + directionX * distanceToCenter;
            const centerY =
              (startYToUse + finishYToUse) / 2 + directionY * distanceToCenter;
            segmentX = startXToUse - centerX;
            segmentY = startYToUse - centerY;
            const radius = Math.sqrt(segmentX * segmentX + segmentY * segmentY);
            positionX = centerX - directionX * radius;
            positionY = centerY - directionY * radius;
            lengthToDisplay = radius * inRadians;
          } else {
            positionX = (startX + finishX) / 2;
            positionY = (startY + finishY) / 2;
            lengthToDisplay = length;
          }
          let position = undefined;
          if (this.offset !== undefined) {
            let forwardX = (deltaX * this.offset.x) / length;
            let forwardY = (deltaY * this.offset.x) / length;
            let sideX = (-deltaY * this.offset.y) / length;
            let sideY = (deltaX * this.offset.y) / length;
            position = new paper.Point(
              positionX + forwardX + sideX,
              positionY + forwardY + sideY
            );
          } else {
            position = new paper.Point(positionX, positionY);
          }
          position = position.transform(transform);

          let style = undefined;
          let isSelected = false;
          if (sketch.selected !== undefined) {
            for (let i = 0; i < sketch.selected.length; i++) {
              if (sketch.selected[i].path === this.sketchPath) {
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
          } else if (
            sketch.selected !== undefined &&
            sketch.selected.length > 0
          ) {
            style = sketch.styleSet.objectOutOfSelectionStyle;
          } else if (this.sketchPath.closed) {
            style = sketch.styleSet.style;
          } else {
            style = sketch.styleSet.openObjectStyle;
          }
          let rectangle = new paper.Rectangle(position.x, position.y, 1, 1);
          if (this.background === undefined) {
            this.background = new paper.Path.Rectangle(rectangle);
          }
          if (this.text === undefined) {
            this.text = new paper.PointText(position);
          }
          if (this.fontColor && !this.alpha)
            this.alpha = Math.round(0xff / 2).toString(16);
          if (this.fontColor && this.fontColor.substr(0, 7) == "#b70f0a") {
            if (sketch.mode == SketchMode.Text) {
              this.fontColor = this.fontColor.substr(0, 7) + this.alpha;
            } else {
              this.fontColor = "#b70f0a00";
            }
          }
          this.alpha = undefined;
          if (this.rotation !== undefined && this.rotation !== null) {
            this.text.rotation = this.rotation;
          }
          SketchUtils.drawTextWithBackground(
            sketch,
            this.text,
            position,
            lengthToDisplay,
            this.background,
            style,
            this
          );
        }
      }
    }
  }

  /**
   * Returns a text selection candidate.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {number} positionX
   * @param {number} positionY
   * @returns {object} - Object with a textObject property
   */
  createSelectionCandidate(sketch, positionX, positionY) {
    if (this.text) {
      let text = this.text;
      const style = sketch.styleSet.textSelectionStyle;
      const expandedBounds = style.applyExpandTo(text.bounds);
      const position = new paper.Point(positionX, positionY);
      if (expandedBounds.contains(position)) {
        return { textObject: this };
      }
      if (style.radius !== undefined) {
        let selectionPointBounds = new paper.Rectangle(
          expandedBounds.x + expandedBounds.width - style.radius,
          expandedBounds.y + expandedBounds.height - style.radius,
          style.radius * 2,
          style.radius * 2
        );
        if (selectionPointBounds.contains(position)) {
          return { textObject: this };
        }
      }
    }
  }
}
