// ptas_SketchLayer.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
 * Contains an array of objects containing Paths (polygons) and Texts.
 */
class SketchLayer {
  /**
   * Creates a Layer with the same properties as itself
   *
   * @returns Layer
   *
   */
  createCopy() {
    let copy = new SketchLayer();
    if (this.name) {
      copy.name = this.name;
    }
    if (this.uniqueIdentifier) {
      copy.uniqueIdentifier = this.uniqueIdentifier;
    }
    if (this.objects) {
      copy.objects = [];
      for (let i = 0; i < this.objects.length; i++) {
        let child = this.objects[i].createCopy();
        copy.objects.push(child);
      }
    }
    if (this.visible) {
      copy.visible = this.visible;
    }
    return copy;
  }

  /**
   * Gets the parent layer of the specified object
   *
   * @param {SketchObject} item - child object
   * @returns {SketchLayer}
   *
   */
  findParent(item) {
    if (this.objects !== undefined && this.objects.length > 0) {
      for (let i = 0; i < this.objects.length; i++) {
        if (this.objects[i] === item) {
          return { parent: this, index: i };
        }
      }
      for (let i = 0; i < this.objects.length; i++) {
        let parent = this.objects[i].findParent(item);
        if (parent) {
          return parent;
        }
      }
    }
  }

  /**
   * Enter frame event handler. Updates the Layer on the canvas.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Matrix} transform - Matrix for converting between canvas and offset positions
   * @returns {void}
   */
  draw(sketch, transform) {
    if (this.visible && this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        this.objects[i].draw(sketch, transform, this.uniqueIdentifier);
      }
    }
  }

  /**
   * Gets the Path candidates that converge with the cursor position.
   *
   * @param {number} positionX
   * @param {number} positionY
   * @param {Matrix} transform - Matrix for converting between canvas and offset positions
   * @param {Array} candidates - Converging Paths found
   * @param {boolean} pathLevelOnly
   * @param {boolean} skipInsidePath
   * @returns {void}
   *
   */
  findPathSelectionCandidates(
    positionX,
    positionY,
    transform,
    candidates,
    pathLevelOnly,
    skipInsidePath
  ) {
    if (this.visible && this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        this.objects[i].findPathSelectionCandidates(
          positionX,
          positionY,
          transform,
          this,
          candidates,
          pathLevelOnly,
          skipInsidePath
        );
      }
    }
  }

  /**
   * Gets the Path candidates that converge with the cursor position.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {number} positionX - Number stating the horizontal position
   * @param {number} positionY - Number stating the vertical position
   * @param {Matrix} transform - Matrix for converting between canvas and offset positions
   * @param {Array} candidates - array of converging Paths
   * @returns {void}
   *
   */
  findTextSelectionCandidates(
    sketch,
    positionX,
    positionY,
    transform,
    candidates
  ) {
    if (this.visible && this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        this.objects[i].findTextSelectionCandidates(
          sketch,
          positionX,
          positionY,
          transform,
          this,
          candidates
        );
      }
    }
  }

  /**
   * Fills up an object with the bounding box positions of a selection
   *
   * @param bounds - object containing the bounding positions
   * @returns void
   *
   */
  getBounds(bounds) {
    if (this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        this.objects[i].getBounds(bounds);
      }
    }
  }

  /**
   * Fills up a projected array within the given object with a center-based position of the current Layer
   *
   * @param {object} projection - object containing an array named 'projected'
   * @returns {void}
   *
   */
  findProjection(projection) {
    if (this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        this.objects[i].findProjection(projection);
      }
    }
  }

  /**
   * Relocates the current Layer instance in y based on the given projection
   *
   * @param {object} projection - object containing an array named 'projected' and a number named 'index'
   * @returns {void}
   *
   */
  flipVertically(projection) {
    if (this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        this.objects[i].flipVertically(projection);
      }
    }
  }

  /**
   * Deletes the selected Paths and Texts
   *
   * @param sketch - Control instance
   * @param selected - selected objects
   * @param layer - parent Layer of the selected objects
   * @returns void
   *
   */
  deleteSelection(sketch, selected, layer) {
    if (this.objects) {
      for (let i = 0; i < this.objects.length; i++) {
        if (this.objects[i].deleteSelection(sketch, selected, layer)) {
          break;
        }
      }
    }
  }

  /**
   * Gets the specified object within a Layer
   *
   * @param sketchObject - Object to find
   * @param relatedLayer - Layer to read
   * @returns found object
   *
   */
  findRelatedSketchObject(sketchObject, relatedLayer) {
    if (this.objects && relatedLayer.objects) {
      for (
        let i = 0;
        i < this.objects.length && i < relatedLayer.objects.length;
        i++
      ) {
        if (this.objects[i] === sketchObject) {
          return relatedLayer.objects[i];
        }
      }
    }
  }

  /**
   * Finds the closest point to an object
   *
   * @param position - object containing the point position in x and y
   * @param closest - object keeping the closest position found
   * @param exclude - point to exclude
   * @returns void
   *
   */
  closestPointTo(position, closest, exclude) {
    if (this.visible && this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        this.objects[i].closestPointTo(position, closest, exclude);
      }
    }
  }

  /**
   * Creates a label within the current Layer
   *
   * @param path - Path parent of the new label
   * @returns void
   *
   */
  addLabel(path) {
    if (this.objects) {
      const object = path ?? this.objects[this.objects.length - 1].objects[0];
      if (object && object.closed) {
        let customText = new SketchCustomText();
        customText.uniqueIdentifier = SketchControl.uuidv4();
        let sketchObject = new SketchObject();
        let projection = { center: {}, projected: [], index: 0 };
        let bounds = {};
        let layers = parent.sketchControl.layers;
        customText.customText = parent.sketchControl.drawNegativeArea || object.negative
          ? "Open"
          : object.layer;
        object.getBounds(bounds);
        projection.bounds = bounds;
        projection.center.x = (bounds.min.x + bounds.max.x) / 2;
        projection.center.y = (bounds.min.y + bounds.max.y) / 2;
        this.findProjection(projection);
        for (let i = 0; i < layers.length; i++) {
          if (layers[i].uniqueIdentifier !== this.uniqueIdentifier) {
            if (
              layers[i].objects &&
              layers[i].objects.length > 0 &&
              layers[i].objects[0].objects
            ) {
              layers[i].objects.map(object => {
                if (
                  object.objects &&
                  object.objects[0] instanceof SketchCustomText &&
                  object.objects[0].x > projection.center.x - 13 &&
                  object.objects[0].x < projection.center.x + 13 &&
                  object.objects[0].y > projection.center.y - 3 &&
                  object.objects[0].y < projection.center.y + 3
                ) {
                  projection.center.y += 3;
                }
              });
            }
          }
        }
        object.label = customText.uniqueIdentifier;
        sketchObject.objects = [customText];
        this.objects.push(sketchObject);
        customText.x = projection.center.x;
        customText.y = projection.center.y;
        customText.rotation = 0;
        customText.horizontalAlign = SketchTextHorizontalAlign.Center;
        customText.isLabel = true;
        customText.path = path;
        this.customTextForLabel = customText.uniqueIdentifier;
      }
    }
  }

  /**
   * Removes a label from the current Layer
   *
   * @param uniqueIdentifier - ID of the label to remove
   * @returns void
   *
   */
  removeLabel(uniqueIdentifier) {
    for (let i = 0; i < this.objects.length; i++) {
      if (
        this.objects[i].objects &&
        this.objects[i].objects.length > 0 &&
        this.objects[i].objects[0].uniqueIdentifier == uniqueIdentifier
      ) {
        this.objects.splice(i, 1);
        break;
      }
    }
  }
}
