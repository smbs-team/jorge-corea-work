// ptas_SketchToJSON.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Converts to JSON in order to save.
*/
class SketchToJSON {
  /**
    * Copies a Path's properties into another Path
    *
    * @param {SketchPath} path - Source
    * @param {SketchPath} targetPath - Destination
    * @returns void
    *
    */
  static writePath(path, targetPath) {
    targetPath.uniqueIdentifier = path.uniqueIdentifier;
    targetPath.startPoint = { X: path.start.x, Y: path.start.y };
    if (path.elements !== undefined) {
      for (let i = 0; i < path.elements.length; i++) {
        const element = path.elements[i];
        let targetElement = { point: { X: element.x, Y: element.y } };
        if (element.angle !== undefined) {
          targetElement.angle = element.angle;
        }
        SketchUtils.copyLineAttributes(element, targetElement);
        if (targetPath.elements === undefined) {
          targetPath.elements = [targetElement];
        } else {
          targetPath.elements.push(targetElement);
        }
      }
    }
    targetPath.closed = path.closed;
    if (path.negative !== undefined) {
      targetPath.negative = path.negative;
    }
    targetPath.area = path.area;
    targetPath.label = path.label;
    targetPath.arrow = path.arrow;
    targetPath.layer = path.layer
  }

  /**
    * Copies a Distance's properties into another Distance
    *
    * @param {SketchDistance} distance - Source
    * @param {SketchDistance} targetDistance - Destination
    * @returns void
    *
    */
  static writeDistance(distance, targetDistance) {
    targetDistance.path = distance.sketchPath.uniqueIdentifier;
    targetDistance.element = distance.start;
    targetDistance.offset = { X: distance.offset.x, Y: distance.offset.y };
    SketchUtils.copyTextAttributes(distance, targetDistance);
  }

  static writeCustomText(customText, targetCustomText) {
    targetCustomText.uniqueIdentifier = customText.uniqueIdentifier;
    targetCustomText.customText = customText.customText;
    targetCustomText.start = { X: customText.x, Y: customText.y };
    targetCustomText.manuallyMoved = customText.manuallyMoved;
    targetCustomText.arrow = customText.arrow;
    SketchUtils.copyTextAttributes(customText, targetCustomText);
  }

  /**
    * Copies a Circle's properties into another Circle
    *
    * @param {SketchCircle} circle - Source
    * @param {SketchCircle} targetCircle - Destination
    * @returns void
    *
    */
  static writeCircle(circle, targetCircle) {
    targetCircle.center = { X: circle.center.x, Y: circle.center.y };
    targetCircle.radius = circle.radius;
    SketchUtils.copyLineAttributes(circle, targetCircle);
  }

  /**
    * Copies an Object's properties into another Object
    *
    * @param {sketchObject} sketchObject - Source
    * @param {sketchObject} targetSketchObject - Destination
    * @returns void
    *
    */
  static writeSketchObject(sketchObject, targetSketchObject) {
    if (sketchObject.objects !== undefined) {
      for (let i = 0; i < sketchObject.objects.length; i++) {
        if (sketchObject.objects[i] instanceof SketchPath) {
          const path = sketchObject.objects[i];
          let targetPath = {};
          this.writePath(path, targetPath);
          if (targetSketchObject.pathList === undefined) {
            targetSketchObject.pathList = [targetPath];
          } else {
            targetSketchObject.pathList.push(targetPath);
          }
        }
        if (sketchObject.objects[i] instanceof SketchDistance) {
          const distance = sketchObject.objects[i];
          let targetDistance = {};
          this.writeDistance(distance, targetDistance);
          if (targetSketchObject.distanceList === undefined) {
            targetSketchObject.distanceList = [targetDistance];
          } else {
            targetSketchObject.distanceList.push(targetDistance);
          }
        }
        if (sketchObject.objects[i] instanceof SketchCustomText) {
          const customText = sketchObject.objects[i];
          let targetCustomText = {};
          this.writeCustomText(customText, targetCustomText);
          if (targetSketchObject.customTextList === undefined) {
            targetSketchObject.customTextList = [targetCustomText];
          } else {
            targetSketchObject.customTextList.push(targetCustomText);
          }
        }
        if (sketchObject.objects[i] instanceof SketchCircle) {
          const circle = sketchObject.objects[i];
          let targetCircle = {};
          this.writeCircle(circle, targetCircle);
          if (targetSketchObject.circleList === undefined) {
            targetSketchObject.circleList = [targetCircle];
          } else {
            targetSketchObject.circleList.push(targetCircle);
          }
        }
      }
    }
  }

  /**
    * Copies a Layer's properties into another Layer
    *
    * @param {SketchLayer} layer - Source
    * @param {SketchLayer} targetLayer - Destination
    * @returns void
    *
    */
  static writeLayer(layer, targetLayer) {
    targetLayer.uniqueIdentifier = layer.uniqueIdentifier;
    targetLayer.name = layer.name;
    targetLayer.visible = true;
    if (layer.objects !== undefined) {
      for (let i = 0; i < layer.objects.length; i++) {
        const sketchObject = layer.objects[i];
        let targetSketchObject = {};
        this.writeSketchObject(sketchObject, targetSketchObject);
        if (targetLayer.objects === undefined) {
          targetLayer.objects = [targetSketchObject];
        } else {
          targetLayer.objects.push(targetSketchObject);
        }
      }
    }
    targetLayer.netArea = layer.netArea;
    targetLayer.grossArea = layer.grossArea;
    if (layer.customTextForLabel !== undefined) {
      targetLayer.customTextForLabel = layer.customTextForLabel;
    }
    if (layer.labelMoved !== undefined) {
      targetLayer.labelMoved = layer.labelMoved;
    }
  }

  /**
    * Copies the current sketch into a string.
    *
    * @param {SketchControl} control - Sketch instance
    * @returns {string}
    *
    */
  static write(control) {
    let sketch = {};
    if (control.layers) {
      for (let i = 0; i < control.layers.length; i++) {
        const layer = control.layers[i];
        let targetLayer = {};
        this.writeLayer(layer, targetLayer);
        if (sketch.layers === undefined) {
          sketch.layers = [targetLayer];
        } else {
          sketch.layers.push(targetLayer);
        }
      }
    }
    if (control.levels) {
      for (let i = 0; i < control.levels.length; i++) {
        const level = control.levels[i];
        if (level.isScratchpad) {
          continue;
        }
        let targetLevel = {
          uniqueIdentifier: level.uniqueIdentifier,
          name: level.name,
          visible: true
        };
        if (sketch.levels === undefined) {
          sketch.levels = [targetLevel];
        } else {
          sketch.levels.push(targetLevel);
        }
        if (level.layers !== undefined) {
          for (let j = 0; j < level.layers.length; j++) {
            const layer = level.layers[j];
            const targetLayer = {
              uniqueIdentifier: layer.uniqueIdentifier
            };
            if (targetLevel.layers === undefined) {
              targetLevel.layers = [layer];
            } else {
              targetLevel.layers.push(layer);
            }
          }
        }
      }
    }
    return JSON.stringify(sketch);
  }
}
