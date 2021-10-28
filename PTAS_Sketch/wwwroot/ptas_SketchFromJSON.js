// ptas_SketchFromJSON.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Loads a sketch as a JSON and converts it to an object.
*/
class SketchFromJSON {
  /**
    * Copies the contents of a Path into another one
    *
    * @param sourcePath - Original Path
    * @param path - Copy destination
    * @returns void
    *
    */
  static readPath(sourcePath, path) {
    path.uniqueIdentifier = sourcePath.uniqueIdentifier;
    path.start = { x: sourcePath.startPoint.X, y: sourcePath.startPoint.Y };
    if (sourcePath.elements) {
      for (let i = 0; i < sourcePath.elements.length; i++) {
        const sourceElement = sourcePath.elements[i];
        let element = { x: sourceElement.point.X, y: sourceElement.point.Y };
        if (sourceElement.angle) {
          element.angle = sourceElement.angle;
        }
        SketchUtils.copyLineAttributes(sourceElement, element);
        if (path.elements === undefined) {
          path.elements = [element];
        } else {
          path.elements.push(element);
        }
      }
    }
    path.closed = sourcePath.closed;
    if (sourcePath.negative) {
      path.negative = sourcePath.negative;
    }
    path.area = sourcePath.area;
    path.label = sourcePath.label;
    path.arrow = sourcePath.arrow;
    path.layer = sourcePath.layer
  }

  /**
    * Copies the contents of a Distance into another one
    *
    * @param sketchObject - Object containing the source Distance
    * @param sourceDistance - Original distance
    * @returns SketchDistance
    *
    */
  static readDistance(sketchObject, sourceDistance) {
    for (let i = 0; i < sketchObject.objects.length; i++) {
      const path = sketchObject.objects[i];
      if (path.uniqueIdentifier === sourceDistance.path) {
        let distance = new SketchDistance(
          path,
          sourceDistance.element,
          sourceDistance.element + 1
        );
        distance.offset = {
          x: sourceDistance.offset.X,
          y: sourceDistance.offset.Y
        };
        SketchUtils.copyTextAttributes(sourceDistance, distance);
        return distance;
      }
    }
  }

  /**
    * Copies the contents of a Text into another one
    *
    * @param sourceCustomText - Original Text
    * @param customText - Copy destination
    * @returns void
    *
    */
  static readCustomText(sourceCustomText, customText) {
    customText.uniqueIdentifier = sourceCustomText.uniqueIdentifier;
    customText.customText = sourceCustomText.customText;
    customText.x = sourceCustomText.start.X;
    customText.y = sourceCustomText.start.Y;
    customText.manuallyMoved = sourceCustomText.manuallyMoved;
    customText.arrow = sourceCustomText.arrow;
    SketchUtils.copyTextAttributes(sourceCustomText, customText);
  }

  /**
    * Copies the contents of a Circle into another one
    *
    * @param sourceCircle - Original Circle
    * @param circle - Copy destination
    * @returns void
    *
    */
  static readCircle(sourceCircle, circle) {
    circle.center = { x: sourceCircle.center.X, y: sourceCircle.center.Y };
    circle.radius = sourceCircle.radius;
    SketchUtils.copyLineAttributes(sourceCircle, circle);
  }

  /**
    * Copies the contents of an Object into another one
    *
    * @param sourceSketchObject - Original Object
    * @param sketchObject - Copy destination
    * @returns void
    *
    */
  static readSketchObject(sourceSketchObject, sketchObject) {
    if (!sketchObject.objects)
      sketchObject.objects = [];
    if (sourceSketchObject.pathList) {
      for (let i = 0; i < sourceSketchObject.pathList.length; i++) {
        const sourcePath = sourceSketchObject.pathList[i];
        if (sourcePath.elements) {
          const path = new SketchPath();
          this.readPath(sourcePath, path);
          sketchObject.objects.push(path);
        }
      }
    }
    if (sourceSketchObject.distanceList) {
      for (let i = 0; i < sourceSketchObject.distanceList.length; i++) {
        const sourceDistance = sourceSketchObject.distanceList[i];
        let distance = this.readDistance(sketchObject, sourceDistance);
        sketchObject.objects.push(distance);
      }
    }
    if (sourceSketchObject.customTextList) {
      for (let i = 0; i < sourceSketchObject.customTextList.length; i++) {
        const sourceCustomText = sourceSketchObject.customTextList[i];
        let customText = new SketchCustomText();
        customText.rotation = 0;
        this.readCustomText(sourceCustomText, customText);
        sketchObject.objects.push(customText);
      }
    }
    if (sourceSketchObject.circleList) {
      for (let i = 0; i < sourceSketchObject.circleList.length; i++) {
        const sourceCircle = sourceSketchObject.circleList[i];
        let circle = new SketchCircle();
        this.readCircle(sourceCircle, circle);
        sketchObject.objects.push(circle);
      }
    }
  }

  /**
    * Copies the contents of a Layer into another one
    *
    * @param sourceLayer - Original Layer
    * @param layer - Copy destination
    * @returns void
    *
    */
  static readLayer(sourceLayer, layer) {
    layer.uniqueIdentifier = sourceLayer.uniqueIdentifier;
    layer.name = sourceLayer.name;
    layer.visible = sourceLayer.visible;
    if (sourceLayer.objects) {
      for (let i = 0; i < sourceLayer.objects.length; i++) {
        const sourceSketchObject = sourceLayer.objects[i];
        let sketchObject = new SketchObject();
        this.readSketchObject(sourceSketchObject, sketchObject);
        if (sketchObject.objects.length > 0) {
          if (!layer.objects) {
            layer.objects = [sketchObject];
          } else {
            layer.objects.push(sketchObject);
          }
        }
      }
    }
    layer.netArea = sourceLayer.netArea;
    layer.grossArea = sourceLayer.grossArea;
    if (sourceLayer.customTextForLabel) {
      layer.customTextForLabel = sourceLayer.customTextForLabel;
    }
    if (sourceLayer.labelMoved) {
      layer.labelMoved = sourceLayer.labelMoved;
    }
  }

  /**
    * Creates an object containing living and gross areas of each layer
    *
    * @param {SketchControl} source
    * @returns {object}
    *
    */
  static readAreas(source) {
    const layerAreas = {}; //'layerName': { livingArea, grossArea }
    let sketch = source;
    if (typeof source === "string") {
      sketch = JSON.parse(source);
      if (typeof sketch === "string") {
        sketch = JSON.parse(sketch);
      }
    }
    if (sketch.layers) {
      for (let i = 0; i < sketch.layers.length; i++) {
        layerAreas[sketch.layers[i].name] = {
          livingArea: sketch.layers[i].netArea,
          grossArea: sketch.layers[i].grossArea
        };
      }
    }
    return layerAreas;
  }

  /**
    * Loads the sketch contents, leaving out duplicate layers.
    *
    * @param source - Source Control instance
    * @param control - Target Control instance
    * @returns void
    *
    */
  static read(source, control) {
    let sketch = source;
    if (typeof source === "string") {
      sketch = JSON.parse(source);
      if (typeof sketch === "string") {
        sketch = JSON.parse(sketch);
      }
    }
    if (sketch.layers) {
      let scratchpad = false;
      for (let i = 0; i < sketch.layers.length; i++) {
        const sourceLayer = sketch.layers[i];
        if (sourceLayer.name !== "Scratchpad" || !scratchpad) {
          let layer = new SketchLayer();
          this.readLayer(sourceLayer, layer);

          if (layer.objects) {
            let layerObjects = [];
            layer.objects.map(object => {
              if (Object.keys(object).length > 0) layerObjects.push(object);
            });
            layer.objects = layerObjects;
          }
          if (!control.autoFit)
            control.autoFitClicked();
          if (!control.layers) {
            control.layers = [];
          }
          control.layers.push(layer);
        }
        if (sourceLayer.name == "Scratchpad") {
          scratchpad = true;
        }
      }
    }
    if (sketch.levels) {
      for (let i = 0; i < sketch.levels.length; i++) {
        const sourceLevel = sketch.levels[i];
        let level = {
          uniqueIdentifier: sourceLevel.uniqueIdentifier,
          name: sourceLevel.name,
          visible: sourceLevel.visible
        };
        if (control.levels === undefined) {
          control.levels = [level];
        } else {
          control.levels.push(level);
        }
        if (sourceLevel.layers) {
          for (let j = 0; j < sourceLevel.layers.length; j++) {
            const sourceLayer = sourceLevel.layers[j];
            const layer = {
              uniqueIdentifier: sourceLayer.uniqueIdentifier
            };
            if (level.layers === undefined) {
              level.layers = [layer];
            } else {
              level.layers.push(layer);
            }
          }
        } else {
          level.layers = [];
        }
      }
    }
  }
}
