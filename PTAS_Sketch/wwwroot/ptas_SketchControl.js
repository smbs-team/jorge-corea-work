// ptas_SketchControl.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
 * Control is the core of the application.
 */
class SketchControl {
  constructor(root, canvas, gridCanvas) {
    this.root = root;
    this.canvas = canvas;
    if (gridCanvas !== undefined) {
      this.gridCanvas = gridCanvas;
    }
    this.textInputElementTextChangedName = "";
    this.drawCounter = 0;
    this.gridVisible = true;
    this.majorGridlineScaleFactor = 10;
    this.offsetX = 0;
    this.offsetY = 0;
    this.resolution = 10;
    this.bearing = 0;
    this.gridThreshold = 5;
    this.snapToGrid = true;
    this.snapToLine = true;
    this.styleSet = new SketchStyleSet();
    this.styleSet.setAsDefault();
    this.updateTopLevelTransform();
    this.mainScope = new paper.PaperScope();
    this.mainScope.setup(canvas);
    this.minimumDistance = 0.5;
    this.newSketchDistanceTemplate = new SketchDistance(
      undefined,
      undefined,
      undefined
    );
    this.xDistance = 0;
    this.yDistance = 0;
    this.autoFit = false;
    this.isRotating = false;
    this.drawNegativeArea = false;
    this.layerList = {}; //layerName: { name, livingArea, grossArea, category, index, entityName, entityProperty, style }
    this.savedLayers = null;
    this.totalsInfo = null;
    this.sketchEntity = null;
    this.sketchEntityId = null;
    this.relatedEntity = null;
    this.relatedEntityId = null;
    this.relatedEntityName = null;
    this.sketchAccessToken = null;
    this.sketchIsOfficial = false;
    this.selectedSketchVersion = null;
    this.sketchDraftCreated = false;
    this.parcelData = null;
    this.userData = null;
    this.draftData = null;
    this.versionsContainerId = null;
    this.customTextEditing = null;
    this.urlParams = new URLSearchParams(window.location.search);
    this.token = this.urlParams.get("token");
    this.movingPenCursor = "moving";
    this.panning = false;
    this.firstLoad = true;
    this.lastAction = "pan";
    this.selectedElements = 0;
    this.selectedAreas = 0;
    if (this.urlParams.get("readonly")) {
      document.getElementById("edit-mode").style.visibility = "collapse";
      document.getElementById("read-only").style.visibility = "visible";
      document.getElementById("sketchCanvas").style.cursor = "grab";
      let toolbars = document.getElementsByName("editToolbar");
      for (let i = 0; i < toolbars.length; i++) {
        toolbars[i].style.display = "none";
      }
      this.mode = SketchMode.Pan;
    } else {
      document.getElementById("edit-mode").style.visibility = "visible";
      document.getElementById("read-only").style.visibility = "collapse";
      document.getElementById("file-info").style.visibility = "collapse";
      this.mode = SketchMode.Pan;
      document.getElementById("sketchCanvas").style.cursor = "grab";
    }
    this.viewMode = "stack";
    if (this.urlParams.get("relatedEntity").indexOf("building") > 0) {
      this.categories = ["Building", "Accessories"];
    } else if (this.urlParams.get("relatedEntity").indexOf("accessory") > 0) {
      this.categories = ["Accessories"];
    } else if (this.urlParams.get("relatedEntity").indexOf("mobile") > 0) {
      this.categories = ["Mobile home", "Accessories"];
    } else if (this.urlParams.get("relatedEntity").indexOf("commercial") > 0) {
      this.categories = ["Commercial"];
    }

    let xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function() {
      if (this.readyState == 4 && this.status == 200) {
        let layerMenu = JSON.parse(this.responseText);
        let menuColumns = document.getElementById("menu-columns");
        parent.sketchControl.entityCategories = [];
        layerMenu.categories.map(menu => {
          if (
            parent.sketchControl.categories.find(cat => cat == menu.category)
          ) {
            parent.sketchControl.entityCategories.push(menu);
            if (
              (menu.category == "Building",
              !menu.residentialView &&
                !parent.sketchControl.categories.find(
                  cat => cat == "Commercial"
                ))
            ) {
              parent.sketchControl.commercial = true;
              parent.sketchControl.categories.push("Commercial");
            }
            menuColumns.innerHTML += menu.category + "<ul></ul>";
            menu.layers.map(layer => {
              menuColumns.lastChild.innerHTML +=
                "<li onclick=\"addLayerClicked('" +
                layer.name +
                "')\">" +
                layer.name +
                "</li>";
              parent.sketchControl.layerList[layer.name] = {
                name: layer.name,
                livingArea: layer.livingArea,
                grossArea: layer.grossArea,
                index: +layer.index,
                category: menu.category,
                entityName: menu.entityName,
                entityProperty: layer.entityProperty,
                style: layer.style,
                residentialView: menu.residentialView,
                residentialFilterProperty: menu.residentialFilterProperty,
                residentialFilterValue: menu.residentialFilterValue
              };
            });
          }
        });
      }
    };
    xmlhttp.open("GET", "ptas_layerMenu.json", true);
    xmlhttp.send();
  }

  get mode() {
    return this._mode;
  }

  set mode(value) {
    this._mode = value;
    this.clearSelection();
    this.dismissTextInput();
    if (this._mode == SketchMode.SingleSelect) {
      this.interaction = new SketchSingleSelectInteraction();
    } else if (this._mode == SketchMode.MultipleSelect) {
      this.interaction = new SketchMultipleSelectInteraction();
    } else if (this._mode == SketchMode.Draw) {
      this.interaction = new SketchDrawInteraction();
    } else if (this._mode == SketchMode.Text) {
      this.interaction = new SketchTextInteraction();
    } else if (this._mode == SketchMode.Pan) {
      this.interaction = new SketchPanInteraction();
    } else {
      this.interaction = undefined;
    }
  }

  get gridVisible() {
    return this._gridVisible;
  }

  set gridVisible(value) {
    this._gridVisible = value;
    this.redrawGrid = true;
  }

  get drawNegativeArea() {
    return this._drawNegativeArea;
  }

  set drawNegativeArea(value) {
    this._drawNegativeArea = value;
  }

  get resolution() {
    if (!this._resolution) {
      this._resolution = 10;
    }
    return this._resolution;
  }

  set resolution(value) {
    this._resolution = value;
    parent.updateZoomLabel(value);
  }

  /**
   * Clears all selections and references.
   *
   * @returns {void}
   *
   */
  clear() {
    this.levels = undefined;
    this.layers = undefined;
    this.sketchLayerToEdit = undefined;
    this.sketchObjectToEdit = undefined;
    this.sketchPathToEdit = undefined;
    this.editing = undefined;
    this.sketchPathToEditPoint = undefined;
    this.cursor = undefined;
    this.penUpCursor = undefined;
    this.clearSelection();
    this.redoHistory = undefined;
    adjustUndoRedoVisibility();
  }

  /**
   * Transforms the matrix positions from JS to PaperJS.
   *
   * @returns {void}
   *
   */
  updateTopLevelTransform() {
    this.offsetChangedForDraw =
      this.offsetChangedForDraw ||
      this.offsetX !== this.previousOffsetXForTransform ||
      this.offsetY !== this.previousOffsetYForTransform;
    this.resolutionChangedForDraw =
      this.resolutionChangedForDraw ||
      this.resolution !== this.previousResolutionForTransform ||
      this.invertedXAxis !== this.previousInvertedXAxisForTransform ||
      this.invertedYAxis !== this.previousInvertedYAxisForTransform;
    this.bearingChangedForDraw =
      this.bearingChangedForDraw ||
      this.bearing !== this.previousBearingForTransform;
    this.topLevelTransform = new paper.Matrix();
    this.topLevelTransform.translate(this.offsetX, this.offsetY);
    let horizontal = this.resolution;
    if (this.invertedXAxis) {
      horizontal = -horizontal;
    }
    let vertical = this.resolution;
    if (this.invertedYAxis) {
      vertical = -vertical;
    }
    this.topLevelTransform.scale(horizontal, vertical);
    this.topLevelTransform.rotate(this.bearing);
    this.inverseTopLevelTransform = this.topLevelTransform.clone().invert();
    this.redrawGrid = true;
    this.previousOffsetXForTransform = this.offsetX;
    this.previousOffsetYForTransform = this.offsetY;
    this.previousResolutionForTransform = this.resolution;
    this.previousInvertedXAxisForTransform = this.invertedXAxis;
    this.previousInvertedYAxisForTransform = this.invertedYAxis;
    this.previousBearingForTransform = this.bearing;
  }

  /**
   * Places the specified object on the canvas.
   *
   * @param {object} item
   * @returns {void}
   *
   */
  relocateInMainScopeActiveLayer(item) {
    if (
      this.indexInMainScopeActiveLayer >=
      paper.project.activeLayer.children.length
    ) {
      item.remove();
      paper.project.activeLayer.addChild(item);
      this.indexInMainScopeActiveLayer =
        paper.project.activeLayer.children.length;
    } else {
      if (
        paper.project.activeLayer.children[this.indexInMainScopeActiveLayer] !==
        item
      ) {
        item.remove();
        paper.project.activeLayer.insertChild(
          this.indexInMainScopeActiveLayer,
          item
        );
      }
      this.indexInMainScopeActiveLayer++;
    }
  }

  /**
   * Displays the prediction points in Draw mode.
   *
   * @param {number} x
   * @param {number} y
   * @returns {void}
   *
   */
  addJumpToIndicatorPoints(x, y) {
    const position = this.topLevelTransform.transform(new paper.Point(x, y));
    let point = undefined;
    if (this.jumpToIndicatorPoints === undefined) {
      point = new paper.Path.Circle(position, 1);
      this.jumpToIndicatorPoints = [point];
      this.jumpToIndicatorPointCount = 1;
    } else if (
      this.jumpToIndicatorPointCount >= this.jumpToIndicatorPoints.length
    ) {
      point = new paper.Path.Circle(position, 1);
      this.jumpToIndicatorPoints.push(point);
      this.jumpToIndicatorPointCount = this.jumpToIndicatorPoints.length;
    } else {
      point = this.jumpToIndicatorPoints[this.jumpToIndicatorPointCount];
      point.position = position;
      this.jumpToIndicatorPointCount++;
    }
    this.relocateInMainScopeActiveLayer(point);
    this.styleSet.jumpToIndicatorStyle.applyPointStyleTo(point);
    if (this.jumpToCoordinates === undefined) {
      this.jumpToCoordinates = [{ x: x, y: y }];
    } else {
      this.jumpToCoordinates.push({ x: x, y: y });
    }
  }

  /**
   * Enter frame event handler. Redraws the canvas.
   *
   * @returns {void}
   *
   */
  draw() {
    if (this.autoFit) {
      let autoProjection = { center: {}, projected: [], index: 0 };
      this.findProjection(autoProjection);
      if (autoProjection.bounds && autoProjection.projected.length > 0) {
        this.zoomToContents(autoProjection.bounds, 0.7);
      }
    }
    this.drawCounter++;
    let setToMainScope = false;
    if (
      this.gridVisible &&
      (this.gridCanvas === undefined || this.redrawGrid)
    ) {
      let isUniformStyle = false;
      if (
        this.styleSet.minorGridlineStyle !== undefined &&
        this.styleSet.majorGridlineStyle !== undefined &&
        this.styleSet.gridlineAxisStyle !== undefined
      ) {
        let color = this.styleSet.minorGridlineStyle.color;
        let width = this.styleSet.minorGridlineStyle.width;
        let pattern = this.styleSet.minorGridlineStyle.pattern;
        if (
          color === this.styleSet.majorGridlineStyle.color &&
          width === this.styleSet.majorGridlineStyle.width &&
          SketchUtils.patternsMatch(
            pattern,
            this.styleSet.majorGridlineStyle.pattern
          ) &&
          color === this.styleSet.gridlineAxisStyle.color &&
          width === this.styleSet.gridlineAxisStyle.width &&
          SketchUtils.patternsMatch(
            pattern,
            this.styleSet.gridlineAxisStyle.pattern
          )
        ) {
          isUniformStyle = true;
        }
      }
      if (this.gridCanvas !== undefined) {
        let devicePixelRatio = 1;
        if (parent.devicePixelRatio !== undefined) {
          devicePixelRatio = parent.devicePixelRatio;
        }
        let context = this.gridCanvas.getContext("2d");
        this.gridCanvas.width = this.root.clientWidth * devicePixelRatio;
        this.gridCanvas.height = this.root.clientHeight * devicePixelRatio;
        context.clearRect(0, 0, this.gridCanvas.width, this.gridCanvas.height);
        if (
          isUniformStyle &&
          !this.redrawGrid &&
          !this.resolutionChangedForDraw &&
          !this.bearingChangedForDraw &&
          this.fullGridCapture !== undefined
        ) {
          let size = this.resolution;
          if (this.resolution < this.gridThreshold) {
            size *= this.majorGridlineScaleFactor;
          }
          let offsetX = (this.offsetX - this.startingOffsetXForCapture) % size;
          let offsetY = (this.offsetY - this.startingOffsetYForCapture) % size;
          size *= devicePixelRatio;
          offsetX *= devicePixelRatio;
          offsetY *= devicePixelRatio;
          context.putImageData(this.fullGridCapture, offsetX, offsetY);
          context.putImageData(this.leftGridCapture, offsetX - size, offsetY);
          context.putImageData(this.topGridCapture, offsetX, offsetY - size);
          context.putImageData(
            this.topLeftGridCapture,
            offsetX - size,
            offsetY - size
          );
        } else {
          let transform = new paper.Matrix();
          transform.scale(devicePixelRatio, devicePixelRatio);
          transform.append(this.topLevelTransform);
          let inverseTransform = new paper.Matrix();
          inverseTransform.append(this.inverseTopLevelTransform);
          inverseTransform.scale(1 / devicePixelRatio, 1 / devicePixelRatio);
          let topLeft = inverseTransform.transform(new paper.Point(0, 0));
          let topRight = inverseTransform.transform(
            new paper.Point(this.gridCanvas.width, 0)
          );
          let bottomRight = inverseTransform.transform(
            new paper.Point(this.gridCanvas.width, this.gridCanvas.height)
          );
          let bottomLeft = inverseTransform.transform(
            new paper.Point(0, this.gridCanvas.height)
          );
          let left = Math.min(
            Math.min(Math.min(topLeft.x, topRight.x), bottomRight.x),
            bottomLeft.x
          );
          let top = Math.min(
            Math.min(Math.min(topLeft.y, topRight.y), bottomRight.y),
            bottomLeft.y
          );
          let right = Math.max(
            Math.max(Math.max(topLeft.x, topRight.x), bottomRight.x),
            bottomLeft.x
          );
          let bottom = Math.max(
            Math.max(Math.max(topLeft.y, topRight.y), bottomRight.y),
            bottomLeft.y
          );
          let zeroXValid = left * right <= 0;
          let zeroYValid = top * bottom <= 0;
          let uniformGridBlockSize = undefined;
          if (isUniformStyle && this.bearing === 0) {
            let size = this.resolution;
            if (size < this.gridThreshold) {
              size *= this.majorGridlineScaleFactor;
            }
            if (size >= this.gridThreshold) {
              size *= devicePixelRatio;
              if (
                size < this.gridCanvas.width / 2 &&
                size < this.gridCanvas.height / 2
              ) {
                uniformGridBlockSize = size;
              }
            }
          }
          context.beginPath();
          if (left < right && top < bottom) {
            let minorGridSize = this.resolution;
            let majorGridSize = minorGridSize * this.majorGridlineScaleFactor;
            if (minorGridSize >= this.gridThreshold) {
              let sourceCoords = [];
              let gridLineX = Math.floor(left);
              while (gridLineX < right) {
                if (
                  gridLineX % this.majorGridlineScaleFactor !== 0 ||
                  majorGridSize < this.gridThreshold
                ) {
                  sourceCoords.push(gridLineX, top, gridLineX, bottom);
                }
                gridLineX++;
              }
              let gridLineY = Math.floor(top);
              while (gridLineY < bottom) {
                if (
                  gridLineY % this.majorGridlineScaleFactor !== 0 ||
                  majorGridSize < this.gridThreshold
                ) {
                  sourceCoords.push(left, gridLineY, right, gridLineY);
                }
                gridLineY++;
              }
              let coords = [];
              transform.transform(
                sourceCoords,
                coords,
                sourceCoords.length / 2
              );
              context.strokeStyle = this.styleSet.minorGridlineStyle.color;
              context.lineWidth = this.styleSet.minorGridlineStyle.width;
              let pattern = undefined;
              if (this.styleSet.minorGridlineStyle.pattern !== undefined) {
                pattern = [];
                for (
                  let i = 0;
                  i < this.styleSet.minorGridlineStyle.pattern.length;
                  i++
                ) {
                  pattern.push(
                    this.styleSet.minorGridlineStyle.pattern[i] *
                      devicePixelRatio
                  );
                }
              }
              context.setLineDash(pattern);
              for (let i = 0; i < coords.length; i += 4) {
                context.moveTo(coords[i], coords[i + 1]);
                context.lineTo(coords[i + 2], coords[i + 3]);
              }
            }
            if (majorGridSize >= this.gridThreshold) {
              let sourceCoords = [];
              let gridLineX = Math.floor(left);
              while (gridLineX % this.majorGridlineScaleFactor !== 0) {
                gridLineX++;
              }
              while (gridLineX < right) {
                if (gridLineX !== 0) {
                  sourceCoords.push(gridLineX, top, gridLineX, bottom);
                }
                gridLineX += this.majorGridlineScaleFactor;
              }
              let gridLineY = Math.floor(top);
              while (gridLineY % this.majorGridlineScaleFactor !== 0) {
                gridLineY++;
              }
              while (gridLineY < bottom) {
                if (gridLineY !== 0) {
                  sourceCoords.push(left, gridLineY, right, gridLineY);
                }
                gridLineY += this.majorGridlineScaleFactor;
              }
              let coords = [];
              transform.transform(
                sourceCoords,
                coords,
                sourceCoords.length / 2
              );
              context.strokeStyle = this.styleSet.majorGridlineStyle.color;
              context.lineWidth = this.styleSet.majorGridlineStyle.width;
              let pattern = undefined;
              if (this.styleSet.majorGridlineStyle.pattern !== undefined) {
                pattern = [];
                for (
                  let i = 0;
                  i < this.styleSet.majorGridlineStyle.pattern.length;
                  i++
                ) {
                  pattern.push(
                    this.styleSet.majorGridlineStyle.pattern[i] *
                      devicePixelRatio
                  );
                }
              }
              context.setLineDash(pattern);
              for (let i = 0; i < coords.length; i += 4) {
                context.moveTo(coords[i], coords[i + 1]);
                context.lineTo(coords[i + 2], coords[i + 3]);
              }
            }
          }
          if (zeroYValid) {
            let sourceCoords = [left, 0, right, 0];
            let coords = [];
            transform.transform(sourceCoords, coords, 2);
            context.strokeStyle = this.styleSet.gridlineAxisStyle.color;
            context.lineWidth = this.styleSet.gridlineAxisStyle.width;
            let pattern = undefined;
            if (this.styleSet.gridlineAxisStyle.pattern !== undefined) {
              pattern = [];
              for (
                let i = 0;
                i < this.styleSet.gridlineAxisStyle.pattern.length;
                i++
              ) {
                pattern.push(
                  this.styleSet.gridlineAxisStyle.pattern[i] * devicePixelRatio
                );
              }
            }
            context.setLineDash(pattern);
            context.moveTo(coords[0], coords[1]);
            context.lineTo(coords[2], coords[3]);
          }
          if (zeroXValid) {
            let sourceCoords = [0, top, 0, bottom];
            let coords = [];
            transform.transform(sourceCoords, coords, 2);
            context.strokeStyle = this.styleSet.gridlineAxisStyle.color;
            context.lineWidth = this.styleSet.gridlineAxisStyle.width;
            let pattern = undefined;
            if (this.styleSet.gridlineAxisStyle.pattern !== undefined) {
              pattern = [];
              for (
                let i = 0;
                i < this.styleSet.gridlineAxisStyle.pattern.length;
                i++
              ) {
                pattern.push(
                  this.styleSet.gridlineAxisStyle.pattern[i] * devicePixelRatio
                );
              }
            }
            context.setLineDash(pattern);
            context.moveTo(coords[0], coords[1]);
            context.lineTo(coords[2], coords[3]);
          }
          context.stroke();
          if (uniformGridBlockSize !== undefined) {
            this.fullGridCapture = context.getImageData(
              0,
              0,
              this.gridCanvas.width,
              this.gridCanvas.height
            );
            this.leftGridCapture = context.getImageData(
              0,
              0,
              uniformGridBlockSize,
              this.gridCanvas.height
            );
            this.topGridCapture = context.getImageData(
              0,
              0,
              this.gridCanvas.width,
              uniformGridBlockSize
            );
            this.topLeftGridCapture = context.getImageData(
              0,
              0,
              uniformGridBlockSize,
              uniformGridBlockSize
            );
            this.startingOffsetXForCapture = this.offsetX;
            this.startingOffsetYForCapture = this.offsetY;
          } else {
            this.fullGridCapture = undefined;
          }
        }
      } else {
        paper = this.mainScope;
        this.indexInMainScopeActiveLayer = 0;
        setToMainScope = true;
        let topLeft = this.inverseTopLevelTransform.transform(
          new paper.Point(0, 0)
        );
        let topRight = this.inverseTopLevelTransform.transform(
          new paper.Point(this.root.clientWidth - 1, 0)
        );
        let bottomRight = this.inverseTopLevelTransform.transform(
          new paper.Point(this.root.clientWidth - 1, this.root.clientHeight - 1)
        );
        let bottomLeft = this.inverseTopLevelTransform.transform(
          new paper.Point(0, this.root.clientHeight - 1)
        );
        let left = Math.min(
          Math.min(Math.min(topLeft.x, topRight.x), bottomRight.x),
          bottomLeft.x
        );
        let top = Math.min(
          Math.min(Math.min(topLeft.y, topRight.y), bottomRight.y),
          bottomLeft.y
        );
        let right = Math.max(
          Math.max(Math.max(topLeft.x, topRight.x), bottomRight.x),
          bottomLeft.x
        );
        let bottom = Math.max(
          Math.max(Math.max(topLeft.y, topRight.y), bottomRight.y),
          bottomLeft.y
        );
        let zeroXValid = left * right <= 0;
        let zeroYValid = top * bottom <= 0;
        if (left < right && top < bottom) {
          let minorGridSize = this.resolution;
          let majorGridSize = minorGridSize * this.majorGridlineScaleFactor;
          if (minorGridSize >= this.gridThreshold) {
            if (this.minorGridlines === undefined) {
              this.minorGridlines = new paper.CompoundPath();
            }
            let minorGridlinesIndex = 0;
            this.relocateInMainScopeActiveLayer(this.minorGridlines);
            this.styleSet.minorGridlineStyle.applyStrokeTo(this.minorGridlines);
            let gridLineX = Math.floor(left);
            while (gridLineX < right) {
              if (
                gridLineX % this.majorGridlineScaleFactor !== 0 ||
                majorGridSize < this.gridThreshold
              ) {
                if (minorGridlinesIndex < this.minorGridlines.children.length) {
                  let path = this.minorGridlines.children[minorGridlinesIndex];
                  let first = path.segments[0];
                  first.point.x = gridLineX;
                  first.point.y = top;
                  let second = path.segments[1];
                  second.point.x = gridLineX;
                  second.point.y = bottom;
                } else {
                  this.minorGridlines.moveTo(gridLineX, top);
                  this.minorGridlines.lineTo(gridLineX, bottom);
                }
                minorGridlinesIndex++;
              }
              gridLineX++;
            }
            let gridLineY = Math.floor(top);
            while (gridLineY < bottom) {
              if (
                gridLineY % this.majorGridlineScaleFactor !== 0 ||
                majorGridSize < this.gridThreshold
              ) {
                if (minorGridlinesIndex < this.minorGridlines.children.length) {
                  let path = this.minorGridlines.children[minorGridlinesIndex];
                  let first = path.segments[0];
                  first.point.x = left;
                  first.point.y = gridLineY;
                  let second = path.segments[1];
                  second.point.x = right;
                  second.point.y = gridLineY;
                } else {
                  this.minorGridlines.moveTo(left, gridLineY);
                  this.minorGridlines.lineTo(right, gridLineY);
                }
                minorGridlinesIndex++;
              }
              gridLineY++;
            }
            this.minorGridlines.removeChildren(minorGridlinesIndex);
            this.minorGridlines.transform(this.topLevelTransform);
          }
          if (majorGridSize >= this.gridThreshold) {
            if (this.majorGridlines === undefined) {
              this.majorGridlines = new paper.CompoundPath();
            }
            let majorGridlinesIndex = 0;
            this.relocateInMainScopeActiveLayer(this.majorGridlines);
            this.styleSet.majorGridlineStyle.applyStrokeTo(this.majorGridlines);
            let gridLineX = Math.floor(left);
            while (gridLineX % this.majorGridlineScaleFactor !== 0) {
              gridLineX++;
            }
            while (gridLineX < right) {
              if (gridLineX !== 0) {
                if (majorGridlinesIndex < this.majorGridlines.children.length) {
                  let path = this.majorGridlines.children[majorGridlinesIndex];
                  let first = path.segments[0];
                  first.point.x = gridLineX;
                  first.point.y = top;
                  let second = path.segments[1];
                  second.point.x = gridLineX;
                  second.point.y = bottom;
                } else {
                  this.majorGridlines.moveTo(gridLineX, top);
                  this.majorGridlines.lineTo(gridLineX, bottom);
                }
                majorGridlinesIndex++;
              }
              gridLineX += this.majorGridlineScaleFactor;
            }
            let gridLineY = Math.floor(top);
            while (gridLineY % this.majorGridlineScaleFactor !== 0) {
              gridLineY++;
            }
            while (gridLineY < bottom) {
              if (gridLineY !== 0) {
                if (majorGridlinesIndex < this.majorGridlines.children.length) {
                  let path = this.majorGridlines.children[majorGridlinesIndex];
                  let first = path.segments[0];
                  first.point.x = left;
                  first.point.y = gridLineY;
                  let second = path.segments[1];
                  second.point.x = right;
                  second.point.y = gridLineY;
                } else {
                  this.majorGridlines.moveTo(left, gridLineY);
                  this.majorGridlines.lineTo(right, gridLineY);
                }
                majorGridlinesIndex++;
              }
              gridLineY += this.majorGridlineScaleFactor;
            }
            this.majorGridlines.removeChildren(majorGridlinesIndex);
            this.majorGridlines.transform(this.topLevelTransform);
          }
        }
        if (zeroYValid) {
          if (this.gridXAxis === undefined) {
            this.gridXAxis = new paper.Path();
          }
          this.gridXAxis.removeSegments();
          this.relocateInMainScopeActiveLayer(this.gridXAxis);
          this.styleSet.gridlineAxisStyle.applyStrokeTo(this.gridXAxis);
          this.gridXAxis.moveTo(left, 0);
          this.gridXAxis.lineTo(right, 0);
          this.gridXAxis.transform(this.topLevelTransform);
        }
        if (zeroXValid) {
          if (this.gridYAxis === undefined) {
            this.gridYAxis = new paper.Path();
          }
          this.gridYAxis.removeSegments();
          this.relocateInMainScopeActiveLayer(this.gridYAxis);
          this.styleSet.gridlineAxisStyle.applyStrokeTo(this.gridYAxis);
          this.gridYAxis.moveTo(0, top);
          this.gridYAxis.lineTo(0, bottom);
          this.gridYAxis.transform(this.topLevelTransform);
        }
      }
      this.redrawGrid = false;
    } else if (!this.gridVisible && this.redrawGrid) {
      if (this.gridCanvas !== undefined) {
        let devicePixelRatio = 1;
        if (parent.devicePixelRatio !== undefined) {
          devicePixelRatio = parent.devicePixelRatio;
        }
        let context = this.gridCanvas.getContext("2d");
        this.gridCanvas.width = this.root.clientWidth * devicePixelRatio;
        this.gridCanvas.height = this.root.clientHeight * devicePixelRatio;
        context.clearRect(0, 0, this.gridCanvas.width, this.gridCanvas.height);
      } else {
        paper = this.mainScope;
        this.indexInMainScopeActiveLayer = 0;
        setToMainScope = true;
      }
      this.redrawGrid = false;
    }
    if (!setToMainScope) {
      paper = this.mainScope;
      this.indexInMainScopeActiveLayer = 0;
    }
    if (this.layers) {
      this.layers.map(layer => {
        let area = 0;
        if (layer.objects) {
          layer.draw(this, this.topLevelTransform);
          layer.objects.map(object => {
            if (
              object.objects &&
              object.objects.length > 0 &&
              object.objects[0].closed
            ) {
              if (object.objects[0].negative)
                area -= object.objects[0].getArea(parent.sketchControl);
              else area += object.objects[0].getArea(parent.sketchControl);
            }
          });
        }
        layer.area = area;
        area = layer.grossArea
          ? layer.grossArea
          : layer.netArea
          ? layer.netArea
          : 0;
        if (layer.uniqueIdentifier == this.sketchLayerToEdit) {
          const innerHTML =
            layer.name == "Scratchpad"
              ? ""
              : `<br />${Math.round(layer.area)} ⏍`;
          document.getElementById("layerName").innerHTML =
            `${layer.name}` + innerHTML;
        }
      });
      if (document.getElementById('layersMenu').style.visibility == 'visible')
        loadLayersMenu();
    }
    if (this.mode === SketchMode.Draw) {
      let path = undefined;
      let elementIndex = undefined;
      let forCurve = false;
      if (this.editing !== undefined) {
        if (this.editing.path) {
          path = this.editing.path;
          for (let i = 0; i < path.elements.length; i++) {
            const element = path.elements[i];
            if (element === this.editing.element) {
              elementIndex = i;
              break;
            }
          }
          forCurve = true;
        }
      } else if (
        this.sketchPathToEdit !== undefined &&
        this.sketchPathToEdit.elements !== undefined &&
        this.sketchPathToEdit.elements.length > 0
      ) {
        path = this.sketchPathToEdit;
        elementIndex = this.sketchPathToEdit.elements.length - 1;
      }
      //Measurement indicators on pen down (and cursor is down)
      if (
        this.penDown &&
        path !== undefined &&
        elementIndex !== undefined &&
        (!this.sketchPathToEdit || this.isDown)
      ) {
        let lastPoint = undefined;
        let beforeLastPoint = undefined;
        let beforeBeforeLastPoint = undefined;
        let rise = undefined;
        let run = undefined;
        let angle = undefined;
        let height = undefined;
        let drawingHeight = undefined;
        let chord = undefined;
        if (elementIndex >= 2) {
          const element = path.elements[elementIndex];
          lastPoint = this.topLevelTransform.transform(
            new paper.Point(element.x, element.y)
          );
          const elementBefore = path.elements[elementIndex - 1];
          beforeLastPoint = this.topLevelTransform.transform(
            new paper.Point(elementBefore.x, elementBefore.y)
          );
          const elementBeforeBefore = path.elements[elementIndex - 2];
          beforeBeforeLastPoint = this.topLevelTransform.transform(
            new paper.Point(elementBeforeBefore.x, elementBeforeBefore.y)
          );
          rise = Math.abs(element.y - elementBefore.y);
          run = Math.abs(element.x - elementBefore.x);
          chord = Math.sqrt(rise * rise + run * run);
          const midPointX = (element.x + elementBefore.x) / 2;
          const midPointY = (element.y + elementBefore.y) / 2;
          angle = 0;
          if (element.angle !== undefined) {
            angle = element.angle;
          }
          if (angle !== 0) {
            let center = SketchUtils.centerPointOfLine(
              elementBefore.x,
              elementBefore.y,
              element.x,
              element.y,
              angle
            );
            let deltaX = center.x - midPointX;
            let deltaY = center.y - midPointY;
            height = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
            center = this.topLevelTransform.transform(center);
            deltaX = center.x - (beforeLastPoint.x + lastPoint.x) / 2;
            deltaY = center.y - (beforeLastPoint.y + lastPoint.y) / 2;
            drawingHeight = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
          }
        } else if (elementIndex === 1) {
          const element = path.elements[elementIndex];
          lastPoint = this.topLevelTransform.transform(
            new paper.Point(element.x, element.y)
          );
          const elementBefore = path.elements[elementIndex - 1];
          beforeLastPoint = this.topLevelTransform.transform(
            new paper.Point(elementBefore.x, elementBefore.y)
          );
          let start = path.start;
          beforeBeforeLastPoint = this.topLevelTransform.transform(
            new paper.Point(start.x, start.y)
          );
          rise = Math.abs(element.y - elementBefore.y);
          run = Math.abs(element.x - elementBefore.x);
          chord = Math.sqrt(rise * rise + run * run);
          const midPointX = (element.x + elementBefore.x) / 2;
          const midPointY = (element.y + elementBefore.y) / 2;
          angle = 0;
          if (element.angle !== undefined) {
            angle = element.angle;
          }
          if (angle !== 0) {
            let center = SketchUtils.centerPointOfLine(
              elementBefore.x,
              elementBefore.y,
              element.x,
              element.y,
              angle
            );
            let deltaX = center.x - midPointX;
            let deltaY = center.y - midPointY;
            height = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
            center = this.topLevelTransform.transform(center);
            deltaX = center.x - (beforeLastPoint.x + lastPoint.x) / 2;
            deltaY = center.y - (beforeLastPoint.y + lastPoint.y) / 2;
            drawingHeight = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
          }
        } else {
          const element = path.elements[elementIndex];
          lastPoint = this.topLevelTransform.transform(
            new paper.Point(element.x, element.y)
          );
          const start = path.start;
          beforeLastPoint = this.topLevelTransform.transform(
            new paper.Point(start.x, start.y)
          );
          rise = Math.abs(element.y - start.y);
          run = Math.abs(element.x - start.x);
          chord = Math.sqrt(rise * rise + run * run);
          const midPointX = (element.x + start.x) / 2;
          const midPointY = (element.y + start.y) / 2;
          angle = 0;
          if (element.angle !== undefined) {
            angle = element.angle;
          }
          if (angle !== 0) {
            let center = SketchUtils.centerPointOfLine(
              start.x,
              start.y,
              element.x,
              element.y,
              angle
            );
            let deltaX = center.x - midPointX;
            let deltaY = center.y - midPointY;
            height = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
            center = this.topLevelTransform.transform(center);
            deltaX = center.x - (beforeLastPoint.x + lastPoint.x) / 2;
            deltaY = center.y - (beforeLastPoint.y + lastPoint.y) / 2;
            drawingHeight = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
          }
        }
        if (beforeBeforeLastPoint !== undefined) {
          const segmentX = lastPoint.x - beforeLastPoint.x;
          const segmentY = lastPoint.y - beforeLastPoint.y;
          const previousSegmentX = beforeLastPoint.x - beforeBeforeLastPoint.x;
          const previousSegmentY = beforeLastPoint.y - beforeBeforeLastPoint.y;
          const segmentLength = Math.sqrt(
            segmentX * segmentX + segmentY * segmentY
          );
          if (segmentLength > 1e-5) {
            const previousSegmentLength = Math.sqrt(
              previousSegmentX * previousSegmentX +
                previousSegmentY * previousSegmentY
            );
            if (previousSegmentLength > 1e-5) {
              const directionX = segmentX / segmentLength;
              const directionY = segmentY / segmentLength;
              const previousDirectionX =
                previousSegmentX / previousSegmentLength;
              const previousDirectionY =
                previousSegmentY / previousSegmentLength;
              const angleCosine =
                directionX * previousDirectionX +
                directionY * previousDirectionY;
              let angleToUse = Math.acos(angleCosine);
              let angleToDisplay = Math.round((angleToUse * 180) / Math.PI);
              if (angleToDisplay == 90) {
                const size = Math.min(
                  50,
                  segmentLength / 2,
                  previousSegmentLength / 2
                );
                const separation = Math.min(
                  4,
                  segmentLength / 2,
                  previousSegmentLength / 2
                );
                if (this.shapeFor90DegreesIndicator === undefined) {
                  this.shapeFor90DegreesIndicator = new paper.Path();
                } else {
                  this.shapeFor90DegreesIndicator.removeSegments();
                }
                this.shapeFor90DegreesIndicator.moveTo(
                  beforeLastPoint.x -
                    previousDirectionX * size +
                    directionX * separation,
                  beforeLastPoint.y -
                    previousDirectionY * size +
                    directionY * separation
                );
                this.shapeFor90DegreesIndicator.lineTo(
                  beforeLastPoint.x -
                    previousDirectionX * size +
                    directionX * size,
                  beforeLastPoint.y -
                    previousDirectionY * size +
                    directionY * size
                );
                this.shapeFor90DegreesIndicator.lineTo(
                  beforeLastPoint.x -
                    previousDirectionX * separation +
                    directionX * size,
                  beforeLastPoint.y -
                    previousDirectionY * separation +
                    directionY * size
                );
                this.relocateInMainScopeActiveLayer(
                  this.shapeFor90DegreesIndicator
                );
                this.styleSet.lineFor90DegreesIndicatorStyle.applyStrokeTo(
                  this.shapeFor90DegreesIndicator
                );
              } else {
                let projection =
                  segmentX * previousDirectionX + segmentY * previousDirectionY;
                let projectedX =
                  beforeLastPoint.x + previousDirectionX * projection;
                let projectedY =
                  beforeLastPoint.y + previousDirectionY * projection;
                let previousSideDirectionX = lastPoint.x - projectedX;
                let previousSideDirectionY = lastPoint.y - projectedY;
                let length = Math.sqrt(
                  previousSideDirectionX * previousSideDirectionX +
                    previousSideDirectionY * previousSideDirectionY
                );
                previousSideDirectionX /= length;
                previousSideDirectionY /= length;
                projection =
                  previousSegmentX * directionX + previousSegmentY * directionY;
                projectedX = beforeLastPoint.x - directionX * projection;
                projectedY = beforeLastPoint.y - directionY * projection;
                let sideDirectionX = beforeBeforeLastPoint.x - projectedX;
                let sideDirectionY = beforeBeforeLastPoint.y - projectedY;
                length = Math.sqrt(
                  sideDirectionX * sideDirectionX +
                    sideDirectionY * sideDirectionY
                );
                sideDirectionX /= length;
                sideDirectionY /= length;
                const outerRing = Math.min(
                  50,
                  segmentLength / 2,
                  previousSegmentLength / 2
                );
                const innerRing = Math.min(
                  12,
                  segmentLength / 2,
                  previousSegmentLength / 2
                );
                const separation = Math.min(
                  3,
                  segmentLength / 2,
                  previousSegmentLength / 2
                );
                let innerAngleToDisplay = 180 - angleToDisplay;
                if (innerAngleToDisplay < 180) {
                  let midDirectionX = (directionX - previousDirectionX) / 2;
                  let midDirectionY = (directionY - previousDirectionY) / 2;
                  const midDirectionLength = Math.sqrt(
                    midDirectionX * midDirectionX +
                      midDirectionY * midDirectionY
                  );
                  midDirectionX /= midDirectionLength;
                  midDirectionY /= midDirectionLength;
                  if (this.innerAngleIndicator === undefined) {
                    this.innerAngleIndicator = new paper.Path();
                  } else {
                    this.innerAngleIndicator.removeSegments();
                  }
                  this.innerAngleIndicator.moveTo(
                    beforeLastPoint.x -
                      previousDirectionX * innerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y -
                      previousDirectionY * innerRing +
                      previousSideDirectionY * separation
                  );
                  this.innerAngleIndicator.lineTo(
                    beforeLastPoint.x -
                      previousDirectionX * outerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y -
                      previousDirectionY * outerRing +
                      previousSideDirectionY * separation
                  );
                  this.innerAngleIndicator.arcTo(
                    beforeLastPoint.x + midDirectionX * outerRing,
                    beforeLastPoint.y + midDirectionY * outerRing,
                    beforeLastPoint.x +
                      directionX * outerRing +
                      sideDirectionX * separation,
                    beforeLastPoint.y +
                      directionY * outerRing +
                      sideDirectionY * separation
                  );
                  this.innerAngleIndicator.lineTo(
                    beforeLastPoint.x +
                      directionX * innerRing +
                      sideDirectionX * separation,
                    beforeLastPoint.y +
                      directionY * innerRing +
                      sideDirectionY * separation
                  );
                  this.innerAngleIndicator.arcTo(
                    beforeLastPoint.x + midDirectionX * innerRing,
                    beforeLastPoint.y + midDirectionY * innerRing,
                    beforeLastPoint.x -
                      previousDirectionX * innerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y -
                      previousDirectionY * innerRing +
                      previousSideDirectionY * separation
                  );
                  this.relocateInMainScopeActiveLayer(this.innerAngleIndicator);
                  this.styleSet.innerAngleIndicatorStyle.applyFillTo(
                    this.innerAngleIndicator
                  );
                  let textPosition = new paper.Point(
                    beforeLastPoint.x + (midDirectionX * outerRing) / 2,
                    beforeLastPoint.y + (midDirectionY * outerRing) / 2
                  );
                  this.innerAngleTextIndicator = new paper.PointText(
                    textPosition
                  );
                  this.innerAngleTextIndicator.content = innerAngleToDisplay;
                  let textAngle =
                    (Math.atan2(-midDirectionX, midDirectionY) * 180) / Math.PI;
                  if (textAngle < -90) {
                    textAngle = 180 + textAngle;
                  } else if (textAngle > 90) {
                    textAngle = -180 + textAngle;
                  }
                  this.innerAngleTextIndicator.position = textPosition;
                  this.innerAngleTextIndicator.rotate(textAngle);
                  this.relocateInMainScopeActiveLayer(
                    this.innerAngleTextIndicator
                  );
                  this.applyTextStyle(
                    this.innerAngleTextIndicator,
                    this.styleSet.innerAngleIndicatorStyle
                  );
                }
                if (angleToDisplay > 0 && angleToDisplay < 90) {
                  let midDirectionX = (directionX + previousDirectionX) / 2;
                  let midDirectionY = (directionY + previousDirectionY) / 2;
                  const midDirectionLength = Math.sqrt(
                    midDirectionX * midDirectionX +
                      midDirectionY * midDirectionY
                  );
                  midDirectionX /= midDirectionLength;
                  midDirectionY /= midDirectionLength;
                  if (this.outerAngleIndicator === undefined) {
                    this.outerAngleIndicator = new paper.Path();
                  } else {
                    this.outerAngleIndicator.removeSegments();
                  }
                  this.outerAngleIndicator.moveTo(
                    beforeLastPoint.x +
                      previousDirectionX * innerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y +
                      previousDirectionY * innerRing +
                      previousSideDirectionY * separation
                  );
                  this.outerAngleIndicator.lineTo(
                    beforeLastPoint.x +
                      previousDirectionX * outerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y +
                      previousDirectionY * outerRing +
                      previousSideDirectionY * separation
                  );
                  this.outerAngleIndicator.arcTo(
                    beforeLastPoint.x + midDirectionX * outerRing,
                    beforeLastPoint.y + midDirectionY * outerRing,
                    beforeLastPoint.x +
                      directionX * outerRing -
                      sideDirectionX * separation,
                    beforeLastPoint.y +
                      directionY * outerRing -
                      sideDirectionY * separation
                  );
                  this.outerAngleIndicator.lineTo(
                    beforeLastPoint.x +
                      directionX * innerRing -
                      sideDirectionX * separation,
                    beforeLastPoint.y +
                      directionY * innerRing -
                      sideDirectionY * separation
                  );
                  this.outerAngleIndicator.arcTo(
                    beforeLastPoint.x + midDirectionX * innerRing,
                    beforeLastPoint.y + midDirectionY * innerRing,
                    beforeLastPoint.x +
                      previousDirectionX * innerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y +
                      previousDirectionY * innerRing +
                      previousSideDirectionY * separation
                  );
                  this.relocateInMainScopeActiveLayer(this.outerAngleIndicator);
                  this.styleSet.outerAngleIndicatorStyle.applyFillTo(
                    this.outerAngleIndicator
                  );
                  let textPosition = new paper.Point(
                    beforeLastPoint.x + (midDirectionX * outerRing) / 2,
                    beforeLastPoint.y + (midDirectionY * outerRing) / 2
                  );
                  this.outerAngleTextIndicator = new paper.PointText(
                    textPosition
                  );
                  this.outerAngleTextIndicator.content = angleToDisplay;
                  let textAngle =
                    (Math.atan2(-midDirectionX, midDirectionY) * 180) / Math.PI;
                  if (textAngle < -90) {
                    textAngle = 180 + textAngle;
                  } else if (textAngle > 90) {
                    textAngle = -180 + textAngle;
                  }
                  this.outerAngleTextIndicator.position = textPosition;
                  this.outerAngleTextIndicator.rotate(textAngle);
                  this.relocateInMainScopeActiveLayer(
                    this.outerAngleTextIndicator
                  );
                  this.applyTextStyle(
                    this.outerAngleTextIndicator,
                    this.styleSet.outerAngleIndicatorStyle
                  );
                }
              }
            }
          }
        }
        if (beforeLastPoint !== undefined) {
          const segmentX = lastPoint.x - beforeLastPoint.x;
          const segmentY = lastPoint.y - beforeLastPoint.y;
          let width = 0;
          if (this.styleSet.riseAndRunIndicatorStyle.width !== undefined) {
            width = this.styleSet.riseAndRunIndicatorStyle.width;
          }
          if (width > 0) {
            let offset = 0;
            if (this.styleSet.riseAndRunIndicatorStyle.offset !== undefined) {
              offset = this.styleSet.riseAndRunIndicatorStyle.offset;
            }
            if (forCurve) {
              if (drawingHeight !== undefined) {
                const segmentLength = Math.sqrt(
                  segmentX * segmentX + segmentY * segmentY
                );
                if (segmentLength > 1e-5) {
                  const directionX = segmentX / segmentLength;
                  const directionY = segmentY / segmentLength;
                  let sideDirectionX = -directionY;
                  let sideDirectionY = directionX;
                  if (angle > 0) {
                    sideDirectionX = -sideDirectionX;
                    sideDirectionY = -sideDirectionY;
                  }
                  const heightStartX = beforeLastPoint.x - directionX * offset;
                  const heightStartY = beforeLastPoint.y - directionY * offset;
                  const heightFinishX =
                    heightStartX + sideDirectionX * drawingHeight;
                  const heightFinishY =
                    heightStartY + sideDirectionY * drawingHeight;
                  if (this.heightIndicator === undefined) {
                    this.heightIndicator = new paper.Path();
                  } else {
                    this.heightIndicator.removeSegments();
                  }
                  this.heightIndicator.moveTo(heightStartX, heightStartY);
                  this.heightIndicator.lineTo(
                    heightStartX -
                      directionX * 1.41 * width +
                      sideDirectionX * 3 * width,
                    heightStartY -
                      directionY * 1.41 * width +
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightStartX -
                      (directionX * width) / 2 +
                      sideDirectionX * 3 * width,
                    heightStartY -
                      (directionY * width) / 2 +
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightFinishX -
                      (directionX * width) / 2 -
                      sideDirectionX * 3 * width,
                    heightFinishY -
                      (directionY * width) / 2 -
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightFinishX -
                      directionX * 1.41 * width -
                      sideDirectionX * 3 * width,
                    heightFinishY -
                      directionY * 1.41 * width -
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(heightFinishX, heightFinishY);
                  this.heightIndicator.lineTo(
                    heightFinishX +
                      directionX * 1.41 * width -
                      sideDirectionX * 3 * width,
                    heightFinishY +
                      directionY * 1.41 * width -
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightFinishX +
                      (directionX * width) / 2 -
                      sideDirectionX * 3 * width,
                    heightFinishY +
                      (directionY * width) / 2 -
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightStartX +
                      (directionX * width) / 2 +
                      sideDirectionX * 3 * width,
                    heightStartY +
                      (directionY * width) / 2 +
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightStartX +
                      directionX * 1.41 * width +
                      sideDirectionX * 3 * width,
                    heightStartY +
                      directionY * 1.41 * width +
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.closed = true;
                  this.relocateInMainScopeActiveLayer(this.heightIndicator);
                  this.styleSet.riseAndRunIndicatorStyle.applyFillTo(
                    this.heightIndicator
                  );
                  let textPositionX = (heightStartX + heightFinishX) / 2;
                  let textPositionY = (heightStartY + heightFinishY) / 2;
                  let textPosition = new paper.Point(
                    textPositionX,
                    textPositionY
                  );
                  let rectangle = new paper.Rectangle(
                    textPositionX,
                    textPositionY,
                    1,
                    1
                  );
                  if (this.heightIndicatorBackground === undefined) {
                    this.heightIndicatorBackground = new paper.Path.Rectangle(
                      rectangle
                    );
                    this.heightIndicatorBackground.fillColor = "white";
                  }
                  if (this.heightIndicatorText === undefined) {
                    this.heightIndicatorText = new paper.PointText(
                      textPosition
                    );
                  }
                  SketchUtils.drawTextWithBackground(
                    this,
                    this.heightIndicatorText,
                    textPosition,
                    height,
                    this.heightIndicatorBackground,
                    this.styleSet.riseAndRunIndicatorStyle
                  );
                  const chordStartX =
                    beforeLastPoint.x - sideDirectionX * offset;
                  const chordStartY =
                    beforeLastPoint.y - sideDirectionY * offset;
                  const chordFinishX = lastPoint.x - sideDirectionX * offset;
                  const chordFinishY = lastPoint.y - sideDirectionY * offset;
                  if (this.chordIndicator === undefined) {
                    this.chordIndicator = new paper.Path();
                  } else {
                    this.chordIndicator.removeSegments();
                  }
                  this.chordIndicator.moveTo(chordStartX, chordStartY);
                  this.chordIndicator.lineTo(
                    chordStartX -
                      sideDirectionX * 1.41 * width +
                      directionX * 3 * width,
                    chordStartY -
                      sideDirectionY * 1.41 * width +
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordStartX -
                      (sideDirectionX * width) / 2 +
                      directionX * 3 * width,
                    chordStartY -
                      (sideDirectionY * width) / 2 +
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordFinishX -
                      (sideDirectionX * width) / 2 -
                      directionX * 3 * width,
                    chordFinishY -
                      (sideDirectionY * width) / 2 -
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordFinishX -
                      sideDirectionX * 1.41 * width -
                      directionX * 3 * width,
                    chordFinishY -
                      sideDirectionY * 1.41 * width -
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(chordFinishX, chordFinishY);
                  this.chordIndicator.lineTo(
                    chordFinishX +
                      sideDirectionX * 1.41 * width -
                      directionX * 3 * width,
                    chordFinishY +
                      sideDirectionY * 1.41 * width -
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordFinishX +
                      (sideDirectionX * width) / 2 -
                      directionX * 3 * width,
                    chordFinishY +
                      (sideDirectionY * width) / 2 -
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordStartX +
                      (sideDirectionX * width) / 2 +
                      directionX * 3 * width,
                    chordStartY +
                      (sideDirectionY * width) / 2 +
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordStartX +
                      sideDirectionX * 1.41 * width +
                      directionX * 3 * width,
                    chordStartY +
                      sideDirectionY * 1.41 * width +
                      directionY * 3 * width
                  );
                  this.chordIndicator.closed = true;
                  this.relocateInMainScopeActiveLayer(this.chordIndicator);
                  this.styleSet.riseAndRunIndicatorStyle.applyFillTo(
                    this.chordIndicator
                  );
                  textPositionX = (chordStartX + chordFinishX) / 2;
                  textPositionY = (chordStartY + chordFinishY) / 2;
                  textPosition = new paper.Point(textPositionX, textPositionY);
                  rectangle = new paper.Rectangle(
                    textPositionX,
                    textPositionY,
                    1,
                    1
                  );
                  if (this.chordIndicatorBackground === undefined) {
                    this.chordIndicatorBackground = new paper.Path.Rectangle(
                      rectangle
                    );
                    this.chordIndicatorBackground.fillColor = "white";
                  }
                  if (this.chordIndicatorText === undefined) {
                    this.chordIndicatorText = new paper.PointText(textPosition);
                  }
                  SketchUtils.drawTextWithBackground(
                    this,
                    this.chordIndicatorText,
                    textPosition,
                    chord,
                    this.chordIndicatorBackground,
                    this.styleSet.riseAndRunIndicatorStyle
                  );
                }
              }
            } else if (segmentX !== 0 && segmentY !== 0) {
              let riseStartX = undefined;
              let riseStartY = undefined;
              let riseFinishX = undefined;
              let riseFinishY = undefined;
              let riseDirection = undefined;
              let runStartX = undefined;
              let runStartY = undefined;
              let runFinishX = undefined;
              let runFinishY = undefined;
              let runDirection = undefined;
              if (path.fillPath === undefined || path.fillPath.clockwise) {
                if (segmentX > 0) {
                  if (segmentY > 0) {
                    riseStartX = lastPoint.x + offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = lastPoint.x + offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = 1;
                    runStartX = beforeLastPoint.x;
                    runStartY = beforeLastPoint.y - offset;
                    runFinishX = lastPoint.x;
                    runFinishY = beforeLastPoint.y - offset;
                    runDirection = 1;
                  } else {
                    riseStartX = beforeLastPoint.x - offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = beforeLastPoint.x - offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = -1;
                    runStartX = beforeLastPoint.x;
                    runStartY = lastPoint.y - offset;
                    runFinishX = lastPoint.x;
                    runFinishY = lastPoint.y - offset;
                    runDirection = 1;
                  }
                } else {
                  if (segmentY > 0) {
                    riseStartX = beforeLastPoint.x + offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = beforeLastPoint.x + offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = 1;
                    runStartX = beforeLastPoint.x;
                    runStartY = lastPoint.y + offset;
                    runFinishX = lastPoint.x;
                    runFinishY = lastPoint.y + offset;
                    runDirection = -1;
                  } else {
                    riseStartX = lastPoint.x - offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = lastPoint.x - offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = -1;
                    runStartX = beforeLastPoint.x;
                    runStartY = beforeLastPoint.y + offset;
                    runFinishX = lastPoint.x;
                    runFinishY = beforeLastPoint.y + offset;
                    runDirection = -1;
                  }
                }
              } else {
                if (segmentX > 0) {
                  if (segmentY > 0) {
                    riseStartX = beforeLastPoint.x - offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = beforeLastPoint.x - offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = 1;
                    runStartX = beforeLastPoint.x;
                    runStartY = lastPoint.y + offset;
                    runFinishX = lastPoint.x;
                    runFinishY = lastPoint.y + offset;
                    runDirection = 1;
                  } else {
                    riseStartX = lastPoint.x + offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = lastPoint.x + offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = -1;
                    runStartX = beforeLastPoint.x;
                    runStartY = beforeLastPoint.y + offset;
                    runFinishX = lastPoint.x;
                    runFinishY = beforeLastPoint.y + offset;
                    runDirection = 1;
                  }
                } else {
                  if (segmentY > 0) {
                    riseStartX = lastPoint.x - offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = lastPoint.x - offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = 1;
                    runStartX = beforeLastPoint.x;
                    runStartY = beforeLastPoint.y - offset;
                    runFinishX = lastPoint.x;
                    runFinishY = beforeLastPoint.y - offset;
                    runDirection = -1;
                  } else {
                    riseStartX = beforeLastPoint.x + offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = beforeLastPoint.x + offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = -1;
                    runStartX = beforeLastPoint.x;
                    runStartY = lastPoint.y - offset;
                    runFinishX = lastPoint.x;
                    runFinishY = lastPoint.y - offset;
                    runDirection = -1;
                  }
                }
              }
              if (runDirection !== undefined && riseDirection !== undefined) {
                if (this.riseIndicator === undefined) {
                  this.riseIndicator = new paper.Path();
                } else {
                  this.riseIndicator.removeSegments();
                }
                this.riseIndicator.moveTo(riseStartX, riseStartY);
                this.riseIndicator.lineTo(
                  riseStartX - 1.41 * width,
                  riseStartY + riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseStartX - width / 2,
                  riseStartY + riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseFinishX - width / 2,
                  riseFinishY - riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseFinishX - 1.41 * width,
                  riseFinishY - riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(riseFinishX, riseFinishY);
                this.riseIndicator.lineTo(
                  riseFinishX + 1.41 * width,
                  riseFinishY - riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseFinishX + width / 2,
                  riseFinishY - riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseStartX + width / 2,
                  riseStartY + riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseStartX + 1.41 * width,
                  riseStartY + riseDirection * 3 * width
                );
                this.riseIndicator.closed = true;
                this.relocateInMainScopeActiveLayer(this.riseIndicator);
                this.styleSet.riseAndRunIndicatorStyle.applyFillTo(
                  this.riseIndicator
                );
                let textPositionX = (riseStartX + riseFinishX) / 2;
                let textPositionY = (riseStartY + riseFinishY) / 2;
                let textPosition = new paper.Point(
                  textPositionX,
                  textPositionY
                );
                let rectangle = new paper.Rectangle(
                  textPositionX,
                  textPositionY,
                  1,
                  1
                );
                if (this.riseIndicatorBackground === undefined) {
                  this.riseIndicatorBackground = new paper.Path.Rectangle(
                    rectangle
                  );
                  this.riseIndicatorBackground.fillColor = "white";
                }
                if (this.riseIndicatorText === undefined) {
                  this.riseIndicatorText = new paper.PointText(textPosition);
                }
                SketchUtils.drawTextWithBackground(
                  this,
                  this.riseIndicatorText,
                  textPosition,
                  rise,
                  this.riseIndicatorBackground,
                  this.styleSet.riseAndRunIndicatorStyle
                );
                if (this.runIndicator === undefined) {
                  this.runIndicator = new paper.Path();
                } else {
                  this.runIndicator.removeSegments();
                }
                this.runIndicator.moveTo(runStartX, runStartY);
                this.runIndicator.lineTo(
                  runStartX + runDirection * 3 * width,
                  runStartY - 1.41 * width
                );
                this.runIndicator.lineTo(
                  runStartX + runDirection * 3 * width,
                  runStartY - width / 2
                );
                this.runIndicator.lineTo(
                  runFinishX - runDirection * 3 * width,
                  runFinishY - width / 2
                );
                this.runIndicator.lineTo(
                  runFinishX - runDirection * 3 * width,
                  runFinishY - 1.41 * width
                );
                this.runIndicator.lineTo(runFinishX, runFinishY);
                this.runIndicator.lineTo(
                  runFinishX - runDirection * 3 * width,
                  runFinishY + 1.41 * width
                );
                this.runIndicator.lineTo(
                  runFinishX - runDirection * 3 * width,
                  runFinishY + width / 2
                );
                this.runIndicator.lineTo(
                  runStartX + runDirection * 3 * width,
                  runStartY + width / 2
                );
                this.runIndicator.lineTo(
                  runStartX + runDirection * 3 * width,
                  runStartY + 1.41 * width
                );
                this.runIndicator.closed = true;
                this.relocateInMainScopeActiveLayer(this.runIndicator);
                this.styleSet.riseAndRunIndicatorStyle.applyFillTo(
                  this.runIndicator
                );
                textPositionX = (runStartX + runFinishX) / 2;
                textPositionY = (runStartY + runFinishY) / 2;
                textPosition = new paper.Point(textPositionX, textPositionY);
                rectangle = new paper.Rectangle(
                  textPositionX,
                  textPositionY,
                  1,
                  1
                );
                if (this.runIndicatorBackground === undefined) {
                  this.runIndicatorBackground = new paper.Path.Rectangle(
                    rectangle
                  );
                  this.runIndicatorBackground.fillColor = "white";
                }
                if (this.runIndicatorText === undefined) {
                  this.runIndicatorText = new paper.PointText(textPosition);
                }
                SketchUtils.drawTextWithBackground(
                  this,
                  this.runIndicatorText,
                  textPosition,
                  run,
                  this.runIndicatorBackground,
                  this.styleSet.riseAndRunIndicatorStyle
                );
              }
            }
          }
        }
      }
      //Measurement indicators on pen down and only hover
      if (
        this.penDown &&
        this.sketchPathToEdit &&
        !this.isDown &&
        this.cursor
      ) {
        let path = this.sketchPathToEdit;

        let lastPoint = undefined;
        let beforeLastPoint = undefined;
        let beforeBeforeLastPoint = undefined;
        let rise = undefined;
        let run = undefined;
        let angle = undefined;
        let height = undefined;
        let drawingHeight = undefined;
        let chord = undefined;
        //Set last points
        if (elementIndex >= 1) {
          const element = { x: this.cursor.x, y: this.cursor.y };
          lastPoint = this.topLevelTransform.transform(
            new paper.Point(element.x, element.y)
          );
          const elementBefore = path.elements[elementIndex];
          beforeLastPoint = this.topLevelTransform.transform(
            new paper.Point(elementBefore.x, elementBefore.y)
          );
          const elementBeforeBefore = path.elements[elementIndex - 1];
          beforeBeforeLastPoint = this.topLevelTransform.transform(
            new paper.Point(elementBeforeBefore.x, elementBeforeBefore.y)
          );
          rise = element.y - elementBefore.y;
          run = element.x - elementBefore.x;
          chord = Math.sqrt(rise * rise + run * run);
          const midPointX = (element.x + elementBefore.x) / 2;
          const midPointY = (element.y + elementBefore.y) / 2;
          angle = 0;
          if (element.angle !== undefined) {
            angle = element.angle;
          }
          if (angle !== 0) {
            let center = SketchUtils.centerPointOfLine(
              elementBefore.x,
              elementBefore.y,
              element.x,
              element.y,
              angle
            );
            let deltaX = center.x - midPointX;
            let deltaY = center.y - midPointY;
            height = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
            center = this.topLevelTransform.transform(center);
            deltaX = center.x - (beforeLastPoint.x + lastPoint.x) / 2;
            deltaY = center.y - (beforeLastPoint.y + lastPoint.y) / 2;
            drawingHeight = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
          }
        } else if (elementIndex === 0) {
          const element = { x: this.cursor.x, y: this.cursor.y };
          lastPoint = this.topLevelTransform.transform(
            new paper.Point(element.x, element.y)
          );
          const elementBefore = path.elements[elementIndex];
          beforeLastPoint = this.topLevelTransform.transform(
            new paper.Point(elementBefore.x, elementBefore.y)
          );
          let start = path.start;
          beforeBeforeLastPoint = this.topLevelTransform.transform(
            new paper.Point(start.x, start.y)
          );
          rise = element.y - elementBefore.y;
          run = element.x - elementBefore.x;
          chord = Math.sqrt(rise * rise + run * run);
          const midPointX = (element.x + elementBefore.x) / 2;
          const midPointY = (element.y + elementBefore.y) / 2;
          angle = 0;
          if (element.angle !== undefined) {
            angle = element.angle;
          }
          if (angle !== 0) {
            let center = SketchUtils.centerPointOfLine(
              elementBefore.x,
              elementBefore.y,
              element.x,
              element.y,
              angle
            );
            let deltaX = center.x - midPointX;
            let deltaY = center.y - midPointY;
            height = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
            center = this.topLevelTransform.transform(center);
            deltaX = center.x - (beforeLastPoint.x + lastPoint.x) / 2;
            deltaY = center.y - (beforeLastPoint.y + lastPoint.y) / 2;
            drawingHeight = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
          }
        } else {
          const element = { x: this.cursor.x, y: this.cursor.y };
          lastPoint = this.topLevelTransform.transform(
            new paper.Point(element.x, element.y)
          );
          const start = path.start;
          beforeLastPoint = this.topLevelTransform.transform(
            new paper.Point(start.x, start.y)
          );
          rise = element.y - start.y;
          run = element.x - start.x;
          chord = Math.sqrt(rise * rise + run * run);
          const midPointX = (element.x + start.x) / 2;
          const midPointY = (element.y + start.y) / 2;
          angle = 0;
          if (element.angle !== undefined) {
            angle = element.angle;
          }
          if (angle !== 0) {
            let center = SketchUtils.centerPointOfLine(
              start.x,
              start.y,
              element.x,
              element.y,
              angle
            );
            let deltaX = center.x - midPointX;
            let deltaY = center.y - midPointY;
            height = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
            center = this.topLevelTransform.transform(center);
            deltaX = center.x - (beforeLastPoint.x + lastPoint.x) / 2;
            deltaY = center.y - (beforeLastPoint.y + lastPoint.y) / 2;
            drawingHeight = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
          }
        }
        //Draw angle and line measurements using last points
        if (beforeBeforeLastPoint !== undefined) {
          //Draw angle(s) measurements
          const segmentX = lastPoint.x - beforeLastPoint.x;
          const segmentY = lastPoint.y - beforeLastPoint.y;
          const previousSegmentX = beforeLastPoint.x - beforeBeforeLastPoint.x;
          const previousSegmentY = beforeLastPoint.y - beforeBeforeLastPoint.y;
          const segmentLength = Math.sqrt(
            segmentX * segmentX + segmentY * segmentY
          );
          if (segmentLength > 1e-5) {
            const previousSegmentLength = Math.sqrt(
              previousSegmentX * previousSegmentX +
                previousSegmentY * previousSegmentY
            );
            if (previousSegmentLength > 1e-5) {
              const directionX = segmentX / segmentLength;
              const directionY = segmentY / segmentLength;
              const previousDirectionX =
                previousSegmentX / previousSegmentLength;
              const previousDirectionY =
                previousSegmentY / previousSegmentLength;
              const angleCosine =
                directionX * previousDirectionX +
                directionY * previousDirectionY;
              let angleToUse = Math.acos(angleCosine);
              let angleToDisplay = Math.round((angleToUse * 180) / Math.PI);
              if (angleToDisplay == 90) {
                const size = Math.min(
                  50,
                  segmentLength / 2,
                  previousSegmentLength / 2
                );
                const separation = Math.min(
                  4,
                  segmentLength / 2,
                  previousSegmentLength / 2
                );
                if (this.shapeFor90DegreesIndicator === undefined) {
                  this.shapeFor90DegreesIndicator = new paper.Path();
                } else {
                  this.shapeFor90DegreesIndicator.removeSegments();
                }
                this.shapeFor90DegreesIndicator.moveTo(
                  beforeLastPoint.x -
                    previousDirectionX * size +
                    directionX * separation,
                  beforeLastPoint.y -
                    previousDirectionY * size +
                    directionY * separation
                );
                this.shapeFor90DegreesIndicator.lineTo(
                  beforeLastPoint.x -
                    previousDirectionX * size +
                    directionX * size,
                  beforeLastPoint.y -
                    previousDirectionY * size +
                    directionY * size
                );
                this.shapeFor90DegreesIndicator.lineTo(
                  beforeLastPoint.x -
                    previousDirectionX * separation +
                    directionX * size,
                  beforeLastPoint.y -
                    previousDirectionY * separation +
                    directionY * size
                );
                this.relocateInMainScopeActiveLayer(
                  this.shapeFor90DegreesIndicator
                );
                this.styleSet.lineFor90DegreesIndicatorStyle.applyStrokeTo(
                  this.shapeFor90DegreesIndicator
                );
              } else {
                let projection =
                  segmentX * previousDirectionX + segmentY * previousDirectionY;
                let projectedX =
                  beforeLastPoint.x + previousDirectionX * projection;
                let projectedY =
                  beforeLastPoint.y + previousDirectionY * projection;
                let previousSideDirectionX = lastPoint.x - projectedX;
                let previousSideDirectionY = lastPoint.y - projectedY;
                let length = Math.sqrt(
                  previousSideDirectionX * previousSideDirectionX +
                    previousSideDirectionY * previousSideDirectionY
                );
                previousSideDirectionX /= length;
                previousSideDirectionY /= length;
                projection =
                  previousSegmentX * directionX + previousSegmentY * directionY;
                projectedX = beforeLastPoint.x - directionX * projection;
                projectedY = beforeLastPoint.y - directionY * projection;
                let sideDirectionX = beforeBeforeLastPoint.x - projectedX;
                let sideDirectionY = beforeBeforeLastPoint.y - projectedY;
                length = Math.sqrt(
                  sideDirectionX * sideDirectionX +
                    sideDirectionY * sideDirectionY
                );
                sideDirectionX /= length;
                sideDirectionY /= length;
                const outerRing = Math.min(
                  50,
                  segmentLength / 2,
                  previousSegmentLength / 2
                );
                const innerRing = Math.min(
                  12,
                  segmentLength / 2,
                  previousSegmentLength / 2
                );
                const separation = Math.min(
                  3,
                  segmentLength / 2,
                  previousSegmentLength / 2
                );
                let innerAngleToDisplay = 180 - angleToDisplay;
                if (innerAngleToDisplay < 180) {
                  let midDirectionX = (directionX - previousDirectionX) / 2;
                  let midDirectionY = (directionY - previousDirectionY) / 2;
                  const midDirectionLength = Math.sqrt(
                    midDirectionX * midDirectionX +
                      midDirectionY * midDirectionY
                  );
                  midDirectionX /= midDirectionLength;
                  midDirectionY /= midDirectionLength;
                  if (this.innerAngleIndicator === undefined) {
                    this.innerAngleIndicator = new paper.Path();
                  } else {
                    this.innerAngleIndicator.removeSegments();
                  }
                  this.innerAngleIndicator.moveTo(
                    beforeLastPoint.x -
                      previousDirectionX * innerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y -
                      previousDirectionY * innerRing +
                      previousSideDirectionY * separation
                  );
                  this.innerAngleIndicator.lineTo(
                    beforeLastPoint.x -
                      previousDirectionX * outerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y -
                      previousDirectionY * outerRing +
                      previousSideDirectionY * separation
                  );
                  this.innerAngleIndicator.arcTo(
                    beforeLastPoint.x + midDirectionX * outerRing,
                    beforeLastPoint.y + midDirectionY * outerRing,
                    beforeLastPoint.x +
                      directionX * outerRing +
                      sideDirectionX * separation,
                    beforeLastPoint.y +
                      directionY * outerRing +
                      sideDirectionY * separation
                  );
                  this.innerAngleIndicator.lineTo(
                    beforeLastPoint.x +
                      directionX * innerRing +
                      sideDirectionX * separation,
                    beforeLastPoint.y +
                      directionY * innerRing +
                      sideDirectionY * separation
                  );
                  this.innerAngleIndicator.arcTo(
                    beforeLastPoint.x + midDirectionX * innerRing,
                    beforeLastPoint.y + midDirectionY * innerRing,
                    beforeLastPoint.x -
                      previousDirectionX * innerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y -
                      previousDirectionY * innerRing +
                      previousSideDirectionY * separation
                  );
                  this.relocateInMainScopeActiveLayer(this.innerAngleIndicator);
                  this.styleSet.innerAngleIndicatorStyle.applyFillTo(
                    this.innerAngleIndicator
                  );
                  let textPosition = new paper.Point(
                    beforeLastPoint.x + (midDirectionX * outerRing) / 2,
                    beforeLastPoint.y + (midDirectionY * outerRing) / 2
                  );
                  this.innerAngleTextIndicator = new paper.PointText(
                    textPosition
                  );
                  this.innerAngleTextIndicator.content = innerAngleToDisplay;
                  let textAngle =
                    (Math.atan2(-midDirectionX, midDirectionY) * 180) / Math.PI;
                  if (textAngle < -90) {
                    textAngle = 180 + textAngle;
                  } else if (textAngle > 90) {
                    textAngle = -180 + textAngle;
                  }
                  this.innerAngleTextIndicator.position = textPosition;
                  this.innerAngleTextIndicator.rotate(textAngle);
                  this.relocateInMainScopeActiveLayer(
                    this.innerAngleTextIndicator
                  );
                  this.applyTextStyle(
                    this.innerAngleTextIndicator,
                    this.styleSet.innerAngleIndicatorStyle
                  );
                }
                if (angleToDisplay > 0 && angleToDisplay < 90) {
                  let midDirectionX = (directionX + previousDirectionX) / 2;
                  let midDirectionY = (directionY + previousDirectionY) / 2;
                  const midDirectionLength = Math.sqrt(
                    midDirectionX * midDirectionX +
                      midDirectionY * midDirectionY
                  );
                  midDirectionX /= midDirectionLength;
                  midDirectionY /= midDirectionLength;
                  if (this.outerAngleIndicator === undefined) {
                    this.outerAngleIndicator = new paper.Path();
                  } else {
                    this.outerAngleIndicator.removeSegments();
                  }
                  this.outerAngleIndicator.moveTo(
                    beforeLastPoint.x +
                      previousDirectionX * innerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y +
                      previousDirectionY * innerRing +
                      previousSideDirectionY * separation
                  );
                  this.outerAngleIndicator.lineTo(
                    beforeLastPoint.x +
                      previousDirectionX * outerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y +
                      previousDirectionY * outerRing +
                      previousSideDirectionY * separation
                  );
                  this.outerAngleIndicator.arcTo(
                    beforeLastPoint.x + midDirectionX * outerRing,
                    beforeLastPoint.y + midDirectionY * outerRing,
                    beforeLastPoint.x +
                      directionX * outerRing -
                      sideDirectionX * separation,
                    beforeLastPoint.y +
                      directionY * outerRing -
                      sideDirectionY * separation
                  );
                  this.outerAngleIndicator.lineTo(
                    beforeLastPoint.x +
                      directionX * innerRing -
                      sideDirectionX * separation,
                    beforeLastPoint.y +
                      directionY * innerRing -
                      sideDirectionY * separation
                  );
                  this.outerAngleIndicator.arcTo(
                    beforeLastPoint.x + midDirectionX * innerRing,
                    beforeLastPoint.y + midDirectionY * innerRing,
                    beforeLastPoint.x +
                      previousDirectionX * innerRing +
                      previousSideDirectionX * separation,
                    beforeLastPoint.y +
                      previousDirectionY * innerRing +
                      previousSideDirectionY * separation
                  );
                  this.relocateInMainScopeActiveLayer(this.outerAngleIndicator);
                  this.styleSet.outerAngleIndicatorStyle.applyFillTo(
                    this.outerAngleIndicator
                  );
                  let textPosition = new paper.Point(
                    beforeLastPoint.x + (midDirectionX * outerRing) / 2,
                    beforeLastPoint.y + (midDirectionY * outerRing) / 2
                  );
                  this.outerAngleTextIndicator = new paper.PointText(
                    textPosition
                  );
                  this.outerAngleTextIndicator.content = angleToDisplay;
                  let textAngle =
                    (Math.atan2(-midDirectionX, midDirectionY) * 180) / Math.PI;
                  if (textAngle < -90) {
                    textAngle = 180 + textAngle;
                  } else if (textAngle > 90) {
                    textAngle = -180 + textAngle;
                  }
                  this.outerAngleTextIndicator.position = textPosition;
                  this.outerAngleTextIndicator.rotate(textAngle);
                  this.relocateInMainScopeActiveLayer(
                    this.outerAngleTextIndicator
                  );
                  this.applyTextStyle(
                    this.outerAngleTextIndicator,
                    this.styleSet.outerAngleIndicatorStyle
                  );
                }
              }
            }
          }
        }
        if (beforeLastPoint !== undefined) {
          //Draw lines with measurements
          const segmentX = lastPoint.x - beforeLastPoint.x;
          const segmentY = lastPoint.y - beforeLastPoint.y;
          let width = 0;
          if (this.styleSet.riseAndRunIndicatorStyle.width !== undefined) {
            width = this.styleSet.riseAndRunIndicatorStyle.width;
          }
          if (width > 0) {
            let offset = 0;
            if (this.styleSet.riseAndRunIndicatorStyle.offset !== undefined) {
              offset = this.styleSet.riseAndRunIndicatorStyle.offset;
            }
            if (forCurve) {
              //TODO: remove?
              if (drawingHeight !== undefined) {
                const segmentLength = Math.sqrt(
                  segmentX * segmentX + segmentY * segmentY
                );
                if (segmentLength > 1e-5) {
                  const directionX = segmentX / segmentLength;
                  const directionY = segmentY / segmentLength;
                  let sideDirectionX = -directionY;
                  let sideDirectionY = directionX;
                  if (angle > 0) {
                    sideDirectionX = -sideDirectionX;
                    sideDirectionY = -sideDirectionY;
                  }
                  const heightStartX = beforeLastPoint.x - directionX * offset;
                  const heightStartY = beforeLastPoint.y - directionY * offset;
                  const heightFinishX =
                    heightStartX + sideDirectionX * drawingHeight;
                  const heightFinishY =
                    heightStartY + sideDirectionY * drawingHeight;
                  if (this.heightIndicator === undefined) {
                    this.heightIndicator = new paper.Path();
                  } else {
                    this.heightIndicator.removeSegments();
                  }
                  this.heightIndicator.moveTo(heightStartX, heightStartY);
                  this.heightIndicator.lineTo(
                    heightStartX -
                      directionX * 1.41 * width +
                      sideDirectionX * 3 * width,
                    heightStartY -
                      directionY * 1.41 * width +
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightStartX -
                      (directionX * width) / 2 +
                      sideDirectionX * 3 * width,
                    heightStartY -
                      (directionY * width) / 2 +
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightFinishX -
                      (directionX * width) / 2 -
                      sideDirectionX * 3 * width,
                    heightFinishY -
                      (directionY * width) / 2 -
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightFinishX -
                      directionX * 1.41 * width -
                      sideDirectionX * 3 * width,
                    heightFinishY -
                      directionY * 1.41 * width -
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(heightFinishX, heightFinishY);
                  this.heightIndicator.lineTo(
                    heightFinishX +
                      directionX * 1.41 * width -
                      sideDirectionX * 3 * width,
                    heightFinishY +
                      directionY * 1.41 * width -
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightFinishX +
                      (directionX * width) / 2 -
                      sideDirectionX * 3 * width,
                    heightFinishY +
                      (directionY * width) / 2 -
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightStartX +
                      (directionX * width) / 2 +
                      sideDirectionX * 3 * width,
                    heightStartY +
                      (directionY * width) / 2 +
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.lineTo(
                    heightStartX +
                      directionX * 1.41 * width +
                      sideDirectionX * 3 * width,
                    heightStartY +
                      directionY * 1.41 * width +
                      sideDirectionY * 3 * width
                  );
                  this.heightIndicator.closed = true;
                  this.relocateInMainScopeActiveLayer(this.heightIndicator);
                  this.styleSet.riseAndRunIndicatorStyle.applyFillTo(
                    this.heightIndicator
                  );
                  let textPositionX = (heightStartX + heightFinishX) / 2;
                  let textPositionY = (heightStartY + heightFinishY) / 2;
                  let textPosition = new paper.Point(
                    textPositionX,
                    textPositionY
                  );
                  let rectangle = new paper.Rectangle(
                    textPositionX,
                    textPositionY,
                    1,
                    1
                  );
                  if (this.heightIndicatorBackground === undefined) {
                    this.heightIndicatorBackground = new paper.Path.Rectangle(
                      rectangle
                    );
                    this.heightIndicatorBackground.fillColor = "white";
                  }
                  if (this.heightIndicatorText === undefined) {
                    this.heightIndicatorText = new paper.PointText(
                      textPosition
                    );
                  }
                  SketchUtils.drawTextWithBackground(
                    this,
                    this.heightIndicatorText,
                    textPosition,
                    height,
                    this.heightIndicatorBackground,
                    this.styleSet.riseAndRunIndicatorStyle
                  );
                  const chordStartX =
                    beforeLastPoint.x - sideDirectionX * offset;
                  const chordStartY =
                    beforeLastPoint.y - sideDirectionY * offset;
                  const chordFinishX = lastPoint.x - sideDirectionX * offset;
                  const chordFinishY = lastPoint.y - sideDirectionY * offset;
                  if (this.chordIndicator === undefined) {
                    this.chordIndicator = new paper.Path();
                  } else {
                    this.chordIndicator.removeSegments();
                  }
                  this.chordIndicator.moveTo(chordStartX, chordStartY);
                  this.chordIndicator.lineTo(
                    chordStartX -
                      sideDirectionX * 1.41 * width +
                      directionX * 3 * width,
                    chordStartY -
                      sideDirectionY * 1.41 * width +
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordStartX -
                      (sideDirectionX * width) / 2 +
                      directionX * 3 * width,
                    chordStartY -
                      (sideDirectionY * width) / 2 +
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordFinishX -
                      (sideDirectionX * width) / 2 -
                      directionX * 3 * width,
                    chordFinishY -
                      (sideDirectionY * width) / 2 -
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordFinishX -
                      sideDirectionX * 1.41 * width -
                      directionX * 3 * width,
                    chordFinishY -
                      sideDirectionY * 1.41 * width -
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(chordFinishX, chordFinishY);
                  this.chordIndicator.lineTo(
                    chordFinishX +
                      sideDirectionX * 1.41 * width -
                      directionX * 3 * width,
                    chordFinishY +
                      sideDirectionY * 1.41 * width -
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordFinishX +
                      (sideDirectionX * width) / 2 -
                      directionX * 3 * width,
                    chordFinishY +
                      (sideDirectionY * width) / 2 -
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordStartX +
                      (sideDirectionX * width) / 2 +
                      directionX * 3 * width,
                    chordStartY +
                      (sideDirectionY * width) / 2 +
                      directionY * 3 * width
                  );
                  this.chordIndicator.lineTo(
                    chordStartX +
                      sideDirectionX * 1.41 * width +
                      directionX * 3 * width,
                    chordStartY +
                      sideDirectionY * 1.41 * width +
                      directionY * 3 * width
                  );
                  this.chordIndicator.closed = true;
                  this.relocateInMainScopeActiveLayer(this.chordIndicator);
                  this.styleSet.riseAndRunIndicatorStyle.applyFillTo(
                    this.chordIndicator
                  );
                  textPositionX = (chordStartX + chordFinishX) / 2;
                  textPositionY = (chordStartY + chordFinishY) / 2;
                  textPosition = new paper.Point(textPositionX, textPositionY);
                  rectangle = new paper.Rectangle(
                    textPositionX,
                    textPositionY,
                    1,
                    1
                  );
                  if (this.chordIndicatorBackground === undefined) {
                    this.chordIndicatorBackground = new paper.Path.Rectangle(
                      rectangle
                    );
                    this.chordIndicatorBackground.fillColor = "white";
                  }
                  if (this.chordIndicatorText === undefined) {
                    this.chordIndicatorText = new paper.PointText(textPosition);
                  }
                  SketchUtils.drawTextWithBackground(
                    this,
                    this.chordIndicatorText,
                    textPosition,
                    chord,
                    this.chordIndicatorBackground,
                    this.styleSet.riseAndRunIndicatorStyle
                  );
                }
              }
            } else if (segmentX !== 0 || segmentY !== 0) {
              let riseStartX = undefined;
              let riseStartY = undefined;
              let riseFinishX = undefined;
              let riseFinishY = undefined;
              let riseDirection = undefined;
              let runStartX = undefined;
              let runStartY = undefined;
              let runFinishX = undefined;
              let runFinishY = undefined;
              let runDirection = undefined;
              if (path.fillPath === undefined || path.fillPath.clockwise) {
                if (segmentX > 0) {
                  if (segmentY > 0) {
                    riseStartX = lastPoint.x + offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = lastPoint.x + offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = 1;
                    runStartX = beforeLastPoint.x;
                    runStartY = beforeLastPoint.y - offset;
                    runFinishX = lastPoint.x;
                    runFinishY = beforeLastPoint.y - offset;
                    runDirection = 1;
                  } else {
                    riseStartX = beforeLastPoint.x - offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = beforeLastPoint.x - offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = -1;
                    runStartX = beforeLastPoint.x;
                    runStartY = lastPoint.y - offset;
                    runFinishX = lastPoint.x;
                    runFinishY = lastPoint.y - offset;
                    runDirection = 1;
                  }
                } else {
                  if (segmentY > 0) {
                    riseStartX = beforeLastPoint.x + offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = beforeLastPoint.x + offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = 1;
                    runStartX = beforeLastPoint.x;
                    runStartY = lastPoint.y + offset;
                    runFinishX = lastPoint.x;
                    runFinishY = lastPoint.y + offset;
                    runDirection = -1;
                  } else {
                    riseStartX = lastPoint.x - offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = lastPoint.x - offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = -1;
                    runStartX = beforeLastPoint.x;
                    runStartY = beforeLastPoint.y + offset;
                    runFinishX = lastPoint.x;
                    runFinishY = beforeLastPoint.y + offset;
                    runDirection = -1;
                  }
                }
              } else {
                if (segmentX > 0) {
                  if (segmentY > 0) {
                    riseStartX = beforeLastPoint.x - offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = beforeLastPoint.x - offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = 1;
                    runStartX = beforeLastPoint.x;
                    runStartY = lastPoint.y + offset;
                    runFinishX = lastPoint.x;
                    runFinishY = lastPoint.y + offset;
                    runDirection = 1;
                  } else {
                    riseStartX = lastPoint.x + offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = lastPoint.x + offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = -1;
                    runStartX = beforeLastPoint.x;
                    runStartY = beforeLastPoint.y + offset;
                    runFinishX = lastPoint.x;
                    runFinishY = beforeLastPoint.y + offset;
                    runDirection = 1;
                  }
                } else {
                  if (segmentY > 0) {
                    riseStartX = lastPoint.x - offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = lastPoint.x - offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = 1;
                    runStartX = beforeLastPoint.x;
                    runStartY = beforeLastPoint.y - offset;
                    runFinishX = lastPoint.x;
                    runFinishY = beforeLastPoint.y - offset;
                    runDirection = -1;
                  } else {
                    riseStartX = beforeLastPoint.x + offset;
                    riseStartY = beforeLastPoint.y;
                    riseFinishX = beforeLastPoint.x + offset;
                    riseFinishY = lastPoint.y;
                    riseDirection = -1;
                    runStartX = beforeLastPoint.x;
                    runStartY = lastPoint.y - offset;
                    runFinishX = lastPoint.x;
                    runFinishY = lastPoint.y - offset;
                    runDirection = -1;
                  }
                }
              }
              if (runDirection !== undefined && riseDirection !== undefined) {
                if (this.riseIndicator === undefined) {
                  this.riseIndicator = new paper.Path();
                } else {
                  this.riseIndicator.removeSegments();
                }
                this.riseIndicator.moveTo(riseStartX, riseStartY);
                this.riseIndicator.lineTo(
                  riseStartX - 1.41 * width,
                  riseStartY + riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseStartX - width / 2,
                  riseStartY + riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseFinishX - width / 2,
                  riseFinishY - riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseFinishX - 1.41 * width,
                  riseFinishY - riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(riseFinishX, riseFinishY);
                this.riseIndicator.lineTo(
                  riseFinishX + 1.41 * width,
                  riseFinishY - riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseFinishX + width / 2,
                  riseFinishY - riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseStartX + width / 2,
                  riseStartY + riseDirection * 3 * width
                );
                this.riseIndicator.lineTo(
                  riseStartX + 1.41 * width,
                  riseStartY + riseDirection * 3 * width
                );
                this.riseIndicator.closed = true;
                this.relocateInMainScopeActiveLayer(this.riseIndicator);
                this.styleSet.riseAndRunIndicatorStyle.applyFillTo(
                  this.riseIndicator
                );
                let textPositionX = (riseStartX + riseFinishX) / 2;
                let textPositionY = (riseStartY + riseFinishY) / 2;
                let textPosition = new paper.Point(
                  textPositionX,
                  textPositionY
                );
                let rectangle = new paper.Rectangle(
                  textPositionX,
                  textPositionY,
                  1,
                  1
                );
                if (this.riseIndicatorBackground === undefined) {
                  this.riseIndicatorBackground = new paper.Path.Rectangle(
                    rectangle
                  );
                  this.riseIndicatorBackground.fillColor = "white";
                }
                if (this.riseIndicatorText === undefined) {
                  this.riseIndicatorText = new paper.PointText(textPosition);
                }
                SketchUtils.drawTextWithBackground(
                  this,
                  this.riseIndicatorText,
                  textPosition,
                  Math.abs(rise),
                  this.riseIndicatorBackground,
                  this.styleSet.riseAndRunIndicatorStyle
                );
                document.getElementById("lineRiseFeet").value = Math.round(
                  rise
                );
                document.getElementById("lineRiseInches").value = 0;
                if (this.runIndicator === undefined) {
                  this.runIndicator = new paper.Path();
                } else {
                  this.runIndicator.removeSegments();
                }
                this.runIndicator.moveTo(runStartX, runStartY);
                this.runIndicator.lineTo(
                  runStartX + runDirection * 3 * width,
                  runStartY - 1.41 * width
                );
                this.runIndicator.lineTo(
                  runStartX + runDirection * 3 * width,
                  runStartY - width / 2
                );
                this.runIndicator.lineTo(
                  runFinishX - runDirection * 3 * width,
                  runFinishY - width / 2
                );
                this.runIndicator.lineTo(
                  runFinishX - runDirection * 3 * width,
                  runFinishY - 1.41 * width
                );
                this.runIndicator.lineTo(runFinishX, runFinishY);
                this.runIndicator.lineTo(
                  runFinishX - runDirection * 3 * width,
                  runFinishY + 1.41 * width
                );
                this.runIndicator.lineTo(
                  runFinishX - runDirection * 3 * width,
                  runFinishY + width / 2
                );
                this.runIndicator.lineTo(
                  runStartX + runDirection * 3 * width,
                  runStartY + width / 2
                );
                this.runIndicator.lineTo(
                  runStartX + runDirection * 3 * width,
                  runStartY + 1.41 * width
                );
                this.runIndicator.closed = true;
                this.relocateInMainScopeActiveLayer(this.runIndicator);
                this.styleSet.riseAndRunIndicatorStyle.applyFillTo(
                  this.runIndicator
                );
                textPositionX = (runStartX + runFinishX) / 2;
                textPositionY = (runStartY + runFinishY) / 2;
                textPosition = new paper.Point(textPositionX, textPositionY);
                rectangle = new paper.Rectangle(
                  textPositionX,
                  textPositionY,
                  1,
                  1
                );
                if (this.runIndicatorBackground === undefined) {
                  this.runIndicatorBackground = new paper.Path.Rectangle(
                    rectangle
                  );
                  this.runIndicatorBackground.fillColor = "white";
                }
                if (this.runIndicatorText === undefined) {
                  this.runIndicatorText = new paper.PointText(textPosition);
                }
                SketchUtils.drawTextWithBackground(
                  this,
                  this.runIndicatorText,
                  textPosition,
                  Math.abs(run),
                  this.runIndicatorBackground,
                  this.styleSet.riseAndRunIndicatorStyle
                );
                document.getElementById("lineRunFeet").value = Math.round(run);
                document.getElementById("lineRunInches").value = 0;
                textPositionY = (riseStartY + riseFinishY) / 2;
                textPosition = new paper.Point(textPositionX, textPositionY);
                this.lengthIndicatorBackground = new paper.Path.Rectangle(
                  rectangle
                );
                this.lengthIndicatorBackground.fillColor = "white";
                this.lengthIndicatorText = new paper.PointText(textPosition);
                rectangle = new paper.Rectangle(
                  textPositionX,
                  textPositionY,
                  1,
                  1
                );
                const length = Math.sqrt(rise * rise + run * run);
                SketchUtils.drawTextWithBackground(
                  this,
                  this.lengthIndicatorText,
                  textPosition,
                  length,
                  this.lengthIndicatorBackground,
                  this.styleSet.riseAndRunIndicatorStyle
                );
                const lengthToUse = SketchUtils.adjustAmount(length);
                const feet = Math.floor(lengthToUse);
                const inches = Math.floor((lengthToUse - feet) * 12);
                document.getElementById("lineLengthFeet").value = Math.round(
                  feet
                );
                document.getElementById("lineLengthInches").value = Math.round(
                  inches
                );
              }
            }
          }
        }
      }
      if (this.cursor !== undefined || this.penUpCursor !== undefined) {
        const position = this.topLevelTransform.transform(
          new paper.Point(
            this.cursor ? this.cursor.x : this.penUpCursor.x,
            this.cursor ? this.cursor.y : this.penUpCursor.y
          )
        );
        if (!this.cursor) {
          this.cursor = { x: this.penUpCursor.x, y: this.penUpCursor.y };
        }
        if (
          this.penDown &&
          this.sketchPathToEdit !== undefined &&
          this.editing === undefined
        ) {
          let lastPoint = undefined;
          if (
            this.sketchPathToEdit.elements &&
            this.sketchPathToEdit.elements.length > 0
          ) {
            const element = this.sketchPathToEdit.elements[
              this.sketchPathToEdit.elements.length - 1
            ];
            lastPoint = this.topLevelTransform.transform(
              new paper.Point(element.x, element.y)
            );
          } else {
            const start = this.sketchPathToEdit.start;
            lastPoint = this.topLevelTransform.transform(
              new paper.Point(start.x, start.y)
            );
          }
          if (lastPoint !== undefined) {
            if (this.lineToPen === undefined) {
              this.lineToPen = new paper.Path();
            } else {
              this.lineToPen.removeSegments();
            }
            this.lineToPen.moveTo(lastPoint.x, lastPoint.y);
            this.lineToPen.lineTo(position.x, position.y);
            this.relocateInMainScopeActiveLayer(this.lineToPen);
            this.styleSet.lineToPenStyle.applyStrokeTo(this.lineToPen);
            this.lineToPen.sendToBack();
          }
        }
        if (
          this.penDown &&
          this.penDownIconWidth !== undefined &&
          this.penDownIconHeight !== undefined
        ) {
          if (!this.penDownPath) {
            this.penDownPath = new paper.CompoundPath();
            if (this.movingPenCursor == "stopped") {
              this.movingPenCursor = "moving";
              this.interaction.down(
                this,
                {
                  offsetX: this.penUpCursor.x,
                  offsetY: this.penUpCursor.y
                },
                true
              );
            }
          } else {
            this.penDownPath.removeChildren();
            if (this.movingPenCursor == "stopped") {
              this.movingPenCursor = "moving";
              this.interaction.down(
                this,
                {
                  offsetX: this.penUpCursor.x,
                  offsetY: this.penUpCursor.y
                },
                true
              );
            }
          }
          let leftLine = new paper.Path();
          leftLine.moveTo(position.x, position.y);
          leftLine.lineTo(position.x - this.penDownIconWidth, position.y);
          this.styleSet.penDownCursorStyle.applyStrokeTo(leftLine);
          this.penDownPath.addChild(leftLine);
          let rightLine = new paper.Path();
          rightLine.moveTo(position.x, position.y);
          rightLine.lineTo(position.x + this.penDownIconWidth, position.y);
          this.styleSet.penDownCursorStyle.applyStrokeTo(rightLine);
          this.penDownPath.addChild(rightLine);
          let downLine = new paper.Path();
          downLine.moveTo(position.x, position.y);
          downLine.lineTo(position.x, position.y + this.penDownIconHeight);
          this.styleSet.penDownCursorStyle.applyStrokeTo(downLine);
          this.penDownPath.addChild(downLine);
          let upLine = new paper.Path();
          upLine.moveTo(position.x, position.y);
          upLine.lineTo(position.x, position.y - this.penDownIconHeight);
          this.styleSet.penDownCursorStyle.applyStrokeTo(upLine);
          this.penDownPath.addChild(upLine);

          this.relocateInMainScopeActiveLayer(leftLine);
          this.relocateInMainScopeActiveLayer(rightLine);
          this.relocateInMainScopeActiveLayer(downLine);
          this.relocateInMainScopeActiveLayer(upLine);
          this.relocateInMainScopeActiveLayer(this.penDownPath);

          let previousPoint = undefined;
          if (this.sketchPathToEdit) {
            if (this.isDown) {
              if (
                this.sketchPathToEdit.elements &&
                this.sketchPathToEdit.elements.length > 1
              ) {
                const element = this.sketchPathToEdit.elements[
                  this.sketchPathToEdit.elements.length - 2
                ];
                previousPoint = this.topLevelTransform.transform(
                  new paper.Point(element.x, element.y)
                );
              } else if (this.sketchPathToEdit.start !== undefined) {
                let start = this.sketchPathToEdit.start;
                previousPoint = this.topLevelTransform.transform(
                  new paper.Point(start.x, start.y)
                );
              }
            } else {
              if (
                this.sketchPathToEdit.elements &&
                this.sketchPathToEdit.elements.length > 0
              ) {
                const element = this.sketchPathToEdit.elements[
                  this.sketchPathToEdit.elements.length - 1
                ];
                previousPoint = this.topLevelTransform.transform(
                  new paper.Point(element.x, element.y)
                );
              } else if (this.sketchPathToEdit.start) {
                let start = this.sketchPathToEdit.start;
                previousPoint = this.topLevelTransform.transform(
                  new paper.Point(start.x, start.y)
                );
              }
            }
          }
          if (previousPoint && this.isDown) {
            const layerName = this.getLayer(this.sketchLayerToEdit).name;
            let layerStyle = undefined;
            if (this.layerList[layerName] && this.layerList[layerName].style) {
              layerStyle = this.layerList[layerName].style;
            } else {
              layerStyle = {
                invertedStrokeColor: "#FFFFFF"
              };
            }
            //Invert stroke color if cursor overlaps drawing line
            if (previousPoint.x === position.x) {
              if (position.y <= previousPoint.y) {
                downLine.strokeColor = layerStyle.invertedStrokeColor;
                downLine.strokeWidth = 1.5;
              } else if (position.y > previousPoint.y) {
                upLine.strokeColor = layerStyle.invertedStrokeColor;
                upLine.strokeWidth = 1.5;
              }
            } else if (previousPoint.y === position.y) {
              if (position.x < previousPoint.x) {
                rightLine.strokeColor = layerStyle.invertedStrokeColor;
                rightLine.strokeWidth = 1.5;
              } else if (position.x > previousPoint.x) {
                leftLine.strokeColor = layerStyle.invertedStrokeColor;
                leftLine.strokeWidth = 1.5;
              }
            }
          }
        } else if (this.penUpIconWidth && this.penUpIconHeight) {
          if (this.movingPenCursor !== "stopped") {
            if (!this.penUpPath) {
              this.penUpPath = new paper.CompoundPath();
            } else {
              this.penUpPath.removeChildren();
            }
            this.penUpPath.moveTo(position.x, position.y);
            this.penUpPath.lineTo(position.x - this.penUpIconWidth, position.y);
            this.penUpPath.moveTo(position.x, position.y);
            this.penUpPath.lineTo(position.x + this.penUpIconWidth, position.y);
            this.penUpPath.moveTo(position.x, position.y);
            this.penUpPath.lineTo(
              position.x,
              position.y + this.penUpIconHeight
            );
            this.penUpPath.moveTo(position.x, position.y);
            this.penUpPath.lineTo(
              position.x,
              position.y - this.penUpIconHeight
            );
            this.penUpPath.moveTo(position.x, position.y);
            this.penUpPath.closed = false;

            if (this.movingPenCursor == "stopping") {
              this.movingPenCursor = "stopped";
            }
          }
          this.styleSet.penUpCursorStyle.applyStrokeTo(this.penUpPath);
          this.relocateInMainScopeActiveLayer(this.penUpPath);
        }
        if (this.penDown && path && path.elements.length > 1) {
          this.jumpToCoordinates = undefined;
          this.jumpToIndicatorPointCount = 0;
          let x = path.start.x;
          let y = path.start.y;
          let xPoints = [x];
          let yPoints = [y];
          const lastPoint = {
            x: path.start.x,
            y: path.start.y
          };
          if (path.elements && path.elements.length >= 2) {
            lastPoint.x = path.elements[path.elements.length - 2].x;
            lastPoint.y = path.elements[path.elements.length - 2].y;
          }
          for (let i = 0; i < path.elements.length; i++) {
            x = path.elements[i].x;
            y = path.elements[i].y;
            xPoints.push(x);
            yPoints.push(y);
          }
          let topLeft = true;
          let topRight = true;
          let bottomLeft = true;
          let bottomRight = true;
          const minX = Math.min(...xPoints);
          const maxX = Math.max(...xPoints);
          const minY = Math.min(...yPoints);
          const maxY = Math.max(...yPoints);
          const currentPoint = {
            x: xPoints[xPoints.length - 1],
            y: yPoints[yPoints.length - 1]
          };
          for (let i = 0; i < xPoints.length; i++) {
            x = xPoints[i];
            y = yPoints[i];
            if (x == minX) {
              if (y == maxY) {
                topLeft = false;
              } else if (y == minY) {
                bottomLeft = false;
              }
            } else if (x == maxX) {
              if (y == maxY) {
                topRight = false;
              } else if (y == minY) {
                bottomRight = false;
              }
            }
            /*if (this.cursor && (y !== currentPoint.y || x !== currentPoint.x)) {
              if (
                (lastPoint.x != this.cursor.x || lastPoint.y != y) &&
                (currentPoint.y !== lastPoint.y || currentPoint.y !== y)
              ) {
                if (
                  (y !== currentPoint.y || minX !== currentPoint.x) &&
                  (minX !== lastPoint.x || y !== lastPoint.y)
                ) {
                  this.addJumpToIndicatorPoints(
                    minX,
                    y,
                    path,
                    y == this.cursor.y && this.cursor.x == minX
                  );
                }
                if (
                  (y !== currentPoint.y || maxX !== currentPoint.x) &&
                  (maxX !== lastPoint.x || y !== lastPoint.y)
                ) {
                  this.addJumpToIndicatorPoints(
                    maxX,
                    y,
                    path,
                    y == this.cursor.y && this.cursor.x == maxX
                  );
                }
              }
              if (
                (lastPoint.x != x || lastPoint.y != this.cursor.y) &&
                (currentPoint.x !== lastPoint.x || currentPoint.x !== x)
              ) {
                if (
                  (x !== currentPoint.x || minY !== currentPoint.y) &&
                  (minY !== lastPoint.y || x !== lastPoint.x)
                ) {
                  this.addJumpToIndicatorPoints(
                    x,
                    minY,
                    path,
                    x == this.cursor.x && this.cursor.y == minY
                  );
                }
                if (
                  (x !== currentPoint.x || maxY !== currentPoint.y) &&
                  (maxY !== lastPoint.y || x !== lastPoint.x) &&
                  lastPoint.y !== currentPoint.y
                ) {
                  this.addJumpToIndicatorPoints(
                    x,
                    maxY,
                    path,
                    x == this.cursor.x && this.cursor.y == maxY
                  );
                }
              }
            }*/
          }

          /*if (topLeft)
            this.addJumpToIndicatorPoints(
              minX,
              maxY,
              path,
              this.cursor.x == minX && this.cursor.y == maxY
            );
          if (topRight)
            this.addJumpToIndicatorPoints(
              maxX,
              maxY,
              path,
              this.cursor.x == maxX && this.cursor.y == maxY
            );
          if (bottomLeft)
            this.addJumpToIndicatorPoints(
              minX,
              minY,
              path,
              this.cursor.x == minX && this.cursor.y == minY
            );
          if (bottomRight)
            this.addJumpToIndicatorPoints(
              maxX,
              minY,
              path,
              this.cursor.x == maxX && this.cursor.y == minY
            );*/
          const last = path.elements[path.elements.length - 1];
          let overlap = false;
          if (
            path.start.x == path.elements[0].x &&
            ((path.start.y <= last.y && last.y <= path.elements[0].y) ||
              (path.start.y >= last.y && last.y >= path.elements[0].y))
          ) {
            overlap = true;
          } else {
            for (let i = 1; i < path.elements.length; i++) {
              if (
                (path.start.x == path.elements[i - 1].x &&
                  path.elements[i - 1].x == path.elements[i].x &&
                  ((path.start.y <= path.elements[i - 1].y &&
                    path.start.y >= path.elements[i].y) ||
                    (path.start.y >= path.elements[i - 1].y &&
                      path.start.y <= path.elements[i].y))) ||
                (last.y == path.elements[i - 1].y &&
                  last.y == path.elements[i].y &&
                  ((path.elements[i - 1].x <= path.start.x &&
                    path.start.x <= path.elements[i].x) ||
                    (path.elements[i - 1].x >= path.start.x &&
                      path.start.x >= path.elements[i].x)))
              ) {
                overlap = true;
                break;
              }
            }
          }
          if (overlap) {
            overlap = false;
          } else {
            this.addJumpToIndicatorPoints(path.start.x, last.y);
          }

          if (
            (last.x == path.elements[path.elements.length - 2].x &&
              ((last.y <= path.start.y &&
                path.start.y <= path.elements[path.elements.length - 2].y) ||
                (last.y >= path.start.y &&
                  path.start.y >=
                    path.elements[path.elements.length - 2].y))) ||
            (path.start.y == path.elements[0].y &&
              ((path.start.x <= last.x && last.x <= path.elements[0].x) ||
                (path.start.x >= last.x && last.x >= path.elements[0].x)))
          ) {
            overlap = true;
          } else {
            for (let i = 0; i < path.elements.length - 2; i++) {
              if (
                last.x == path.elements[i].x &&
                path.elements[i].x == path.elements[i + 1].x &&
                ((last.y <= path.elements[i].y &&
                  last.y >= path.elements[i + 1].y) ||
                  (last.y >= path.elements[i].y &&
                    last.y <= path.elements[i + 1].y))
              ) {
                overlap = true;
                break;
              }
            }
          }
          if (!overlap) {
            this.addJumpToIndicatorPoints(last.x, path.start.y);
          }

          this.addJumpToIndicatorPoints(path.start.x, path.start.y);
        }
      }
    }
    paper.project.activeLayer.removeChildren(this.indexInMainScopeActiveLayer);
    paper.view.draw();
    this.offsetChangedForDraw = false;
    this.resolutionChangedForDraw = false;
    this.bearingChangedForDraw = false;
  }

  /**
   * Toggles the Pen cursor to whether follow the mouse cursor.
   *
   * @param {boolean} stop
   * @returns {void}
   *
   */
  stopPenCursor(stop = true) {
    if (stop) this.movingPenCursor = "stopping";
    else this.movingPenCursor = "moving";
  }

  /**
   * Gets the Path candidates that converge with the cursor position.
   *
   * @param {number} positionX
   * @param {number} positionY
   * @param {Array} candidates - Converging Paths found
   * @param {boolean} pathLevelOnly
   * @param {boolean} skipInsidePath
   * @returns {void}
   *
   */
  findPathSelectionCandidates(
    positionX,
    positionY,
    candidates,
    pathLevelOnly,
    skipInsidePath
  ) {
    if (this.layers) {
      for (let i = 0; i < this.layers.length; i++) {
        this.layers[i].findPathSelectionCandidates(
          positionX,
          positionY,
          this.topLevelTransform,
          candidates,
          pathLevelOnly,
          skipInsidePath
        );
      }
    }
  }

  /**
   * Pushes all visible objects into the selected array.
   *
   * @returns {void}
   *
   */
  selectAll() {
    if (this.layers) {
      this.selected = [];
      this.layers.map(layer => {
        if (layer.objects && layer.objects.length > 0) {
          layer.objects.map(path => {
            if (path.objects && path.objects.length > 0) {
              if (path.objects[0].start) {
                this.selected.push({
                  layer: layer,
                  path: path.objects[0],
                  sketchObject: path
                });
              } else if (path.objects[0].customText) {
                this.selected.push({
                  layer: layer,
                  textObject: path.objects[0],
                  sketchObject: path
                });
              }
            }
          });
        }
      });
    }
    this.draw();
  }

  /**
   * Selects the contents of the specified Layer.
   *
   * @param {SketchLayer} layer
   * @returns {void}
   *
   */
  selectLayer(layer) {
    if (layer && layer.objects && layer.objects.length > 0) {
      if (!this.selected) this.selected = [];
      layer.objects.map(path => {
        if (path.objects && path.objects.length > 0) {
          if (path.objects[0].start)
            this.selected.push({
              layer: layer,
              path: path.objects[0],
              sketchObject: path
            });
          else
            this.selected.push({
              layer: layer,
              textObject: path.objects[0],
              sketchObject: path
            });
        }
      });
    }
  }

  /**
   * Gets the Path candidates that converge with the cursor position.
   *
   * @param {SketchControl} sketch - Control instance
   * @param {number} positionX - Number stating the horizontal position
   * @param {number} positionY - Number stating the vertical position
   * @param {Array} candidates - array of converging Paths
   * @returns {void}
   *
   */
  findTextSelectionCandidates(sketch, positionX, positionY, candidates) {
    if (this.layers !== undefined) {
      for (let i = 0; i < this.layers.length; i++) {
        this.layers[i].findTextSelectionCandidates(
          sketch,
          positionX,
          positionY,
          this.topLevelTransform,
          candidates
        );
      }
    }
  }

  /**
   * Deselects all objects.
   *
   * @returns {void}
   *
   */
  clearSelection() {
    this.selected = undefined;
    this.overrideSelection = true;
  }

  /**
   * Deselects text input.
   *
   * @returns {void}
   *
   */
  dismissTextInput() {
    if (this.textInputElement !== undefined) {
      this.textInputElement.style.visibility = "collapse";
      this.textInputElement.blur();
      this.setTextsVisible();
    }
  }

  /**
   * Saves the current sketch and creates a History step.
   *
   * @returns {void}
   *
   */
  async autoSave() {
    if (this.sketchEntity) {
      this.setAreaTotals();
      this.draw();
      if (this.sketchEntity["ptas_isofficial"]) {
        if (this.draftData) {
          if (this.draftData.draftTemplateId !== this.sketchEntityId) {
            document.getElementById(
              "sketchSelectionContainer"
            ).style.visibility = "visible";
          } else {
            loadSketch(this.draftData.draftId);
          }
        } else {
          showLoading("Creating draft...");
          const draftTemplateId = this.sketchEntityId;
          SketchEntitiesHandler.createDraftSketch(this)
            .then(createDraftRes => {
              SketchAPIService.getSketch(
                this.sketchEntityId,
                this.sketchAccessToken,
                this.relatedEntityId,
                this.relatedEntityName
              ).then(sketchRes => {
                this.clear();
                SketchFromJSON.read(sketchRes.sketch, this);

                if (sketchRes.draftId) {
                  this.draftData = {
                    draftId: sketchRes.draftId,
                    draftLocked: sketchRes.draftLocked,
                    draftLockedBy: sketchRes.draftLockedBy,
                    draftTemplateId: draftTemplateId
                  };
                } else {
                  this.draftData = null;
                }

                sketchRes.items.forEach(item => {
                  if (item.entityName != "ptas_sketch") {
                    parent.sketchControl.relatedEntity = item.changes;
                    let entity = parent.sketchControl.sketchEntity;
                    let topCenter = document.getElementById(
                      "currentSketchInfo"
                    );
                    let fileInfo = document.getElementById("file-info");
                    let date = getLocalDateString(entity.ptas_drawdate);
                    topCenter.innerHTML = fileInfo.innerHTML = `
                    <div>${parent.sketchControl.parcelData.parcelName}</div>
                    <div>${item.changes.ptas_name}</div>
                    <div id=top-line3>${
                      entity.ptas_isofficial
                        ? "Official"
                        : entity.ptas_iscomplete
                        ? "V. " + entity.ptas_version
                        : "Draft"
                    }, ${date}</div>`;
                  }
                });

                let projection = { center: {}, projected: [], index: 0 };
                this.findProjection(projection);
                if (projection.projected.length > 0) {
                  this.zoomToContents(projection.bounds, 0.7);
                }
                addScratchpadLayer();
                if (this.layers && this.layers.length > 0) {
                  this.updateSavedLayers();
                  this.updateSavedTotalsInfo(this.savedLayers);
                }
                hideLoading();
              });
            })
            .catch(err => {
              console.error(err);
              showLoading(err);
              return;
            });
        }
      } else if (this.sketchEntity["ptas_iscomplete"]) {
        if (this.draftData) {
          if (this.draftData.draftTemplateId !== this.sketchEntityId) {
            document.getElementById(
              "sketchSelectionContainer"
            ).style.visibility = "visible";
          } else {
            loadSketch(this.draftData.draftId);
          }
        } else {
          loadSketch(this.sketchEntityId, true);
        }
      } else {
        return SketchEntitiesHandler.autoSaveSketch(
          this,
          SketchToJSON.write(this)
        )
          .then(saveRes => {
            if (!saveRes) {
              showDialog("Error while auto-saving sketch");
            }
            if (this.layers && this.layers.length > 0) {
              this.updateSavedLayers();
              this.updateSavedTotalsInfo(this.savedLayers);
            }
          })
          .catch(err => {
            console.error(err);
            showLoading(err);
            return;
          });
      }
    }
  }

  /**
   * Save button event handler.
   *
   * @returns {void}
   *
   */
  doSaveSketch() {
    /*
       The save on the Save screen contains multiple scenarios for data:
       - Doing a Save on a current Completed sketch
        - If the Sketch is loaded on a completed sketch and no edits happen, the Save 
            should just call SketchEntitiesHandler.updateSketchEntityOnly to update the entity,
            this is the scenario of updating only the tags.
        - If the current completed (officials should not be able to turn off the switch)
            sketch is marked as official, call SketchEntitiesHandler.promoteCompletedToOfficial.
            This is true also when the user select the complete sketch from the versions and click
            on the Official Link.
       - Doing a Save on a current Draft
        - If no Official switch just call SketchEntitiesHandler.autoSaveSketch to update the
            entity and sketch.
        - If Official switch is on, call SketchEntitiesHandler.promoteDraftToOfficial this will promote the current 
            draft to the official version.
            This is true also when the user select the draft sketch from the versions and click
            on the Official Link.
    */

    showLoading("Saving...");
    this.setAreaTotals();
    if (this.sketchEntity["ptas_iscomplete"]) {
      if (!this.sketchIsOfficial && this.sketchEntity["ptas_isofficial"]) {
        SketchEntitiesHandler.promoteDraftToOfficial(
          this,
          this.sketchEntity,
          SketchToJSON.write(this)
        )
          .then(promoteRes => {
            if (promoteRes) {
              let modal = document.getElementById("saveModal");
              modal.style.visibility = "collapse";
              loadTotalsInfo("updateModal");
              let updateModal = document.getElementById("updateModal");
              updateModal.style.visibility = "inherit";

              if (this.layers && this.layers.length > 0) {
                this.updateSavedLayers();
                this.updateSavedTotalsInfo(this.savedLayers);
              }

              //Send to hand mode, so this official sketch cannot be edited
              panClicked();
              hideLoading();
            } else {
              hideLoading();
              showDialog("Error while saving sketch");
            }
          })
          .catch(err => {
            console.error(err);
            showLoading(err);
            return;
          });
      } else {
        SketchEntitiesHandler.updateSketchEntityOnly(
          this.sketchEntityId,
          this.sketchEntity,
          this.sketchAccessToken
        )
          .then(updateRes => {
            if (updateRes) {
              let modal = document.getElementById("saveModal");
              modal.style.visibility = "collapse";
              loadTotalsInfo("updateModal");
              let updateModal = document.getElementById("updateModal");
              updateModal.style.visibility = "inherit";
              hideLoading();
            } else {
              hideLoading();
              showDialog("Error saving entity");
            }
          })
          .catch(err => {
            console.error(err);
            showLoading(err);
            return;
          });
      }
    } else {
      if (this.sketchEntity["ptas_isofficial"]) {
        SketchEntitiesHandler.promoteDraftToOfficial(
          this,
          this.sketchEntity,
          SketchToJSON.write(this)
        )
          .then(promoteRes => {
            if (promoteRes) {
              //Clear draftData if it was the promoted sketch
              if (
                this.draftData &&
                this.draftData.draftId === this.sketchEntityId
              ) {
                this.draftData = null;
              }

              let modal = document.getElementById("saveModal");
              modal.style.visibility = "collapse";
              loadTotalsInfo("updateModal");
              let updateModal = document.getElementById("updateModal");
              updateModal.style.visibility = "inherit";

              if (this.layers && this.layers.length > 0) {
                this.updateSavedLayers();
                this.updateSavedTotalsInfo(this.savedLayers);
              }

              //Send to hand mode, so this official sketch cannot be edited
              panClicked();
              hideLoading();
            } else {
              panClicked();
              hideLoading();
              showDialog("Error while saving sketch");
            }
          })
          .catch(err => {
            console.error(err);
            showLoading(err);
            return;
          });
      } else {
        SketchEntitiesHandler.autoSaveSketch(this, SketchToJSON.write(this))
          .then(saveRes => {
            let modal = document.getElementById("saveModal");
            modal.style.visibility = "collapse";
            if (saveRes) {
              loadTotalsInfo("updateModal");
              let updateModal = document.getElementById("updateModal");
              updateModal.style.visibility = "inherit";

              if (this.layers && this.layers.length > 0) {
                this.updateSavedLayers();
                this.updateSavedTotalsInfo(this.savedLayers);
              }
              hideLoading();
            } else {
              hideLoading();
              showDialog("Error while saving sketch");
            }
          })
          .catch(err => {
            console.error(err);
            showLoading(err);
            return;
          });
      }
    }
    let date = getLocalDateString(new Date().toString());
    document.getElementById("top-line3").innerHTML = `<div id=top-line3>${
      this.sketchEntity.ptas_isofficial
        ? "Official"
        : this.sketchEntity.ptas_iscomplete
        ? "V. " + this.sketchEntity.ptas_version
        : "Draft"
    }, ${date}</div>`;
  }

  /**
   * Saves the current sketch and creates a History step.
   *
   * @returns {void}
   *
   */
  setTextsVisible() {
    for (let i = 0; i < this.layers.length; i++) {
      if (
        this.layers[i].objects !== undefined &&
        this.layers[i].objects.length > 0
      ) {
        for (let j = 0; j < this.layers[i].objects.length; j++) {
          if (
            this.layers[i].objects[j].objects !== undefined &&
            this.layers[i].objects[j].objects.length > 0
          ) {
            for (let k = 0; k < this.layers[i].objects[j].objects.length; k++) {
              if (
                this.layers[i].objects[j].objects[k] instanceof SketchCustomText
              ) {
                this.layers[i].objects[j].objects[k].editingText = false;
                this.layers[i].objects[j].objects[k].texts.forEach(t => {
                  t.visible = true;
                });
              }
            }
          }
        }
      }
    }
  }

  /**
   * Fills up an object with the bounding box positions of a selection.
   *
   * @param {object} bounds - object containing the bounding positions
   * @returns {void}
   *
   */
  getBounds(bounds) {
    if (this.layers !== undefined) {
      for (let i = 0; i < this.layers.length; i++) {
        this.layers[i].getBounds(bounds);
      }
    }
  }

  /**
   * Fills up an object with the bounding box positions of multiple selections.
   *
   * @param {object} bounds - object containing the bounding positions
   * @returns {void}
   *
   */
  getSelectionBounds(bounds) {
    if (this.selected && this.selected.length > 0) {
      for (let i = 0; i < this.selected.length; i++) {
        let selection = this.selected[i];
        if (selection.path) {
          selection.path.getBounds(bounds);
        }
      }
    }
  }

  /**
   * Relocates the current Object instance in Y based on the given projection.
   *
   * @param {object} projection - object containing an array named 'projected' and a number named 'index'
   * @returns {void}
   *
   */
  flipVertically(projection) {
    if (this.layers !== undefined) {
      for (let i = 0; i < this.layers.length; i++) {
        this.layers[i].flipVertically(projection);
      }
    }
  }

  /**
   * Zooms to fit specified bounds.
   *
   * @param {object} bounds - object containing the bounding positions
   * @param {number} coveragePercentage
   * @returns {void}
   *
   */
  zoomToContents(bounds, coveragePercentage) {
    const sketchViewWidth = this.root.clientWidth * coveragePercentage;
    const sketchViewHeight = this.root.clientHeight * coveragePercentage;
    const sketchWidth = bounds.max.x - bounds.min.x;
    const sketchHeight = bounds.max.y - bounds.min.y;
    const widthProportion =
      sketchWidth !== 0 ? sketchViewWidth / sketchWidth : 10;
    const heightProportion =
      sketchHeight !== 0 ? sketchViewHeight / sketchHeight : 10;
    if (widthProportion !== 0 && heightProportion !== 0) {
      let borderX = 0;
      let borderY = 0;
      if (widthProportion < heightProportion) {
        this.resolution = Math.min(widthProportion, 30);
        borderX = (this.root.clientWidth * (1.0 - coveragePercentage)) / 2.0;
        const proportion =
          (sketchHeight * this.resolution) / this.root.clientHeight;
        borderY = (this.root.clientHeight * (1.0 - proportion)) / 2.0;
      } else {
        this.resolution = Math.min(heightProportion, 30);
        const proportion =
          (sketchWidth * this.resolution) / this.root.clientWidth;
        borderX = (this.root.clientWidth * (1.0 - proportion)) / 2.0;
        borderY = (this.root.clientHeight * (1.0 - coveragePercentage)) / 2.0;
      }
      if (this.invertedXAxis) {
        this.offsetX = borderX + bounds.max.x * this.resolution;
      } else {
        this.offsetX = borderX - bounds.min.x * this.resolution;
      }
      if (this.invertedYAxis) {
        this.offsetY = borderY + bounds.max.y * this.resolution;
      } else {
        this.offsetY = borderY - bounds.min.y * this.resolution;
      }
      this.updateTopLevelTransform();
    }
  }

  /**
   * Zoom In/Out event handler.
   *
   * @param {number} factor
   * @returns {void}
   *
   */
  zoom(factor) {
    let position = this.inverseTopLevelTransform.transform(
      new paper.Point(this.root.clientWidth / 2, this.root.clientHeight / 2)
    );
    if (this.autoFit) {
      this.autoFitClicked();
    }
    this.resolution *= factor;
    this.updateTopLevelTransform();
    position = this.topLevelTransform.transform(position);
    this.offsetX -= position.x - this.root.clientWidth / 2;
    this.offsetY -= position.y - this.root.clientHeight / 2;
    this.updateTopLevelTransform();
    this.draw();
  }

  /**
   * Mouse down event handler.
   *
   * @param {MouseEvent} event
   * @returns {void}
   *
   */
  down(event) {
    if (this.interaction !== undefined && this.interaction.down !== undefined) {
      this.interaction.down(this, event, this.isRotating);
    }
  }

  /**
   * Double click event handler.
   *
   * @param {MouseEvent} event
   * @returns {void}
   *
   */
  doubleClick(event) {
    if (this.interaction && this.interaction.doubleClick) {
      this.interaction.doubleClick(this, event);
    }
  }

  /**
   * Mouse move event handler.
   *
   * @param {MouseEvent} event
   * @returns {void}
   *
   */
  move(event = undefined) {
    if (event) {
      this.lastMove = event;
    } else {
      event = this.lastMove;
      this.interaction.move(this, event, this.isRotating);
    }
    if (this.interaction && this.interaction.move) {
      if (
        this.autoFit &&
        this.mode == SketchMode.Pan &&
        this.interaction.isDown
      ) {
        this.autoFitClicked();
      }
      this.interaction.move(this, event, this.isRotating);
    }
  }

  /**
   * Mouse up event handler.
   *
   * @param {MouseEvent} event
   * @returns {void}
   *
   */
  up(event) {
    if (this.interaction !== undefined && this.interaction.up !== undefined) {
      this.interaction.up(this, event);
    }
  }

  /**
   * Mouse enter event handler.
   *
   * @param {MouseEvent} event
   * @returns {void}
   *
   */
  enter(event) {
    if (
      this.interaction !== undefined &&
      this.interaction.enter !== undefined
    ) {
      this.interaction.enter(this, event);
    }
  }

  /**
   * Mouse wheel event handler.
   *
   * @param {MouseEvent} event
   * @returns {void}
   *
   */
  wheel(event) {
    if (this.interaction !== undefined) {
      if (this.autoFit) {
        this.autoFitClicked();
      }
      event.preventDefault();
      let factor = undefined;
      if (event.deltaMode === 0) {
        // DOM_DELTA_PIXEL
        factor = 1 - event.deltaY / 1000;
      } else if (event.deltaMode === 1) {
        // DOM_DELTA_LINE
        factor = 1 - event.deltaY / 100;
      } else if (event.deltaMode === 2) {
        // DOM_DELTA_PAGE
        factor = 1 - event.deltaY / 10;
      }
      if (factor !== undefined) {
        let position = this.inverseTopLevelTransform.transform(
          new paper.Point(event.offsetX, event.offsetY)
        );
        this.resolution *= factor;
        this.updateTopLevelTransform();
        position = this.topLevelTransform.transform(position);
        this.offsetX -= position.x - event.offsetX;
        this.offsetY -= position.y - event.offsetY;
        this.updateTopLevelTransform();
        this.draw();
      }
    }
  }

  /**
   * Touch gesture start event handler.
   *
   * @param {TouchEvent} event
   * @returns {void}
   */
  gestureStart(event) {
    if (this.interaction !== undefined) {
      event.preventDefault();
      this.startingResolutionFromGesture = this.resolution;
    }
  }

  /**
   * Touch gesture change event handler.
   *
   * @param {TouchEvent} event
   * @returns {void}
   */
  gestureChange(event) {
    if (this.interaction !== undefined) {
      event.preventDefault();
      const bounds = this.root.getBoundingClientRect();
      const offsetX = event.pageX - bounds.left;
      const offsetY = event.pageY - bounds.top;
      let position = this.inverseTopLevelTransform.transform(
        new paper.Point(offsetX, offsetY)
      );
      this.resolution = this.startingResolutionFromGesture * event.scale;
      this.updateTopLevelTransform();
      position = this.topLevelTransform.transform(position);
      this.offsetX -= position.x - offsetX;
      this.offsetY -= position.y - offsetY;
      this.updateTopLevelTransform();
      this.draw();
    }
  }

  /**
   * Touch gesture end event handler.
   *
   * @param {TouchEvent} event
   * @returns {void}
   */
  gestureEnd(event) {
    if (this.interaction !== undefined) {
      event.preventDefault();
      this.startingResolutionFromGesture = undefined;
      this.offsetXFromGesture = undefined;
      this.offsetYFromGesture = undefined;
    }
  }

  /**
   * Touch start event handler.
   *
   * @param {TouchEvent} event
   * @returns {void}
   */
  touchStart(event) {
    if (this.interaction !== undefined) {
      event.preventDefault();
      if (this.firstTouchGesture !== undefined) {
        if (this.interaction.up !== undefined) {
          for (let i = 0; i < event.changedTouches.length; i++) {
            if (event.changedTouches[i].identifier === this.firstTouchGesture) {
              const newEvent = {
                offsetX: event.changedTouches[i].clientX,
                offsetY: event.changedTouches[i].clientY
              };
              this.interaction.up(this, newEvent);
              break;
            }
          }
        }
        this.firstTouchGesture = undefined;
      } else {
        if (this.interaction.down) {
          const newEvent = {
            offsetX: event.changedTouches[0].clientX,
            offsetY: event.changedTouches[0].clientY
          };
          this.interaction.down(this, newEvent);
        }
        this.firstTouchGesture = event.changedTouches[0].identifier;
      }
    }
  }

  /**
   * Touch move event handler.
   *
   * @param {TouchEvent} event
   * @returns {void}
   */
  touchMove(event) {
    if (this.interaction !== undefined) {
      event.preventDefault();
      if (
        this.firstTouchGesture !== undefined &&
        this.interaction.move !== undefined
      ) {
        for (let i = 0; i < event.changedTouches.length; i++) {
          if (event.changedTouches[i].identifier === this.firstTouchGesture) {
            const newEvent = {
              offsetX: event.changedTouches[i].clientX,
              offsetY: event.changedTouches[i].clientY
            };
            this.interaction.move(this, newEvent);
            break;
          }
        }
      }
    }
  }

  /**
   * Touch end event handler.
   *
   * @param {TouchEvent} event
   * @returns {void}
   */
  touchEnd(event) {
    if (this.interaction !== undefined) {
      event.preventDefault();
      if (this.firstTouchGesture !== undefined) {
        if (this.interaction.up !== undefined) {
          for (let i = 0; i < event.changedTouches.length; i++) {
            if (event.changedTouches[i].identifier === this.firstTouchGesture) {
              const newEvent = {
                offsetX: event.changedTouches[i].clientX,
                offsetY: event.changedTouches[i].clientY
              };
              this.interaction.up(this, newEvent);
              break;
            }
          }
        }
        this.firstTouchGesture = undefined;
      }
    }
  }

  /**
   * Touch cancel event handler.
   *
   * @param {TouchEvent} event
   * @returns {void}
   */
  touchCancel(event) {
    touchEnd(event);
  }

  /**
   * Text input change event handler.
   *
   * @returns {void}
   */
  textInputElementTextChanged() {
    if (this.interaction && this.interaction.textChanged) {
      this.interaction.textChanged(this);
    }
  }

  /**
   * Key down event handler
   *
   * @param {KeyboardEvent} event
   * @returns {void}
   */
  keyDown(event) {
    if (this.interaction) {
      if (this.interaction.keyDown) {
        this.interaction.keyDown(this, event);
      } else if (event.key.indexOf("Arrow") == 0 && this.cursor) {
        if (this.sketchPathToEdit && this.lastEvent == "mousemove") {
          const path = parent.sketchControl.sketchPathToEdit;
          parent.sketchControl.cursor.x = path.elements
            ? path.elements[path.elements.length - 1].x
            : path.start.x;
          parent.sketchControl.cursor.y = path.elements
            ? path.elements[path.elements.length - 1].y
            : path.start.y;
        }
      } else if (event.key == "Escape") {
        addLayer("collapse");
      }
    }
  }

  /**
   * Key up event handler.
   *
   * @param {KeyboardEvent} event
   * @returns {void}
   */
  keyUp(event) {
    this.panning = false;
    if (
      this.interaction !== undefined &&
      this.interaction.keyUp !== undefined
    ) {
      this.interaction.keyUp(this, event);
    }
  }

  /**
   * Sets a point's new position on the canvas.
   *
   * @param {object} point
   * @param {number} x
   * @param {number} y
   * @returns {void}
   */
  setPosition(point, x, y) {
    if (!isNaN(point.x)) {
      if (this.snapToGrid) {
        point.x = Math.round(x);
        point.y = Math.round(y);
      } else {
        point.x = x;
        point.y = y;
      }
    }
  }

  /**
   * Returns a new point based on snap conditions.
   *
   * @param {number} x
   * @param {number} y
   * @returns {object}
   */
  newPosition(x, y) {
    if (this.snapToGrid) {
      return { x: Math.round(x), y: Math.round(y) };
    } else {
      return { x: x, y: y };
    }
  }

  /**
   * Sets a point's new position on the canvas based on snap conditions.
   *
   * @param {object} point
   * @returns {void}
   */
  adjustPosition(point) {
    if (this.snapToGrid) {
      point.x = Math.round(point.x);
      point.y = Math.round(point.y);
    }
  }

  /**
   * Adds a new point on the canvas.
   *
   * @param {object} point
   * @param {object} left
   * @param {number} rightX
   * @param {number} rightY
   * @returns {void}
   */
  addToPosition(point, left, rightX, rightY) {
    if (this.snapToGrid) {
      point.x = left.x + Math.round(rightX);
      point.y = left.y + Math.round(rightY);
    } else {
      point.x = left.x + rightX;
      point.y = left.y + rightY;
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
    let bounds = {};
    this.getBounds(bounds);
    if (bounds.started) {
      projection.bounds = bounds;
      projection.center.x = (bounds.min.x + bounds.max.x) / 2;
      projection.center.y = (bounds.min.y + bounds.max.y) / 2;
      for (let i = 0; i < this.layers.length; i++) {
        this.layers[i].findProjection(projection);
      }
    }
  }

  /**
   * Finds a projection of all selected paths.
   *
   * @param {object} projection - object containing an array named 'projected'
   * @returns {void}
   *
   */
  findSelectionProjection(projection) {
    let bounds = {};
    this.getSelectionBounds(bounds);
    if (bounds.started) {
      projection.bounds = bounds;
      projection.center.x = (bounds.min.x + bounds.max.x) / 2;
      projection.center.y = (bounds.min.y + bounds.max.y) / 2;
      for (let i = 0; i < this.selected.length; i++) {
        let selection = this.selected[i];
        if (selection.path) {
          selection.path.findProjection(projection);
        }
      }
    }
  }

  /**
   * Mirrors the position of selected paths' points on the X axis.
   *
   * @returns {void}
   */
  flipSelectionHorizontally() {
    if (this.selected !== undefined && this.selected.length > 0) {
      let projection = { center: {}, projected: [], index: 0 };
      this.findSelectionProjection(projection);
      if (projection.projected.length > 0) {
        const history = this.createHistory();
        this.addToUndoHistory(history);
        for (let i = 0; i < this.selected.length; i++) {
          let selection = this.selected[i];
          if (selection.path) {
            selection.path.flipHorizontally(projection, selection.layer);
            if (selection.path.elements) {
              selection.path.elements.map(element => {
                if (element.angle) element.angle *= -1;
              });
            }
          }
        }
      }
    }
  }

  /**
   * Mirrors the position of selected paths' points on the Y axis.
   *
   * @returns {void}
   */
  flipSelectionVertically() {
    if (this.selected !== undefined && this.selected.length > 0) {
      let projection = { center: {}, projected: [], index: 0 };
      this.findSelectionProjection(projection);
      if (projection.projected.length > 0) {
        const history = this.createHistory();
        this.addToUndoHistory(history);
        projection.index = 0;
        for (let i = 0; i < this.selected.length; i++) {
          let selection = this.selected[i];
          if (selection.path) {
            selection.path.flipVertically(projection, selection.layer);
          }
        }
      }
    }
  }

  /**
   * Rotates the position of selected paths' points by -90º based on the projected center.
   *
   * @returns {void}
   */
  rotateSelectionLeft() {
    if (this.selected !== undefined && this.selected.length > 0) {
      let projection = { center: {}, projected: [], index: 0 };
      this.findSelectionProjection(projection);
      if (projection.projected.length > 0) {
        const history = this.createHistory();
        this.addToUndoHistory(history);
        projection.index = 0;
        for (let i = 0; i < this.selected.length; i++) {
          let selection = this.selected[i];
          if (selection.path) {
            selection.path.rotateLeft(projection, selection.layer);
          }
        }
      }
    }
  }

  /**
   * Rotates the position of selected paths' points by 90º based on the projected center.
   *
   * @returns {void}
   */
  rotateSelectionRight() {
    if (this.selected !== undefined && this.selected.length > 0) {
      let projection = { center: {}, projected: [], index: 0 };
      this.findSelectionProjection(projection);
      if (projection.projected.length > 0) {
        const history = this.createHistory();
        this.addToUndoHistory(history);
        projection.index = 0;
        for (let i = 0; i < this.selected.length; i++) {
          let selection = this.selected[i];
          if (selection.path) {
            selection.path.rotateRight(projection, selection.layer);
          }
        }
      }
    }
  }

  // taken from https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript :
  static uuidv4() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
      (
        c ^
        (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
      ).toString(16)
    );
  }

  /**
   * Gets the parent Object of the specified object
   *
   * @param {SketchObject} item - child object
   * @returns {SketchObject}
   *
   */
  findParent(item) {
    if (this.layers !== undefined && this.layers.length > 0) {
      for (let i = 0; i < this.layers.length; i++) {
        if (this.layers[i] === item) {
          return { parent: this, index: i };
        }
      }
      for (let i = 0; i < this.layers.length; i++) {
        const parent = this.layers[i].findParent(item);
        if (parent) {
          return parent;
        }
      }
    }
  }

  /**
   * Applies the PaperJS style on the specified Text.
   *
   * @param {PointText} text - Text to stylize
   * @param {SketchStyle} style - Non-CSS style
   * @returns {void}
   *
   */
  applyTextStyle(text, style) {
    let fontFamily = undefined;
    if (style !== undefined && style.fontFamily !== undefined) {
      fontFamily = style.fontFamily;
    } else {
      fontFamily = this.canvas.style.fontFamily;
    }
    let fontSize = undefined;
    if (style !== undefined && style.fontSize !== undefined) {
      fontSize = style.fontSize;
    } else {
      fontSize = this.canvas.style.fontSize;
    }
    let color = this.canvas.style.color;
    if (style !== undefined) {
      if (style.fontColor !== undefined) {
        color = style.fontColor;
      } else if (style.color !== undefined) {
        color = style.color;
      }
    }
    text.fontFamily = fontFamily;
    text.fontSize = fontSize;
    text.fillColor = color;
  }

  /**
   * Applies the PaperJS style on the specified Text.
   *
   * @param {PointText} text - Text to stylize
   * @param {SketchStyle} style - Non-CSS style
   * @returns {void}
   *
   */
  applyTextStyleWithAttributes(text, style, attributes) {
    let fontFamily = undefined;
    if (attributes !== undefined && attributes.fontFamily !== undefined) {
      fontFamily = attributes.fontFamily + ", sans-serif";
    } else if (style !== undefined && style.fontFamily !== undefined) {
      fontFamily = style.fontFamily;
    } else {
      fontFamily = this.canvas.style.fontFamily;
    }
    let fontSize = undefined;
    if (attributes !== undefined && attributes.fontSize !== undefined) {
      if (attributes.worldUnits)
        fontSize = attributes.fontSize * this.resolution;
      else fontSize = attributes.fontSize;
    } else if (style !== undefined && style.fontSize !== undefined) {
      fontSize = style.fontSize;
    } else {
      fontSize = this.canvas.style.fontSize;
    }
    let color = this.canvas.style.color;
    if (attributes !== undefined && attributes.fontColor !== undefined) {
      color = attributes.fontColor;
    } else if (style !== undefined) {
      if (style.fontColor !== undefined) {
        color = style.fontColor;
      } else if (style.color !== undefined) {
        color = style.color;
      }
    }
    let weight = "normal";
    if (attributes !== undefined && attributes.fontWeight !== undefined) {
      if (attributes.fontWeight === 0) {
        weight = "lighter";
      } else {
        weight = attributes.fontWeight;
      }
    }
    text.fontFamily = fontFamily;
    text.fontSize = fontSize;
    text.fillColor = color;
    text.fontWeight = weight;
  }

  /**
   * Deletes every object within the this.selected array
   *
   * @returns {void}
   *
   */
  deleteSelection() {
    if (this.selected && this.layers && this.layers.length > 0) {
      const history = this.createHistory();
      this.addToUndoHistory(history);
      while (this.selected.length > 0) {
        if (this.selected[0].point) {
          let elements = this.selected[0].path.elements;
          let index = elements.findIndex(
            object => object == this.selected[0].point
          );
          this.selected[0].element =
            elements[index > -1 ? index : elements.length - 1];
        }
        if (
          this.selected[0].element &&
          this.selected[0].path.elements.length > 1
        ) {
          this.layers.map(layer => {
            if (layer == this.selected[0].layer) {
              this.selected[0].path.elements.map((element, index) => {
                if (
                  element.x == this.selected[0].element.x &&
                  element.y == this.selected[0].element.y
                ) {
                  if (this.selected[0].path.closed) {
                    this.selected[0].path.closed = false;
                    if (index > -1) {
                      this.selected[0].path.elements = this.selected[0].path.elements
                        .slice(this.selected[0].point ? index + 1 : index)
                        .concat(this.selected[0].path.elements.slice(0, index));
                    } else {
                      this.selected[0].path.elements = this.selected[0].path.elements.slice(
                        1,
                        this.selected[0].path.elements.length - 2
                      );
                    }
                    this.selected[0].path.start = this.selected[0].path.elements.shift();
                    this.selected[0].sketchObject.objects.splice(
                      this.selected[0].path.elements.length + 1
                    );
                    const label = layer.objects.find(
                      object =>
                        object.objects &&
                        object.objects.length > 0 &&
                        object.objects[0].uniqueIdentifier ==
                          this.selected[0].path.label
                    );
                    if (label) {
                      layer.removeLabel(label);
                      layer.deleteSelection(this, label.objects[0], layer);
                    }
                  } else {
                    const newObject = this.selected[0].sketchObject.createCopy();
                    if (newObject.objects.length > 0) {
                      newObject.objects[0].uniqueIdentifier = SketchControl.uuidv4();
                      if (
                        !this.selected[0].point ||
                        this.selected[0].point == this.selected[0].element
                      ) {
                        if (index == 0) {
                          if (
                            this.selected[0].point &&
                            this.selected[0].path.elements.length > 1
                          ) {
                            this.selected[0].path.elements.shift();
                            this.selected[0].sketchObject.objects.pop();
                          }
                          this.selected[0].path.start = {
                            x: this.selected[0].path.elements[0].x,
                            y: this.selected[0].path.elements[0].y
                          };
                          this.selected[0].path.elements.shift();
                          this.selected[0].sketchObject.objects.pop();
                          if (this.selected[0].path.elements.length < 1) {
                            for (
                              let i = 0;
                              i < this.selected[0].layer.objects.length;
                              i++
                            ) {
                              if (
                                this.selected[0].path ==
                                this.selected[0].layer.objects[i].objects[0]
                              ) {
                                this.selected[0].layer.objects.splice(i, 1);
                              }
                            }
                          }
                        } else {
                          if (
                            newObject.objects[0].elements &&
                            index < newObject.objects[0].elements.length - 1
                          ) {
                            let diff = this.selected[0].point ? 1 : 0;
                            newObject.objects[0].start =
                              newObject.objects[0].elements[index + diff];
                            newObject.objects[0].elements = newObject.objects[0].elements.slice(
                              index + diff
                            );
                            newObject.objects = newObject.objects.slice(
                              0,
                              newObject.objects[0].elements.length + 1
                            );
                            layer.objects.push(newObject);
                          }
                          for (let i = 1; i < this.selected.length; i++) {
                            if (
                              this.selected[0].path == this.selected[i].path
                            ) {
                              for (
                                let j = index;
                                j < this.selected[0].path.elements.length;
                                j++
                              ) {
                                if (
                                  this.selected[i].path.elements[j] &&
                                  newObject.objects[0].elements[j - index] &&
                                  this.selected[i].element.x ==
                                    this.selected[i].path.elements[j].x &&
                                  this.selected[i].element.y ==
                                    this.selected[i].path.elements[j].y
                                ) {
                                  this.selected[i].sketchObject = newObject;
                                  this.selected[i].path = newObject.objects[0];
                                  this.selected[i].element = {
                                    x:
                                      newObject.objects[0].elements[j - index]
                                        .x,
                                    y:
                                      newObject.objects[0].elements[j - index]
                                        .y,
                                    angle:
                                      newObject.objects[0].elements[j - index]
                                        .angle
                                  };
                                }
                              }
                            }
                          }
                          this.selected[0].path.elements.splice(
                            index,
                            this.selected[0].path.elements.length
                          );
                          this.selected[0].sketchObject.objects.splice(
                            index + 1,
                            this.selected[0].sketchObject.objects.length
                          );
                        }
                        this.sketchPathToEditPoint = index;
                      } else {
                        this.selected[0].path.start = this.selected[0].path.elements[0];
                        this.selected[0].path.elements = this.selected[0].path.elements.slice(
                          1
                        );
                        this.selected[0].sketchObject.objects = this.selected[0].sketchObject.objects.slice(
                          0,
                          this.selected[0].path.elements.length + 1
                        );
                        this.sketchPathToEditPoint = 0;
                      }
                      let distance = this.newSketchDistanceTemplate.createCopy();
                      distance.sketchPath = this.sketchPathToEdit;
                      distance.start = this.sketchPathToEditPoint - 2;
                      distance.finish = this.sketchPathToEditPoint - 1;
                    }
                  }
                }
              });
              if (this.selected[0].point) {
                this.selected[0].element = undefined;
              }
            }
          });
        } else {
          for (let j = 0; j < this.layers.length; j++) {
            if (this.selected[0].layer === this.layers[j]) {
              if (this.selected[0].path) {
                if (this.selected[0].path.arrow) {
                  const arrow = this.selected[0].layer.objects.find(
                    object =>
                      object.objects[0].arrow ==
                      this.selected[0].path.uniqueIdentifier
                  );
                  if (arrow) {
                    delete arrow.objects[0].arrow;
                  }
                }
                const label = this.selected[0].layer.objects.find(
                  object =>
                    object.objects &&
                    object.objects.length > 0 &&
                    object.objects[0].uniqueIdentifier ==
                      this.selected[0].path.label
                );
                if (label) {
                  this.selected.push({
                    textObject: label.objects[0],
                    layer: this.selected[0].layer,
                    sketchObject: label
                  });
                }
              }
              if (
                this.selected[0].textObject &&
                this.selected[0].textObject.arrow
              ) {
                const arrow = this.selected[0].layer.objects.find(
                  object =>
                    object.objects &&
                    object.objects.length > 0 &&
                    object.objects[0].uniqueIdentifier ==
                      this.selected[0].textObject.arrow
                );
                this.selected.push({
                  path: arrow.objects[0],
                  layer: this.selected[0].layer
                });
                this.sketchLayerToEdit = this.selected[0].layer.uniqueIdentifier;
              }
              this.layers[j].deleteSelection(this, this.selected[0]);
              this.cleanLayer(this.layers[j].objects);
              break;
            }
          }
        }
        this.selected.shift();
      }
    }
    this.clearSelection();
    this.dismissTextInput();
    this.autoSave();
    this.lastAction = "delete";
    this.selectedElements = 0;
    this.sketchPathToEditPoint = undefined;
  }

  /**
   * Removes empty Arrays from within a Layer object.
   *
   * @param {Array} objects - Layer contents
   * @returns {void}
   *
   */
  cleanLayer(objects) {
    for (let i = 0; i < objects.length; i++) {
      if (objects[i].objects.length < 1) {
        objects.splice(i, 1);
        break;
      }
    }
  }

  /**
   * Sets attributes "area" in SketchPath, as well as "netArea" and "grossArea" in SketchLayer.
   *
   * @returns {void}
   *
   */
  setAreaTotals() {
    for (let i = 0; i < this.layers.length; i++) {
      const layer = this.layers[i];
      if (this.layerList[layer.name]) {
        if (this.layerList[layer.name].livingArea) {
          layer.netArea = 0;
        }
        if (this.layerList[layer.name].grossArea) {
          layer.grossArea = 0;
        }

        if (layer.objects && layer.objects.length > 0) {
          for (const obj of layer.objects) {
            if (obj.objects && obj.objects.length > 0) {
              for (const sketchObj of obj.objects) {
                if (sketchObj instanceof SketchPath) {
                  const pathArea = sketchObj.getArea(this);
                  sketchObj.area = pathArea;

                  if (sketchObj.negative && pathArea > 0) {
                    //Negative area
                    if (this.pathInsidePath(sketchObj, layer)) {
                      //Only subtract negative areas from layers that add (+) to net/gross areas
                      if (this.layerList[layer.name].livingArea === "+") {
                        layer.netArea += pathArea;
                      }

                      if (this.layerList[layer.name].grossArea === "+") {
                        layer.grossArea += pathArea;
                      }
                    }
                  } else {
                    //Positive area
                    if (this.layerList[layer.name].livingArea === "+") {
                      layer.netArea += pathArea;
                    } else if (this.layerList[layer.name].livingArea === "-") {
                      layer.netArea -= pathArea;
                    }

                    if (this.layerList[layer.name].grossArea === "+") {
                      layer.grossArea += pathArea;
                    } else if (this.layerList[layer.name].grossArea === "-") {
                      layer.grossArea -= pathArea;
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }

  /**
   * Updates the Negative switch according to the area selected.
   *
   * @param {boolean} isNegative
   * @returns {void}
   *
   */
  negativeAreaSelection(isNegative) {
    parent.updateNegativeSwitch(this.mode, isNegative);
  }

  /**
   * If there are closed paths selected, convert-to/revert negative area.
   *
   * @param {boolean} isNegative
   * @returns {void}
   *
   */
  selectionToNegative(isNegative) {
    if (
      (this.mode == SketchMode.SingleSelect ||
        this.mode === SketchMode.MultipleSelect) &&
      this.selected !== undefined &&
      this.selected.length > 0 &&
      this.layers !== undefined &&
      this.layers.length > 0
    ) {
      const history = this.createHistory();
      this.addToUndoHistory(history);
      for (let i = 0; i < this.selected.length; i++) {
        if (this.selected[i].path.closed) {
          this.selected[i].path.negative = isNegative;
        }
        const objects = this.selected[i].layer.objects;
        for (let j = 0; j < objects.length; j++) {
          const label = objects[j].objects.find(
            label => label.uniqueIdentifier == this.selected[i].path.label
          );
          if (label) {
            label.customText = isNegative
              ? "Open"
              : this.selected[i].layer.name;
            break;
          }
        }
      }
      if (isNegative) {
        this.lastAction = "negative select";
      } else if (this.mode == SketchMode.SingleSelect) {
        this.lastAction = "single";
      } else {
        this.lastAction = "multiple";
      }
    }
  }

  get editing() {
    return this._editing;
  }

  set editing(value) {
    const previous = this._editing;
    if (previous && this.editingFinished && value !== previous) {
      this.editingFinished();
    }
    this._editing = value;
    if (value !== undefined && this.editingStarted !== undefined) {
      this.editingStarted(value);
    }
  }

  get penDown() {
    return this._penDown;
  }

  set penDown(value) {
    const previous = this._penDown;
    this._penDown = value;
    if (previous !== value && value) {
      this.sketchPathToEdit = undefined;
      this.sketchObjectToEdit = undefined;
      if (this.mode == SketchMode.Draw && this.cursor !== undefined) {
        const history = this.createHistory();
        if (this.addPoint(this.cursor)) {
          this.addToUndoHistory(history);
        }
      }
    }
  }

  /**
   * Determines whether the specified position closer than the minimum distance to the specified Path.
   *
   * @param {object} position
   * @param {SketchPath} path
   * @returns {object} containing tooClose as parameter
   *
   */
  isTooCloseToPath(position, path) {
    if (path.start !== undefined) {
      const deltaX = path.start.x - position.x;
      const deltaY = path.start.y - position.y;
      const distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
      if (distance < this.minimumDistance) {
        return { tooClose: true, closestPoint: 0 };
      }
    }
    if (path.elements !== undefined && path.elements.length > 0) {
      for (let i = 0; i < path.elements.length; i++) {
        const deltaX = path.elements[i].x - position.x;
        const deltaY = path.elements[i].y - position.y;
        const distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
        if (distance < this.minimumDistance) {
          return { tooClose: true, closestPoint: i + 1 };
        }
      }
    }
    return { tooClose: false };
  }

  /**
   * Modifies the given position based on the jumpToCoordinates contents.
   *
   * @param {object} position
   * @returns {void}
   *
   */
  relocateToJumpTo(position) {
    let newPosition;
    if (this.jumpToCoordinates) {
      const newTransformedPosition = this.topLevelTransform.transform(
        new paper.Point(position.x, position.y)
      );
      for (let i = 0; i < this.jumpToCoordinates.length; i++) {
        const transformedJumpToPosition = this.topLevelTransform.transform(
          new paper.Point(
            this.jumpToCoordinates[i].x,
            this.jumpToCoordinates[i].y
          )
        );
        const deltaX = newTransformedPosition.x - transformedJumpToPosition.x;
        const deltaY = newTransformedPosition.y - transformedJumpToPosition.y;
        if (
          deltaX * deltaX + deltaY * deltaY <
          this.styleSet.jumpToIndicatorStyle.radius *
            this.styleSet.jumpToIndicatorStyle.radius
        ) {
          newPosition = {
            x: this.jumpToCoordinates[i].x,
            y: this.jumpToCoordinates[i].y
          };
          break;
        }
      }
    }
    if (newPosition) {
      position.x = newPosition.x;
      position.y = newPosition.y;
    }
  }

  /**
   * Pushes a new point into the sketchPathToEdit's element array. Closes the Path if it converges with the start point.
   *
   * @param {object} position
   * @returns {void}
   *
   */
  addPointInSketchPathElements(position) {
    let layer;
    if (!this.sketchPathToEdit.elements) {
      this.sketchPathToEdit.elements = [{ x: position.x, y: position.y }];
      this.sketchPathToEditPoint = 1;
    } else {
      this.sketchPathToEdit.elements.push({ x: position.x, y: position.y });
      this.sketchPathToEditPoint = this.sketchPathToEdit.elements.length;
      if (this.sketchPathToEdit.elements.length > 2) {
        const deltaX = position.x - this.sketchPathToEdit.start.x;
        const deltaY = position.y - this.sketchPathToEdit.start.y;
        const distanceSquared = deltaX * deltaX + deltaY * deltaY;
        if (distanceSquared < 1e-5) {
          this.sketchPathToEdit.closed = true;
          this.sketchPathToEdit.negative = this.drawNegativeArea;
          layer = this.layers.find(
            layer => layer.name == this.sketchPathToEdit.layer
          );
          layer.addLabel(this.sketchPathToEdit);
        }
      }
    }
    let distance = this.newSketchDistanceTemplate.createCopy();
    distance.sketchPath = this.sketchPathToEdit;
    distance.start = this.sketchPathToEditPoint - 1;
    distance.finish = this.sketchPathToEditPoint;
    this.sketchObjectToEdit.objects.push(distance);
    if (distance.sketchPath) {
      this.xDistance =
        distance.sketchPath.elements[distance.sketchPath.elements.length - 1]
          .x - distance.sketchPath.start.x;
      this.yDistance =
        distance.sketchPath.elements[distance.sketchPath.elements.length - 1]
          .y - distance.sketchPath.start.y;
    }
    if (this.sketchPathToEdit.closed && layer) {
      this.alignDistances(layer);
    }
  }

  /**
   * When closing a figure, aligns the Distances either inside or outside the figure, depending on its layer.
   *
   * @param {SketchLayer} layer
   * @returns {void}
   *
   */
  alignDistances(layer) {
    const object = layer.objects.find(
      object =>
        object.objects &&
        object.objects.length > 0 &&
        object.objects[0].uniqueIdentifier ==
          this.sketchPathToEdit.uniqueIdentifier
    );
    if (object && object.objects[0].fillPath) {
      const offset = object.objects[0].fillPath.clockwise ? 2 : -2;
      for (let i = 1; i < object.objects.length; i++) {
        if (object.objects[i].offset) {
          switch (layer.name) {
            case "2nd floor":
              object.objects[i].offset.y = -offset;
              break;
            case "Half floor":
              object.objects[i].offset.y = -offset;
              break;
            case "Upper floor":
              object.objects[i].offset.y = -offset;
              break;
            case "Unfinished half floor":
              object.objects[i].offset.y = -offset;
              break;
            case "Basement total":
              object.objects[i].offset.y = -offset;
              break;
            case "Basement finished":
              object.objects[i].offset.y = -offset;
              break;
            default:
              object.objects[i].offset.y = offset;
          }
        }
      }
    }
  }

  /**
   * Adds a point on the given position to the current sketchPathToEdit. Otherwise, creates a new Path to add that point.
   *
   * @param {object} position
   * @returns {boolean}
   *
   */
  addPoint(position) {
    let layer;
    let extension;
    let toDelete;
    if (this.sketchPathToEdit === undefined) {
      if (this.sketchLayerToEdit === undefined) {
        this.sketchLayerToEdit = this.defaultSketchLayerToEdit;
      }
      if (this.layers !== undefined && this.layers.length > 0) {
        for (let i = 0; i < this.layers.length; i++) {
          if (this.layers[i].uniqueIdentifier === this.sketchLayerToEdit) {
            layer = this.layers[i];
            break;
          }
        }
      }
      if (layer === undefined) {
        return false;
      }
      let converged = false;
      for (let i = 0; i < this.layers.length; i++) {
        if (
          this.layers[i].objects !== undefined &&
          this.layers[i].objects.length > 0
        ) {
          for (let j = 0; j < this.layers[i].objects.length; j++) {
            if (
              this.layers[i].objects[j].objects !== undefined &&
              this.layers[i].objects[j].objects.length > 0
            ) {
              for (
                let k = 0;
                k < this.layers[i].objects[j].objects.length;
                k++
              ) {
                if (
                  this.layers[i].objects[j].objects[k] instanceof SketchPath
                ) {
                  let toPath = this.isTooCloseToPath(
                    position,
                    this.layers[i].objects[j].objects[k]
                  );
                  if (
                    toPath.tooClose &&
                    !this.layers[i].objects[j].objects[k].closed
                  ) {
                    converged =
                      this.sketchLayerToEdit == this.layers[i].uniqueIdentifier;
                    this.sketchObjectToEdit = this.layers[i].objects[j];
                    this.sketchPathToEdit = this.layers[i].objects[j].objects[
                      k
                    ];
                    if (toPath.closestPoint == 0) {
                      this.sketchPathToEdit.reverse();
                    }
                    break;
                  }
                }
              }
            }
            if (converged) {
              break;
            }
          }
        }
        if (converged) break;
      }
      if (!converged) {
        this.sketchPathToEdit = new SketchPath();
        this.sketchPathToEdit.uniqueIdentifier = SketchControl.uuidv4();
        this.sketchPathToEdit.layer = this.getLayer(this.sketchLayerToEdit).name;
        this.sketchObjectToEdit = new SketchObject();
        this.sketchObjectToEdit.objects = [this.sketchPathToEdit];
        if (layer.objects === undefined) {
          layer.objects = [this.sketchObjectToEdit];
        } else {
          layer.objects.push(this.sketchObjectToEdit);
        }
      }
    } else {
      layer = this.getLayer(this.sketchLayerToEdit);
      for (let i = 0; i < layer.objects.length; i++) {
        if (layer.objects[i].objects && layer.objects[i].objects.length > 1) {
          if (layer.objects[i].objects[0] instanceof SketchPath) {
            let toPath = this.isTooCloseToPath(
              position,
              layer.objects[i].objects[0]
            );
            if (toPath.tooClose && !layer.objects[i].objects[0].closed) {
              extension = layer.objects[i].objects[0];
              toDelete = i;
              break;
            }
          }
        }
      }
    }
    if (this.sketchPathToEdit.start === undefined) {
      this.sketchPathToEdit.start = { x: position.x, y: position.y };
      this.sketchPathToEditPoint = 0;
      return true;
    } else {
      if (
        this.sketchLayerToEdit.objects !== undefined &&
        this.sketchLayerToEdit.objects.length > 0
      ) {
        for (let j = 0; j < this.sketchLayerToEdit.objects.length; j++) {
          if (
            this.sketchLayerToEdit.objects[j].objects !== undefined &&
            this.sketchLayerToEdit.objects[j].objects.length > 0
          ) {
            for (
              let k = 0;
              k < this.sketchLayerToEdit.objects[j].objects.length;
              k++
            ) {
              let path = this.sketchLayerToEdit.objects[j].objects[k];
              if (path instanceof SketchPath) {
                let toPath = this.isTooCloseToPath(position, path);
                if (
                  toPath.tooClose &&
                  path !== this.sketchPathToEdit &&
                  !path.closed
                ) {
                  if (toPath.closestPoint == path.elements.length) {
                    path.reverse();
                    toPath.closestPoint = 0;
                  }
                  if (toPath.closestPoint == 0) {
                    this.relocateToJumpTo(position);
                    this.addPointInSketchPathElements(position);
                    this.sketchPathToEdit.elements = this.sketchPathToEdit.elements.concat(
                      path.elements
                    );
                    this.sketchLayerToEdit.objects.splice(j, 1);
                    return true;
                  }
                }
              }
            }
          }
        }
      }
      const tooClose = this.isTooCloseToPath(position, this.sketchPathToEdit);
      if (!tooClose.tooClose) {
        this.relocateToJumpTo(position);
        this.addPointInSketchPathElements(position);
        if (extension) {
          this.concatPaths(extension, position);
          layer.objects.splice(toDelete, 1);
          penUpClicked();
        }
        this.autoSave();
        return true;
      } else if (
        tooClose.closestPoint === 0 &&
        this.sketchPathToEdit.elements !== undefined &&
        this.sketchPathToEdit.elements.length >= 2
      ) {
        this.addPointInSketchPathElements(this.sketchPathToEdit.start);
        this.autoSave();
        return true;
      }
    }
  }

  /**
   * Fuses 2 Paths together.
   *
   * @param {SketchPath} extension
   * @param {object} position
   * @returns {void}
   *
   */
  concatPaths(extension, position) {
    if (extension.start.x !== position.x || extension.start.y !== position.y) {
      extension.elements.reverse();
      extension.elements.push(extension.start);
      extension.start = extension.elements.shift();
    }
    for (let i = 0; i < extension.elements.length; i++) {
      this.addPointInSketchPathElements(extension.elements[i]);
    }
  }

  /**
   * Relocates the Pen cursor in Draw mode.
   *
   * @param {number} x
   * @param {number} y
   * @returns {void}
   *
   */
  movePen(x, y) {
    if (this.mode == SketchMode.Draw && this.cursor) {
      const position = { x: this.cursor.x + x, y: this.cursor.y + y };
      let reposition = false;
      if (this.penDown) {
        const history = this.createHistory();
        if (this.addPoint(position)) {
          this.addToUndoHistory(history);
          if (
            this.sketchPathToEdit !== undefined &&
            this.sketchPathToEdit.closed
          ) {
            this.sketchPathToEdit = undefined;
            this.sketchObjectToEdit = undefined;
          }
          reposition = true;
        }
      } else {
        reposition = true;
      }
      if (reposition) {
        this.cursor.x = position.x;
        this.cursor.y = position.y;
      }
    }
  }

  /**
   * Sets the editing line length based on the user input.
   *
   * @param {number} amount
   * @returns {void}
   *
   */
  setLength(amount) {
    const editing = this.editing;
    if (this.selected) {
      this.editing = this.selected[0];
      if (!this.editing.element) {
        this.editing.element = this.editing.path.elements[
          this.editing.path.elements.length - 1
        ];
      }
      if (isNaN(this.editing.element.angle) || this.editing.element.angle == 0)
        this.showContextBar("line");
    }
    if (this.editing !== undefined && this.editing.curveData !== undefined) {
      const history = this.createHistory();
      this.addToUndoHistory(history);
      const angle = this.editing.element.angle ?? 0;
      const chord = this.editing.curveData.chord;
      let newAngle = undefined;
      if (amount <= chord) {
        newAngle = 0;
      } else {
        let upperLimit = (chord * Math.PI) / 2;
        const remainder = upperLimit % Math.floor(upperLimit);
        const inchRemainder = remainder % (1 / 12);
        //Subtract this remainder to handle cases when the user enters the max
        //curve length posible and this length could be reached with an angle
        //slightly smaller than 180.
        upperLimit = upperLimit - inchRemainder;
        if (amount >= upperLimit - 1e-5) {
          if (angle < 0) {
            newAngle = -180;
          } else {
            newAngle = 180;
          }
        } else {
          for (let ang = 0; ang <= 180; ang += 0.01) {
            const inRadians = ang * (Math.PI / 180);
            const radius = Math.sqrt(
              (chord * chord) / (2 - 2 * Math.cos(inRadians))
            );
            let arcLength = inRadians * radius;
            if (arcLength >= amount) {
              newAngle = ang + 1e-5;
              break;
            }
          }

          if (angle < 0) {
            newAngle = -newAngle;
          }
        }
      }
      this.editing.element.angle = newAngle;
      SketchUtils.createCurveDataIn(
        this.editing,
        this.editing.curveData.startX,
        this.editing.curveData.startY,
        this.editing.curveData.finishX,
        this.editing.curveData.finishY,
        newAngle
      );
      if (this.editingValuesChanged !== undefined) {
        this.editingValuesChanged(this.editing);
      }
      if (this.editing.point) this.editing.element = undefined;
      this.draw();
    }
    this.editing = editing;
  }

  /**
   * Sets the editing line rise based on the user input.
   *
   * @param {number} amount
   * @returns {void}
   *
   */
  setRise(amount) {
    const editing = this.editing;
    if (this.selected) {
      this.editing = this.selected[0];
      if (!this.editing.element) {
        this.editing.element = this.editing.path.elements[
          this.editing.path.elements.length - 1
        ];
      }
      if (isNaN(this.editing.element.angle) || this.editing.element.angle == 0)
        this.showContextBar("line");
    }
    if (this.editing !== undefined && this.editing.curveData !== undefined) {
      const history = this.createHistory();
      this.addToUndoHistory(history);
      const element = amount - this.editing.curveData.rise;
      if (
        this.editing.element.x == this.editing.path.start.x &&
        this.editing.element.y == this.editing.path.start.y
      ) {
        this.editing.path.start.y -= element;
      }
      this.editing.element.y -= element;
      SketchUtils.createCurveDataIn(
        this.editing,
        this.editing.curveData.startX,
        this.editing.curveData.startY,
        this.editing.curveData.finishX,
        this.editing.element.y,
        this.editing.curveData.angle
      );
      if (this.editingValuesChanged !== undefined) {
        this.editingValuesChanged(this.editing);
      }
      if (this.editing.point) this.editing.element = undefined;
      this.draw();
    }
    this.editing = editing;
  }

  /**
   * Sets the editing line run based on the user input.
   *
   * @param {number} amount
   * @returns {void}
   *
   */
  setRun(amount) {
    const editing = this.editing;
    if (this.selected) {
      this.editing = this.selected[0];
      if (!this.editing.element) {
        this.editing.element = this.editing.path.elements[
          this.editing.path.elements.length - 1
        ];
      }
      if (isNaN(this.editing.element.angle) || this.editing.element.angle == 0)
        this.showContextBar("line");
    }
    if (this.editing !== undefined && this.editing.curveData !== undefined) {
      const history = this.createHistory();
      this.addToUndoHistory(history);
      const element = amount - this.editing.curveData.run;
      if (
        this.editing.element.x == this.editing.path.start.x &&
        this.editing.element.y == this.editing.path.start.y
      ) {
        this.editing.path.start.x += element;
      }
      this.editing.element.x += element;
      SketchUtils.createCurveDataIn(
        this.editing,
        this.editing.curveData.startX,
        this.editing.curveData.startY,
        this.editing.element.x,
        this.editing.curveData.finishY,
        this.editing.curveData.angle
      );
      if (this.editingValuesChanged !== undefined) {
        this.editingValuesChanged(this.editing);
      }
      if (this.editing.point) this.editing.element = undefined;
      this.draw();
    }
    this.editing = editing;
  }

  /**
   * Sets the editing curve angle based on the user input.
   *
   * @param {number} amount
   * @returns {void}
   *
   */
  setCurveAngle(amount) {
    const editing = this.editing;
    if (this.selected) this.editing = this.selected[0];
    if (this.editing !== undefined && this.editing.curveData !== undefined) {
      const history = this.createHistory();
      this.addToUndoHistory(history);
      let newAngle = Math.max(Math.min(amount, 180), -180);
      this.editing.element.angle = newAngle;
      SketchUtils.createCurveDataIn(
        this.editing,
        this.editing.curveData.startX,
        this.editing.curveData.startY,
        this.editing.curveData.finishX,
        this.editing.curveData.finishY,
        newAngle
      );
      if (this.editingValuesChanged !== undefined) {
        this.editingValuesChanged(this.editing);
      }
      this.draw();
    }
    this.editing = editing;
  }

  /**
   * Sets the editing curve chord based on the user input.
   *
   * @param {number} amount
   * @returns {void}
   *
   */
  setCurveChord(amount) {
    const editing = this.editing;
    if (this.selected) {
      this.editing = this.selected[0];
      if (!this.editing.element) {
        this.editing.element = this.editing.path.elements[
          this.editing.path.elements.length - 1
        ];
      }
      if (isNaN(this.editing.element.angle) || this.editing.element.angle == 0)
        this.showContextBar("line");
    }
    if (
      this.editing !== undefined &&
      this.editing.curveData !== undefined &&
      amount > 1e-5
    ) {
      const segmentX =
        this.editing.curveData.finishX - this.editing.curveData.startX;
      const segmentY =
        this.editing.curveData.finishY - this.editing.curveData.startY;
      const length = Math.sqrt(segmentX * segmentX + segmentY * segmentY);
      if (length > 1e-5) {
        const history = this.createHistory();
        this.addToUndoHistory(history);
        const directionX = segmentX / length;
        const directionY = segmentY / length;
        const element = {
          x: this.editing.curveData.startX + directionX * amount,
          y: this.editing.curveData.startY + directionY * amount
        };
        if (
          this.editing.element.x == this.editing.path.start.x &&
          this.editing.element.y == this.editing.path.start.y
        ) {
          this.editing.path.start.x = element.x;
          this.editing.path.start.y = element.y;
        }
        this.editing.element.x = element.x;
        this.editing.element.y = element.y;
        SketchUtils.createCurveDataIn(
          this.editing,
          this.editing.curveData.startX,
          this.editing.curveData.startY,
          this.editing.element.x,
          this.editing.element.y,
          this.editing.curveData.angle
        );
        if (this.editingValuesChanged !== undefined) {
          this.editingValuesChanged(this.editing);
        }
        if (this.editing.point) this.editing.element = undefined;
        this.draw();
      }
      this.editing = editing;
    }
  }

  /**
   * Sets the editing curve height based on the user input.
   *
   * @param {number} amount
   * @returns {void}
   *
   */
  setCurveHeight(amount) {
    const editing = this.editing;
    if (this.selected) this.editing = this.selected[0];
    if (
      this.editing !== undefined &&
      this.editing.curveData !== undefined &&
      amount > 1e-5
    ) {
      const history = this.createHistory();
      this.addToUndoHistory(history);
      const deltaX =
        this.editing.curveData.finishX - this.editing.curveData.startX;
      const deltaY =
        this.editing.curveData.finishY - this.editing.curveData.startY;
      const length = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
      const directionX = -deltaY / length;
      const directionY = deltaX / length;
      const midX =
        (this.editing.curveData.startX + this.editing.curveData.finishX) / 2;
      const midY =
        (this.editing.curveData.startY + this.editing.curveData.finishY) / 2;
      const toPointX = midX + directionX * amount;
      const toPointY = midY + directionY * amount;
      const startToPointDeltaX = toPointX - this.editing.curveData.startX;
      const startToPointDeltaY = toPointY - this.editing.curveData.startY;
      const finishToPointDeltaX = toPointX - this.editing.curveData.finishX;
      const finishToPointDeltaY = toPointY - this.editing.curveData.finishY;
      const angleCosine =
        (startToPointDeltaX * finishToPointDeltaX +
          startToPointDeltaY * finishToPointDeltaY) /
        (Math.sqrt(
          startToPointDeltaX * startToPointDeltaX +
            startToPointDeltaY * startToPointDeltaY
        ) *
          Math.sqrt(
            finishToPointDeltaX * finishToPointDeltaX +
              finishToPointDeltaY * finishToPointDeltaY
          ));
      let angle = Math.PI - Math.acos(angleCosine);
      if (this.editing.curveData.angle < 0) {
        angle = -angle;
      }
      angle = Math.max(Math.min(Math.PI / 2, angle), -Math.PI / 2);
      this.editing.element.angle = Math.round((2 * angle * 180) / Math.PI);
      SketchUtils.createCurveDataIn(
        this.editing,
        this.editing.curveData.startX,
        this.editing.curveData.startY,
        this.editing.curveData.finishX,
        this.editing.curveData.finishY,
        this.editing.element.angle
      );
      if (this.editingValuesChanged !== undefined) {
        this.editingValuesChanged(this.editing);
      }
      this.draw();
    }
    this.editing = editing;
  }

  /**
   * Creates a history point.
   *
   * @returns {void}
   *
   */
  createHistory() {
    const levels = [];
    if (this.levels) {
      for (let i = 0; i < this.levels.length; i++) {
        levels.push(this.levels[i]);
      }
    }
    const layers = [];
    if (this.layers) {
      for (let i = 0; i < this.layers.length; i++) {
        const layer = this.layers[i].createCopy();
        layers.push(layer);
      }
    }
    let sketchLayerToEdit;
    let sketchObjectToEdit;
    let sketchPathToEdit;
    let editing;
    let sketchPathToEditPoint;
    if (this.layers) {
      for (let i = 0; i < this.layers.length; i++) {
        const sketchObject = this.layers[i].findRelatedSketchObject(
          this.sketchObjectToEdit,
          layers[i]
        );
        if (sketchObject) {
          sketchLayerToEdit = layers[i].uniqueIdentifier;
          sketchObjectToEdit = sketchObject;
          sketchPathToEdit = this.sketchObjectToEdit.findRelatedPath(
            this.sketchPathToEdit,
            sketchObjectToEdit
          );
          if (this.editing)
            editing = this.sketchObjectToEdit.findRelatedPath(
              this.editing.path,
              sketchObjectToEdit
            );
          sketchPathToEditPoint = this.sketchPathToEditPoint;
          break;
        }
      }
    }
    let cursor = undefined;
    if (this.cursor) {
      cursor = { x: this.cursor.x, y: this.cursor.y };
    }
    return {
      levels: levels,
      layers: layers,
      sketchLayerToEdit: sketchLayerToEdit ?? this.sketchLayerToEdit,
      sketchObjectToEdit: sketchObjectToEdit,
      sketchPathToEdit: sketchPathToEdit,
      sketchPathToEditPoint: sketchPathToEditPoint,
      cursor: cursor,
      editing: editing,
      xKeypad: this.xKeypad,
      yKeypad: this.yKeypad
    };
  }

  /**
   * Adds a history point to the Undo steps.
   *
   * @returns {void}
   *
   */
  addToUndoHistory(history) {
    if (this.undoHistory === undefined) {
      this.undoHistory = [history];
    } else {
      this.undoHistory.push(history);
    }
    this.redoHistory = undefined;
    adjustUndoRedoVisibility();
  }

  /**
   * Restores the sketch to the specified history point.
   *
   * @returns {Function}
   *
   */
  async restoreFromHistory(history) {
    this.levels = history.levels;
    this.layers = history.layers;
    this.layers.map(layer => {
      const currentLayer = this.getLayer(layer.uniqueIdentifier);
      if (currentLayer) layer.visible = currentLayer.visible;
    });
    this.sketchLayerToEdit = history.sketchLayerToEdit;
    this.sketchObjectToEdit = history.sketchObjectToEdit;
    this.sketchPathToEdit = history.sketchPathToEdit;
    if (this.editing) {
      this.editing.path = history.editing;
    }
    this.sketchPathToEditPoint = history.sketchPathToEditPoint;
    this.cursor =
      this.undoHistory.length > 0
        ? this.undoHistory[this.undoHistory.length - 1].cursor
        : history.cursor;
    this.clearSelection();
    this.xKeypad = history.xKeypad;
    this.yKeypad = history.yKeypad;
    return this.autoSave();
  }

  /**
   * Restores the sketch to the last history point.
   *
   * @returns {void}
   *
   */
  undo() {
    if (this.undoHistory !== undefined && this.undoHistory.length > 0) {
      this.dismissTextInput();
      let history = this.createHistory();
      if (this.redoHistory === undefined) {
        this.redoHistory = [history];
      } else {
        this.redoHistory.push(history);
      }
      history = this.undoHistory.pop();
      this.restoreFromHistory(history);
    }
  }

  /**
   * Returns the sketch to the last undone action.
   *
   * @returns {void}
   *
   */
  redo() {
    if (this.redoHistory !== undefined && this.redoHistory.length > 0) {
      this.dismissTextInput();
      let history = this.createHistory();
      if (this.undoHistory === undefined) {
        this.undoHistory = [history];
      } else {
        this.undoHistory.push(history);
      }
      history = this.redoHistory.pop();
      this.restoreFromHistory(history);
    }
  }

  /**
   * Shows the corresponding context bar as specified.
   *
   * @param {string} bar
   * @returns {void}
   *
   */
  showContextBar(bar) {
    let curveContextArea = document.getElementById("curveContextArea");
    let lineContextArea = document.getElementById("lineContextArea");
    let perimeterContextArea = document.getElementById("perimeterContextArea");
    lineContextArea.classList.remove("context-visible");
    curveContextArea.classList.remove("context-visible");
    perimeterContextArea.classList.remove("context-visible");
    switch (bar) {
      case "line":
        lineContextArea.classList.add("context-visible");
        break;
      case "curve":
        curveContextArea.classList.add("context-visible");
        break;
      case "area":
        perimeterContextArea.classList.add("context-visible");
        break;
      default:
        break;
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
    if (this.layers !== undefined) {
      for (let i = 0; i < this.layers.length; i++) {
        this.layers[i].closestPointTo(position, closest, exclude);
      }
    }
  }

  /**
   * Creates the Pen Down cursor graphic in Draw mode.
   *
   * @param {number} offsetX
   * @param {number} offsetY
   * @returns {void}
   */
  createCursor(offsetX, offsetY) {
    if (!this.cursor) {
      let position = undefined;
      if (offsetX === undefined || offsetY === undefined) {
        let upperLeft = this.inverseTopLevelTransform.transform(
          new paper.Point(0, 0)
        );
        let lowerRight = this.inverseTopLevelTransform.transform(
          new paper.Point(this.root.clientWidth, this.root.clientHeight)
        );
        position = {
          x: (upperLeft.x + lowerRight.x) / 2,
          y: (upperLeft.y + lowerRight.y) / 2
        };
      } else {
        position = this.inverseTopLevelTransform.transform(
          new paper.Point(offsetX, offsetY)
        );
      }
      this.cursor = this.newPosition(position.x, position.y);
    }
  }

  /**
   * Creates the Pen Up cursor graphic in Draw mode.
   *
   * @param {number} offsetX
   * @param {number} offsetY
   * @returns {void}
   */
  createPenUpCursor(offsetX, offsetY) {
    if (!this.penUpCursor) {
      let position;
      if (offsetX === undefined || offsetY === undefined) {
        let upperLeft = this.inverseTopLevelTransform.transform(
          new paper.Point(0, 0)
        );
        let lowerRight = this.inverseTopLevelTransform.transform(
          new paper.Point(this.root.clientWidth, this.root.clientHeight)
        );
        position = {
          x: (upperLeft.x + lowerRight.x) / 2,
          y: (upperLeft.y + lowerRight.y) / 2
        };
      } else {
        position = this.inverseTopLevelTransform.transform(
          new paper.Point(offsetX, offsetY)
        );
      }
      if (this.snapToLine && this.sketchPathToEdit) {
        const tooClose = this.isTooCloseToPath(position, this.sketchPathToEdit);
        if (tooClose.tooClose) {
          const closest =
            tooClose.closestPoint > 0
              ? this.sketchPathToEdit.elements[tooClose.closestPoint - 1]
              : this.sketchPathToEdit.start;
          position.x = closest.x;
          position.y = closest.y;
        }
      }
      this.penUpCursor = this.newPosition(position.x, position.y);
    }
  }

  /**
   * View -> Auto Fit event handler. Also triggered after a Zoom has been made.
   *
   * @returns {void}
   */
  autoFitClicked() {
    let autoFitImage = document.getElementById("autoFitImage");
    if (autoFitImage.style.visibility === "collapse") {
      autoFitImage.style.visibility = "inherit";
      this.autoFit = true;
      this.draw();
    } else {
      autoFitImage.style.visibility = "collapse";
      this.autoFit = false;
    }
    collapseViewMenu();
  }

  /**
   * Updates the savedLayers array with a copy of the current layers array.
   *
   * @returns {void}
   */
  updateSavedLayers() {
    if (this.layers && this.layers.length > 0) {
      this.savedLayers = [];
      for (let i = 0; i < this.layers.length; i++) {
        this.savedLayers.push(this.layers[i].createCopy());
      }
    }
  }

  /**
   * Loads the layers array with a copy of the savedLayers contents.
   *
   * @returns {void}
   */
  restoreSavedLayers() {
    if (this.savedLayers && this.savedLayers.length > 0) {
      this.layers = [];
      for (let i = 0; i < this.savedLayers.length; i++) {
        this.layers.push(this.savedLayers[i].createCopy());
      }
    }

    this.dismissTextInput();
  }

  /**
   * Updates the sum of all the layers' new areas within the totalsInfo.
   *
   * @returns {void}
   */
  updateNewTotalsInfo() {
    if (!this.layers) {
      return;
    }

    if (!this.totalsInfo) {
      //#region Structure of objects in totalsInfo.layers
      // 'uniqueIdentifier': 	{
      // 							name,
      // 							livingArea: { saved: 0, new: 0,	gap: 0 },
      // 							grossArea: { saved: 0, new: 0, gap: 0 },
      //              index
      // 						}
      //#endregion
      this.totalsInfo = {
        layers: {},
        totalLivingArea: {
          saved: 0,
          new: 0,
          gap: 0
        },
        totalGrossArea: {
          saved: 0,
          new: 0,
          gap: 0
        }
      };
    }

    this.totalsInfo.totalLivingArea.new = 0;
    this.totalsInfo.totalLivingArea.gap = 0;
    this.totalsInfo.totalGrossArea.new = 0;
    this.totalsInfo.totalGrossArea.gap = 0;

    for (let i = 0; i < this.layers.length; i++) {
      const layer = this.layers[i];
      let addToInfo = false;
      let infoLayer = this.totalsInfo.layers[layer.uniqueIdentifier];
      if (!infoLayer) {
        //If current layer is not in totalsInfo
        infoLayer = {
          name: layer.name,
          livingArea: { saved: 0, new: 0, gap: 0 },
          grossArea: { saved: 0, new: 0, gap: 0 },
          index: this.layerList[layer.name]
            ? this.layerList[layer.name].index
            : 1000
        };
      } else {
        //If current layer is already in totalsInfo
        infoLayer.livingArea.new = 0;
        infoLayer.livingArea.gap = 0;
        infoLayer.grossArea.new = 0;
        infoLayer.grossArea.gap = 0;
      }

      if (
        layer.objects &&
        layer.objects.length > 0 &&
        this.layerList[layer.name]
      ) {
        for (const obj of layer.objects) {
          if (obj.objects && obj.objects.length > 0) {
            for (const sketchObj of obj.objects) {
              if (sketchObj instanceof SketchPath) {
                const objArea = sketchObj.getArea(this);
                if (sketchObj.negative && objArea > 0) {
                  //Negative area
                  if (this.pathInsidePath(sketchObj, layer)) {
                    //Only subtract negative areas from layers that add (+) to net/gross areas
                    if (this.layerList[layer.name].livingArea === "+") {
                      infoLayer.livingArea.new -= objArea;
                      this.totalsInfo.totalLivingArea.new -= objArea;
                    }

                    if (this.layerList[layer.name].grossArea === "+") {
                      infoLayer.grossArea.new -= objArea;
                      this.totalsInfo.totalGrossArea.new -= objArea;
                    }
                  }
                } else {
                  //Positive area
                  if (this.layerList[layer.name].livingArea === "+") {
                    infoLayer.livingArea.new += objArea;
                    this.totalsInfo.totalLivingArea.new += objArea;
                  } else if (this.layerList[layer.name].livingArea === "-") {
                    infoLayer.livingArea.new -= objArea;
                    this.totalsInfo.totalLivingArea.new -= objArea;
                  }

                  if (this.layerList[layer.name].grossArea === "+") {
                    infoLayer.grossArea.new += objArea;
                    this.totalsInfo.totalGrossArea.new += objArea;
                  } else if (this.layerList[layer.name].grossArea === "-") {
                    infoLayer.grossArea.new -= objArea;
                    this.totalsInfo.totalGrossArea.new -= objArea;
                  }
                }

                addToInfo = true;
              }
            }
          }
        }
      }

      if (addToInfo) {
        //Add layer to totals info
        this.totalsInfo.layers[layer.uniqueIdentifier] = infoLayer;
      }
    }

    //Calculate total gaps
    this.totalsInfo.totalLivingArea.gap =
      this.totalsInfo.totalLivingArea.new -
      this.totalsInfo.totalLivingArea.saved;
    this.totalsInfo.totalGrossArea.gap =
      this.totalsInfo.totalGrossArea.new - this.totalsInfo.totalGrossArea.saved;

    //Calculate gaps of each layer
    const layerKeys = Object.keys(this.totalsInfo.layers);
    for (let i = 0; i < layerKeys.length; i++) {
      this.totalsInfo.layers[layerKeys[i]].livingArea.gap =
        this.totalsInfo.layers[layerKeys[i]].livingArea.new -
        this.totalsInfo.layers[layerKeys[i]].livingArea.saved;

      this.totalsInfo.layers[layerKeys[i]].grossArea.gap =
        this.totalsInfo.layers[layerKeys[i]].grossArea.new -
        this.totalsInfo.layers[layerKeys[i]].grossArea.saved;
    }
  }

  /**
   * Updates the sum of all the layers' saved areas within the totalsInfo.
   *
   * @returns {void}
   */
  updateSavedTotalsInfo() {
    if (!this.relatedEntity) {
      return;
    }

    let totalLivingAreaSaved = 0;
    let totalGrossAreaSaved = 0;
    //#region Structure of objects in totalsInfo.layers
    // 'uniqueIdentifier': 	{
    // 							name,
    // 							livingArea: { saved: 0, new: 0,	gap: 0 },
    // 							grossArea: { saved: 0, new: 0, gap: 0 },
    //              index
    // 						}
    //#endregion
    this.totalsInfo = {
      layers: {},
      totalLivingArea: {
        saved: 0,
        new: 0,
        gap: 0
      },
      totalGrossArea: {
        saved: 0,
        new: 0,
        gap: 0
      }
    };

    for (let i = 0; i < this.layers.length; i++) {
      const layer = this.layers[i];
      let addToInfo = false;
      let infoLayer = {
        name: layer.name,
        livingArea: { saved: 0, new: 0, gap: 0 },
        grossArea: { saved: 0, new: 0, gap: 0 },
        index: this.layerList[layer.name]
          ? this.layerList[layer.name].index
          : 1000
      };

      const layerEntityProperty = this.layerList[layer.name]?.entityProperty;
      if (
        layerEntityProperty &&
        typeof this.relatedEntity[layerEntityProperty] === "number" &&
        this.relatedEntity[layerEntityProperty] > 0
      ) {
        const layerArea = this.relatedEntity[layerEntityProperty];

        if (this.layerList[layer.name].livingArea === "+") {
          infoLayer.livingArea.saved += layerArea;
          totalLivingAreaSaved += layerArea;
        } else if (this.layerList[layer.name].livingArea === "-") {
          infoLayer.livingArea.saved -= layerArea;
          totalLivingAreaSaved -= layerArea;
        }

        if (this.layerList[layer.name].grossArea === "+") {
          infoLayer.grossArea.saved += layerArea;
          totalGrossAreaSaved += layerArea;
        } else if (this.layerList[layer.name].grossArea === "-") {
          infoLayer.grossArea.saved -= layerArea;
          totalGrossAreaSaved -= layerArea;
        }

        addToInfo = true;
      }

      if (addToInfo) {
        //Add layer to totals info
        this.totalsInfo.layers[layer.uniqueIdentifier] = infoLayer;
      }
    }

    this.totalsInfo.totalLivingArea.saved = totalLivingAreaSaved;
    this.totalsInfo.totalGrossArea.saved = totalGrossAreaSaved;
  }

  /**
   * Generates 'totalsInfo' object using area totals in sketch's related entity (saved) and in sketch version file (new).
   *
   * @param {SketchControl} sketchFile
   * @returns {void}
   */
  getSketchVersionTotals(sketchFile) {
    //#region Structure of objects in totalsInfo.layers
    // 'layerName': 	{
    //              name,
    // 							livingArea: { saved: 0, new: 0,	gap: 0 },
    // 							grossArea: { saved: 0, new: 0, gap: 0 },
    //              index
    // 						}
    //#endregion
    const totalsInfo = {
      layers: {},
      totalLivingArea: { saved: 0, new: 0, gap: 0 },
      totalGrossArea: { saved: 0, new: 0, gap: 0 }
    };
    let layerNames = Object.keys(this.layerList);
    const layerAreasInFile = SketchFromJSON.readAreas(sketchFile); //Array of objects -> 'layerName': { livingArea, grossArea }

    //Saved totals from related entity
    let totalLivingAreaSaved = 0;
    let totalGrossAreaSaved = 0;
    for (let i = 0; i < layerNames.length; i++) {
      const layerEntityProperty = this.layerList[layerNames[i]].entityProperty;

      if (
        typeof this.relatedEntity[layerEntityProperty] === "number" &&
        this.relatedEntity[layerEntityProperty] > 0 &&
        this.layerList[layerNames[i]].entityName === this.relatedEntityName &&
        layerAreasInFile[layerNames[i]]
      ) {
        if (this.layerList[layerNames[i]].livingArea) {
          if (this.layerList[layerNames[i]].livingArea === "-") {
            totalsInfo.layers[layerNames[i]] = {
              name: layerNames[i],
              livingArea: {
                saved: this.relatedEntity[layerEntityProperty] * -1,
                new: 0,
                gap: 0
              },
              index: this.layerList[layerNames[i]].index
            };
            totalLivingAreaSaved -= this.relatedEntity[layerEntityProperty];
          } else {
            totalsInfo.layers[layerNames[i]] = {
              name: layerNames[i],
              livingArea: {
                saved: this.relatedEntity[layerEntityProperty],
                new: 0,
                gap: 0
              },
              index: this.layerList[layerNames[i]].index
            };
            totalLivingAreaSaved += this.relatedEntity[layerEntityProperty];
          }
        }

        if (this.layerList[layerNames[i]].grossArea) {
          if (this.layerList[layerNames[i]].grossArea === "-") {
            totalsInfo.layers[layerNames[i]] = {
              name: layerNames[i],
              grossArea: {
                saved: this.relatedEntity[layerEntityProperty] * -1,
                new: 0,
                gap: 0
              },
              index: this.layerList[layerNames[i]].index
            };
            totalGrossAreaSaved -= this.relatedEntity[layerEntityProperty];
          } else {
            totalsInfo.layers[layerNames[i]] = {
              name: layerNames[i],
              grossArea: {
                saved: this.relatedEntity[layerEntityProperty],
                new: 0,
                gap: 0
              },
              index: this.layerList[layerNames[i]].index
            };
            totalGrossAreaSaved += this.relatedEntity[layerEntityProperty];
          }
        }
      }
    }
    totalsInfo.totalLivingArea.saved = totalLivingAreaSaved;
    totalsInfo.totalGrossArea.saved = totalGrossAreaSaved;

    //New totals from sketch file
    let totalLivingAreaNew = 0;
    let totalGrossAreaNew = 0;

    for (let i = 0; i < layerNames.length; i++) {
      if (
        layerAreasInFile[layerNames[i]] &&
        this.layerList[layerNames[i]].entityName === this.relatedEntityName
      ) {
        let infoLayer = totalsInfo.layers[layerNames[i]];
        if (!infoLayer) {
          infoLayer = {
            name: layerNames[i],
            livingArea: { saved: 0, new: 0, gap: 0 },
            grossArea: { saved: 0, new: 0, gap: 0 },
            index: this.layerList[layerNames[i]]
              ? this.layerList[layerNames[i]].index
              : 1000
          };
          totalsInfo.layers[layerNames[i]] = infoLayer;
        }

        if (this.layerList[layerNames[i]].livingArea) {
          infoLayer.livingArea.new = layerAreasInFile[layerNames[i]].livingArea;
          totalLivingAreaNew += layerAreasInFile[layerNames[i]].livingArea;
        }

        if (this.layerList[layerNames[i]].grossArea) {
          infoLayer.grossArea.new = layerAreasInFile[layerNames[i]].grossArea;
          totalGrossAreaNew += layerAreasInFile[layerNames[i]].grossArea;
        }
      }
    }
    totalsInfo.totalLivingArea.new = totalLivingAreaNew;
    totalsInfo.totalGrossArea.new = totalGrossAreaNew;

    //Calculate gaps
    totalsInfo.totalLivingArea.gap =
      totalsInfo.totalLivingArea.new - totalsInfo.totalLivingArea.saved;
    totalsInfo.totalGrossArea.gap =
      totalsInfo.totalGrossArea.new - totalsInfo.totalGrossArea.saved;
    layerNames = Object.keys(totalsInfo.layers);
    for (let i = 0; i < layerNames.length; i++) {
      if (this.layerList[layerNames[i]].livingArea) {
        totalsInfo.layers[layerNames[i]].livingArea.gap =
          totalsInfo.layers[layerNames[i]].livingArea.new -
          totalsInfo.layers[layerNames[i]].livingArea.saved;
      }

      if (this.layerList[layerNames[i]].grossArea) {
        totalsInfo.layers[layerNames[i]].grossArea.gap =
          totalsInfo.layers[layerNames[i]].grossArea.new -
          totalsInfo.layers[layerNames[i]].grossArea.saved;
      }
    }

    return totalsInfo;
  }

  /**
   * Checks whether the given path is inside any other path in the given layer
   *
   * @param {SketchPath} path
   * @param {SketchLayer} layer
   * @returns {boolean}
   */
  pathInsidePath(path, layer) {
    if (layer.objects && layer.objects.length > 0) {
      const pathArea = path.fillPath ? Math.abs(path.fillPath.area) : 0;
      for (const obj of layer.objects) {
        if (obj.objects && obj.objects.length > 0) {
          for (const sketchObj of obj.objects) {
            if (
              sketchObj instanceof SketchPath &&
              path.uniqueIdentifier != sketchObj.uniqueIdentifier
            ) {
              if (sketchObj.getArea(this) > 0) {
                let intersection = sketchObj.fillPath.intersect(path.fillPath);
                if (intersection) {
                  let area = Math.abs(intersection.area);
                  if (Math.abs(pathArea - area) <= 0.1) {
                    return true;
                  }
                }
              }
            }
          }
        }
      }
    }

    return false;
  }

  /**
   * Returns the layer associated with the given id.
   *
   * @param {string} id
   * @returns {SketchLayer}
   */
  getLayer(id) {
    return this.layers.find(layer => layer.uniqueIdentifier == id)
  }
}
