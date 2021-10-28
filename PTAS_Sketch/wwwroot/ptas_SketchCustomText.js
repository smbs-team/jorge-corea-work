// ptas_SketchCustomText.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
 * Text elements on the sketch, including distances, are Custom Texts.
 */
class SketchCustomText {
  /**
   * Copies the current Text's properties into a new Custom Text.
   *
   * @returns {SketchCustomText}
   */
  createCopy() {
    let copy = new SketchCustomText();
    copy.uniqueIdentifier = this.uniqueIdentifier;
    copy.customText = this.customText;
    copy.x = this.x;
    copy.y = this.y;
    copy.rotation = this.rotation;
    copy.isLabel = this.isLabel;
    copy.path = this.path;
    copy.manuallyMoved = this.manuallyMoved;
    copy.arrow = this.arrow;
    SketchUtils.copyTextAttributes(this, copy);
    return copy;
  }

  /**
   * Relocates Text in case another Text is found converging the same area.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {SketchCustomText} text - Text to compare
   * @returns {void}
   */
  detectCollision(sketch, text) {
    let current = sketch.layers.length;
    for (let i = 0; i < sketch.layers.length; i++) {
      const layer = sketch.layers[i];
      if (layer.objects && text.fontSize.length) {
        const layerObject = layer.objects.find(
          object =>
            object.objects[0] instanceof SketchCustomText &&
            object.objects[0].isLabel &&
            object.objects[0].texts
        );
        if (layerObject) {
          if (layerObject.objects[0].texts[0] == text) {
            current = i;
          } else {
            const layerText = layerObject.objects[0].texts[0];
            if (
              text.bounds.x + text.bounds.width > layerText.bounds.x &&
              text.bounds.x < layerText.bounds.x + layerText.bounds.width &&
              text.bounds.y + text.bounds.height * 0.5 > layerText.bounds.y &&
              text.bounds.y + text.bounds.height * 0.5 <
                layerText.bounds.y + layerText.bounds.height
            ) {
              const currentText = i > current ? text : layerText;
              currentText.bounds.y += Math.min(
                layerText.bounds.height,
                text.bounds.height
              );
              this.detectCollision(sketch, currentText);
            }
          }
        }
      }
    }
  }

  /**
   * Enter frame event handler. Updates the Text on the canvas.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Matrix} transform - Matrix for converting between canvas and offset positions
   * @returns {void}
   */
  draw(sketch, transform) {
    if (this.drawCounter !== sketch.drawCounter) {
      this.drawCounter = sketch.drawCounter;
      this.textCount = 0;
    }
    let style = sketch.styleSet.customTextStyle;
    let position = new paper.Point(this.x, this.y);
    position = position.transform(transform);
    let text = undefined;
    if (this.texts === undefined) {
      text = new paper.PointText(position);
      this.texts = [text];
      this.textCount = 1;
    } else if (this.textCount >= this.texts.length) {
      text = new paper.PointText(position);
      this.texts.push(text);
      this.textCount = this.texts.length;
    } else {
      text = this.texts[this.textCount];
      if (!this.isLabel) text.content = "";
      text.position = position;
      this.textCount++;
    }
    this.detectCollision(sketch, text);
    sketch.relocateInMainScopeActiveLayer(text);
    sketch.applyTextStyleWithAttributes(text, style, this);
    text.content = `${this.customText} ${
      sketch.urlParams.get("readonly") &&
      this.isLabel &&
      !sketch.hideSF &&
      this.path
        ? "\n" + Math.round(this.path.area) + " ⏍"
        : ""
    }`;
    let justification = "left";
    if (this.horizontalAlign === SketchTextHorizontalAlign.Center) {
      justification = "center";
    } else if (this.horizontalAlign === SketchTextHorizontalAlign.Right) {
      justification = "right";
    }
    text.justification = justification;
    let bounds = text.bounds;
    let offsetY = 0;
    if (this.verticalAlign === SketchTextVerticalAlign.Top) {
      offsetY = -text.fontSize * sketch.resolution;
    } else if (this.verticalAlign === SketchTextVerticalAlign.Middle) {
      offsetY = bounds.height / 2 - text.fontSize * sketch.resolution;
    } else if (this.verticalAlign === SketchTextVerticalAlign.Bottom) {
      offsetY = bounds.height - text.fontSize * sketch.resolution;
    }
    let rotation = 0;
    let rotationInRadians = 0;
    if (this.rotation !== undefined) {
      rotation = this.rotation;
      rotationInRadians = rotation * (Math.PI / 180);
    }
    let rotationDiff = rotation - text.rotation;
    if (rotationDiff !== 0) {
      text.rotate(rotationDiff);
    }
    let isSelected = false;
    if (sketch.selected !== undefined) {
      for (let i = 0; i < sketch.selected.length; i++) {
        if (sketch.selected[i].textObject === this) {
          isSelected = true;
          break;
        }
      }
    }
    if (this.selectionPath === undefined) {
      this.selectionPath = new paper.Path();
    } else {
      this.selectionPath.removeSegments();
      this.selectionPath.strokeColor = undefined;
    }
    style = sketch.styleSet.textSelectionStyle;
    let expandedBounds = style.applyExpandTo(bounds);
    let internalBounds = text.internalBounds;
    let expandedInternalBounds = style.applyExpandTo(internalBounds);
    sketch.relocateInMainScopeActiveLayer(this.selectionPath);
    const frameWidth = expandedInternalBounds.width;
    const frameHeight = expandedInternalBounds.height;
    this.selectionPath.moveTo(
      text.position.x - frameWidth / 2,
      text.position.y + frameHeight / 2
    );
    this.selectionPath.lineTo(
      text.position.x + frameWidth / 2,
      text.position.y + frameHeight / 2
    );
    this.selectionPath.lineTo(
      text.position.x + frameWidth / 2,
      text.position.y - frameHeight / 2
    );
    this.selectionPath.lineTo(
      text.position.x - frameWidth / 2,
      text.position.y - frameHeight / 2
    );
    this.selectionPath.lineTo(
      text.position.x - frameWidth / 2,
      text.position.y + frameHeight / 2
    );
    this.selectionPath.closed = true;
    if (this.arrow) {
      style.applyFillTo(this.selectionPath);
      paper.project.activeLayer.insertChild(1, this.selectionPath);
    } else {
      this.selectionPath.fillColor = undefined;
    }
    if (isSelected) {
      //Rotate icon
      let rotateIconPosition = { x: 0, y: 0 };
      const rotateIconImageSrc = sketch.isRotating
        ? sketch.rotateIconHighlighted
        : sketch.rotateIcon;
      const distanceToRotateIcon = 80;
      rotateIconPosition.x =
        Math.cos(rotationInRadians - Math.PI / 2) * distanceToRotateIcon +
        text.position.x;
      rotateIconPosition.y =
        Math.sin(rotationInRadians - Math.PI / 2) * distanceToRotateIcon +
        text.position.y;
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

      //Text frame
      style.applyStrokeTo(this.selectionPath, true);

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
        text.position.x;
      lineStart.y =
        Math.sin(rotationInRadians - Math.PI / 2) * (frameHeight / 2) +
        text.position.y;
      lineEnd.x =
        Math.cos(rotationInRadians - Math.PI / 2) *
          (distanceToRotateIcon - 10) +
        text.position.x;
      lineEnd.y =
        Math.sin(rotationInRadians - Math.PI / 2) *
          (distanceToRotateIcon - 10) +
        text.position.y;
      this.lineToRotateIcon.moveTo(lineStart.x, lineStart.y);
      this.lineToRotateIcon.lineTo(lineEnd.x, lineEnd.y);
      lineToRotateIconStyle.applyStrokeTo(this.lineToRotateIcon);
      sketch.relocateInMainScopeActiveLayer(this.lineToRotateIcon);

      const selectionPointPosition = new paper.Point(
        text.position.x + frameWidth / 2,
        text.position.y + frameHeight / 2
      );
      if (this.selectionPoint === undefined) {
        this.selectionPoint = new paper.Path.Circle(selectionPointPosition, 1);
      } else {
        this.selectionPoint.position = selectionPointPosition;
      }
      sketch.relocateInMainScopeActiveLayer(this.selectionPoint);
      style.applyPointStyleTo(this.selectionPoint);

      if (rotation !== 0) {
        this.selectionPath.rotate(rotation);
        this.selectionPoint.rotate(
          rotation,
          new paper.Point(text.position.x, text.position.y)
        );
      }

      if (this.editingText) {
        text.visible = false;
        this.rotateIconRaster.visible = false;
        this.selectionPath.visible = false;
        this.lineToRotateIcon.visible = false;
        this.selectionPoint.visible = false;

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
        sketch.textInputElement.style.left = "" + expandedBounds.x + "px";
        sketch.textInputElement.style.top = "" + event.offsetY + "px";
        sketch.textInputElement.style.visibility = "visible";
        sketch.textInputElement.value = text.content;
        let width = sketch.textInputElement.value.length + 3;
        sketch.textInputElement.setAttribute("size", Math.max(width, 10));
        sketch.textInputElement.focus();
        sketch.textInputElementPosition = { x: this.x, y: this.y };
        sketch.customTextEditing = this;
      } else {
        text.visible = true;
        this.selectionPath.visible = true;
        this.selectionPoint.visible = true;
        this.rotateIconRaster.visible = true;
        this.lineToRotateIcon.visible = true;
      }
      paper.project.activeLayer.insertChild(1, this.selectionPath);
    } else {
      if (text && !this.editingText) {
        text.visible = true;
      }
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
    if (bounds.started) {
      bounds.min.x = Math.min(bounds.min.x, this.x);
      bounds.min.y = Math.min(bounds.min.y, this.y);
      bounds.max.x = Math.max(bounds.max.x, this.x);
      bounds.max.y = Math.max(bounds.max.y, this.y);
    } else {
      bounds.min = { x: this.x, y: this.y };
      bounds.max = { x: this.x, y: this.y };
      bounds.started = true;
    }
  }

  /**
   * Fills up a projected array within the given object with a center-based position of the current Custom Text
   *
   * @param {object} projection - object containing an array named 'projected'
   * @returns {void}
   *
   */
  findProjection(projection) {
    projection.projected.push({
      x: this.x - projection.center.x,
      y: this.y - projection.center.y
    });
  }

  /**
   * Relocates the current Text instance in y based on the given projection
   *
   * @param {object} projection - object containing an array named 'projected' and a number named 'index'
   * @returns {void}
   *
   */
  flipVertically(projection) {
    let index = projection.index;
    this.y = projection.center.y - projection.projected[index].y;
    index++;
    projection.index = index;
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
    if (this.texts !== undefined && this.texts.length > 0) {
      let text = this.texts[0];
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

  /**
   * Deletes the specified object.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {SketchObject} selected - Selected object
   * @param {SketchObject} parent - Object container
   * @returns {boolean} - Located the selected object
   */
  deleteSelection(sketch, selected, parent) {
    if (
      (selected.textObject && selected.textObject === this) ||
      selected.uniqueIdentifier == this.uniqueIdentifier
    ) {
      if (this.texts !== undefined) {
        for (let i = 0; i < this.texts.length; i++) {
          this.texts[i].remove();
        }
      }
      if (this.selectionPath !== undefined) {
        this.selectionPath.remove();
      }
      if (this.selectionPoint !== undefined) {
        this.selectionPoint.remove();
      }
      if (this.rotateIconRaster !== undefined) {
        this.rotateIconRaster.remove();
      }
      if (this.lineToRotateIcon !== undefined) {
        this.lineToRotateIcon.remove();
      }
      for (let i = 0; i < parent.objects.length; i++) {
        if (parent.objects[i] === this) {
          parent.objects.splice(i, 1);
          const currentLayer = sketch.getLayer(sketch.sketchLayerToEdit);
          if (currentLayer && parent.objects.length < 1) {
            for (let i = 0; i < currentLayer.objects.length; i++) {
              if (currentLayer.objects[i] == parent) {
                currentLayer.objects.splice(i, 1);
                break;
              }
            }
          }
          break;
        }
      }
      return true;
    }
    return false;
  }
}
