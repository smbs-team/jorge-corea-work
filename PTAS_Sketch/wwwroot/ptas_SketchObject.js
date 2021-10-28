// ptas_SketchObject.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Contains an array containing Paths, Distances, or Texts.
*/
class SketchObject {
  /**
    * Creates an Object with the same properties as itself
    *
    * @returns {SketchObject}
    *
    */
  createCopy() {
    let copy = new SketchObject();
    if (this.objects !== undefined) {
      copy.objects = [];
      let path = undefined;
      for (let i = 0; i < this.objects.length; i++) {
        let child = this.objects[i].createCopy();
        if (child instanceof SketchPath) {
          path = child;
        }
        copy.objects.push(child);
      }
      for (let i = 0; i < copy.objects.length; i++) {
        let child = copy.objects[i];
        if (child instanceof SketchDistance) {
          child.sketchPath = path;
        }
      }
    }
    return copy;
  }

  /**
   * Gets the parent Object of the specified object
   *
   * @param {SketchObject} item - child object
   * @returns {SketchObject}
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
        if (this.objects[i] instanceof SketchObject) {
          let parent = this.objects[i].findParent(item);
          if (parent !== undefined) {
            return parent;
          }
        }
      }
    }
  }

  /**
   * Deletes its own objects.
   *
   * @returns {void}
   *
   */
  delete() {
    if (this.objects) {
      for (let i = 0; i < this.objects.length; i++) {
        this.objects[i].delete();
      }
    }
  }

  /**
   * Enter frame event handler. Updates this on the canvas.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {Matrix} transform - Matrix for converting between canvas and offset positions
   * @param {SketchLayer} layer - Parent Layer
   * @returns {void}
   *
   */
  draw(sketch, transform, layer) {
    if (this.objects !== undefined) {
      let transformToUse = undefined;
      if (this.transform === undefined) {
        transformToUse = transform;
      } else {
        transformToUse = transform.clone();
        transformToUse.append(this.transform);
      }
      for (let i = 0; i < this.objects.length; i++) {
        this.objects[i].draw(sketch, transformToUse, layer);
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
    layer,
    candidates,
    pathLevelOnly,
    skipInsidePath
  ) {
    if (this.objects !== undefined) {
      let transformToUse = transform;
      if (this.transform !== undefined) {
        transformToUse = transformToUse.clone().append(this.transform);
      }
      for (let i = 0; i < this.objects.length; i++) {
        if (
          this.objects[i] instanceof SketchPath &&
          (!this.objects[i].arrow ||
            parent.sketchControl.mode !== SketchMode.Draw)
        ) {
          let candidate = this.objects[i].createSelectionCandidate(
            positionX,
            positionY,
            transform,
            pathLevelOnly,
            skipInsidePath
          );
          if (candidate !== undefined) {
            candidate.layer = layer;
            candidate.sketchObject = this;
            candidates.unshift(candidate);
          }
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
  findTextSelectionCandidates(
    sketch,
    positionX,
    positionY,
    transform,
    layer,
    candidates
  ) {
    if (this.objects !== undefined) {
      let transformToUse = transform;
      if (this.transform !== undefined) {
        transformToUse = transformToUse.clone().append(this.transform);
      }
      for (let i = 0; i < this.objects.length; i++) {
        if (
          this.objects[i] instanceof SketchCustomText ||
          this.objects[i] instanceof SketchDistance
        ) {
          let candidate = this.objects[i].createSelectionCandidate(
            sketch,
            positionX,
            positionY,
            transform
          );
          if (candidate !== undefined) {
            candidate.layer = layer;
            candidate.sketchObject = this;
            candidates.push(candidate);
          }
        }
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
    if (this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        if (
          this.objects[i] instanceof SketchPath && this.objects[i].start.x ||
          this.objects[i] instanceof SketchCircle
        ) {
          this.objects[i].getBounds(bounds);
        }
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
    if (this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        if (
          this.objects[i] instanceof SketchPath ||
          this.objects[i] instanceof SketchCircle ||
          this.objects[i] instanceof SketchCustomText
        ) {
          this.objects[i].findProjection(projection);
        }
      }
    }
  }

  /**
    * Relocates the current Object instance in Y based on the given projection
    *
    * @param {object} projection - object containing an array named 'projected' and a number named 'index'
    * @returns {void}
    *
    */
  flipVertically(projection) {
    if (this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        if (
          this.objects[i] instanceof SketchPath ||
          this.objects[i] instanceof SketchCircle ||
          this.objects[i] instanceof SketchCustomText
        ) {
          this.objects[i].flipVertically(projection);
        }
      }
    }
  }

  /**
   * Deletes the specified object.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {SketchObject} selected - Selected object
   * @param {SketchLayer} layer - Parent Layer
   * @returns {boolean} - Located the selected object
   */
  deleteSelection(sketch, selected, layer) {
    if (!layer) layer = selected.layer;
    if (this.objects) {
      let toDelete = undefined;
      let found = false;
      for (let i = 0; i < this.objects.length; i++) {
        toDelete = this.objects[i];
        if (
          toDelete.deleteSelection &&
          toDelete.deleteSelection(sketch, selected, this)
        ) {
          found = true;
          break;
        }
      }
      if (found) {
        if (toDelete instanceof SketchPath) {
          while (true) {
            let deleted = false;
            for (let i = 0; i < this.objects.length; i++) {
              if (
                (this.objects[i] instanceof SketchDistance &&
                  this.objects[i].sketchPath === toDelete) ||
                this.objects[i].sketchObject === this
              ) {
                this.objects[i].delete();
                this.objects.splice(i, 1);
                if (this.objects.length < 1 && layer) {
                  for (let i = 0; i < layer.objects.length; i++) {
                    if (layer.objects[i] == this) {
                      layer.objects.splice(i, 1);
                      break;
                    }
                  }
                }
                deleted = true;
                break;
              }
            }
            if (!deleted) {
              break;
            }
          }
        }
        return true;
      }
    }
    return false;
  }

  /**
   * Returns the specified Object's related Path.
   *
   * @param {SketchPath} path
   * @param {SketchObject} relatedSketchObject - Selected object
   * @returns {SketchPath}
   */
  findRelatedPath(path, relatedSketchObject) {
    if (
      this.objects && relatedSketchObject.objects
    ) {
      for (
        let i = 0;
        i < this.objects.length && i < relatedSketchObject.objects.length;
        i++
      ) {
        if (this.objects[i] === path) {
          return relatedSketchObject.objects[i];
        }
      }
    }
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
    if (this.objects !== undefined) {
      for (let i = 0; i < this.objects.length; i++) {
        if (this.objects[i] instanceof SketchPath) {
          this.objects[i].closestPointTo(position, closest, exclude);
        }
      }
    }
  }
}
