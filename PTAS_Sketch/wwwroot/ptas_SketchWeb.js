// ptas_SketchWeb.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
 * The visual elements and their respective functions are contained here.
 */

/**
 * Mouse down event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function sketchMouseDown(event) {
  collapseMenus();
  parent.sketchControl.down(event);
}

/**
 * Double click event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function sketchDoubleClick(event) {
  parent.sketchControl.doubleClick(event);
}

/**
 * Mouse move event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function sketchMouseMove(event) {
  parent.sketchControl.move(event);
}

/**
 * Mouse up event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function sketchMouseUp(event) {
  parent.sketchControl.up(event);
}

/**
 * Mouse enter event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function sketchMouseEnter(event) {
  parent.sketchControl.enter(event);
}

/**
 * Mouse wheel event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function sketchWheel(event) {
  parent.sketchControl.wheel(event);
}

/**
 * Touch gesture start event handler.
 *
 * @param {TouchEvent} event
 * @returns {void}
 */
function sketchGestureStart(event) {
  parent.sketchControl.gestureStart(event);
}

/**
 * Touch gesture change event handler.
 *
 * @param {TouchEvent} event
 * @returns {void}
 */
function sketchGestureChange(event) {
  parent.sketchControl.gestureChange(event);
}

/**
 * Touch gesture end event handler.
 *
 * @param {TouchEvent} event
 * @returns {void}
 */
function sketchGestureEnd(event) {
  parent.sketchControl.gestureEnd(event);
}

/**
 * Touch start event handler.
 *
 * @param {TouchEvent} event
 * @returns {void}
 */
function sketchTouchStart(event) {
  parent.sketchControl.touchStart(event);
}

/**
 * Touch move event handler.
 *
 * @param {TouchEvent} event
 * @returns {void}
 */
function sketchTouchMove(event) {
  parent.sketchControl.touchMove(event);
}

/**
 * Touch end event handler.
 *
 * @param {TouchEvent} event
 * @returns {void}
 */
function sketchTouchEnd(event) {
  parent.sketchControl.touchEnd(event);
}

/**
 * Touch cancel event handler.
 *
 * @param {TouchEvent} event
 * @returns {void}
 */
function sketchTouchCancel(event) {
  parent.sketchControl.touchCancel(event);
}

/**
 * Key down event handler.
 *
 * @param {KeyboardEvent} event
 * @returns {void}
 */
function sketchKeyDown(event) {
  if (
    document.activeElement === undefined ||
    !(document.activeElement instanceof HTMLInputElement)
  ) {
    if (event.ctrlKey) {
      event.preventDefault();
      event.stopPropagation();
      switch (event.keyCode) {
        case 65: // A
          if (
            parent.sketchControl.mode == SketchMode.SingleSelect ||
            parent.sketchControl.mode == SketchMode.MultipleSelect
          )
            selectAllClicked();
          break;
        case 73: // I
          importClicked();
          break;
        case 76: // L
          if (event.shiftKey) addLevel();
          else addLayer();
          break;
        case 82: // R
          redoClicked();
          break;
        case 83: // S
          saveClicked("saveModal");
          break;
        case 90: // Z
          if (event.shiftKey) redoClicked();
          else undoClicked();
          break;
      }
    } else if (
      event.key === "A" ||
      event.key === "a" ||
      event.key === "S" ||
      event.key === "s"
    ) {
      selectClicked();
    } else if (event.key === "I" || event.key === "i") {
      infoClicked();
    } else if (event.key === " ") {
      event.preventDefault();
      event.stopPropagation();
      panClicked();
      parent.sketchControl.panning = true;
    }
    if (
      event.key !== "S" &&
      event.key !== "s" &&
      event.key !== "D" &&
      event.key !== "d" &&
      event.key !== "T" &&
      event.key !== "t" &&
      event.key !== "P" &&
      event.key !== "p"
    ) {
      parent.sketchControl.keyDown(event);
    }
  }
}

/**
 * Displays the dialog element for a fixed time.
 *
 * @param {string} content - Dialog text
 * @returns {void}
 */
function showDialog(content) {
  let dialog = document.getElementById("dialog");
  dialog.textContent = content;
  if (!dialog.classList.contains("animated")) {
    dialog.classList.add("animated");
    setTimeout(() => dialog.classList.remove("animated"), 3000);
  }
}

/**
 * Displays the loading element.
 *
 * @param {string} content - Dialog text
 * @returns {void}
 */
function showLoading(content) {
  if (!content) {
    content = "Loading...";
  }

  let dialog = document.getElementById("loading");
  let dialogContent = dialog.getElementsByClassName("content")[0];
  dialogContent.textContent = content;
  if (!dialog.classList.contains("visible")) {
    dialog.classList.add("visible");
  }
}

/**
 * Collapses the loading element.
 *
 * @returns {void}
 */
function hideLoading() {
  let dialog = document.getElementById("loading");
  dialog.classList.remove("visible");
}

/**
 * Adds a Level to the Layers panel.
 *
 * @returns {void}
 */
function addLevel() {
  if (!parent.sketchControl.levels) {
    parent.sketchControl.levels = [];
  }
  let indexForName = 1;
  let found;
  let name;
  do {
    found = false;
    name = "Level " + indexForName;
    for (let i = 0; i < parent.sketchControl.levels.length; i++) {
      if (name === parent.sketchControl.levels[i].name) {
        found = true;
        indexForName++;
        break;
      }
    }
  } while (found);
  let level = {
    uniqueIdentifier: SketchControl.uuidv4(),
    name: name,
    area: 0,
    visible: true,
    layers: []
  };
  parent.sketchControl.levels.unshift(level);
  parent.sketchControl.draw();
  renameLevelClicked(level.uniqueIdentifier);
}

/**
 * Adds a Layer to the Layers panel.
 *
 * @param {string} visibility - CSS visibility value
 * @returns {void}
 */
function addLayer(visibility) {
  let layerTypes = document.getElementById("layer-types");
  if (visibility) {
    layerTypes.style.visibility = visibility;
  } else {
    layerTypes.style.visibility == "visible"
      ? (layerTypes.style.visibility = "collapse")
      : (layerTypes.style.visibility = "visible");
  }
}

/**
 * Import button handler.
 *
 * @returns {void}
 */
function importClicked() {
  let importRecent = document
    .getElementById("import-recent")
    .getElementsByClassName("row")[0];
  let importModal = document.getElementById("importModal");
  importModal.style.visibility = "inherit";
  importRecent.innerHTML = `Loading sketches...`;
  SketchAPIService.getRecentSketches(parent.sketchControl.sketchAccessToken)
    .then(data => {
      importRecent.innerHTML = ``;
      if (data) {
        data.map(sketch => {
          const sketchContainer = document.createElement("div");
          sketchContainer.classList.add("thumbnail-large-container");
          sketchContainer.setAttribute("tabindex", "0");
          const sketchThumbnail = document.createElement("div");
          sketchThumbnail.classList.add("thumbnail-large");
          sketchThumbnail.innerHTML = `<div class="parcel-number">${sketch.parcelName}</div>
                                        ${sketch.svg}
                                        <div class="parcel-address">${sketch.address ?? 'N/A'}</div>`;
          sketchThumbnail.onmousedown = event => {
            selectSketchTemplate(event);
          };
          const confirmPopup = getConfirmImportSketchPopup(event, sketch);
          sketchContainer.appendChild(sketchThumbnail);
          sketchContainer.appendChild(confirmPopup);
          importRecent.appendChild(sketchContainer);
        });
      }
    })
    .catch(err => {
      console.error(err);
      return;
    });
}

/**
 * Creates an Import confirmation dialog.
 *
 * @param {Event} event
 * @param {SketchControl} sketch
 * @returns {HTMLElement}
 */
function getConfirmImportSketchPopup(event, sketch) {
  const confirmPopup = document.createElement("div");
  confirmPopup.classList.add("popover__content");
  confirmPopup.innerHTML = `<div>
                                <span>Importing from a template will overwrite your current sketch.</span>
                            </div>`;
  const button = document.createElement("button");
  button.innerHTML = "Import";
  button.onclick = () => {
    importSketchTemplate(sketch);
  };
  confirmPopup.appendChild(button);

  return confirmPopup;
}

/**
 * Imports a template from the Control instance.
 *
 * @param {Event} event
 * @param {SketchControl} sketch - Control instance
 * @returns {void}
 */
function importSketchTemplate(sketch) {
  const history = parent.sketchControl.createHistory();
  parent.sketchControl.addToUndoHistory(history);
  if (
    parent.sketchControl.sketchEntity &&
    parent.sketchControl.sketchEntity["ptas_iscomplete"]
  ) {
    showLoading("Importing...");
    let newSketchEntity = SketchEntitiesHandler.createSketchObject(
      parent.sketchControl.relatedEntity,
      parent.sketchControl.relatedEntityName,
      parent.sketchControl.userData,
      parent.sketchControl.sketchEntity
    );
    newSketchEntity["ptas_isofficial"] = false;
    newSketchEntity["ptas_iscomplete"] = false;
    newSketchEntity["_ptas_templateid_value"] = sketch.sketchId;

    SketchAPIService.getSketch(
      sketch.sketchId,
      parent.sketchControl.sketchAccessToken,
      parent.sketchControl.relatedEntityId,
      parent.sketchControl.relatedEntityName
    )
      .then(getSketchRes => {
        if (getSketchRes) {
          parent.sketchControl.clear();
          SketchFromJSON.read(getSketchRes.sketch, parent.sketchControl);
          parent.sketchControl.sketchEntity = newSketchEntity;
          parent.sketchControl.sketchEntityLoaded = { ...newSketchEntity }
          parent.sketchControl.sketchEntityId =
            newSketchEntity["ptas_sketchid"];
          let projection = { center: {}, projected: [], index: 0 };
          parent.sketchControl.findProjection(projection);
          if (projection.projected.length > 0) {
            parent.sketchControl.zoomToContents(projection.bounds, 0.7);
          }
          addScratchpadLayer();
          //Update saved layers once the sketch has been drawn (and fillPath is set)
          if (
            parent.sketchControl.layers &&
            parent.sketchControl.layers.length > 0
          ) {
            parent.sketchControl.updateSavedLayers();
            parent.sketchControl.updateSavedTotalsInfo(
              parent.sketchControl.savedLayers
            );
          }

          SketchAPIService.upsertSketch(
            newSketchEntity.ptas_sketchid,
            JSON.parse(SketchToJSON.write(parent.sketchControl)),
            newSketchEntity,
            parent.sketchControl.sketchAccessToken
          )
            .then(upsertRes => {
              const recent = document
                .getElementById("import-recent")
                .getElementsByClassName("row")[0];
              const results = document
                .getElementById("import-results")
                .getElementsByClassName("row")[0];
              while (recent.firstChild) {
                recent.removeChild(recent.lastChild);
              }
              while (results.firstChild) {
                results.removeChild(results.lastChild);
              }
              closeModal("importModal");
              if (!upsertRes) {
                showDialog("Error updating sketch with template data");
              }
            })
            .catch(err => {
              console.error(err);
              showLoading(err);
              return;
            });
          hideLoading();
        } else {
          hideLoading();
          showDialog("Could not get sketch template.");
        }
      })
      .catch(err => {
        console.error(err);
        showLoading(err);
        return;
      });
  } else {
    showLoading("Importing...");
    SketchAPIService.getSketch(
      sketch.sketchId,
      parent.sketchControl.sketchAccessToken,
      parent.sketchControl.relatedEntityId,
      parent.sketchControl.relatedEntityName
    )
      .then(getSketchRes => {
        if (getSketchRes) {
          parent.sketchControl.clear();
          SketchFromJSON.read(getSketchRes.sketch, parent.sketchControl);
          parent.sketchControl.sketchEntity["_ptas_templateid_value"] =
            sketch.sketchId;
          let projection = { center: {}, projected: [], index: 0 };
          parent.sketchControl.findProjection(projection);
          if (projection.projected.length > 0) {
            parent.sketchControl.zoomToContents(projection.bounds, 0.7);
          }
          //Update saved layers once the sketch has been drawn (and fillPath is set)
          if (
            parent.sketchControl.layers &&
            parent.sketchControl.layers.length > 0
          ) {
            parent.sketchControl.updateSavedLayers();
            parent.sketchControl.updateSavedTotalsInfo(
              parent.sketchControl.savedLayers
            );
          }
          parent.sketchControl.autoSave();
          const recent = document
            .getElementById("import-recent")
            .getElementsByClassName("row")[0];
          const results = document
            .getElementById("import-results")
            .getElementsByClassName("row")[0];
          while (recent.firstChild) {
            recent.removeChild(recent.lastChild);
          }
          while (results.firstChild) {
            results.removeChild(results.lastChild);
          }
          closeModal("importModal");
          hideLoading();
        } else {
          hideLoading();
          showDialog("Could not get sketch template.");
        }
      })
      .catch(err => {
        console.error(err);
        showLoading(err);
        return;
      });
  }
}

/**
 * Template selection handler.
 *
 * @param {Event} event
 * @returns {void}
 */
function selectSketchTemplate(event) {
  let thumbnails = document
    .getElementById("importModal")
    .getElementsByClassName("thumbnail-large");
  for (let i = 0; i < thumbnails.length; i++) {
    thumbnails[i].classList.remove("sketchTemplateSelected");
  }
  event.currentTarget.classList.add("sketchTemplateSelected");
}

/**
 * Performs a search based on the Import Search field value
 *
 * @returns {void}
 */
function searchClicked() {
  const searchField = document.getElementById("searchField");
  const importResults = document.getElementById("import-results");
  const results = importResults.getElementsByClassName("row")[0];
  document.getElementById("import-recent").style.display = "none";
  if (searchField.value.length > 0) {
    importResults.style.visibility = "inherit";
    results.innerHTML = `Loading sketches...`;
    SketchAPIService.searchStringInSketch(
      searchField.value,
      parent.sketchControl.sketchAccessToken
    ).then(data => {
      if (data) {
        results.innerHTML = ``;
        data.map(sketch => {
          const sketchContainer = document.createElement("div");
          sketchContainer.classList.add("thumbnail-large-container");
          sketchContainer.setAttribute("tabindex", "0");
          const sketchThumbnail = document.createElement("div");
          sketchThumbnail.classList.add("thumbnail-large");
          sketchThumbnail.innerHTML = `<div class="parcel-number">${
            sketch.parcelName
          }</div>
                                            ${
                                              sketch.svg == "{}"
                                                ? ""
                                                : sketch.svg
                                            }
                                            <div class="parcel-address">${
                                              sketch.address ?? 'N/A'
                                            }</div>`;
          sketchThumbnail.onmousedown = () => {
            selectSketchTemplate(event);
          };
          const confirmPopup = getConfirmImportSketchPopup(event, sketch);
          sketchContainer.appendChild(sketchThumbnail);
          sketchContainer.appendChild(confirmPopup);
          results.appendChild(sketchContainer);
        });
      }
    });
  } else {
    importResults.style.visibility = "collapse";
    document.getElementById("import-recent").style.display = "block";
  }
}

/**
 * Import Search event handler
 *
 * @param {Event} event - Input event
 * @returns {void}
 */
function checkSearch(event) {
  if (event.key) searchClicked();
}

/**
 * Closes all menus on all sections.
 *
 * @returns {void}
 */
function collapseMenus() {
  closeDropdowns();
  collapseDiscardMenu();
  collapseViewMenu();
  collapseTransformMenu();
  collapseAllCopyAndMoveToMenus();
  addLayer("collapse");
  document.getElementById("helpMenu").style.visibility = "collapse";
  hideKeypadInput();
}

/**
 * Help event handler
 *
 * @returns {void}
 */
function helpClicked() {
  let menu = document.getElementById("helpMenu");
  if (menu.style.visibility !== "collapse") {
    menu.style.visibility = "collapse";
  } else {
    collapseMenus();
    menu.style.visibility = "inherit";
  }
}

/**
 * Quick Help event handler
 *
 * @returns {void}
 */
function quickHelpClicked() {
  document.getElementById("helpMenu").style.visibility = "collapse";
  let overlay = document.getElementById("help-overlay");
  overlay.style.visibility = "visible";
  overlay.classList.add("fadingIn");
}

/**
 * Reference Manual event handler
 *
 * @returns {void}
 */
function referenceManualClicked() {
  document.getElementById("helpMenu").style.visibility = "collapse";
  let topic;
  switch (parent.sketchControl.lastAction) {
    case "multiple":
      topic = "652495a1b4c940ea99439acf0263e2c8";
      break;
    case "single":
      topic = "c1618bcb31b84e248a24daf4aae808fc";
      break;
    case "negative select":
      topic = "15abd259b5144cd3be0f53c999108e6b";
      break;
    case "transform":
      topic = "2bd8fca2d72f46c4ab2f09c8d765e400";
      break;
    case "delete":
      topic = "c19d66bf3c6d458393806117ab993f65";
      break;
    case "copy/move":
      topic = "dbd59b71b78c4c54b053e8b1b3568e75";
      break;
    case "pen up":
      topic = "c983993af97c4ae6ade7f2dda9466994";
      break;
    case "pen down":
      topic = "967b597372984e10976d9c247d9a6aed";
      break;
    case "negative pen":
      topic = "b5c46a35aa964b45b52807bd6f284e15";
      break;
    case "open area":
      topic = "8d73a8eb48df4ff68816c8f9f6205f1b";
      break;
    case "closed area":
      topic = "d4400005931540329d33505775c22509";
      break;
    case "no text":
      topic = "052a0303e8d84adfb6037928c1a2f04e";
      break;
    case "text":
      topic = "707ad335ee054cdab769f4ba29f46973";
      break;
    case "distance":
      topic = "20cc3323c7ce487cbcca238acbd06654";
      break;
    case "pan":
      topic = "bcb44d74476c45a999a18b9f7042eeb5";
      break;
    case "keypad":
      topic = "20b5ca7fb417423a9c57ec5f0bda2949";
      break;
  }
  window.open(
    "https://ptaslearning-app-production.azurewebsites.net/?contextid=" + topic,
    "_blank"
  );
}

/**
 * Collapses the specified element.
 *
 * @param {HTMLElement} obj - Element to collapse
 * @returns {void}
 */
function collapseOverlay(obj) {
  obj.style.visibility = "collapse";
  obj.classList.remove("fadingIn");
}

/**
 * Info event handler.
 *
 * @returns {void}
 */
function infoClicked() {
  showSketchTags(parent.sketchControl.sketchEntity, "infoSketchTags");

  const infoLoaded = loadTotalsInfo("infoModal");
  if (infoLoaded) {
    let infoModal = document.getElementById("infoModal");
    infoModal.style.visibility = "visible";
  } else {
    showDialog("No data to show in current sketch.");
  }

  parent.sketchControl.versionsContainerId = "infoTabVersionsContent";
  showSketchVersions(parent.sketchControl.versionsContainerId);
}

/**
 * Populates the modal's info chart.
 *
 * @param {string} modalId
 * @returns {boolean}
 */
function loadTotalsInfo(modalId) {
  parent.sketchControl.updateNewTotalsInfo();
  if (!parent.sketchControl.layers || !parent.sketchControl.totalsInfo) {
    return false;
  }

  const areaTotalsDiv = document
    .getElementById(modalId)
    .getElementsByClassName("tabTotalsContent")[0];
  while (areaTotalsDiv.firstChild) {
    areaTotalsDiv.removeChild(areaTotalsDiv.lastChild);
  }
  const areaTotalsTableTemplate = document.getElementById(
    "areaTotalsTableTemplate"
  );
  const areaTotalsTable = areaTotalsTableTemplate.content
    .cloneNode(true)
    .querySelector(".table-template");
  const totals = parent.sketchControl.totalsInfo;
  populateAreaTotalsTable(
    modalId,
    areaTotalsTable.querySelector("table"),
    totals,
    areaTotalsTable.querySelector(".list-container")
  );
  areaTotalsDiv.appendChild(areaTotalsTable);

  parent.sketchControl.previousTotals = parent.sketchControl.totalsInfo;
  return true;
}

/**
 * Updates the living and gross area values from each layer to the entity.
 *
 * @returns {void}
 */
function updateEntityFieldValues(event) {
  // Only try to update if the related entity is known.
  if (
    parent.sketchControl.relatedEntityName &&
    parent.sketchControl.relatedEntity
  ) {
    let relatedEntity = parent.sketchControl.relatedEntity;
    if (event) {
      event.target.style.visibility = 'collapse'
      parent.sketchControl.layers.forEach(layer => {
        const layerName = parent.sketchControl.layerList[layer.name]
        if (layerName) {
          relatedEntity[layerName.entityProperty] = Math.abs(parseInt(layer.area));
        }
      })
    }
    else {
      const livingSaved = document.getElementsByName("livingSaved");
      const grossSaved = document.getElementsByName("grossSaved");
      livingSaved.forEach(input => {
        const entityProperty = input.getAttribute("data-entity-prop");
        const layerName = input.getAttribute("data-layer-name");
        if (
          parent.sketchControl.layerList[layerName] &&
          parent.sketchControl.layerList[layerName].entityName ===
          parent.sketchControl.relatedEntityName
        ) {
          relatedEntity[entityProperty] = Math.abs(parseInt(input.value));
        }
      });
      grossSaved.forEach(input => {
        const entityProperty = input.getAttribute("data-entity-prop");
        const layerName = input.getAttribute("data-layer-name");
        if (
          parent.sketchControl.layerList[layerName] &&
          parent.sketchControl.layerList[layerName].entityName ===
          parent.sketchControl.relatedEntityName
        ) {
          relatedEntity[entityProperty] = Math.abs(parseInt(input.value));
        }
      });
    }

    showLoading("Saving...");
    SketchAPIService.upsertGenericEntity(
      parent.sketchControl.relatedEntityName,
      parent.sketchControl.relatedEntityId,
      relatedEntity,
      parent.sketchControl.sketchAccessToken
    )
      .then(data => {
        hideLoading();
        if (data) {
          closeModal("updateModal");
          showDialog("Values updated successfully");
        } else {
          showDialog("Error updating values");
        }
      })
      .catch(err => {
        console.error(err);
        showLoading(err);
        return;
      });
  }
}

/**
 * Displays the contents of the specified tab.
 *
 * @param {string} modalId - Parent modal
 * @param {string} tabName - Tab to show within the modal
 * @returns {void}
 */
function showModalTab(modalId, tabName) {
  const modal = document.getElementById(modalId);
  let tabs = modal.getElementsByClassName("modal-tab");
  let contents = modal.getElementsByClassName("modal-tab-content");
  for (let i = 0; i < tabs.length; i++) {
    tabs[i].children[0].style.borderBottom = "none";
    tabs[i].children[0].style.color = "rgba(0,0,0,0.4)";
    contents[i].style.display = "none";
  }
  document.getElementById(tabName + "Content").style.display = "block";
  document.getElementById(tabName).children[0].style.borderBottom =
    "3px #a5c727 solid";
  document.getElementById(tabName).children[0].style.color = "black";
}

/**
 * Key up event handler.
 *
 * @param {KeyboardEvent} event - Input event
 * @returns {void}
 */
function sketchKeyUp(event) {
  if (
    document.activeElement === undefined ||
    !(document.activeElement instanceof HTMLInputElement)
  ) {
    if (event.key === "P" || event.key === "p") {
      if (parent.sketchControl.mode == SketchMode.Draw) {
        if (parent.sketchControl.penDown) {
          penUpClicked();
        } else {
          penDownClicked();
        }
      }
    } else if (event.key === "D" || event.key === "d") {
      drawClicked();
    } else if (event.key === "T" || event.key === "t") {
      textClicked();
    } else if (event.key === "F" || event.key === "f") {
      fitClicked();
    } else if (event.key === "L" || (event.key === "l" && !event.ctrlKey)) {
      layersClicked();
    } else if (event.key === "V" || event.key === "v") {
      viewClicked();
    } else if (event.key === "G" || event.key === "g") {
      hideGridlinesClicked();
    } else if (event.key === "H" || event.key === "h") {
      layerVisibilityClicked(event, parent.sketchControl.highlightedLayer);
    } else if (event.key === "M" || event.key === "m") {
      moveToClicked("select");
    } else if (event.key === "C" || event.key === "c") {
      copyToClicked("select");
    } else if (event.key === "?") {
      helpClicked();
    } else if (event.key === "+") {
      parent.sketchControl.zoom(1.1);
    } else if (event.key === "-") {
      parent.sketchControl.zoom(0.9);
    } else if (event.key === "Delete") {
      if (parent.sketchControl.mode == SketchMode.Draw) {
        keypadDeleteClicked();
      } else {
        parent.sketchControl.deleteSelection();
      }
    } else if (event.key === "Backspace") {
      this.undoClicked();
    } else if (
      event.key === "ArrowUp" ||
      event.key === "ArrowRight" ||
      event.key === "ArrowDown" ||
      event.key === "ArrowLeft"
    ) {
      keyboardArrowPressed(event);
    } else if (
      ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"].indexOf(event.key) >= 0
    ) {
      keypadNumberClicked(event.key);
    } else if (event.key === "'" || event.key === '"' || event.key === "-") {
      keypadDashClicked();
    } else if (event.key == "Escape") {
      if (parent.sketchControl.mode == SketchMode.Draw) {
        keypadEscClicked();
      }
      for (
        let i = 0;
        i < document.getElementsByClassName("modal").length;
        i++
      ) {
        let modal = document.getElementsByClassName("modal")[i];
        if (
          modal.style.visibility == "visible" ||
          modal.style.visibility == "inherit"
        ) {
          closeModal(modal.id);
          break;
        }
      }
    } else {
      if (
        event.key === "Enter" &&
        parent.sketchControl.mode == SketchMode.Draw
      ) {
        keypadEnterClicked();
      } else {
        parent.sketchControl.keyUp(event);
      }
    }
  }
}

/**
 * Arrow key event handler.
 *
 * @param {KeyboardEvent} event - Input event
 * @returns {void}
 */
function keyboardArrowPressed(event) {
  if (event.key === "ArrowUp") {
    keypadInputArrow = 8;
    keypadArrowClicked(event);
  } else if (event.key === "ArrowRight") {
    keypadInputArrow = 6;
    keypadArrowClicked(event);
  } else if (event.key === "ArrowDown") {
    keypadInputArrow = 2;
    keypadArrowClicked(event);
  } else if (event.key === "ArrowLeft") {
    keypadInputArrow = 4;
    keypadArrowClicked(event);
  }
}

/**
 * Level selection handler.
 *
 * @param {MouseEvent} event - Input event
 * @param {string} uniqueIdentifier - Level ID
 * @returns {void}
 */
function selectLevel(event, uniqueIdentifier) {
  let sketchLayers = document.getElementById("sketchLayersLayers");
  for (let i = 0; i < sketchLayers.children.length; i++) {
    if (sketchLayers.children[i].className)
      sketchLayers.children[i].style.backgroundColor = "white";
    else sketchLayers.children[i].style.backgroundColor = "black";
  }
  parent.sketchControl.levels.map(level => {
    level.selected = false;
  });
  let selectedLevel = parent.sketchControl.levels.find(
    level => level.uniqueIdentifier == uniqueIdentifier
  );
  selectedLevel.selected = true;
  event.target.style.backgroundColor = "#a5c727";
  let selectedLayer = selectedLevel.layers.find(
    layer => layer.uniqueIdentifier == parent.sketchControl.sketchLayerToEdit
  );
  if (!selectedLayer) {
    if (selectedLevel.layers && selectedLevel.layers.length > 0) {
      parent.sketchControl.sketchLayerToEdit =
        selectedLevel.layers[0].uniqueIdentifier;
    } else {
      parent.sketchControl.sketchLayerToEdit =
        parent.sketchControl.defaultSketchLayerToEdit;
    }
    loadLayersMenu();
  }
}

/**
 * Generate the Layers menu.
 *
 * @returns {void}
 */
function loadLayersMenu() {
  let sketchLayers = document.getElementById("sketchLayersLayers");
  while (sketchLayers.firstElementChild !== null) {
    sketchLayers.removeChild(sketchLayers.firstElementChild);
  }
  if (parent.sketchControl.levels === undefined) {
    return;
  }
  for (let i = 0; i < parent.sketchControl.levels.length; i++) {
    const level = parent.sketchControl.levels[i];
    let levelArea = 0;
    let levelDifference = 0;
    let cell = document.createElement("div");
    cell.style.position = "relative";
    cell.style.height = "34px";
    cell.style.backgroundColor = level.selected ? "#a5c727" : "black";
    cell.addEventListener("click", event =>
      selectLevel(event, level.uniqueIdentifier)
    );
    if (!level.locked) {
      cell.setAttribute("ondragover", "allowDrop(event)");
      cell.setAttribute("ondrop", "levelDrop(event)");
    }
    cell.setAttribute("name", level.name);
    const visibility = level.visible ? "visible" : "hidden";
    if (level.isScratchpad) {
      cell.innerHTML =
        '<div class="sketchLayerVisibilityBox" onclick="levelVisibilityClicked(\'' +
        level.uniqueIdentifier +
        '\')"><img src="ptas_' +
        visibility +
        '.png" style="position: absolute; width: 20px; height: 13px; top: 10px; left: 7px;" /></div><img src="ptas_scratchpad.png" style="position: absolute; width: 20px; height: 20px; top: 8px; left: 45px; pointer-events: none;" /><span class="sketchClickableText" style="position: absolute; top: 8px; left: 75px; pointer-events: none;">' +
        level.name +
        "</span>";
    } else {
      cell.innerHTML = `
				<div class="sketchLayerVisibilityBox" onclick="levelVisibilityClicked('${level.uniqueIdentifier}')">
					<img src="ptas_${visibility}.png" style="position: absolute; width: 20px; height: 13px; top: 10px; left: 7px; pointer-events: none;" />
				</div>
				<input type="text" class="sketchClickableText level-input" value="${level.name}" onkeypress="renameLevel(event, '${level.uniqueIdentifier}')" onblur="renameLevel(event, '${level.uniqueIdentifier}')">
				<div tabindex=0 class="sketchLevelOptions" onclick="sketchLevelOptionsClicked(event, this)">
					<img src="ptas_more.png" style="position: absolute; width: 20px; height: 20px; top: 7px; right: 5px" />
					<div id="popover_${level.uniqueIdentifier}" class="popover__content">
						<span class="popoverRenameLevelTitle" onclick="renameLevelClicked('${level.uniqueIdentifier}')"
							style="cursor: pointer; margin: 0; color: white;">
								Rename
						</span>
						<span class="popoverDeleteLevelTitle" onclick="deleteLevelClicked('${level.uniqueIdentifier}')"
							style="cursor: pointer; color: red;">
								Delete
						</span>
					</div>
				</div>`;
    }
    sketchLayers.appendChild(cell);
    if (level.layers === undefined) {
      continue;
    }
    for (let j = 0; j < level.layers.length; j++) {
      const entry = level.layers[j];
      for (let k = 0; k < parent.sketchControl.layers.length; k++) {
        const layer = parent.sketchControl.layers[k];
        let layerStyle = getLayerStyle(layer);
        if (layer.uniqueIdentifier === entry.uniqueIdentifier) {
          let cell = document.createElement("div");
          cell.className = "sketchCopyAndMoveToMenuItem";
          cell.setAttribute(
            "onclick",
            'sketchLayerMenuItemClicked("' + layer.uniqueIdentifier + '")'
          );
          cell.setAttribute(
            "onmouseover",
            'sketchLayerMenuItemMouseOver(this, "' +
              layer.uniqueIdentifier +
              '")'
          );
          cell.setAttribute("onmouseout", "sketchLayerMenuItemMouseOut(this)");
          if (!level.locked) {
            cell.setAttribute("draggable", "true");
            cell.setAttribute("ondragstart", "drag(event)");
            cell.setAttribute("ondragover", "allowDrop(event)");
            cell.setAttribute("ondrop", "layerDrop(event)");
          }
          cell.setAttribute("name", layer.name);
          const layerVisibility = layer.visible ? "visible" : "hidden";
          let area = 0;
          if (layer.objects) {
            layer.objects.map(object => {
              if (
                object.objects &&
                object.objects.length > 0 &&
                object.objects[0].closed
              ) {
                area += object.objects[0].getArea(parent.sketchControl);
              }
            });
            levelDifference += layer.grossArea
              ? layer.grossArea
              : layer.netArea
              ? layer.netArea
              : 0;
          }
          levelArea += area;
          let layerDifference = layer.grossArea
            ? layer.grossArea
            : layer.netArea
            ? layer.netArea
            : 0;
          if (
            layer.uniqueIdentifier === parent.sketchControl.sketchLayerToEdit
          ) {
            layerDifference = Math.round(layer.area - layerDifference);
            cell.style.backgroundColor = "#d4e693";
            if (parent.sketchControl.urlParams.get("readonly")) {
              cell.innerHTML = `<div class="sketchLayerVisibilityBox" onclick="layerVisibilityClicked(event, '${
                layer.uniqueIdentifier
              }')">
								<img src="ptas_${layerVisibility}_highlighted.png" style="position: absolute; width: 20px; height: 13px; top: 10px; left: 7px" />
							</div><div class="legend ${layerStyle}"></div>
							<span class="sketchLayerClickableText" style="position: absolute; top: 8px; left: 81px; pointer-events: none">${
                layer.name
              }</span>
                <div style="display: flex; align-items: center; position: absolute; right: 29px; top: 6px;">
							<span style="height: 20px; top: 7px; right: 5px">${Math.round(
                layer.area
              )} ⏍</span>
                    </div>
						`;
            } else {
              cell.innerHTML = `<div class="sketchLayerVisibilityBox" onclick="layerVisibilityClicked(event, '${
                layer.uniqueIdentifier
              }')">
								<img src="ptas_${layerVisibility}_highlighted.png" style="position: absolute; width: 20px; height: 13px; top: 10px; left: 7px" />
							</div><div class="legend ${layerStyle}"></div>
							<span class="sketchLayerClickableText" style="position: absolute; top: 8px; left: 81px; pointer-events: none">${
                layer.name
              }</span>
                <div style="display: flex; align-items: center; position: absolute; right: 5px;">
					<span style="height: 20px; top: 7px; right: 5px">${Math.round(
            layer.area
          )} ⏍</span>
							&nbsp;<div tabindex=0 class="sketchLayerOptions" onclick="sketchLayerOptionsClicked(event, this)">
								<img src="ptas_more_highlighted.png" style="width: 20px; height: 20px; margin-top: 7px; right: 5px" />
								<div id="popover_${layer.uniqueIdentifier}" class="popover__content">
									<span class="popoverDeleteLayerTitle" onclick="deleteLayerClicked('${
                    layer.uniqueIdentifier
                  }')"
										style="cursor: pointer; color: red; width: 100%; text-align: right;">
											Delete
									</span>
								</div>
							</div>
                    </div>
						`;
            }
          } else {
            layerDifference = Math.round(
              layer.area -
                (layer.grossArea
                  ? layer.grossArea
                  : layer.netArea
                  ? layer.netArea
                  : 0)
            );
            const innerHTML =
              layer.name == "Scratchpad"
                ? ""
                : `<span style="position: absolute; height: 20px; top: 6px; right: 29px;">${Math.round(
                    layer.area
                  )} ⏍</span>`;
            cell.innerHTML =
              `<div class="sketchLayerVisibilityBox" onclick="layerVisibilityClicked(event, '${layer.uniqueIdentifier}')">
              <img src="ptas_${layerVisibility}_highlighted.png" style="position: absolute; width: 20px; height: 13px; top: 10px; left: 7px" />
            </div><div class="legend ${layerStyle}"></div><span class="sketchLayerClickableText" style="position: absolute; top: 8px; left: 81px; pointer-events: none">${layer.name}</span>` +
              innerHTML;
          }
          sketchLayers.appendChild(cell);
          break;
        }
      }
    }
    level.area = Math.round(levelDifference);
    level.difference = Math.round(levelArea);
  }
}

/**
 * Mouse drag over event handler for Layer reordering.
 *
 * @param {MouseEvent} event - Input event
 * @returns {void}
 */
function allowDrop(event) {
  event.preventDefault();
}

/**
 * Mouse drag start event handler for Layer reordering.
 *
 * @param {MouseEvent} event - Input event
 * @returns {void}
 */
function drag(event) {
  event.dataTransfer.setData("text", event.target.getAttribute("name"));
}

/**
 * Mouse drop event handler for Layer reordering.
 *
 * @param {MouseEvent} event - Input event
 * @returns {void}
 */
function layerDrop(event) {
  event.preventDefault();
  let movedLayer = parent.sketchControl.layers.find(
    layer => layer.name == event.dataTransfer.getData("text")
  );
  let currentLayer = parent.sketchControl.layers.find(
    layer => layer.name == event.target.getAttribute("name")
  );
  parent.sketchControl.levels.map(level => {
    level.layers &&
      level.layers.length > 0 &&
      level.layers.map((layer, index) => {
        if (layer.uniqueIdentifier == movedLayer.uniqueIdentifier) {
          movedLayer = layer;
          level.layers.splice(index, 1);
        }
      });
  });
  parent.sketchControl.levels.map(level => {
    if (level.layers && level.layers.length > 0) {
      for (let i = 0; i < level.layers.length; i++) {
        if (level.layers[i].uniqueIdentifier == currentLayer.uniqueIdentifier) {
          currentLayer = level.layers[i];
          level.layers.splice(i + 1, 0, movedLayer);
          break;
        }
      }
    }
  });
  updateLayerOrder();
  parent.sketchControl.draw();
}

/**
 * Mouse drop event handler for Level reordering.
 *
 * @param {MouseEvent} event - Input event
 * @returns {void}
 */
function levelDrop(event) {
  event.preventDefault();
  let movedLayer = parent.sketchControl.layers.find(
    layer => layer.name == event.dataTransfer.getData("text")
  );
  let currentLevel = parent.sketchControl.levels.find(
    level => level.name == event.target.getAttribute("name")
  );
  parent.sketchControl.levels.map(level => {
    level.layers &&
      level.layers.length > 0 &&
      level.layers.map((layer, index) => {
        if (layer.uniqueIdentifier == movedLayer.uniqueIdentifier) {
          movedLayer = layer;
          level.layers.splice(index, 1);
        }
      });
  });
  if (!currentLevel.layers) currentLevel.layers = [];
  currentLevel.layers.unshift(movedLayer);
  updateLayerOrder();
  parent.sketchControl.draw();
}

/**
 * Reorders the Layer list after moving a Layer or Level.
 *
 * @returns {void}
 */
function updateLayerOrder() {
  let newLayerOrder = [];
  parent.sketchControl.levels.map(level => {
    level.layers.map(layer => {
      let newLayer = parent.sketchControl.getLayer(layer.uniqueIdentifier);
      if (newLayer) newLayerOrder.push(newLayer);
    });
  });
  parent.sketchControl.layers = newLayerOrder.reverse();
}

/**
 * Generates the Layer menu for Copy/Move purposes.
 *
 * @returns {void}
 */
function loadCopyAndMoveToMenu() {
  let copyAndMoveToLayers = document.getElementById("copyAndMoveToLayers");
  while (copyAndMoveToLayers.firstElementChild) {
    copyAndMoveToLayers.removeChild(copyAndMoveToLayers.firstElementChild);
  }
  if (parent.sketchControl.levels === undefined) {
    return;
  }
  for (let i = 0; i < parent.sketchControl.levels.length; i++) {
    const level = parent.sketchControl.levels[i];
    let cell = document.createElement("div");
    cell.style.position = "relative";
    cell.style.height = "34px";
    cell.style.backgroundColor = "black";
    cell.style.flex = "1 0 auto";
    const visibility = level.visible ? "visible" : "hidden";
    if (level.isScratchpad) {
      cell.innerHTML =
        '<div class="sketchLayerVisibilityBox"><img src="ptas_' +
        visibility +
        '.png" style="position: absolute; width: 20px; height: 13px; top: 10px; left: 7px" /></div><img src="ptas_scratchpad.png" style="position: absolute; width: 20px; height: 20px; top: 8px; left: 45px" /><span class="sketchClickableText" style="position: absolute; top: 8px; left: 75px">' +
        level.name +
        "</span>";
    } else {
      cell.innerHTML =
        '<div class="sketchLayerVisibilityBox"><img src="ptas_' +
        visibility +
        '.png" style="position: absolute; width: 20px; height: 13px; top: 10px; left: 7px" /></div><span class="sketchClickableText" style="position: absolute; top: 8px; left: 45px">' +
        level.name +
        "</span>";
    }
    copyAndMoveToLayers.appendChild(cell);
    if (level.layers === undefined) {
      continue;
    }
    for (let j = 0; j < level.layers.length; j++) {
      const entry = level.layers[j];
      for (let k = 0; k < parent.sketchControl.layers.length; k++) {
        const layer = parent.sketchControl.layers[k];
        if (layer.uniqueIdentifier === entry.uniqueIdentifier) {
          let cell = document.createElement("div");
          cell.className = "sketchCopyAndMoveToMenuItem";
          cell.setAttribute(
            "onclick",
            'copyAndMoveToMenuItemClicked("' + layer.uniqueIdentifier + '")'
          );
          cell.setAttribute(
            "onmouseover",
            "copyAndMoveToMenuItemMouseOver(this)"
          );
          cell.setAttribute(
            "onmouseout",
            "copyAndMoveToMenuItemMouseOut(this)"
          );
          cell.style.flex = "1 0 auto";
          const layerVisibility = layer.visible ? "visible" : "hidden";
          let layerStyle = getLayerStyle(layer);
          if (
            layer.uniqueIdentifier === parent.sketchControl.sketchLayerToEdit
          ) {
            cell.style.backgroundColor = "#d4e693";
            cell.innerHTML =
              '<div class="sketchLayerVisibilityBox"><img src="ptas_' +
              layerVisibility +
              '_highlighted.png" style="position: absolute; width: 20px; height: 13px; top: 10px; left: 7px" /></div><div class="legend ' +
              layerStyle +
              '"></div><span class="sketchLayerClickableText" style="position: absolute; top: 8px; left: 81px">' +
              layer.name +
              "</span>";
          } else {
            cell.innerHTML =
              '<div class="legend ' +
              layerStyle +
              '"></div><span class="sketchLayerClickableText" style="position: absolute; top: 8px; left: 81px">' +
              layer.name +
              "</span>";
          }
          copyAndMoveToLayers.appendChild(cell);
          break;
        }
      }
    }
  }
}

/**
 * Returns a CSS style based on the Layer name.
 *
 * @param {SketchLayer} layer
 * @returns {string}
 */
function getLayerStyle(layer) {
  let layerStyle = "black-solid-2";
  switch (layer.name) {
    case "1st floor":
      layerStyle = "black-solid-4";
      break;
    case "2nd floor":
      layerStyle = "teal-dashed-4";
      break;
    case "Upper floor":
      layerStyle = "green-dashed-4";
      break;
    case "Half floor":
      layerStyle = "green-dotted-4";
      break;
    case "Unfinished half floor":
      layerStyle = "purple-dashed-4";
      break;
    case "Unfinished full floor":
      layerStyle = "purple-dashed-4";
      break;
    case "Basement - total":
      layerStyle = "magenta-dotted-4";
      break;
    case "Basement - finished":
      layerStyle = "magenta-dotted-4";
      break;
    case "Garage - basement":
      layerStyle = "gray-dotted-4";
      break;
    case "Garage - attached":
      layerStyle = "gray-dotted-4";
      break;
    case "Porch - open":
      layerStyle = "gray-dashed-2";
      break;
    case "Porch - enclosed":
      layerStyle = "gray-dashed-2";
      break;
    case "Deck":
      layerStyle = "gray-dotted-2";
      break;
    case "Garage - detached":
      layerStyle = "gray-dotted-4";
      break;
    case "Carport":
      layerStyle = "gray-dotted-4";
      break;
    case "Area over detached garage":
      layerStyle = "gray-dotted-4";
      break;
    case "Barn - steel":
      layerStyle = "gray-dotted-4";
      break;
    case "Barn - wood":
      layerStyle = "gray-dotted-4";
      break;
    case "Barn - loft/2nd floor":
      layerStyle = "gray-dotted-4";
      break;
    case "Sport court":
      layerStyle = "gray-dotted-4";
      break;
    case "Stables":
      layerStyle = "gray-dotted-4";
      break;
    case "Mezzanine - display":
      layerStyle = "green-dotted-2";
      break;
    case "Mezzanine - office":
      layerStyle = "green-dotted-4";
      break;
    case "Mezzanine - storage":
      layerStyle = "green-dashed-4";
      break;
    case "Balcony":
      layerStyle = "gray-dashed-2";
      break;
  }
  return layerStyle;
}

/**
 * Fills a Style Set with a PaperJS style.
 *
 * @param {SketchStyleSet} styleSet
 * @returns {void}
 */
function createStyleSet(styleSet) {
  styleSet.setAsDefault();
  styleSet.style.fontFamily = "Open Sans, sans-serif";
  styleSet.style.fontSize = "12px";
  styleSet.style.negativeFill = "#e293e650";
  styleSet.objectSelectionStyle.point = "#0000004c";
  styleSet.objectSelectionStyle.fill = "#d4e693b3";
  styleSet.objectSelectionStyle.negativeFill = "#d4e693b3";
  styleSet.objectSelectionStyle.width = 2;
  styleSet.objectSelectionStyle.radius = 10;
  styleSet.objectSelectionStyle.fontFamily = "Open Sans, sans-serif";
  styleSet.objectSelectionStyle.fontSize = "12px";
  styleSet.objectGroupSelectionStyle.point = "#0000004c";
  styleSet.objectGroupSelectionStyle.fill = "#d4e693b3";
  styleSet.objectGroupSelectionStyle.negativeFill = "#d4e693b3";
  styleSet.objectGroupSelectionStyle.width = 2;
  styleSet.objectGroupSelectionStyle.radius = 10;
  styleSet.objectGroupSelectionStyle.fontFamily = "Open Sans, sans-serif";
  styleSet.objectGroupSelectionStyle.fontSize = "12px";
  styleSet.objectOutOfSelectionStyle.color = "#9a9a9a";
  styleSet.objectOutOfSelectionStyle.fontFamily = "Open Sans, sans-serif";
  styleSet.objectOutOfSelectionStyle.fontSize = "12px";
  styleSet.objectOutOfSelectionStyle.negativeFill = "#e293e650";
  styleSet.shapeSelectionStyle.color = "#a5c727";
  styleSet.shapeSelectionStyle.point = "#a5c72789";
  styleSet.shapeSelectionStyle.width = 4;
  styleSet.shapeSelectionStyle.radius = 10;
  styleSet.shapeSelectionStyle.center = "#a5c727";
  styleSet.shapeSelectionStyle.centerRadius = 5;
  styleSet.shapeSelectionStyle.fontFamily = "Open Sans, sans-serif";
  styleSet.shapeSelectionStyle.fontSize = "12px";
  styleSet.openObjectStyle.point = "#a5c72789";
  styleSet.openObjectStyle.width = 2;
  styleSet.openObjectStyle.radius = 10;
  styleSet.openObjectStyle.fontFamily = "Open Sans, sans-serif";
  styleSet.openObjectStyle.fontSize = "12px";
  styleSet.customTextStyle.fontFamily = "Open Sans, sans-serif";
  styleSet.customTextStyle.fontSize = "16px";
  styleSet.textSelectionStyle.color = "#a5c727";
  styleSet.textSelectionStyle.point = "#a5c72789";
  styleSet.textSelectionStyle.radius = 10;
  styleSet.textSelectionStyle.pattern = [8, 4];
  styleSet.textSelectionStyle.expand = 15;
  styleSet.textSelectionLineToRotateIconStyle.color = "#a5c727";
  styleSet.textSelectionLineToRotateIconStyle.width = 2;
  styleSet.textSelectionLineToRotateIconStyle.pattern = [0, 0];
  styleSet.majorGridlineStyle.color = "#d4e693";
  styleSet.majorGridlineStyle.pattern = [1, 1];
  styleSet.minorGridlineStyle.color = "#d4e693";
  styleSet.minorGridlineStyle.pattern = [1, 1];
  styleSet.gridlineAxisStyle.color = "#d4e693";
  styleSet.gridlineAxisStyle.pattern = [1, 1];
  styleSet.lineToPenStyle.color = "black";
  styleSet.lineToPenStyle.width = 2;
  styleSet.lineToPenStyle.pattern = [8, 4];
  styleSet.innerAngleIndicatorStyle.fill = "#3c7893";
  styleSet.innerAngleIndicatorStyle.fontFamily = "Open Sans, sans-serif";
  styleSet.innerAngleIndicatorStyle.fontSize = "12px";
  styleSet.innerAngleIndicatorStyle.color = "white";
  styleSet.outerAngleIndicatorStyle.fill = "#00acc1";
  styleSet.outerAngleIndicatorStyle.fontFamily = "Open Sans, sans-serif";
  styleSet.outerAngleIndicatorStyle.fontSize = "12px";
  styleSet.outerAngleIndicatorStyle.color = "white";
  styleSet.riseAndRunIndicatorStyle.fill = "#3c7893";
  styleSet.riseAndRunIndicatorStyle.fontFamily = "Open Sans, sans-serif";
  styleSet.riseAndRunIndicatorStyle.fontSize = "12px";
  styleSet.riseAndRunIndicatorStyle.fontColor = "black";
  styleSet.riseAndRunIndicatorStyle.offset = 20;
}

let sketchStyleSet = new SketchStyleSet();
let sketchDrawingStyleSet = new SketchStyleSet();

/**
 * Loads the Control instance.
 *
 * @returns {void}
 */
function instantiateSketchControl() {
  createStyleSet(sketchStyleSet);
  createStyleSet(sketchDrawingStyleSet);
  sketchDrawingStyleSet.style.point = "#a5c72760";
  sketchDrawingStyleSet.style.radius = 10;
  sketchDrawingStyleSet.style.center = "#a5c727";
  sketchDrawingStyleSet.style.centerRadius = 5;
  sketchDrawingStyleSet.shapeSelectionStyle.point = "#a5c72789";
  sketchDrawingStyleSet.shapeSelectionStyle.radius = 10;
  sketchDrawingStyleSet.openObjectStyle.point = "#a5c72789";
  sketchDrawingStyleSet.openObjectStyle.radius = 10;
  sketchDrawingStyleSet.openObjectStyle.center = "#a5c727";
  sketchDrawingStyleSet.openObjectStyle.centerRadius = 5;
  let mainCanvas = document.getElementById("sketchCanvas");
  parent.sketchControl = new SketchControl(
    document.getElementById("sketch"),
    mainCanvas,
    document.getElementById("sketchGridCanvas")
  );
  parent.sketchControl.textInputElementTextChangedName =
    "sketchTextInputElementTextChanged";
  parent.sketchControl.textInputElementKeyUpName =
    "sketchTextInputElementKeyUp";
  parent.sketchControl.styleSet = sketchStyleSet;
  parent.sketchControl.canvas.onmousedown = sketchMouseDown;
  parent.sketchControl.canvas.ondblclick = sketchDoubleClick;
  parent.sketchControl.canvas.onmousemove = sketchMouseMove;
  parent.sketchControl.canvas.onmouseup = sketchMouseUp;
  parent.sketchControl.canvas.onmouseenter = sketchMouseEnter;
  parent.sketchControl.canvas.onwheel = sketchWheel;
  parent.sketchControl.editingStarted = sketchEditingStarted;
  parent.sketchControl.editingValuesChanged = sketchEditingValuesChanged;
  parent.sketchControl.editingFinished = sketchEditingFinished;
  parent.sketchControl.penDownIconWidth = 35;
  parent.sketchControl.penDownIconHeight = 35;
  parent.sketchControl.penUpIconWidth = 35;
  parent.sketchControl.penUpIconHeight = 35;
  parent.sketchControl.rotateIcon = "ptas_rotate.png";
  parent.sketchControl.rotateIconHighlighted = "ptas_rotate_highlighted.png";
  parent.sketchControl.newSketchDistanceTemplate.offset = { x: 0, y: 2 };
  parent.sketchControl.newSketchDistanceTemplate.fontSize = 2;
  parent.sketchControl.newSketchDistanceTemplate.worldUnits = true;
  parent.sketchControl.invertedYAxis = true;
  parent.sketchControl.updateTopLevelTransform();
  parent.sketchControl.draw();
  parent.onkeydown = sketchKeyDown;
  parent.onkeyup = sketchKeyUp;
  parent.addEventListener("gesturestart", sketchGestureStart);
  parent.addEventListener("gesturechange", sketchGestureChange);
  parent.addEventListener("gestureend", sketchGestureEnd);
  mainCanvas.addEventListener("touchstart", sketchTouchStart);
  mainCanvas.addEventListener("touchmove", sketchTouchMove);
  mainCanvas.addEventListener("touchend", sketchTouchEnd);
  mainCanvas.addEventListener("touchcancel", sketchTouchCancel);

  let generateB2CToken = true;
  const savedAadToken = localStorage.getItem("aadToken");
  let b2cToken = localStorage.getItem("b2cToken");
  const urlParams = new URLSearchParams(window.location.search);
  let sketchEntityId = urlParams.get("sketchId");
  let aadToken = urlParams.get("token");
  let relatedEntityName = urlParams.get("relatedEntity");
  let relatedEntityId = urlParams.get("relatedEntityId");
  if (!relatedEntityId) {
    relatedEntityId = urlParams.get("relatedId");
  }
  if (!relatedEntityId) {
    showLoading(
      "No related entity information provided, please confirm the associated entity is saved before navigating to the Sketch Editor."
    );
  } else {
    if (history.pushState) {
      let newURL =
        window.location.protocol +
        "//" +
        window.location.host +
        window.location.pathname +
        "?";
      if (sketchEntityId) {
        newURL += `sketchId=${sketchEntityId}&`;
      }
      if (relatedEntityName) {
        newURL += `relatedEntity=${relatedEntityName}&`;
      }
      if (relatedEntityId) {
        newURL += `relatedEntityId=${relatedEntityId}`;
      }
      window.history.pushState({ path: newURL }, "", newURL);
    }

    if (b2cToken) {
      //If no token from URL or if saved token is equal to URL token, don't ask new B2C token
      if (!aadToken || aadToken === savedAadToken) {
        generateB2CToken = false;
      }
    }
    if (!aadToken) {
      aadToken = localStorage.getItem("aadToken");
    } else {
      localStorage.setItem("aadToken", aadToken);
    }

    if (aadToken || b2cToken) {
      showLoading("Checking authorization token");
      SketchAPIService.getMagicLinkToken(aadToken, generateB2CToken)
        .then(resB2CToken => {
          if (resB2CToken) {
            localStorage.setItem("b2cToken", resB2CToken);
            b2cToken = resB2CToken;
            parent.sketchControl.sketchEntityId = sketchEntityId;
            parent.sketchControl.relatedEntityName = relatedEntityName;
            parent.sketchControl.relatedEntityId = relatedEntityId;
            parent.sketchControl.sketchAccessToken = b2cToken;
            if (!sketchEntityId) {
              showLoading("Loading sketch...");
              SketchAPIService.getGenericEntity(
                relatedEntityName,
                relatedEntityId,
                b2cToken
              )
                .then(relatedEntityRes => {
                  if (relatedEntityRes) {
                    parent.sketchControl.relatedEntity = relatedEntityRes;
                    parent.sketchControl.relatedEntityLoaded = { ...relatedEntityRes }

                    SketchAPIService.getSystemUser(b2cToken).then(userData => {
                      parent.sketchControl.userData = userData;
                      /*if (
                        parent.sketchControl.relatedEntity[
                          "_ptas_sketchid_value"
                        ]
                      ) {
                        parent.sketchControl.sketchEntityId =
                          parent.sketchControl.relatedEntity[
                            "_ptas_sketchid_value"
                          ];
                        loadSketch(
                          parent.sketchControl.relatedEntity[
                            "_ptas_sketchid_value"
                          ]
                        );
                      } else {*/
                        let queryName;
                        switch (relatedEntityName) {
                          case "ptas_buildingdetail":
                            queryName = "_ptas_buildingid_value";
                            break;
                          case "ptas_accessorydetail":
                            queryName = "_ptas_accessoryid_value";
                            break;
                          default:
                            queryName = "_ptas_unitid_value";
                            break;
                        }
                        SketchAPIService.getGenericList(
                          "ptas_sketch",
                          `$filter=${queryName} eq '${relatedEntityId}'`,
                          b2cToken
                        )
                          .then(genericRes => {
                            if (
                              genericRes.items &&
                              genericRes.items.length > 0
                            ) {
                              let draftId = genericRes.items.find(
                                item =>
                                  !item.changes.ptas_isofficial &&
                                  !item.changes.ptas_iscomplete
                              );
                              if (!draftId) {
                                draftId = genericRes.items[0];
                              }
                              parent.sketchControl.sketchEntityId =
                                draftId.changes.ptas_sketchid;

                              let sketchId =
                                genericRes.items &&
                                genericRes.items.find(
                                  item => item.changes.ptas_isofficial
                                );
                              if (sketchId) {
                                hideLoading();
                                loadSketch(sketchId.changes.ptas_sketchid);
                              } else {
                                let version = 0;
                                genericRes.items.map(item => {
                                  if (
                                    item.changes.ptas_iscomplete &&
                                    parseInt(item.changes.ptas_version) >
                                      version
                                  ) {
                                    sketchId = item;
                                    version = item.changes.ptas_version;
                                  }
                                });
                                if (version > 0) {
                                  loadSketch(sketchId.changes.ptas_sketchid);
                                } else {
                                  loadSketch(
                                    parent.sketchControl.sketchEntityId
                                  );
                                }
                              }
                            } else {
                              SketchEntitiesHandler.createFirstSketch(
                                parent.sketchControl
                              ).then(createRes => {
                                if (createRes) {
                                  SketchAPIService.getSketch(
                                    parent.sketchControl.sketchEntityId,
                                    b2cToken,
                                    relatedEntityId,
                                    relatedEntityName
                                  ).then(getSketchRes => {
                                    parent.sketchControl.clear();

                                    parent.sketchControl.parcelData =
                                      getSketchRes.parcel;
                                    getSketchRes.items.forEach(item => {
                                      if (item.entityName === "ptas_sketch") {
                                        parent.sketchControl.sketchEntity =
                                          item.changes;
                                        parent.sketchControl.sketchEntityId =
                                          item.changes["ptas_sketchid"];
                                      }
                                    });

                                    let entity =
                                      parent.sketchControl.sketchEntity;
                                    let relatedEntity =
                                      parent.sketchControl.relatedEntity;
                                    let topCenter = document.getElementById(
                                      "currentSketchInfo"
                                    );
                                    let fileInfo = document.getElementById(
                                      "file-info"
                                    );
                                    let date = getLocalDateString(
                                      entity.ptas_drawdate
                                    );
                                    topCenter.innerHTML = fileInfo.innerHTML = `
                              <div>${
                                parent.sketchControl.parcelData.parcelName
                              }</div>
                              <div>${relatedEntity.ptas_name}</div>
                              <div id=top-line3>${
                                entity.ptas_isofficial
                                  ? "Official"
                                  : entity.ptas_iscomplete
                                  ? "V. " + entity.ptas_version
                                  : "Draft"
                              }, ${date}</div>`;

                                    SketchFromJSON.read(
                                      getSketchRes.sketch,
                                      parent.sketchControl
                                    );
                                    let projection = {
                                      center: {},
                                      projected: [],
                                      index: 0
                                    };
                                    parent.sketchControl.findProjection(
                                      projection
                                    );
                                    if (projection.projected.length > 0) {
                                      parent.sketchControl.zoomToContents(
                                        projection.bounds,
                                        0.7
                                      );
                                    }
                                    addScratchpadLayer();
                                    //Update saved layers once the sketch has been drawn (and fillPath is set)
                                    if (
                                      parent.sketchControl.layers &&
                                      parent.sketchControl.layers.length > 0
                                    ) {
                                      parent.sketchControl.updateSavedLayers();
                                      parent.sketchControl.updateSavedTotalsInfo(
                                        parent.sketchControl.savedLayers
                                      );
                                    }
                                    hideLoading();
                                  });
                                } else {
                                  showLoading("Error creating new sketch");
                                }
                              });
                            }
                            parent.sketchControl.genericList = genericRes;
                          })
                          .catch(error => console.error(error));
                      //}
                    });
                  } else {
                    showLoading("Could not get entity.");
                    console.error(
                      "Invalid related entity response. Check token expiration."
                    );
                  }
                })
                .catch(err => {
                  console.error(err);
                  showLoading(err);
                  return;
                });
            } else {
              loadSketch(sketchEntityId);
            }
          } else {
            console.error("Invalid AAD token provided");
            showLoading("Invalid token provided");
          }
        })
        .catch(err => {
          console.error("Could not get Magic Link token:", err);
          showLoading("Could not get auth token");
        });
    } else {
      showLoading("No AAD token provided");
    }
  }
}

/**
 * Loads a default Layer in a default Level, or creates it if none is found.
 *
 * @returns {void}
 */
function addScratchpadLayer() {
  if (
    !parent.sketchControl.levels ||
    parent.sketchControl.levels.length === 0
  ) {
    let level = {
      uniqueIdentifier: SketchControl.uuidv4(),
      name: "Level 1",
      area: 0,
      visible: true,
      layers: []
    };
    parent.sketchControl.levels = [level];
  }

  if (!parent.sketchControl.layers) {
    parent.sketchControl.layers = [];
  }
  let scratchpad = parent.sketchControl.layers.find(
    ly => ly.name == "Scratchpad"
  );
  if (!scratchpad) {
    scratchpad = new SketchLayer();
    scratchpad.visible = true;
    scratchpad.name = "Scratchpad";
    scratchpad.uniqueIdentifier = SketchControl.uuidv4();
    scratchpad.nonExportable = true;
    parent.sketchControl.layers.push(scratchpad);
  }

  if (parent.sketchControl.layers && parent.sketchControl.layers.length > 0) {
    let level = {
      uniqueIdentifier: SketchControl.uuidv4(),
      name: "Scratchpad",
      area: 0,
      visible: true,
      isScratchpad: true,
      layers: [],
      locked: true
    };
    if (parent.sketchControl.levels === undefined) {
      parent.sketchControl.levels = [level];
    } else {
      parent.sketchControl.levels.push(level);
    }
    level.layers.push({ uniqueIdentifier: scratchpad.uniqueIdentifier });
  }
  parent.sketchControl.defaultSketchLayerToEdit = scratchpad.uniqueIdentifier;
  let layer = parent.sketchControl.getLayer(scratchpad.uniqueIdentifier);
  document.getElementById("layerName").innerHTML = `${layer.name}`;
  parent.sketchControl.draw();
}

function addLayerClicked(name) {
  const history = parent.sketchControl.createHistory();
  parent.sketchControl.addToUndoHistory(history);
  document.getElementById("layer-types").style.visibility = "collapse";
  let levels = parent.sketchControl.levels;
  if (levels.find(level => level.locked && level.selected)) {
    levels.find(level => level.locked).selected = false;
    levels.find(level => !level.locked).selected = true;
  }
  let layer = parent.sketchControl.layers.find(layer => layer.name == name);
  let scratchpad;
  if (layer) scratchpad = layer;
  if (!scratchpad) {
    scratchpad = new SketchLayer();
    let level = 0;
    scratchpad.visible = true;
    for (let i = 0; i < levels.length; i++) {
      if (levels[i].selected) {
        level = i;
      }
    }
    scratchpad.name = name;
    scratchpad.uniqueIdentifier = SketchControl.uuidv4();
    parent.sketchControl.layers.push(scratchpad);
    levels[level].layers.unshift({
      uniqueIdentifier: scratchpad.uniqueIdentifier
    });
  }
  if (copyAndMoveToMenuOperation !== "") {
    copyAndMoveToMenuItemClicked(scratchpad.uniqueIdentifier);
    scratchpad.addLabel();
  }
  sketchLayerMenuItemClicked(scratchpad.uniqueIdentifier);
  parent.sketchControl.autoSave();
}

/**
 * Adds a search tag to the current sketch.
 *
 * @param {KeyboardEvent} event
 * @param {string} sketchTagsElementName
 * @returns {void}
 */
function addSketchTag(event, sketchTagsElementName) {
  if (event.target.value != "" && event.key === "Enter") {
    if (parent.sketchControl.sketchEntity) {
      if (!parent.sketchControl.sketchEntity.ptas_tags) {
        parent.sketchControl.sketchEntity.ptas_tags = "";
      } else {
        parent.sketchControl.sketchEntity.ptas_tags += ",";
      }

      parent.sketchControl.sketchEntity.ptas_tags += event.target.value;
      event.target.value = "";

      //Refresh tags in UI
      showSketchTags(parent.sketchControl.sketchEntity, sketchTagsElementName);
    }
  }
}

/**
 * Text input change event handler.
 *
 * @returns {void}
 */
function sketchTextInputElementTextChanged() {
  parent.sketchControl.textInputElementTextChanged();
}

/**
 * Text input key up event handler.
 *
 * @param {KeyboardEvent} event
 * @returns {void}
 */
function sketchTextInputElementKeyUp(event) {
  parent.sketchControl.keyUp(event);
}

let keypadInputFeetId = "keypadInputFeet";
let keypadInputInchesId = "keypadInputInches";
let allowShowingKeypadInput = true;

/**
 * Update the displayed measurements of the currently selected Object.
 *
 * @param {SketchObject} editing
 * @returns {void}
 */
function sketchEditingValuesChanged(editing) {
  if (editing.curveData) {
    if (!editing.curveData.angle || editing.curveData.angle == 0) {
      let lineLengthFeet = document.getElementById("lineLengthFeet");
      lineLengthFeet.value = SketchUtils.feetTextFor(editing.curveData.chord);
      let lineLengthInches = document.getElementById("lineLengthInches");
      lineLengthInches.value = SketchUtils.inchesTextFor(
        editing.curveData.chord
      );
      let lineRiseFeet = document.getElementById("lineRiseFeet");
      lineRiseFeet.value = SketchUtils.feetTextFor(editing.curveData.rise);
      let lineRiseInches = document.getElementById("lineRiseInches");
      lineRiseInches.value = SketchUtils.inchesTextFor(editing.curveData.rise);
      let lineRunFeet = document.getElementById("lineRunFeet");
      lineRunFeet.value = SketchUtils.feetTextFor(editing.curveData.run);
      let lineRunInches = document.getElementById("lineRunInches");
      lineRunInches.value = SketchUtils.inchesTextFor(editing.curveData.run);
      let curveAngle = document.getElementById("curveAngle");
      curveAngle.value = "0";
      let curveHeightFeet = document.getElementById("curveHeightFeet");
      curveHeightFeet.value = SketchUtils.feetTextFor(0);
    } else {
      let curveLengthFeet = document.getElementById("curveLengthFeet");
      curveLengthFeet.value = SketchUtils.feetTextFor(editing.curveData.length);
      let curveLengthInches = document.getElementById("curveLengthInches");
      curveLengthInches.value = SketchUtils.inchesTextFor(
        editing.curveData.length
      );
      let curveRiseFeet = document.getElementById("curveRiseFeet");
      curveRiseFeet.value = SketchUtils.feetTextFor(editing.curveData.rise);
      let curveRiseInches = document.getElementById("curveRiseInches");
      curveRiseInches.value = SketchUtils.inchesTextFor(editing.curveData.rise);
      let curveRunFeet = document.getElementById("curveRunFeet");
      curveRunFeet.value = SketchUtils.feetTextFor(editing.curveData.run);
      let curveRunInches = document.getElementById("curveRunInches");
      curveRunInches.value = SketchUtils.inchesTextFor(editing.curveData.run);
      let curveAngle = document.getElementById("curveAngle");
      curveAngle.value = Math.floor(editing.curveData.angle).toString();
      let curveChordFeet = document.getElementById("curveChordFeet");
      curveChordFeet.value = SketchUtils.feetTextFor(editing.curveData.chord);
      let curveChordInches = document.getElementById("curveChordInches");
      curveChordInches.value = SketchUtils.inchesTextFor(
        editing.curveData.chord
      );
      let curveHeightFeet = document.getElementById("curveHeightFeet");
      curveHeightFeet.value = SketchUtils.feetTextFor(editing.curveData.height);
      let curveHeightInches = document.getElementById("curveHeightInches");
      curveHeightInches.value = SketchUtils.inchesTextFor(
        editing.curveData.height
      );
    }
  }
}

/**
 * Displays the context bar based on the selected Object's properties.
 *
 * @param {SketchObject} editing
 * @returns {void}
 */
function sketchEditingStarted(editing) {
  if (editing.curveData && isNaN(editing.curveData.height)) {
    parent.sketchControl.showContextBar("line");
  } else {
    parent.sketchControl.showContextBar("curve");
  }
  sketchEditingValuesChanged(editing);
}

/**
 * Stops showing the edited Object's measurements in the context bar.
 *
 * @returns {void}
 */
function sketchEditingFinished() {
  if (
    !parent.sketchControl.mode == SketchMode.Draw ||
    !parent.sketchControl.penDown
  )
  parent.sketchControl.showContextBar("distance");
  keypadInputFeetId = "keypadInputFeet";
  keypadInputInchesId = "keypadInputInches";
  allowShowingKeypadInput = true;
  hideKeypad();
}

/**
 * Updates the Undo and Redo buttons according to the available History steps.
 *
 * @returns {void}
 */
function adjustUndoRedoVisibility() {
  let undoButton = document.getElementById("undoButton");
  if (
    parent.sketchControl.undoHistory &&
    parent.sketchControl.undoHistory.length > 0
  ) {
    undoButton.classList.add("undo-enabled");
  } else {
    undoButton.classList.remove("undo-enabled");
  }
  let redoButton = document.getElementById("redoButton");
  if (
    parent.sketchControl.redoHistory &&
    parent.sketchControl.redoHistory.length > 0
  ) {
    redoButton.classList.add("undo-enabled");
  } else {
    redoButton.classList.remove("undo-enabled");
  }
}

/**
 * Hides the Discard menu.
 *
 * @returns {void}
 */
function collapseDiscardMenu() {
  let discardButtonText = document.getElementById("discardButtonText");
  discardButtonText.className = "sketchClickableText";
  let discardButtonImage = document.getElementById("discardButtonImage");
  discardButtonImage.src = "ptas_expand_down.png";
  let discardMenu = document.getElementById("discardMenu");
  discardMenu.style.visibility = "collapse";
}

/**
 * Hides the View menu.
 *
 * @returns {void}
 */
function collapseViewMenu() {
  let viewButtonText = document.getElementById("viewButtonText");
  viewButtonText.className = "sketchClickableText";
  let viewButtonImage = document.getElementById("viewButtonImage");
  viewButtonImage.src = "ptas_expand_down.png";
  let viewMenu = document.getElementById("viewMenu");
  viewMenu.style.visibility = "collapse";
  document
    .getElementById("distance-buttons")
    .classList.remove("distance-buttons-visible");
}

/**
 * Hides the Transform menu.
 *
 * @returns {void}
 */
function collapseTransformMenu() {
  let transformButtonText = document.getElementById("transformButtonText");
  transformButtonText.className = "sketchClickableText";
  let transformButtonImage = document.getElementById("transformButtonImage");
  transformButtonImage.src = "ptas_expand_down.png";
  let transformMenu = document.getElementById("transformMenu");
  transformMenu.style.visibility = "collapse";
}

let copyAndMoveToMenuOperation = "";

/**
 * Hides the specified modal.
 *
 * @param {string} modalID
 * @returns {void}
 */
function closeModal(modalID) {
  let modal = document.getElementById(modalID);
  modal.style.visibility = "collapse";
  if (modalID == "importModal") {
    document.getElementById("import-recent").style.display = "block";
    document.getElementById("import-results").style.visibility = "collapse";
  } else if (modalID === "saveModal") {
    //If user toggled 'is official' and did not save
    if (
      parent.sketchControl.sketchEntity["ptas_isofficial"] &&
      !parent.sketchControl.sketchEntity["ptas_iscomplete"] &&
      !parent.sketchControl.sketchIsOfficial
    ) {
      parent.sketchControl.sketchEntity["ptas_isofficial"] = false;
    }
  }
  if (modalID == "updateModal") {
    saveAndClose();
  }
}

/**
 * Restore the Copy and Move buttons' CSS styles to their original state, and hide their menu.
 *
 * @returns {void}
 */
function collapseAllCopyAndMoveToMenus() {
  const selectCopyToBackground = document.getElementById(
    "selectCopyToBackground"
  );
  selectCopyToBackground.className = "sketchClickableTextBackground";
  const selectCopyToText = document.getElementById("selectCopyToText");
  selectCopyToText.className = "sketchClickableText";
  const selectMoveToBackground = document.getElementById(
    "selectMoveToBackground"
  );
  selectMoveToBackground.className = "sketchClickableTextBackground";
  const selectMoveToText = document.getElementById("selectMoveToText");
  selectMoveToText.className = "sketchClickableText";

  const drawCopyToBackground = document.getElementById("drawCopyToBackground");
  drawCopyToBackground.className = "sketchClickableTextBackground";
  const drawCopyToText = document.getElementById("drawCopyToText");
  drawCopyToText.className = "sketchClickableText";
  const drawMoveToBackground = document.getElementById("drawMoveToBackground");
  drawMoveToBackground.className = "sketchClickableTextBackground";
  const drawMoveToText = document.getElementById("drawMoveToText");
  drawMoveToText.className = "sketchClickableText";

  const copyAndMoveToMenu = document.getElementById("copyAndMoveToMenu");
  copyAndMoveToMenu.style.visibility = "collapse";
  copyAndMoveToMenuOperation = "";
}

/**
 * Hide the Layer Types menu.
 *
 * @returns {void}
 */
function collapseLayersMenu() {
  let layersButton = document.getElementById("layersButton");
  layersButton.src = "ptas_layers.png";
  let layersMenu = document.getElementById("layersMenu");
  layersMenu.style.visibility = "collapse";
  document.getElementById("layer-types").style.visibility = "collapse";
}

/**
 * Set all Layers to visible.
 *
 * @returns {void}
 */
function restoreLayersVisibility() {
  if (
    parent.sketchControl.layers !== undefined &&
    parent.sketchControl.layers.length > 0
  ) {
    for (let i = 0; i < parent.sketchControl.layers.length; i++) {
      let layer = parent.sketchControl.layers[i];
      layer.visible = true;
    }
  }
  collapseLayersMenu();
}

/**
 * Back button event handler.
 *
 * @returns {void}
 */
function closeClicked() {
  //Temporary. Pending integration with back-end
  window.location.reload();
}

/**
 * Discard dropdown event handler.
 *
 * @returns {void}
 */
function discardClicked() {
  let discardButtonText = document.getElementById("discardButtonText");
  let discardButtonImage = document.getElementById("discardButtonImage");
  let discardMenu = document.getElementById("discardMenu");
  if (discardButtonText.className === "sketchClickableText") {
    collapseMenus();
    discardButtonText.className = "sketchClickableTextHighlighted";
    discardButtonImage.src = "ptas_expand_down_highlighted.png";
    discardMenu.style.visibility = "visible";
  } else {
    discardButtonText.className = "sketchClickableText";
    discardButtonImage.src = "ptas_expand_down.png";
    discardMenu.style.visibility = "collapse";
  }
}

/**
 * All Changes button event handler.
 *
 * @returns {void}
 */
function discardAllChangesClicked() {
  if (parent.sketchControl.viewMode !== "stack") viewStackClicked(null);
  const history = parent.sketchControl.createHistory();
  parent.sketchControl.addToUndoHistory(history);
  parent.sketchControl.restoreSavedLayers();
  if (parent.sketchControl.undoHistory)
    parent.sketchControl.restoreFromHistory(
      parent.sketchControl.undoHistory[0]
    );
  parent.sketchControl.autoSave();
  collapseDiscardMenu();
  showDialog("All changes have been discarded.");
}

/**
 * All Changes and Close button event handler.
 *
 * @returns {void}
 */
function discardAllChangesAndCloseClicked() {
  if (parent.sketchControl.undoHistory) {
    parent.sketchControl
      .restoreFromHistory(parent.sketchControl.undoHistory[0])
      .then(() => {
        saveAndClose();
      });
  } else {
    saveAndClose();
  }
}

/**
 * Saves and closes the sketch tab.
 *
 * @returns {void}
 */
function saveAndClose() {
  parent.sketchControl.autoSave().then(() => {
    window.close();
  });
}

/**
 * Save event handler.
 *
 * @param {string} modalId
 * @returns {void}
 */
function saveClicked(modalId) {
  if (parent.sketchControl.viewMode !== "stack") viewStackClicked(null);
  let sketchEntity = parent.sketchControl.sketchEntity;
  //#region Save section
  SketchAPIService.getSketchSVG(
    SketchToJSON.write(parent.sketchControl),
    parent.sketchControl.sketchAccessToken
  ).then(svg => {
    const sketchThumbnail = document.getElementById("saveSketchThumbnail");
    sketchThumbnail.style.visibility = "inherit";
    sketchThumbnail.innerHTML = svg;
  });

  let versionTitle = "";
  if (parent.sketchControl.sketchIsOfficial) {
    versionTitle =
      "Official V." + parent.sketchControl.sketchEntity["ptas_version"];
  } else if (parent.sketchControl.sketchEntity["ptas_iscomplete"]) {
    versionTitle = "V." + parent.sketchControl.sketchEntity["ptas_version"];
  } else {
    versionTitle =
      "Draft V." + parent.sketchControl.sketchEntity["ptas_version"];
  }
  const thumbnailTitle = document.getElementById("saveSketchThumbnailTitle");
  const thumbnailDate = document.getElementById("saveSketchThumbnailDate");
  thumbnailTitle.innerHTML = versionTitle;
  thumbnailDate.innerHTML = getLocalDateString(
    parent.sketchControl.sketchEntity["ptas_drawdate"]
  );

  const sketchIsOfficialInput = document.getElementById(
    "saveSketchIsOfficialInput"
  );
  sketchIsOfficialInput.checked = sketchEntity["ptas_isofficial"];

  showSketchTags(sketchEntity, "saveSketchTags");
  //#endregion

  //#region Area totals section
  parent.sketchControl.updateNewTotalsInfo();
  if (!parent.sketchControl.layers || !parent.sketchControl.totalsInfo) {
    return false;
  }

  loadTotalsInfo(modalId);
  //#endregion

  parent.sketchControl.versionsContainerId = "saveTabVersionsContent";
  showSketchVersions(parent.sketchControl.versionsContainerId);

  let modal = document.getElementById(modalId);
  modal.style.visibility = "visible";
}

/**
 * Generates the list of sketch versions from the current entity.
 *
 * @param {string} versionsContainerId
 * @returns {void}
 */
function showSketchVersions(versionsContainerId) {
  const versionsTab = document.getElementById(versionsContainerId);
  while (versionsTab.firstChild) {
    versionsTab.removeChild(versionsTab.lastChild);
  }
  const versionsTemplate = document.getElementById("versionsTemplate");
  const versionsDiv = versionsTemplate.content
    .cloneNode(true)
    .querySelector("div");
  versionsTab.appendChild(versionsDiv);
  const versionsList = versionsDiv.querySelector(".versions-list");

  SketchAPIService.getSketchHistory(
    parent.sketchControl.sketchEntityId,
    parent.sketchControl.sketchAccessToken
  ).then(versionsRes => {
    if (versionsRes && versionsRes.length > 0) {
      versionsRes.forEach(v => {
        const versionElem = document.createElement("div");
        versionElem.classList.add("sketchVersion");
        if (v.ptas_sketchid === parent.sketchControl.sketchEntityId) {
          parent.sketchControl.selectedSketchVersion = v;
          versionElem.classList.add("sketchVersionSelected");

          //Generate totalsInfo using area totals in relatedEntity (saved) and in sketch version file (new)
          const versionTabView = versionsDiv.querySelector(
            ".versions-tab-view"
          );
          while (versionTabView.firstChild) {
            versionTabView.removeChild(versionTabView.lastChild);
          }
          const areaTotalsTableTemplate = document.getElementById(
            "areaTotalsTableTemplate"
          );
          const areaTotalsTable = areaTotalsTableTemplate.content
            .cloneNode(true)
            .querySelector(".table-template");

          const sketchVersionTotals = parent.sketchControl.getSketchVersionTotals(
            v.sketch
          );
          populateAreaTotalsTable(
            "versions",
            areaTotalsTable.querySelector("table"),
            sketchVersionTotals,
            areaTotalsTable.querySelector(".list-container")
          );
          versionTabView.appendChild(areaTotalsTable);
        }

        versionElem.onclick = event => {
          selectVersion(event, v, versionsDiv);
        };
        const versionMain = document.createElement("div");
        versionMain.classList.add("version-main");
        versionElem.appendChild(versionMain);
        const versionHeader = document.createElement("div");
        versionHeader.classList.add("version-header");
        versionMain.appendChild(versionHeader);
        const versionThumbnail = document.createElement("div");
        versionThumbnail.classList.add("version-thumbnail");
        versionThumbnail.innerHTML = v.svg;
        versionMain.appendChild(versionThumbnail);

        if (v.isOfficial || v.isOffical) {
          versionHeader.classList.add("official");
          versionThumbnail.classList.add("official");
          versionHeader.innerHTML = "Official V." + v.version;
        } else if (v.isComplete) {
          versionHeader.innerHTML = "V." + v.version;
        } else {
          versionHeader.innerHTML = "Draft V." + v.version;
        }

        const versionFooter = document.createElement("div");
        versionFooter.classList.add("version-footer");
        versionFooter.innerHTML = v.drawDate
          ? getLocalDateString(v.drawDate)
          : "-";
        versionElem.appendChild(versionFooter);

        versionsList.appendChild(versionElem);
      });
    }
  });
}

/**
 * Returns the given date in a readable format.
 *
 * @param {string} dateString
 * @returns {string}
 */
function getLocalDateString(dateString) {
  let date = new Date(Date.parse(dateString));
  let hour = date.getHours();
  let minutes = date.getMinutes();
  let meridian = "am";
  if (hour > 11) {
    meridian = "pm";
    hour -= 12;
  }
  if (hour == 0) hour += 12;
  if (minutes < 10) minutes = "0" + minutes;

  return isNaN(date)
    ? "*no date*"
    : date.getFullYear() +
        "/" +
        (date.getMonth() + 1) +
        "/" +
        date.getDate() +
        " " +
        hour +
        ":" +
        minutes +
        meridian;
}

/**
 * Sets the currently open version of the sketch as the official.
 *
 * @returns {void}
 */
function setSketchVersionAsOfficial() {
  document.getElementsByName('updateFieldsButton')[0].style.visibility = 'inherit'
  if (!parent.sketchControl.selectedSketchVersion.isOfficial) {
    showLoading("Updating...");
    if (parent.sketchControl.selectedSketchVersion.isComplete) {
      SketchEntitiesHandler.promoteCompletedVersionToOfficial(
        parent.sketchControl,
        parent.sketchControl.sketchEntity,
        SketchToJSON.write(parent.sketchControl)
      )
        .then(promoteRes => {
          if (promoteRes) {
            //If version promoted is the one being edited
            if (
              parent.sketchControl.sketchEntityId ===
              parent.sketchControl.selectedSketchVersion.ptas_sketchid
            ) {
              const sketchIsOfficialInput = document.getElementById(
                "saveSketchIsOfficialInput"
              );
              sketchIsOfficialInput.checked = true;
              parent.sketchControl.sketchIsOfficial = true;
            } else if (parent.sketchControl.sketchIsOfficial) {
              const sketchIsOfficialInput = document.getElementById(
                "saveSketchIsOfficialInput"
              );
              sketchIsOfficialInput.checked = false;
              parent.sketchControl.sketchIsOfficial = false;
              parent.sketchControl.sketchEntity["ptas_isofficial"] = false;
            }
            hideLoading();
            showSketchVersions(parent.sketchControl.versionsContainerId);
          } else {
            hideLoading();
            showDialog("Error while promoting complete version to official");
          }
        })
        .catch(err => {
          console.error(err);
          showLoading(err);
          return;
        });
    } else {
      SketchEntitiesHandler.promoteDraftVersionToOfficial(
        parent.sketchControl,
        parent.sketchControl.sketchEntity,
        SketchToJSON.write(parent.sketchControl)
      )
        .then(promoteRes => {
          if (promoteRes) {
            if (
              parent.sketchControl.sketchEntityId ===
              parent.sketchControl.selectedSketchVersion.ptas_sketchid
            ) {
              const sketchIsOfficialInput = document.getElementById(
                "saveSketchIsOfficialInput"
              );
              sketchIsOfficialInput.checked = true;
              loadSketch(parent.sketchControl.sketchEntityId);
            } else if (parent.sketchControl.sketchIsOfficial) {
              const sketchIsOfficialInput = document.getElementById(
                "saveSketchIsOfficialInput"
              );
              sketchIsOfficialInput.checked = false;
              parent.sketchControl.sketchIsOfficial = false;
              parent.sketchControl.sketchEntity["ptas_isofficial"] = false;
            }
            hideLoading();
            showSketchVersions(parent.sketchControl.versionsContainerId);
          } else {
            hideLoading();
            showDialog("Error while promoting draft version to official");
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
 * Info -> Versions -> Edit button event handler.
 *
 * @returns {void}
 */
function editSketchVersion() {
  if (
    parent.sketchControl.sketchEntityId !=
    parent.sketchControl.selectedSketchVersion.ptas_sketchid
  ) {
    loadSketch(parent.sketchControl.selectedSketchVersion.ptas_sketchid);
    panClicked();
    closeModal("saveModal");
    closeModal("infoModal");
  } else {
    if (parent.sketchControl.sketchEntity["ptas_iscomplete"]) {
      parent.sketchControl.autoSave();
      closeModal("saveModal");
      closeModal("infoModal");
    } else {
      showDialog("This sketch version is already being edited.");
    }
  }
}

/**
 * Opens the selected sketch.
 *
 * @param {string} sketchEntityId
 * @param {boolean} replace - Replaces old draft.
 * @returns {void}
 */
function loadSketch(sketchEntityId, replace) {
  document.getElementById("sketchSelectionContainer").style.visibility =
    "hidden";
  if (replace) {
    SketchAPIService.getSketchHistory(
      parent.sketchControl.sketchEntityId,
      parent.sketchControl.sketchAccessToken
    )
      .then(historyRes => {
        showLoading("Deleting sketch version...");
        if (parent.sketchControl.draftData) {
          SketchAPIService.deleteSketch(
            parent.sketchControl.draftData.draftId,
            parent.sketchControl.sketchAccessToken
          );
        }
        showLoading("Creating draft...");
        const draftTemplateId = parent.sketchControl.sketchEntityId;
        SketchEntitiesHandler.createDraftSketch(parent.sketchControl)
          .then(createDraftRes => {
            SketchAPIService.getSketch(
              parent.sketchControl.sketchEntityId,
              parent.sketchControl.sketchAccessToken,
              parent.sketchControl.relatedEntityId,
              parent.sketchControl.relatedEntityName
            ).then(sketchRes => {
              parent.sketchControl.clear();
              SketchFromJSON.read(sketchRes.sketch, parent.sketchControl);

              if (sketchRes.draftId) {
                parent.sketchControl.draftData = {
                  draftId: sketchRes.draftId,
                  draftLocked: sketchRes.draftLocked,
                  draftLockedBy: sketchRes.draftLockedBy,
                  draftTemplateId: draftTemplateId
                };
              } else {
                parent.sketchControl.draftData = null;
              }

              sketchRes.items.forEach(item => {
                if (item.entityName != "ptas_sketch") {
                  parent.sketchControl.relatedEntity = item.changes;
                  parent.sketchControl.relatedEntityLoaded = { ...item.changes }
                  let entity = parent.sketchControl.sketchEntity;
                  let topCenter = document.getElementById("currentSketchInfo");
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
              parent.sketchControl.findProjection(projection);
              if (projection.projected.length > 0) {
                parent.sketchControl.zoomToContents(projection.bounds, 0.7);
              }
              addScratchpadLayer();
              if (
                parent.sketchControl.layers &&
                parent.sketchControl.layers.length > 0
              ) {
                parent.sketchControl.updateSavedLayers();
                parent.sketchControl.updateSavedTotalsInfo(
                  parent.sketchControl.savedLayers
                );
              }
              hideLoading();
            });
          })
          .catch(err => {
            console.error(err);
            showLoading(err);
            return;
          });
      })
      .catch(err => {
        console.error(err);
        showLoading(err);
        return;
      });
  } else {
    showLoading("Loading sketch...");
    SketchAPIService.getSketch(
      sketchEntityId,
      parent.sketchControl.sketchAccessToken,
      parent.sketchControl.relatedEntityId,
      parent.sketchControl.relatedEntityName
    )
      .then(getSketchRes => {
        if (getSketchRes) {
          parent.sketchControl.clear();
          parent.sketchControl.parcelData = getSketchRes.parcel;
          parent.sketchControl.userData = getSketchRes.user;
          if (getSketchRes.draftId) {
            parent.sketchControl.draftData = {
              draftId: getSketchRes.draftId,
              draftLocked: getSketchRes.draftLocked,
              draftLockedBy: getSketchRes.draftLockedBy
            };
          } else {
            parent.sketchControl.draftData = null;
          }

          getSketchRes.items.forEach(item => {
            if (item.entityName === "ptas_sketch") {
              parent.sketchControl.sketchEntity = item.changes;
              parent.sketchControl.sketchEntityLoaded = { ...item.changes }
              parent.sketchControl.sketchEntityId =
                item.changes["ptas_sketchid"];
              if (parent.sketchControl.sketchEntity["ptas_isofficial"]) {
                parent.sketchControl.sketchIsOfficial = true;
              } else {
                parent.sketchControl.sketchIsOfficial = false;
              }
            } else {
              parent.sketchControl.relatedEntity = item.changes;
              parent.sketchControl.relatedEntityLoaded = { ...item.changes }
              let entity = parent.sketchControl.sketchEntity;
              let topCenter = document.getElementById("currentSketchInfo");
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

          SketchFromJSON.read(getSketchRes.sketch, parent.sketchControl);
          let projection = { center: {}, projected: [], index: 0 };
          parent.sketchControl.findProjection(projection);
          if (projection.projected.length > 0) {
            parent.sketchControl.zoomToContents(projection.bounds, 0.7);
          }

          if (parent.sketchControl.draftData) {
            SketchAPIService.getSketch(
              parent.sketchControl.draftData.draftId,
              parent.sketchControl.sketchAccessToken,
              parent.sketchControl.relatedEntityId,
              parent.sketchControl.relatedEntityName
            ).then(draftRes => {
              const draftEntity = draftRes.items.find(
                item => item.entityName == "ptas_sketch"
              ).changes;
              parent.sketchControl.draftData.draftTemplateId =
                draftEntity._ptas_templateid_value;
              hideLoading();
            });
          } else {
            hideLoading();
          }

          addScratchpadLayer();
          //Update saved layers once the sketch has been drawn (and fillPath is set)
          if (
            parent.sketchControl.layers &&
            parent.sketchControl.layers.length > 0
          ) {
            parent.sketchControl.updateSavedLayers();
            parent.sketchControl.updateSavedTotalsInfo(
              parent.sketchControl.savedLayers
            );
            parent.sketchControl.layers.map(layer => {
              layer.savedArea = Math.round(
                layer.netArea
                  ? layer.netArea
                  : layer.grossArea
                  ? layer.grossArea
                  : 0
              );
            });
          }
          if (parent.sketchControl.firstLoad) {
            parent.sketchControl.firstLoad = false;
          }
        } else {
          showLoading("Could not get sketch");
          console.error("Invalid sketch response. Check token expiration.");
        }
      })
      .catch(err => {
        console.error(err);
        showLoading(err);
        return;
      });
  }
}

/**
 * Delete Version button event handler.
 *
 * @returns {void}
 */
function deleteSketchVersion() {
  if (
    parent.sketchControl.sketchEntityId !==
    parent.sketchControl.selectedSketchVersion.ptas_sketchid
  ) {
    showLoading("Deleting sketch version...");
    SketchAPIService.deleteSketch(
      parent.sketchControl.selectedSketchVersion.ptas_sketchid,
      parent.sketchControl.sketchAccessToken
    ).then(deleteRes => {
      showDialog("Sketch version deleted");
      if (
        parent.sketchControl.draftData &&
        parent.sketchControl.selectedSketchVersion.ptas_sketchid ===
          parent.sketchControl.draftData["draftId"]
      ) {
        parent.sketchControl.draftData = null;
      }
      hideLoading();
      showSketchVersions(parent.sketchControl.versionsContainerId);
    });
  } else {
    showDialog("The sketch being edited cannot be deleted");
  }
}

/**
 * Displays the specified entity's tags.
 *
 * @param {string} sketchEntity
 * @param {string} sketchTagsElementName
 * @returns {void}
 */
function showSketchTags(sketchEntity, sketchTagsElementName) {
  const sketchTagsDiv = document.getElementById(sketchTagsElementName);
  while (sketchTagsDiv.firstChild) {
    sketchTagsDiv.removeChild(sketchTagsDiv.lastChild);
  }
  if (sketchEntity.ptas_tags) {
    const sketchTags = sketchEntity.ptas_tags.split(",");

    for (let i = 0; i < sketchTags.length; i++) {
      if (sketchTags[i].length > 0) {
        const tag = document.createElement("div");
        tag.setAttribute(
          "style",
          `background: black; color: white; 
          margin: 5px; padding: 4px 10px; border-radius: 15px;`
        );
        const span = document.createElement("span");
        span.innerHTML = sketchTags[i];
        const deleteButton = document.createElement("button");
        deleteButton.innerHTML = "X";
        deleteButton.onclick = event => {
          if (sketchEntity.ptas_tags.indexOf(sketchTags[i] + ",") >= 0) {
            sketchEntity.ptas_tags = sketchEntity.ptas_tags.replace(
              sketchTags[i] + ",",
              ""
            );
          } else if (sketchEntity.ptas_tags.indexOf("," + sketchTags[i]) >= 0) {
            sketchEntity.ptas_tags = sketchEntity.ptas_tags.replace(
              "," + sketchTags[i],
              ""
            );
          } else if (sketchEntity.ptas_tags.indexOf(sketchTags[i]) >= 0) {
            sketchEntity.ptas_tags = sketchEntity.ptas_tags.replace(
              sketchTags[i],
              ""
            );
          }
          showSketchTags(sketchEntity, sketchTagsElementName);
        };
        deleteButton.setAttribute(
          "style",
          `background: none;
          color: white; border: none; outline: none; cursor: pointer;
          padding: 0; margin-left: 5px;`
        );

        tag.appendChild(span);
        tag.appendChild(deleteButton);
        sketchTagsDiv.appendChild(tag);
      }
    }
  }
}

/**
 * Is Official slider event handler.
 *
 * @returns {void}
 */
function saveSketchIsOfficial() {
  if (parent.sketchControl.sketchEntity) {
    if (parent.sketchControl.sketchEntity["ptas_isofficial"]) {
      parent.sketchControl.sketchEntity["ptas_isofficial"] = false;
    } else {
      parent.sketchControl.sketchEntity["ptas_isofficial"] = true;
    }
  }
}

/**
 * Fill up the total areas list.
 *
 * @param {string} modalId
 * @param {HTMLElement} table
 * @param {object} totals
 * @param {HTMLElement} bodyTable
 * @returns {void}
 */
function populateAreaTotalsTable(modalId, table, totals, bodyTable) {
  const entityCat = parent.sketchControl.entityCategories.find(
    cat => cat.category == "Building"
  );
  if (
    entityCat &&
    parent.sketchControl.relatedEntity.residentialFilterProperty ==
      entityCat.residentialFilterProperty &&
    parent.sketchControl.relatedEntity.residentialFilterValue ==
      entityCat.residentialFilterValue
  ) {
    parent.sketchControl.commercial = true;
  } else {
    parent.sketchControl.commercial = false;
  }
  //#region Header/totals rows
  const totalsRow = table.querySelector(".areaTotalsSumRow");
  for (let i = totalsRow.cells.length - 1; i > 0; i--) {
    totalsRow.deleteCell(i);
  }
  const totalLivingNew = totalsRow.insertCell(1);
  const totalLivingGap = totalsRow.insertCell(2);
  const totalGrossTitle = totalsRow.insertCell(3);
  const totalGrossNew = totalsRow.insertCell(4);
  const totalGrossGap = totalsRow.insertCell(5);
  let negative = false;
  let area = 0;
  totalGrossTitle.setAttribute("colspan", "3");
  if (
    modalId === "infoModal" ||
    modalId === "saveModal" ||
    modalId === "versions"
  ) {
    negative = totals.totalLivingArea.new < 0;
    area =
      Math.round(Math.round(Math.abs(totals.totalLivingArea.new)) / 10) * 10;
    totalLivingNew.innerHTML = `${negative ? -area : area} ⏍`;
    negative = totals.totalLivingArea.gap < 0;
    area =
      Math.round(Math.round(Math.abs(totals.totalLivingArea.gap)) / 10) * 10;
    totalLivingGap.innerHTML = `${negative ? -area : area} ⏍`;
    negative = totals.totalGrossArea.new < 0;
    area =
      Math.round(Math.round(Math.abs(totals.totalGrossArea.new)) / 10) * 10;
    totalGrossNew.innerHTML = `${negative ? -area : area} ⏍`;
    negative = totals.totalGrossArea.gap < 0;
    area =
      Math.round(Math.round(Math.abs(totals.totalGrossArea.gap)) / 10) * 10;
    totalGrossGap.innerHTML = `${negative ? -area : area} ⏍`;
  } else if (modalId === "updateModal") {
    totalLivingNew.setAttribute("id", "total-living-new");
    totalLivingGap.setAttribute("id", "total-living-gap");
    totalGrossNew.setAttribute("id", "total-gross-new");
    totalGrossGap.setAttribute("id", "total-gross-gap");
    negative = totals.totalLivingArea.new < 0;
    area =
      Math.round(Math.round(Math.abs(totals.totalLivingArea.new)) / 10) * 10;
    totalLivingNew.innerHTML = `${negative ? -area : area} ⏍`;
    negative = totals.totalLivingArea.gap < 0;
    area =
      Math.round(Math.round(Math.abs(totals.totalLivingArea.gap)) / 10) * 10;
    totalLivingGap.innerHTML = `${negative ? -area : area} ⏍`;
    if (parent.sketchControl.commercial) {
      category = parent.sketchControl.entityCategories.find(
        cat => cat.category == "Commercial"
      );
      table.getElementsByClassName("modal-title")[0].innerHTML =
        "Net rentable area";
      table.getElementsByClassName("modal-title")[1].innerHTML = "Gross area";
    }
    negative = totals.totalGrossArea.new < 0;
    area =
      Math.round(Math.round(Math.abs(totals.totalGrossArea.new)) / 10) * 10;
    totalGrossNew.innerHTML = `${negative ? -area : area} ⏍`;
    negative = totals.totalGrossArea.gap < 0;
    area =
      Math.round(Math.round(Math.abs(totals.totalGrossArea.gap)) / 10) * 10;
    totalGrossGap.innerHTML = `${negative ? -area : area} ⏍`;
    bodyTable.classList.add("modal-list");
  }
  totalLivingNew.style.borderBottom = totalLivingGap.style.borderBottom = totalGrossTitle.style.borderBottom = totalGrossNew.style.borderBottom = totalGrossGap.style.borderBottom =
    "1px black solid";
  totalLivingNew.style.fontWeight = totalGrossNew.style.fontWeight = totalLivingGap.style.fontWeight = totalGrossGap.style.fontWeight =
    "bold";
  if (totals.totalLivingArea.gap >= 5) {
    totalLivingGap.innerHTML = "+" + totalLivingGap.innerHTML;
    totalLivingGap.style.color = "green";
  } else if (totals.totalLivingArea.gap <= -5) {
    totalLivingGap.style.color = "red";
  }
  if (totals.totalGrossArea.gap >= 5) {
    totalGrossGap.innerHTML = "+" + totalGrossGap.innerHTML;
    totalGrossGap.style.color = "green";
  } else if (totals.totalGrossArea.gap <= -5) {
    totalGrossGap.style.color = "red";
  }
  //#endregion

  //#region Data rows
  const tableRowStart = 4;
  const layerKeys = Object.keys(totals.layers);
  layerKeys.sort((layerKey1, layerKey2) => {
    if (totals.layers[layerKey1].index < totals.layers[layerKey2].index) {
      return -1;
    }
    return 1;
  });

  for (let i = table.rows.length - 1; i > tableRowStart - 1; i--) {
    table.deleteRow(i);
  }
  if (totals.layers && layerKeys && layerKeys.length > 0) {
    const layerTable = bodyTable.querySelector(".list-body");
    let livingIndex = 0;
    let grossIndex = 0;
    let tr;
    let name;
    let livingNew;
    let livingGap;
    let grossName;
    let grossNew;
    let grossGap;

    for (let i = 0; i < layerKeys.length; i++) {
      const layer = totals.layers[layerKeys[i]];
      if (parent.sketchControl.layerList[layer.name].livingArea) {
        livingIndex++;
        if (livingIndex > grossIndex) {
          tr = layerTable.insertRow(layerTable.rows.length);
          name = tr.insertCell(0);
          livingNew = tr.insertCell(1);
          livingGap = tr.insertCell(2);
          grossName = tr.insertCell(3);
          grossNew = tr.insertCell(4);
          grossGap = tr.insertCell(5);
        } else {
          tr = layerTable.rows[livingIndex].cells;
          name = layerTable.rows[livingIndex].cells[0];
          livingNew = layerTable.rows[livingIndex].cells[1];
          livingGap = layerTable.rows[livingIndex].cells[2];
        }
        name.innerHTML = layer.name;
      } else {
        grossIndex++;
        if (livingIndex < grossIndex) {
          tr = layerTable.insertRow(layerTable.rows.length);
          name = tr.insertCell(0);
          livingNew = tr.insertCell(1);
          livingGap = tr.insertCell(2);
          grossName = tr.insertCell(3);
          grossNew = tr.insertCell(4);
          grossGap = tr.insertCell(5);
        } else {
          tr = layerTable.rows[grossIndex].cells;
          grossName = layerTable.rows[grossIndex].cells[3];
          grossNew = layerTable.rows[grossIndex].cells[4];
          grossGap = layerTable.rows[grossIndex].cells[5];
        }
        grossName.innerHTML = layer.name;
      }

      if (
        modalId === "infoModal" ||
        modalId === "saveModal" ||
        modalId === "versions"
      ) {
        if (parent.sketchControl.layerList[layer.name].livingArea) {
          negative = layer.livingArea.new < 0;
          area =
            Math.round(Math.round(Math.abs(layer.livingArea.new)) / 10) * 10;
          livingNew.innerHTML = negative ? -area : area;
          negative = layer.livingArea.gap < 0;
          area =
            Math.round(Math.round(Math.abs(layer.livingArea.gap)) / 10) * 10;
          livingGap.innerHTML = negative ? -area : area;
        }
        if (parent.sketchControl.layerList[layer.name].grossArea) {
          negative = layer.grossArea.new < 0;
          area =
            Math.round(Math.round(Math.abs(layer.grossArea.new)) / 10) * 10;
          grossNew.innerHTML = negative ? -area : area;
          negative = layer.grossArea.gap < 0;
          area =
            Math.round(Math.round(Math.abs(layer.grossArea.gap)) / 10) * 10;
          grossGap.innerHTML = negative ? -area : area;
        }
      } else if (modalId === "updateModal") {
        const entityProperty =
          parent.sketchControl.layerList[layer.name].entityProperty;
        if (parent.sketchControl.layerList[layer.name].livingArea) {
          negative = layer.livingArea.new < 0;
          area =
            Math.round(
              Math.round(Math.abs(layer.livingArea.new.toFixed(2))) / 10
            ) * 10;
          livingNew.innerHTML = `<input type="number" step="10" data-entity-prop="${entityProperty}" data-layer-name="${
            layer.name
          }"
              name="livingSaved" onChange="changeField(event, ${
                layer.livingArea.saved
              }, 'living')"
              value=${negative ? -area : area} />`;
          livingGap.setAttribute("name", layer.name);
          negative = layer.livingArea.gap < 0;
          area =
            Math.round(
              Math.round(Math.abs(layer.livingArea.gap.toFixed(2))) / 10
            ) * 10;
          livingGap.innerHTML = negative ? -area : area;
        } else {
          negative = layer.grossArea.new < 0;
          area =
            Math.round(
              Math.round(Math.abs(layer.grossArea.new.toFixed(2))) / 10
            ) * 10;
          grossNew.innerHTML = `<input type="number" step="10" data-entity-prop="${entityProperty}" data-layer-name="${
            layer.name
          }"
              name="grossSaved" onChange="changeField(event, ${
                layer.livingArea.saved
              }, 'gross')"
              value=${negative ? -area : area} />`;
          grossGap.setAttribute("name", layer.name);
          negative = layer.grossArea.gap < 0;
          area =
            Math.round(
              Math.round(Math.abs(layer.grossArea.gap.toFixed(2))) / 10
            ) * 10;
          grossGap.innerHTML = negative ? -area : area;
        }
      }

      name.setAttribute("colspan", "3");
      grossName.setAttribute("colspan", "3");
      livingNew.style.fontWeight = "bold";
      grossNew.style.fontWeight = "bold";
      if (layer.livingArea) {
        if (layer.livingArea.gap >= 5) {
          livingGap.innerHTML = "+" + livingGap.innerHTML;
          livingGap.style.color = "green";
          livingGap.style.fontWeight = "bold";
        } else if (layer.livingArea.gap <= -5) {
          livingGap.style.color = "red";
          livingGap.style.fontWeight = "bold";
        }
      }
      if (layer.grossArea) {
        if (layer.grossArea.gap >= 5) {
          grossGap.innerHTML = "+" + grossGap.innerHTML;
          grossGap.style.color = "green";
          grossGap.style.fontWeight = "bold";
        } else if (layer.grossArea.gap <= -5) {
          grossGap.style.color = "red";
          grossGap.style.fontWeight = "bold";
        }
      }
    }
  }
  //#endregion
}

/**
 * Area input change event handler.
 *
 * @param {Event} event - Input event
 * @param {number} saved
 * @param {string} area
 * @returns {void}
 */
function changeField(event, saved, area) {
  let change = event.target.value - saved;
  const candidates = document.getElementsByName(
    event.target.getAttribute("data-layer-name")
  );
  const areaSaved = document.getElementsByName(area + "Saved");
  const totalSaved = document.getElementById("total-" + area + "-gap");
  for (let i = 0; i < candidates.length; i++) {
    if (candidates[i].nodeName == "TD") {
      if (change > 0) {
        candidates[i].style.color = "green";
        candidates[i].style.fontWeight = "bold";
        candidates[i].innerHTML = "+" + change;
      } else {
        if (change < 0) {
          candidates[i].style.color = "red";
          candidates[i].style.fontWeight = "bold";
        } else {
          candidates[i].style.color = "black";
          candidates[i].style.fontWeight = "normal";
        }
        candidates[i].innerHTML = change;
      }
      break;
    }
  }
  let newArea = 0;
  for (let i = 0; i < areaSaved.length; i++) {
    newArea += parseInt(areaSaved[i].value);
  }
  change =
    newArea -
    parent.sketchControl.totalsInfo[
      "total" + area.charAt(0).toUpperCase() + area.slice(1) + "Area"
    ].saved;
  document.getElementById("total-" + area + "-new").innerHTML = newArea;
  if (change > 0) {
    totalSaved.style.color = "green";
    totalSaved.style.fontWeight = "bold";
    totalSaved.innerHTML = "+" + change;
  } else {
    if (change < 0) {
      totalSaved.style.color = "red";
      totalSaved.style.fontWeight = "bold";
    } else {
      totalSaved.style.color = "black";
      totalSaved.style.fontWeight = "normal";
    }
    totalSaved.innerHTML = change;
  }
}

/**
 * View button event handler.
 *
 * @returns {void}
 */
function viewClicked() {
  let viewButtonText = document.getElementById("viewButtonText");
  let viewButtonImage = document.getElementById("viewButtonImage");
  let viewMenu = document.getElementById("viewMenu");
  if (viewButtonText.className === "sketchClickableText") {
    collapseMenus();
    viewButtonText.className = "sketchClickableTextHighlighted";
    viewButtonImage.src = "ptas_expand_down_highlighted.png";
    viewMenu.style.visibility = "visible";
  } else {
    viewButtonText.className = "sketchClickableText";
    viewButtonImage.src = "ptas_expand_down.png";
    viewMenu.style.visibility = "collapse";
  }
}

/**
 * View -> Tile event handler.
 *
 * @param {MouseEvent} event - Input event
 * @returns {void}
 */
function viewTileClicked(event) {
  penUpClicked()
  switchActiveView(event);
  if (parent.sketchControl.viewMode !== "tile") {
    if (parent.sketchControl.autoFit) parent.sketchControl.autoFitClicked();
    parent.sketchControl.viewMode = "tile";
    let usedLayers = 0;
    const maxBounds = { max: {}, min: {} };
    parent.sketchControl.layers.map(layer => {
      if (layer.objects && layer.objects.length > 0) {
        const bounds = {};
        layer.getBounds(bounds);
        if (maxBounds.max.y) {
          maxBounds.max.x = Math.max(maxBounds.max.x, bounds.max.x);
          maxBounds.min.x = Math.min(maxBounds.min.x, bounds.min.x);
          maxBounds.max.y = Math.max(maxBounds.max.y, bounds.max.y);
          maxBounds.min.y = Math.min(maxBounds.min.y, bounds.min.y);
        } else if (bounds.max) {
          maxBounds.max.x = bounds.max.x;
          maxBounds.min.x = bounds.min.x;
          maxBounds.max.y = bounds.max.y;
          maxBounds.min.y = bounds.min.y;
        }
      }
    });
    let layerY = maxBounds.min.y;
    const margin = 2;
    for (let i = parent.sketchControl.layers.length - 1; i > -1; i--) {
      const layer = parent.sketchControl.layers[i];
      if (layer.objects && layer.objects.length > 0) {
        const layerX =
          usedLayers % 2 < 1 ? maxBounds.min.x : maxBounds.max.x + margin * 2;
        const bounds = {};
        layer.getBounds(bounds);
        layer.objects.map(object => {
          if (bounds.min) {
            if (object.objects && object.objects[0] instanceof SketchPath) {
              object.objects[0].start.stackX = object.objects[0].start.x;
              object.objects[0].start.stackY = object.objects[0].start.y;
              object.objects[0].start.x =
                layerX + object.objects[0].start.x - bounds.min.x;
              object.objects[0].start.y -= maxBounds.max.y + layerY;

              if (object.objects[0].elements) {
                object.objects[0].elements.map(element => {
                  element.stackX = element.x;
                  element.stackY = element.y;
                  element.x = layerX + element.x - bounds.min.x;
                  element.y -= maxBounds.max.y + layerY;
                });
              }
            } else if (
              object.objects &&
              object.objects[0] instanceof SketchCustomText
            ) {
              object.objects[0].stackX = object.objects[0].x;
              object.objects[0].stackY = object.objects[0].y;
              object.objects[0].x = layerX + object.objects[0].x - bounds.min.x;
              object.objects[0].y -= maxBounds.max.y + layerY;
            }
          }
        });
        const object = new SketchObject();
        const path = new SketchPath();
        object.objects = [path];
        path.tile = true;
        path.start = {
          x: layerX - margin,
          y: -layerY + margin
        };
        path.elements = [
          {
            x: layerX + maxBounds.max.x - maxBounds.min.x + margin,
            y: -layerY + margin
          },
          {
            x: layerX + maxBounds.max.x - maxBounds.min.x + margin,
            y: -layerY + maxBounds.min.y - maxBounds.max.y - margin
          },
          {
            x: layerX - margin,
            y: -layerY + maxBounds.min.y - maxBounds.max.y - margin
          },
          {
            x: layerX - margin,
            y: -layerY + margin
          }
        ];
        layer.objects.push(object);
        if (usedLayers % 2 > 0)
          layerY += maxBounds.max.y - maxBounds.min.y + margin * 2;
        usedLayers++;
      }
    }
    fitClicked();
    panClicked();
  }
}

/**
 * View -> Stack event handler.
 *
 * @param {MouseEvent} event - Input event
 * @returns {void}
 */
function viewStackClicked(event) {
  switchActiveView(event);
  if (parent.sketchControl.viewMode !== "stack") {
    if (parent.sketchControl.autoFit) parent.sketchControl.autoFitClicked();
    parent.sketchControl.viewMode = "stack";
    parent.sketchControl.layers.map(layer => {
      if (layer.objects && layer.objects.length > 0) {
        if (layer.objects[layer.objects.length - 1].objects[0].tile) {
          layer.objects.pop();
        }
        layer.objects.map(object => {
          if (object.objects && object.objects[0] instanceof SketchPath) {
            object.objects[0].start.x = object.objects[0].start.stackX;
            object.objects[0].start.y = object.objects[0].start.stackY;
            if (object.objects[0].elements) {
              object.objects[0].elements.map(element => {
                element.x = element.stackX;
                element.y = element.stackY;
              });
            }
          } else if (
            object.objects &&
            object.objects[0] instanceof SketchCustomText
          ) {
            object.objects[0].x = object.objects[0].stackX;
            object.objects[0].y = object.objects[0].stackY;
          }
        });
      }
    });
    fitClicked();
  }
}

/**
 * Toggles the highlight between Tile and Stack in the View menu.
 *
 * @param {MouseEvent} event - Input event
 * @returns {void}
 */
function switchActiveView(event) {
  let views = document.getElementsByClassName("active-view");
  for (let i = 0; i < views.length; i++) {
    views[i].style.visibility = "collapse";
  }
  if (event)
    event.target.children.namedItem("active-view").style.visibility = "inherit";
  else document.getElementById("active-stack").style.visibility = "inherit";
}

/**
 * View -> Fit event handler.
 *
 * @param {MouseEvent} event - Input event
 * @returns {void}
 */
function fitClicked() {
  if (!parent.sketchControl.autoFit) {
    parent.sketchControl.autoFit = true;
    parent.sketchControl.draw();
    parent.sketchControl.autoFit = false;
  } else {
    parent.sketchControl.autoFitClicked();
  }
  collapseViewMenu();
}

/**
 * Switches the clicked dropdown menu's visibility.
 *
 * @param {MouseEvent} event - Input event
 * @returns {void}
 */
function showDropdown(event) {
  let menuArray = event.target.getElementsByClassName(
    "read-only-dropdown-menu"
  );
  closeDropdowns();
  if (menuArray.length > 0) {
    menuArray[0].style.visibility =
      menuArray[0].style.visibility == "visible" ? "collapse" : "visible";
  }
}

/**
 * Sets the zoom to the given level on read-only mode.
 *
 * @param {number} level
 * @returns {void}
 */
function setZoom(level) {
  let position = parent.sketchControl.inverseTopLevelTransform.transform(
    new paper.Point(window.innerWidth / 2, window.innerHeight / 2)
  );
  parent.sketchControl.resolution = level / 10;
  parent.sketchControl.updateTopLevelTransform();
  position = parent.sketchControl.topLevelTransform.transform(position);
  parent.sketchControl.offsetX -= position.x - window.innerWidth / 2;
  parent.sketchControl.offsetY -= position.y - window.innerHeight / 2;
  parent.sketchControl.updateTopLevelTransform();
  parent.sketchControl.draw();
  closeDropdowns();
}

/**
 * Toggles the view of the selected option's check icon.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function toggleCheck(event) {
  let check = event.target.getElementsByClassName("check")[0];
  check.style.visibility =
    check.style.visibility == "inherit" ? "collapse" : "inherit";
}

/**
 * Show -> Distances event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function showDistances(event) {
  toggleCheck(event);
  showDistancesClicked();
}

/**
 * Show -> Square Feet event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function showSF(event) {
  toggleCheck(event);
  showSFClicked();
}

/**
 * Show -> Grid event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function showGrid(event) {
  toggleCheck(event);
  hideGridlinesClicked();
}

/**
 * Style -> Thick event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function styleThick(event) {
  toggleCheck(event);
}

/**
 * Style -> Thin event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function styleThin(event) {
  toggleCheck(event);
}

/**
 * Style -> Solid event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function styleSolid(event) {
  toggleCheck(event);
}

/**
 * Style -> Hollow event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function styleHollow(event) {
  toggleCheck(event);
}

/**
 * Style -> Color event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function styleColor(event) {
  toggleCheck(event);
}

/**
 * Style -> Gray event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function styleGray(event) {
  toggleCheck(event);
}

/**
 * Style -> VCADD event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function styleVCADD(event) {
  toggleCheck(event);
}

/**
 * Style -> IRP event handler on read-only mode.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function styleIRP(event) {
  toggleCheck(event);
}

/**
 * Collapses all dropdown menus on read-only mode.
 *
 * @returns {void}
 */
function closeDropdowns() {
  let menuArray = document.getElementsByClassName("read-only-dropdown-menu");
  for (let i = 0; i < menuArray.length; i++) {
    menuArray[i].style.visibility = "collapse";
  }
}

/**
 * View -> Snap to Grid event handler.
 *
 * @returns {void}
 */
function snapToGridClicked() {
  let snapToGridImage = document.getElementById("snapToGridImage");
  if (snapToGridImage.style.visibility === "collapse") {
    snapToGridImage.style.visibility = "inherit";
    parent.sketchControl.snapToGrid = true;
  } else {
    snapToGridImage.style.visibility = "collapse";
    parent.sketchControl.snapToGrid = false;
  }
  collapseViewMenu();
}

/**
 * View -> Snap to Line event handler.
 *
 * @returns {void}
 */
function snapToLineClicked() {
  let snapToLineImage = document.getElementById("snapToLineImage");
  if (snapToLineImage.style.visibility === "collapse") {
    snapToLineImage.style.visibility = "inherit";
    parent.sketchControl.snapToLine = true;
  } else {
    snapToLineImage.style.visibility = "collapse";
    parent.sketchControl.snapToLine = false;
  }
  collapseViewMenu();
}

/**
 * View -> Hide Grid Lines event handler.
 *
 * @returns {void}
 */
function hideGridlinesClicked() {
  let hideGridlinesImage = document.getElementById("hideGridlinesImage");
  if (hideGridlinesImage.style.visibility === "collapse") {
    hideGridlinesImage.style.visibility = "inherit";
    parent.sketchControl.gridVisible = false;
  } else {
    hideGridlinesImage.style.visibility = "collapse";
    parent.sketchControl.gridVisible = true;
  }
  collapseViewMenu();
  parent.sketchControl.draw();
}

/**
 * Select tool button event handler.
 *
 * @returns {void}
 */
function selectClicked() {
  collapseMenus();
  let selectButton = document.getElementById("selectButton");
  selectButton.src = "ptas_select_highlighted.png";
  let drawButton = document.getElementById("drawButton");
  drawButton.src = "ptas_draw.png";
  let textButton = document.getElementById("textButton");
  textButton.src = "ptas_text.png";
  let panButton = document.getElementById("panButton");
  panButton.src = "ptas_pan.png";
  let selectToolbar = document.getElementById("selectToolbar");
  selectToolbar.style.visibility = "visible";
  let drawToolbar = document.getElementById("drawToolbar");
  drawToolbar.style.visibility = "collapse";
  let textToolbar = document.getElementById("textToolbar");
  textToolbar.style.visibility = "collapse";
  let multipleSelectOn = document.getElementById("multipleSelectOn");
  if (multipleSelectOn.style.visibility === "inherit") {
    parent.sketchControl.mode = SketchMode.MultipleSelect;
    lastAction("multiple");
  } else {
    parent.sketchControl.mode = SketchMode.SingleSelect;
    lastAction("single");
  }
  parent.sketchControl.styleSet = sketchStyleSet;
  hideKeypad();
  if (parent.sketchControl.viewMode !== "stack") viewStackClicked(null);
  document.getElementById("sketchCanvas").style.cursor = "auto";
  parent.sketchControl.showContextBar("none");
  parent.sketchControl.autoSave();
}

/**
 * Draw tool button event handler.
 *
 * @returns {void}
 */
function drawClicked() {
  collapseMenus();
  let selectButton = document.getElementById("selectButton");
  selectButton.src = "ptas_select.png";
  let drawButton = document.getElementById("drawButton");
  drawButton.src = "ptas_draw_highlighted.png";
  let textButton = document.getElementById("textButton");
  textButton.src = "ptas_text.png";
  let panButton = document.getElementById("panButton");
  panButton.src = "ptas_pan.png";
  let selectToolbar = document.getElementById("selectToolbar");
  selectToolbar.style.visibility = "collapse";
  let drawToolbar = document.getElementById("drawToolbar");
  drawToolbar.style.visibility = "visible";
  let textToolbar = document.getElementById("textToolbar");
  textToolbar.style.visibility = "collapse";
  parent.sketchControl.showContextBar("distance");
  parent.sketchControl.mode = SketchMode.Draw;
  parent.sketchControl.styleSet = sketchDrawingStyleSet;
  if (parent.sketchControl.penDown) {
    parent.sketchControl.showContextBar("line");
    lastAction("pen down");
  } else {
    if (!parent.sketchControl.penUpCursor) {
      const newEvent = {
        offsetX: window.innerhWidth / 2,
        offsetY: window.innerHeight / 2
      };
      parent.sketchControl.move(this, newEvent);
    }
    lastAction("pen up");
  }
  if (parent.sketchControl.viewMode !== "stack") viewStackClicked(null);
  document.getElementById("sketchCanvas").style.cursor = "auto";
  parent.sketchControl.autoSave();
}

/**
 * Text tool button event handler.
 *
 * @returns {void}
 */
function textClicked() {
  collapseMenus();
  let selectButton = document.getElementById("selectButton");
  selectButton.src = "ptas_select.png";
  let drawButton = document.getElementById("drawButton");
  drawButton.src = "ptas_draw.png";
  let textButton = document.getElementById("textButton");
  textButton.src = "ptas_text_highlighted.png";
  let panButton = document.getElementById("panButton");
  panButton.src = "ptas_pan.png";
  let selectToolbar = document.getElementById("selectToolbar");
  selectToolbar.style.visibility = "collapse";
  let drawToolbar = document.getElementById("drawToolbar");
  drawToolbar.style.visibility = "collapse";
  let textToolbar = document.getElementById("textToolbar");
  textToolbar.style.visibility = "visible";
  parent.sketchControl.mode = SketchMode.Text;
  parent.sketchControl.styleSet = sketchStyleSet;
  hideKeypad();
  if (parent.sketchControl.viewMode !== "stack") viewStackClicked(null);
  document.getElementById("sketchCanvas").style.cursor = "auto";
  lastAction("no text");
  parent.sketchControl.autoSave();
}

/**
 * Pan tool button event handler.
 *
 * @returns {void}
 */
function panClicked() {
  collapseMenus();
  let selectButton = document.getElementById("selectButton");
  selectButton.src = "ptas_select.png";
  let drawButton = document.getElementById("drawButton");
  drawButton.src = "ptas_draw.png";
  let textButton = document.getElementById("textButton");
  textButton.src = "ptas_text.png";
  let panButton = document.getElementById("panButton");
  panButton.src = "ptas_pan_highlighted.png";
  let selectToolbar = document.getElementById("selectToolbar");
  selectToolbar.style.visibility = "collapse";
  let drawToolbar = document.getElementById("drawToolbar");
  drawToolbar.style.visibility = "collapse";
  let textToolbar = document.getElementById("textToolbar");
  textToolbar.style.visibility = "collapse";
  parent.sketchControl.mode = SketchMode.Pan;
  parent.sketchControl.styleSet = sketchStyleSet;
  hideKeypad();
  document.getElementById("sketchCanvas").style.cursor = "grab";
  lastAction("pan");
}

/**
 * Select -> Multiple event handler.
 *
 * @returns {void}
 */
function multipleSelectClicked() {
  let multipleSelectOn = document.getElementById("multipleSelectOn");
  multipleSelectOn.style.visibility = "inherit";
  let singleSelectOn = document.getElementById("singleSelectOn");
  singleSelectOn.style.visibility = "collapse";
  parent.sketchControl.mode = SketchMode.MultipleSelect;
  parent.sketchControl.styleSet = sketchStyleSet;
  updateNegativeSwitch(SketchMode.MultipleSelect, false);
  parent.sketchControl.draw();
  lastAction("multiple");
}

/**
 * Select -> Single event handler.
 *
 * @returns {void}
 */
function singleSelectClicked() {
  let multipleSelectOn = document.getElementById("multipleSelectOn");
  multipleSelectOn.style.visibility = "collapse";
  let singleSelectOn = document.getElementById("singleSelectOn");
  singleSelectOn.style.visibility = "inherit";
  parent.sketchControl.mode = SketchMode.SingleSelect;
  parent.sketchControl.styleSet = sketchStyleSet;
  updateNegativeSwitch(SketchMode.SingleSelect, false);
  parent.sketchControl.draw();
  lastAction("single");
}

/**
 * Select -> Transform event handler.
 *
 * @returns {void}
 */
function transformClicked() {
  lastAction("transform");
  collapseLayersMenu();
  let transformButtonText = document.getElementById("transformButtonText");
  let transformButtonImage = document.getElementById("transformButtonImage");
  let transformMenu = document.getElementById("transformMenu");
  if (transformButtonText.className === "sketchClickableText") {
    collapseMenus();
    transformButtonText.className = "sketchClickableTextHighlighted";
    transformButtonImage.src = "ptas_expand_down_highlighted.png";
    transformMenu.style.visibility = "visible";
  } else {
    transformButtonText.className = "sketchClickableText";
    transformButtonImage.src = "ptas_expand_down.png";
    transformMenu.style.visibility = "collapse";
  }
}

/**
 * Menu item mouse over event handler.
 *
 * @param {HTMLElement} menuItem
 * @returns {void}
 */
function menuItemOver(menuItem) {
  menuItem.className = "sketchMenuItemHighlighted";
}

/**
 * Menu item mouse out event handler.
 *
 * @param {HTMLElement} menuItem
 * @returns {void}
 */
function menuItemOut(menuItem) {
  menuItem.className = "sketchMenuItem";
}

/**
 * Copies/Moves the selected Path into an existing Layer from the Layer menu.
 *
 * @param {string} uniqueIdentifier
 * @returns {void}
 */
function copyAndMoveToMenuItemClicked(uniqueIdentifier) {
  let operation = copyAndMoveToMenuOperation;
  collapseAllCopyAndMoveToMenus();
  if (
    parent.sketchControl.selected &&
    parent.sketchControl.selected.length > 0
  ) {
    const history = parent.sketchControl.createHistory();
    parent.sketchControl.addToUndoHistory(history);
    for (let i = 0; i < parent.sketchControl.selected.length; i++) {
      let selection = parent.sketchControl.selected[i];
      let sketchObject = selection.sketchObject;
      if (sketchObject && !selection.textObject) {
        let parentSketchObject = parent.sketchControl.findParent(sketchObject);
        if (parentSketchObject !== undefined) {
          if (operation === "move") {
            parentSketchObject.parent.removeLabel(
              parentSketchObject.parent.objects[parentSketchObject.index]
                .objects[0].label
            );
            parentSketchObject.parent.objects.splice(
              parentSketchObject.index,
              1
            );
            if (
              parentSketchObject.parent.objects.length == 1 &&
              parentSketchObject.parent.objects[0].objects &&
              parentSketchObject.parent.objects[0].objects.length > 0 &&
              parentSketchObject.parent.objects[0].objects[0].isLabel
            ) {
              delete parentSketchObject.parent.objects;
              delete parentSketchObject.parent.customTextForLabel;
            }
          } else if (operation === "copy") {
            sketchObject = sketchObject.createCopy();
          }
          for (let j = 0; j < parent.sketchControl.layers.length; j++) {
            let layer = parent.sketchControl.layers[j];
            if (sketchObject.objects)
              sketchObject.objects[0].layer = layer.name;
            if (layer.uniqueIdentifier === uniqueIdentifier) {
              if (layer.objects) {
                layer.objects.push(sketchObject);
              } else {
                layer.objects = [sketchObject];
              }
              layer.addLabel();
              parent.sketchControl.sketchPathToEdit = selection.path;
              parent.sketchControl.sketchLayerToEdit = uniqueIdentifier;
              parent.sketchControl.alignDistances(layer);
              break;
            }
          }
        }
      }
    }
    document.getElementById("layersMenu").style.visibility = "visible";
    parent.sketchControl.clearSelection();
  }
  lastAction("copy/move");
  parent.sketchControl.autoSave();
  parent.sketchControl.sketchPathToEdit = undefined;
}

/**
 * Copy/Move menu item mouse over event handler.
 *
 * @param {HTMLElement} menuItem
 * @returns {void}
 */
function copyAndMoveToMenuItemMouseOver(menuItem) {
  menuItem.className = "sketchCopyAndMoveToMenuItemHighlighted";
}

/**
 * Copy/Move menu item mouse out event handler.
 *
 * @param {HTMLElement} menuItem
 * @returns {void}
 */
function copyAndMoveToMenuItemMouseOut(menuItem) {
  menuItem.className = "sketchCopyAndMoveToMenuItem";
}

/**
 * Select -> Negative event handler.
 *
 * @param {string} mode
 * @returns {void}
 */
function negativeSwitchClicked(mode) {
  let negativeSwitch = document.getElementById(mode + "NegativeSwitch");
  if (negativeSwitch.src.endsWith("ptas_switch_off.png")) {
    negativeSwitch.src = "ptas_switch_on.png";
    if (mode === "select") {
      parent.sketchControl.selectionToNegative(true);
    } else if (mode === "draw") {
      parent.sketchControl.drawNegativeArea = true;
      if (parent.sketchControl.sketchPathToEdit) {
        parent.sketchControl.sketchPathToEdit.negative = true;
        lastAction("negative pen");
      }
    }
  } else {
    negativeSwitch.src = "ptas_switch_off.png";
    if (mode === "select") {
      parent.sketchControl.selectionToNegative(false);
    } else if (mode === "draw") {
      parent.sketchControl.drawNegativeArea = false;
      if (parent.sketchControl.sketchPathToEdit) {
        parent.sketchControl.sketchPathToEdit.negative = false;
        lastAction("open area");
      }
    }
  }
  parent.sketchControl.draw();
}

/**
 * Toggles the Select -> Negative switch.
 *
 * @param {string} mode
 * @param {boolean} isNegative
 * @returns {void}
 */
function updateNegativeSwitch(mode, isNegative) {
  if (mode === SketchMode.SingleSelect || mode === SketchMode.MultipleSelect) {
    let negativeSwitch = document.getElementById("selectNegativeSwitch");
    negativeSwitch.src = isNegative
      ? "ptas_switch_on.png"
      : "ptas_switch_off.png";
  }
}

/**
 * Updates the Zoom label in the View menu.
 *
 * @param {number} resolution
 * @returns {void}
 */
function updateZoomLabel(resolution) {
  let zoomLabels = document.getElementsByName("zoomLabel");
  for (let i = 0; i < zoomLabels.length; i++) {
    zoomLabels[i].textContent = (resolution * 10).toFixed(0) + "%";
  }
}

/**
 * Transform -> Mirror event handler.
 *
 * @returns {void}
 */
function mirrorClicked() {
  collapseTransformMenu();
  parent.sketchControl.flipSelectionHorizontally();
  parent.sketchControl.autoSave();
}

/**
 * Transform -> Rotate Right event handler.
 *
 * @returns {void}
 */
function rotateRightClicked() {
  collapseTransformMenu();
  parent.sketchControl.rotateSelectionRight();
  parent.sketchControl.autoSave();
}

/**
 * Transform -> Rotate Left event handler.
 *
 * @returns {void}
 */
function rotateLeftClicked() {
  collapseTransformMenu();
  parent.sketchControl.rotateSelectionLeft();
  parent.sketchControl.autoSave();
}

/**
 * Select -> All event handler.
 *
 * @returns {void}
 */
function selectAllClicked() {
  multipleSelectClicked();
  parent.sketchControl.selectAll();
}

/**
 * Move To button event handler.
 *
 * @param {string} mode - Sketch Mode selected.
 * @returns {void}
 */
function moveToClicked(mode) {
  if (!parent.sketchControl.selected) {
    parent.sketchControl.selectLayer(
      parent.sketchControl.getLayer(parent.sketchControl.sketchLayerToEdit)
    );
  }
  collapseLayersMenu();
  let moveToBackground = document.getElementById(mode + "MoveToBackground");
  let moveToText = document.getElementById(mode + "MoveToText");
  let copyAndMoveToMenu = document.getElementById("copyAndMoveToMenu");
  if (
    copyAndMoveToMenu.style.visibility === "collapse" ||
    copyAndMoveToMenuOperation !== "move"
  ) {
    collapseMenus();
    moveToBackground.className = "sketchClickableTextBackgroundHighlighted";
    moveToText.className = "sketchClickableTextWithinHighlighted";
    loadCopyAndMoveToMenu();
    copyAndMoveToMenuOperation = "move";
    copyAndMoveToMenu.style.visibility = "visible";
  } else {
    moveToBackground.className = "sketchClickableTextBackground";
    moveToText.className = "sketchClickableText";
    copyAndMoveToMenuOperation = "";
    copyAndMoveToMenu.style.visibility = "collapse";
  }
  lastAction("copy/move");
}

/**
 * Copy To button event handler.
 *
 * @param {string} mode - Sketch Mode selected.
 * @returns {void}
 */
function copyToClicked(mode) {
  if (!parent.sketchControl.selected) {
    parent.sketchControl.selectLayer(
      parent.sketchControl.getLayer(parent.sketchControl.sketchLayerToEdit)
    );
  }
  collapseLayersMenu();
  let copyToBackground = document.getElementById(mode + "CopyToBackground");
  let copyToText = document.getElementById(mode + "CopyToText");
  let copyAndMoveToMenu = document.getElementById("copyAndMoveToMenu");
  if (
    copyAndMoveToMenu.style.visibility === "collapse" ||
    copyAndMoveToMenuOperation !== "copy"
  ) {
    collapseMenus();
    copyToBackground.className = "sketchClickableTextBackgroundHighlighted";
    copyToText.className = "sketchClickableTextWithinHighlighted";
    loadCopyAndMoveToMenu();
    copyAndMoveToMenuOperation = "copy";
    copyAndMoveToMenu.style.visibility = "visible";
  } else {
    copyToBackground.className = "sketchClickableTextBackground";
    copyToText.className = "sketchClickableText";
    copyAndMoveToMenuOperation = "";
    copyAndMoveToMenu.style.visibility = "collapse";
  }
  lastAction("copy/move");
}

/**
 * Draw -> Pen Down event handler.
 *
 * @returns {void}
 */
function penDownClicked() {
  const keypadCenterImage = document.getElementById("keypadCenter");
  keypadCenterImage.src = "ptas_keypad_center_pen_down.png";
  let penDownOn = document.getElementById("penDownOn");
  penDownOn.style.visibility = "inherit";
  let penUpOn = document.getElementById("penUpOn");
  penUpOn.style.visibility = "collapse";
  parent.sketchControl.penDown = true;
  if (parent.sketchControl.mode == SketchMode.Draw) {
    parent.sketchControl.interaction.performUp(parent.sketchControl);
  }
  lastAction("pen down");
  parent.sketchControl.draw();
}

/**
 * Draw -> Pen Up event handler.
 *
 * @returns {void}
 */
function penUpClicked() {
  const keypadCenterImage = document.getElementById("keypadCenter");
  keypadCenterImage.src = "ptas_keypad_center_pen_up.png";
  let penDownOn = document.getElementById("penDownOn");
  penDownOn.style.visibility = "collapse";
  let penUpOn = document.getElementById("penUpOn");
  penUpOn.style.visibility = "inherit";
  parent.sketchControl.penDown = false;
  lastAction("pen up");
  if (parent.sketchControl.sketchPathToEdit && !parent.sketchControl.sketchPathToEdit.elements) {
    parent.sketchControl.selected = [{
      path: parent.sketchControl.sketchPathToEdit,
      layer: parent.sketchControl.getLayer(parent.sketchControl.sketchLayerToEdit)
    }];
    parent.sketchControl.deleteSelection()
  }
  parent.sketchControl.draw();
}

/**
 * Layers button event handler.
 *
 * @returns {void}
 */
function layersClicked() {
  collapseMenus();
  loadLayersMenu();
  let layersButton = document.getElementById("layersButton");
  let layersMenu = document.getElementById("layersMenu");
  if (layersMenu.style.visibility === "collapse") {
    layersButton.src = "ptas_layers_highlighted.png";
    layersMenu.style.visibility = "visible";
  } else {
    collapseLayersMenu();
  }
}

/**
 * Level Visibility button event handler.
 *
 * @param {string} uniqueIdentifier
 * @returns {void}
 */
function levelVisibilityClicked(uniqueIdentifier) {
  for (let i = 0; i < parent.sketchControl.levels.length; i++) {
    let level = parent.sketchControl.levels[i];
    if (level.uniqueIdentifier === uniqueIdentifier) {
      level.visible = !level.visible;
      if (level.layers !== undefined) {
        for (let j = 0; j < level.layers.length; j++) {
          const entry = level.layers[j];
          for (let k = 0; k < parent.sketchControl.layers.length; k++) {
            let layer = parent.sketchControl.layers[k];
            if (layer.uniqueIdentifier === entry.uniqueIdentifier) {
              layer.visible = level.visible;
              break;
            }
          }
        }
      }
      loadLayersMenu();
      parent.sketchControl.draw();
      break;
    }
  }
}

/**
 * Layer Visibility button event handler.
 *
 * @param {MouseEvent} event - Input event
 * @param {string} uniqueIdentifier
 * @returns {void}
 */
function layerVisibilityClicked(event, uniqueIdentifier) {
  event.stopPropagation();
  for (let i = 0; i < parent.sketchControl.levels.length; i++) {
    let level = parent.sketchControl.levels[i];
    let found = false;
    let oneVisible = false;
    if (level.layers !== undefined) {
      for (let j = 0; j < level.layers.length; j++) {
        const entry = level.layers[j];
        for (let k = 0; k < parent.sketchControl.layers.length; k++) {
          let layer = parent.sketchControl.layers[k];
          if (layer.uniqueIdentifier === entry.uniqueIdentifier) {
            if (layer.uniqueIdentifier === uniqueIdentifier) {
              found = true;
              layer.visible = !layer.visible;
            }
            if (!layer.visible && parent.sketchControl.selected) {
              for (let i = 0; i < parent.sketchControl.selected.length; i++) {
                if (
                  (parent.sketchControl.selected[i].layer,
                  layer,
                  parent.sketchControl.selected[i].layer == layer)
                ) {
                  parent.sketchControl.selected.splice(i, 1);
                  i--;
                }
              }
            }
            oneVisible = oneVisible || layer.visible;
            break;
          }
        }
      }
    }
    if (found) {
      level.visible = oneVisible;
      parent.sketchControl.draw();
      break;
    }
  }
}

/**
 * Layer Options button event handler.
 *
 * @param {MouseEvent} event - Input event
 * @param {HTMLElement} element - Selected button
 * @returns {void}
 */
function sketchLayerOptionsClicked(event, element) {
  if (element.getAttribute("data-focused")) {
    element.blur();
    element.removeAttribute("data-focused");
  } else {
    element.setAttribute("data-focused", "true");
  }

  event.stopPropagation();
}

/**
 * Level Options button event handler.
 *
 * @param {MouseEvent} event - Input event
 * @param {HTMLElement} element - Selected button
 * @returns {void}
 */
function sketchLevelOptionsClicked(event, element) {
  if (element.getAttribute("data-focused")) {
    element.blur();
    element.removeAttribute("data-focused");
  } else {
    element.setAttribute("data-focused", "true");
  }

  event.stopPropagation();
}

/**
 * Level name text input event handler.
 *
 * @param {Event} event - Input event
 * @param {string} uniqueIdentifier
 * @returns {void}
 */
function renameLevel(event, uniqueIdentifier) {
  event.stopPropagation();
  if (event.key === "Enter") {
    event.target.blur();
  } else if (event.type == "blur") {
    let level = parent.sketchControl.levels.find(
      lv => lv.uniqueIdentifier == uniqueIdentifier
    );
    if (
      parent.sketchControl.levels.find(
        lv => lv.name == event.target.value && lv !== level
      )
    ) {
      showDialog("There's already a level with that name.");
      event.target.focus();
    } else {
      document
        .getElementsByName(level.name)[0]
        .setAttribute("name", event.target.value);
      level.name = event.target.value;
      parent.sketchControl.autoSave();
    }
  }
}

/**
 * Layer -> Options -> Delete event handler.
 *
 * @param {string} layerUniqueIdentifier
 * @returns {void}
 */
function deleteLayerClicked(layerUniqueIdentifier) {
  const history = parent.sketchControl.createHistory();
  parent.sketchControl.addToUndoHistory(history);
  for (let i = 0; i < parent.sketchControl.layers.length; i++) {
    if (
      parent.sketchControl.layers[i].uniqueIdentifier == layerUniqueIdentifier
    ) {
      parent.sketchControl.layers.splice(i, 1);
      break;
    }
  }
  parent.sketchControl.autoSave();
}

/**
 * Level -> Options -> Rename event handler.
 *
 * @param {string} uniqueIdentifier
 * @returns {void}
 */
function renameLevelClicked(uniqueIdentifier) {
  let level = parent.sketchControl.levels.find(
    lv => lv.uniqueIdentifier == uniqueIdentifier
  );
  let input = document
    .getElementsByName(level.name)[0]
    .getElementsByClassName("level-input")[0];
  input.focus();
  input.setSelectionRange(input.value.length, input.value.length);
}

/**
 * Level -> Options -> Delete event handler.
 *
 * @param {string} levelUniqueIdentifier
 * @returns {void}
 */
function deleteLevelClicked(levelUniqueIdentifier) {
  for (let i = 0; i < parent.sketchControl.levels.length; i++) {
    if (
      parent.sketchControl.levels[i].uniqueIdentifier == levelUniqueIdentifier
    ) {
      let level = parent.sketchControl.levels[i];
      level.layers &&
        level.layers.map(layer => {
          deleteLayerClicked(layer.uniqueIdentifier);
        });
      parent.sketchControl.levels.splice(i, 1);
      break;
    }
  }
  parent.sketchControl.autoSave();
}

/**
 * Layer selection event handler.
 *
 * @param {string} uniqueIdentifier
 * @returns {void}
 */
function sketchLayerMenuItemClicked(uniqueIdentifier) {
  if (parent.sketchControl.penDown) {
    penUpClicked();
  }
  if (parent.sketchControl.sketchLayerToEdit !== uniqueIdentifier) {
    parent.sketchControl.levels.map(level => {
      if (
        level.layers.find(layer => layer.uniqueIdentifier == uniqueIdentifier)
      )
        level.selected = true;
      else level.selected = false;
    });
    parent.sketchControl.sketchLayerToEdit = uniqueIdentifier;
    parent.sketchControl.clearSelection();
    loadLayersMenu();
    let layer = parent.sketchControl.getLayer(uniqueIdentifier)
    let area = 0;
    if (layer.objects) {
      layer.objects.map(object => {
        if (
          object.objects &&
          object.objects.length > 0 &&
          object.objects[0].closed
        ) {
          area += object.objects[0].getArea(parent.sketchControl);
        }
      });
    }
    let savedArea = layer.savedArea;
    area = Math.round(area);
    savedArea -= area;
    if (savedArea < 0) {
      difference = area - savedArea;
    } else if (savedArea > 0) {
      difference = savedArea - area;
    }
    document.getElementById(
      "layerName"
    ).innerHTML = `${layer.name}<br />${area} ⏍`;
  }
}

/**
 * Layer mouse over event handler.
 *
 * @param {HTMLElement} menuItem - Layer div
 * @param {string} uniqueIdentifier - Layer ID
 * @returns {void}
 */
function sketchLayerMenuItemMouseOver(menuItem, uniqueIdentifier) {
  parent.sketchControl.highlightedLayer = uniqueIdentifier;
  menuItem.className = "sketchLayerMenuItemHighlighted";
}

/**
 * Layer mouse out event handler.
 *
 * @param {HTMLElement} menuItem - Layer div
 * @returns {void}
 */
function sketchLayerMenuItemMouseOut(menuItem) {
  parent.sketchControl.highlightedLayer = null;
  menuItem.className = "sketchLayerMenuItem";
}

/**
 * View -> Zoom Out event handler.
 *
 * @returns {void}
 */
function layersZoomOutClicked() {
  parent.sketchControl.zoom(0.9);
}

/**
 * View -> Zoom In event handler.
 *
 * @returns {void}
 */
function layersZoomInClicked() {
  parent.sketchControl.zoom(1.1);
}

/**
 * Undo input event handler.
 *
 * @returns {void}
 */
function undoClicked() {
  parent.sketchControl.undo();
  adjustUndoRedoVisibility();
}

/**
 * Redo input event handler.
 *
 * @returns {void}
 */
function redoClicked() {
  parent.sketchControl.redo();
  adjustUndoRedoVisibility();
}

let keypadInputStep = 0; // 0 == enter feet, 1 == enter inches
let keypadInputArrow = 0; // 0 == none, 1 == bottom left, 2 == down, 3 = bottom right, 4 == left, 6 == right, 7 == top left, 8 == up, 9 == top right
let keypadInputFeetPreviousValue = 0;
let keypadInputInchesPreviousValue = 0;

/**
 * Restore the keypad UI to its initial state.
 *
 * @returns {void}
 */
function resetAllKeypadArrows() {
  let keypadTopLeftArrow = document.getElementById("keypadTopLeftArrow");
  keypadTopLeftArrow.src = "ptas_keypad_top_left_arrow.png";
  let keypadUpArrow = document.getElementById("keypadUpArrow");
  keypadUpArrow.src = "ptas_keypad_up_arrow.png";
  let keypadTopRightArrow = document.getElementById("keypadTopRightArrow");
  keypadTopRightArrow.src = "ptas_keypad_top_right_arrow.png";
  let keypadLeftArrow = document.getElementById("keypadLeftArrow");
  keypadLeftArrow.src = "ptas_keypad_left_arrow.png";
  let keypadRightArrow = document.getElementById("keypadRightArrow");
  keypadRightArrow.src = "ptas_keypad_right_arrow.png";
  let keypadBottomLeftArrow = document.getElementById("keypadBottomLeftArrow");
  keypadBottomLeftArrow.src = "ptas_keypad_bottom_left_arrow.png";
  let keypadDownArrow = document.getElementById("keypadDownArrow");
  keypadDownArrow.src = "ptas_keypad_down_arrow.png";
  let keypadBottomRightArrow = document.getElementById(
    "keypadBottomRightArrow"
  );
  keypadBottomRightArrow.src = "ptas_keypad_bottom_right_arrow.png";
  let keypadInputFeet = document.getElementById("keypadInputFeet");
  let keypadInputInches = document.getElementById("keypadInputInches");
  keypadInputFeet.value = 0;
  keypadInputInches.value = 0;
}

/**
 * Displays the keypad's text input.
 *
 * @returns {void}
 */
function showKeypadInput() {
  let keypad = document.getElementById("keypad");
  let keypadInput = document.getElementById("keypadInput");
  keypadInput.style.visibility = "visible";
  if (keypad.style.visibility === "collapse") {
    keypadInput.style.bottom = 0;
  } else {
    keypadInput.style.bottom = "160px";
  }
}

/**
 * Hides the keypad's text input.
 *
 * @returns {void}
 */
function hideKeypadInput() {
  let keypadInput = document.getElementById("keypadInput");
  keypadInput.style.visibility = "unset";
}

/**
 * Displays the keypad UI.
 *
 * @returns {void}
 */
function showKeypad() {
  let keypadButtonText = document.getElementById("keypadButtonText");
  let keypadButtonImage = document.getElementById("keypadButtonImage");
  let keypad = document.getElementById("keypad");
  keypad.style.visibility = "visible";
  keypadButtonText.className = "sketchClickableTextHighlighted";
  keypadButtonImage.src = "ptas_expand_down_highlighted.png";

  let keypadInputFeet = document.getElementById(keypadInputFeetId);
  let keypadInputInches = document.getElementById(keypadInputInchesId);
  if (keypadInputFeet && keypadInputFeet.value === "") {
    keypadInputFeet.value = 0;
  }
  if (keypadInputInches && keypadInputInches.value === "") {
    keypadInputInches.value = 0;
  }
  keypadInputFeetPreviousValue = keypadInputFeet.value;
  keypadInputInchesPreviousValue = keypadInputInches ? keypadInputInches : 0;
  keypadInputStep = 0;
}

/**
 * Hides the keypad UI.
 *
 * @returns {void}
 */
function hideKeypad() {
  let keypadButtonText = document.getElementById("keypadButtonText");
  let keypadButtonImage = document.getElementById("keypadButtonImage");
  let keypad = document.getElementById("keypad");
  keypad.style.visibility = "collapse";
  keypadButtonText.className = "sketchClickableText";
  keypadButtonImage.src = "ptas_expand_down.png";
  hideKeypadInput();
}

/**
 * Keypad button event handler.
 *
 * @returns {void}
 */
function keypadClicked() {
  let keypadButtonText = document.getElementById("keypadButtonText");
  let keypadButtonImage = document.getElementById("keypadButtonImage");
  let keypad = document.getElementById("keypad");
  if (keypad.style.visibility === "collapse") {
    keypad.style.visibility = "visible";
    keypadButtonText.className = "sketchClickableTextHighlighted";
    keypadButtonImage.src = "ptas_expand_down_highlighted.png";
    keypadInputFeetPreviousValue = 0;
    keypadInputInchesPreviousValue = 0;
    let keypadInputFeet = document.getElementById(keypadInputFeetId);
    keypadInputFeet.value = keypadInputFeetPreviousValue;
    if (keypadInputInchesId !== "") {
      let keypadInputInches = document.getElementById(keypadInputInchesId);
      keypadInputInches.value = keypadInputInchesPreviousValue;
    }
    keypadInputStep = 0;
    resetAllKeypadArrows();
    mapKeypadInputs("keypadInputFeet", "keypadInputInches", 0);
  } else {
    keypad.style.visibility = "collapse";
    keypadButtonText.className = "sketchClickableText";
    keypadButtonImage.src = "ptas_expand_down.png";
  }
  showKeypadInput();
  lastAction("keypad");
}

/**
 * Tracks the last user interaction.
 *
 * @param {string} action - Tool or input
 * @returns {void}
 */
function lastAction(action) {
  parent.sketchControl.lastAction = action;
}

/**
 * Keypad feet input change event handler.
 *
 * @param {Event} event - Input event
 * @returns {void}
 */
function keypadInputFeetChanged(event) {
  let keypadInputFeet = document.getElementById(keypadInputFeetId);
  if (event && event.data && event.data == "-" && keypadInputFeet.value >= 0) {
    keypadInputFeet.value = keypadInputFeetPreviousValue;
    keypadInputFeet.value *= -1;
    keypadInputFeetPreviousValue = keypadInputFeet.value;
  }
}

/**
 * Keypad inches input change event handler.
 *
 * @param {Event} event - Input event
 * @returns {void}
 */
function keypadInputInchesChanged(event) {
  let keypadInputInches = document.getElementById(keypadInputInchesId);
  if (
    event &&
    event.data &&
    event.data == "-" &&
    keypadInputInches.value >= 0
  ) {
    keypadInputInches.value = keypadInputInchesPreviousValue;
    keypadInputInches.value *= -1;
    keypadInputInchesPreviousValue = keypadInputInches.value;
  }
}

/**
 * Keypad text input key up event handler.
 *
 * @param {Event} event - Input event
 * @returns {void}
 */
function keypadInputKeyUp(event) {
  if (event.key === "Enter") {
    keypadEnterClicked();
  } else if (event.key == "'" || event.key == '"') {
    keypadDashClicked(event);
  }
}

/**
 * Keypad number buttons event handler.
 *
 * @param {string} numberAsText
 * @returns {void}
 */
function keypadNumberClicked(numberAsText) {
  showKeypadInput();
  if (keypadInputStep !== 0 && keypadInputStep !== 1) {
    keypadInputStep = 0;
  }
  if (keypadInputStep === 0) {
    let keypadInputFeet = document.getElementById(keypadInputFeetId);
    if (numberAsText == "-") {
      keypadInputFeet.value *= -1;
    } else {
      keypadInputFeet.value = keypadInputFeet.value + numberAsText;
    }
  } else if (keypadInputStep === 1) {
    let keypadInputInches = document.getElementById(keypadInputInchesId);
    if (numberAsText == "-") {
      keypadInputInches.value *= -1;
    } else {
      keypadInputInches.value = keypadInputInches.value + numberAsText;
    }
  }
}

/**
 * Keypad Esc button event handler.
 *
 * @returns {void}
 */
function keypadEscClicked() {
  const elements = parent.sketchControl.sketchPathToEdit.elements;
  const position = elements
    ? elements[elements.length - 1]
    : parent.sketchControl.sketchPathToEdit.start;
  parent.sketchControl.cursor.x = parent.sketchControl.penUpCursor.x =
    position.x;
  parent.sketchControl.cursor.y = parent.sketchControl.penUpCursor.y =
    position.y;
  this.penUpClicked();
  document.getElementById("keypadInputFeet").value = 0;
  document.getElementById("keypadInputInches").value = 0;
}

/**
 * Keypad n'n" button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadDashClicked(event) {
  if (event instanceof KeyboardEvent) {
    let ending = event.target.id.length - 4;
    let tabbedInput;
    if (event.target.id.slice(ending) == "Feet") {
      tabbedInput = document.getElementById(
        event.target.id.slice(0, ending).concat("Inches")
      );
    } else {
      tabbedInput = document.getElementById(
        event.target.id.slice(0, ending - 2).concat("Feet")
      );
    }
    tabbedInput.focus();
  }
  if (keypadInputStep === 0 && keypadInputInchesId !== "") {
    keypadInputStep = 1;
  } else {
    keypadInputStep = 0;
  }
}

/**
 * Keypad Del button event handler.
 *
 * @returns {void}
 */
function keypadDeleteClicked() {
  if (keypadInputStep !== 0 && keypadInputStep !== 1) {
    keypadInputStep = 0;
  }
  if (keypadInputStep === 0) {
    let keypadInputFeet = document.getElementById(keypadInputFeetId);
    if (keypadInputFeet.value.length === 1) {
      keypadInputFeet.value = "";
    } else if (keypadInputFeet.value.length > 1) {
      keypadInputFeet.value = keypadInputFeet.value.slice(
        0,
        keypadInputFeet.value.length - 1
      );
    }
  } else if (keypadInputStep === 1) {
    let keypadInputInches = document.getElementById(keypadInputInchesId);
    if (keypadInputInches.value.length === 1) {
      keypadInputInches.value = "";
    } else if (keypadInputInches.value.length > 1) {
      keypadInputInches.value = keypadInputInches.value.slice(
        0,
        keypadInputInches.value.length - 1
      );
    }
  }
}

/**
 * Keypad up + left arrow button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadTopLeftArrowClicked(event) {
  if (
    keypadInputFeetId === "keypadInputFeet" &&
    keypadInputInchesId == "keypadInputInches"
  ) {
    keypadInputStep = 2;
    let keypadTopLeftArrow = document.getElementById("keypadTopLeftArrow");
    keypadTopLeftArrow.src = "ptas_keypad_top_left_arrow_highlighted.png";
    keypadInputArrow = 7;
    keypadArrowClicked(event);
  }
}

/**
 * Keypad up arrow button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadUpArrowClicked(event) {
  if (
    keypadInputFeetId === "keypadInputFeet" &&
    keypadInputInchesId == "keypadInputInches"
  ) {
    keypadInputStep = 2;
    let keypadUpArrow = document.getElementById("keypadUpArrow");
    keypadUpArrow.src = "ptas_keypad_up_arrow_highlighted.png";
    keypadInputArrow = 8;
    keypadArrowClicked(event);
  }
}

/**
 * Keypad up + right arrow button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadTopRightArrowClicked(event) {
  if (
    keypadInputFeetId === "keypadInputFeet" &&
    keypadInputInchesId == "keypadInputInches"
  ) {
    keypadInputStep = 2;
    let keypadTopRightArrow = document.getElementById("keypadTopRightArrow");
    keypadTopRightArrow.src = "ptas_keypad_top_right_arrow_highlighted.png";
    keypadInputArrow = 9;
    keypadArrowClicked(event);
  }
}

/**
 * Keypad left arrow button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadLeftArrowClicked(event) {
  if (
    keypadInputFeetId === "keypadInputFeet" &&
    keypadInputInchesId == "keypadInputInches"
  ) {
    keypadInputStep = 2;
    let keypadLeftArrow = document.getElementById("keypadLeftArrow");
    keypadLeftArrow.src = "ptas_keypad_left_arrow_highlighted.png";
    keypadInputArrow = 4;
    keypadArrowClicked(event);
  }
}

/**
 * Keypad center button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadCenterClicked() {
  if (parent.sketchControl.mode == SketchMode.Draw) {
    if (parent.sketchControl.penDown) {
      penUpClicked();
    } else {
      penDownClicked();
    }
  }
}

/**
 * Keypad right arrow button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadRightArrowClicked(event) {
  if (
    keypadInputFeetId === "keypadInputFeet" &&
    keypadInputInchesId == "keypadInputInches"
  ) {
    keypadInputStep = 2;
    let keypadRightArrow = document.getElementById("keypadRightArrow");
    keypadRightArrow.src = "ptas_keypad_right_arrow_highlighted.png";
    keypadInputArrow = 6;
    keypadArrowClicked(event);
  }
}

/**
 * Keypad bottom left arrow button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadBottomLeftArrowClicked(event) {
  if (
    keypadInputFeetId === "keypadInputFeet" &&
    keypadInputInchesId == "keypadInputInches"
  ) {
    keypadInputStep = 2;
    let keypadBottomLeftArrow = document.getElementById(
      "keypadBottomLeftArrow"
    );
    keypadBottomLeftArrow.src = "ptas_keypad_bottom_left_arrow_highlighted.png";
    keypadInputArrow = 1;
    keypadArrowClicked(event);
  }
}

/**
 * Keypad down arrow button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadDownArrowClicked(event) {
  if (
    keypadInputFeetId === "keypadInputFeet" &&
    keypadInputInchesId == "keypadInputInches"
  ) {
    keypadInputStep = 2;
    let keypadDownArrow = document.getElementById("keypadDownArrow");
    keypadDownArrow.src = "ptas_keypad_down_arrow_highlighted.png";
    keypadInputArrow = 2;
    keypadArrowClicked(event);
  }
}

/**
 * Keypad down + right arrow button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadBottomRightArrowClicked(event) {
  if (
    keypadInputFeetId === "keypadInputFeet" &&
    keypadInputInchesId == "keypadInputInches"
  ) {
    keypadInputStep = 2;
    let keypadBottomRightArrow = document.getElementById(
      "keypadBottomRightArrow"
    );
    keypadBottomRightArrow.src =
      "ptas_keypad_bottom_right_arrow_highlighted.png";
    keypadInputArrow = 3;
    keypadArrowClicked(event);
  }
}

/**
 * Keypad Enter button event handler.
 *
 * @param {MouseEvent} event
 * @returns {void}
 */
function keypadEnterClicked() {
  let keypadInputFeet = document.getElementById(keypadInputFeetId);
  let keypadInputInches = document.getElementById(keypadInputInchesId);
  if (
    parent.sketchControl.mode == SketchMode.Draw ||
    parent.sketchControl.mode == SketchMode.SingleSelect
  ) {
    let amount = Number.parseInt(keypadInputFeet.value);
    if (keypadInputInchesId !== "") {
      amount += Number.parseInt(keypadInputInches.value) / 12;
    }
    if (keypadInputFeetId === "curveLengthFeet") {
      parent.sketchControl.setLength(amount);
    } else if (
      keypadInputFeetId === "curveRiseFeet" ||
      keypadInputFeetId === "lineRiseFeet"
    ) {
      parent.sketchControl.setRise(amount);
    } else if (
      keypadInputFeetId === "curveRunFeet" ||
      keypadInputFeetId === "lineRunFeet"
    ) {
      parent.sketchControl.setRun(amount);
    } else if (keypadInputFeetId === "curveAngle") {
      parent.sketchControl.setCurveAngle(amount);
    } else if (
      keypadInputFeetId === "curveChordFeet" ||
      keypadInputFeetId === "lineLengthFeet"
    ) {
      parent.sketchControl.setCurveChord(amount);
    } else if (keypadInputFeetId === "curveHeightFeet") {
      parent.sketchControl.setCurveHeight(amount);
    } else {
      parent.sketchControl.movePen(0, 0);
      parent.sketchControl.xKeypad = undefined;
      parent.sketchControl.yKeypad = undefined;
      resetAllKeypadArrows();
      if (keypadInputFeetId === "keypadInputFeet") {
        keypadInputFeet.value = 0;
        parent.sketchControl.showContextBar("line");
      }
      if (keypadInputInchesId === "keypadInputInches") {
        keypadInputInches.value = 0;
        parent.sketchControl.showContextBar("line");
      }
    }
  }
}

/**
 * Moves the cursor based on the input given.
 *
 * @param {Event} event
 * @returns {void}
 */
function keypadArrowClicked(event) {
  hideKeypadInput();
  let keypadInputFeet = document.getElementById(keypadInputFeetId);
  let keypadInputInches = document.getElementById(keypadInputInchesId);
  if (parent.sketchControl.mode == SketchMode.Draw) {
    let amount = keypadInputFeet.value
      ? Number.parseInt(keypadInputFeet.value)
      : 0;
    if (keypadInputInchesId && keypadInputInches.value) {
      amount += Number.parseInt(keypadInputInches.value) / 12;
    }
    if (isNaN(amount)) amount = 0;
    if (keypadInputArrow > 0) {
      let x = 0;
      let y = 0;
      if ((!parent.sketchControl.penDown && amount == 0) || (amount == 0 && parent.sketchControl.lastEvent !== 'mousemove')) {
        switch (keypadInputArrow) {
          case 1: x--;
            y--;
            break;
          case 2: y--;
            break;
          case 3: x++;
            y--;
            break;
          case 4: x--;
            break;
          case 6: x++;
            break;
          case 7: x--;
            y++;
            break;
          case 8: y++;
            break;
          case 9: x++;
            y++;
            break;
        }
      } else {
        const axis = Math.sqrt((amount * amount) / 2);
        switch (keypadInputArrow) {
          case 1: x = -axis;
            y = -axis;
            break;
          case 2: y = -amount;
            break;
          case 3: x = axis;
            y = -axis;
            break;
          case 4: x = -amount;
            break;
          case 6: x = amount;
            break;
          case 7: x = -axis;
            y = axis;
            break;
          case 8: y = amount;
            break;
          case 9: x = axis;
            y = axis;
            break;
        }
      }
      parent.sketchControl.cursor.x += x;
      parent.sketchControl.cursor.y += y;
      if (parent.sketchControl.penUpCursor) {
        parent.sketchControl.penUpCursor.x += x;
        parent.sketchControl.penUpCursor.y += y;
      }
      parent.sketchControl.xKeypad = parent.sketchControl.cursor.x;
      parent.sketchControl.yKeypad = parent.sketchControl.cursor.y;
      parent.sketchControl.movingPenCursor = "moving";
      parent.sketchControl.lastEvent = event.type
      parent.sketchControl.draw();
    }
  }
  resetAllKeypadArrows();
  keypadInputStep = 0;
  keypadInputArrow = 0;
}

/**
 * Maps the keypad buttons to the selected text input.
 *
 * @param {string} forFeet
 * @param {string} forInches
 * @param {number} step
 * @returns {void}
 */
function mapKeypadInputs(forFeet, forInches, step) {
  keypadInputFeetId = forFeet;
  keypadInputInchesId = forInches;
  allowShowingKeypadInput = false;
  showKeypad();
  resetAllKeypadArrows();
  keypadInputStep = step;
  keypadInputArrow = 0;
}

/**
 * Context bar's Length feet input event handler.
 *
 * @returns {void}
 */
function curveLengthFeetFocused() {
  mapKeypadInputs("curveLengthFeet", "curveLengthInches", 0);
}

/**
 * Context bar's Length inches input event handler.
 *
 * @returns {void}
 */
function curveLengthInchesFocused() {
  mapKeypadInputs("curveLengthFeet", "curveLengthInches", 1);
}

/**
 * Context bar's Rise feet input event handler.
 *
 * @returns {void}
 */
function curveRiseFeetFocused() {
  mapKeypadInputs("curveRiseFeet", "curveRiseInches", 0);
}

/**
 * Context bar's Rise inches input event handler.
 *
 * @returns {void}
 */
function curveRiseInchesFocused() {
  mapKeypadInputs("curveRiseFeet", "curveRiseInches", 1);
}

/**
 * Context bar's Run feet input event handler.
 *
 * @returns {void}
 */
function curveRunFeetFocused() {
  mapKeypadInputs("curveRunFeet", "curveRunInches", 0);
}

/**
 * Context bar's Run inches input event handler.
 *
 * @returns {void}
 */
function curveRunInchesFocused() {
  mapKeypadInputs("curveRunFeet", "curveRunInches", 1);
}

/**
 * Context bar's Angle input event handler.
 *
 * @returns {void}
 */
function curveAngleFocused() {
  mapKeypadInputs("curveAngle", "", 0);
}

/**
 * Context bar's Chord feet input event handler.
 *
 * @returns {void}
 */
function curveChordFeetFocused() {
  mapKeypadInputs("curveChordFeet", "curveChordInches", 0);
}

/**
 * Context bar's Chord inches input event handler.
 *
 * @returns {void}
 */
function curveChordInchesFocused() {
  mapKeypadInputs("curveChordFeet", "curveChordInches", 1);
}

/**
 * Context bar's Height feet input event handler.
 *
 * @returns {void}
 */
function curveHeightFeetFocused() {
  mapKeypadInputs("curveHeightFeet", "curveHeightInches", 0);
}

/**
 * Context bar's Height inches input event handler.
 *
 * @returns {void}
 */
function curveHeightInchesFocused() {
  mapKeypadInputs("curveHeightFeet", "curveHeightInches", 1);
}

/**
 * Context bar's Line Length feet input event handler.
 *
 * @returns {void}
 */
function lineLengthFeetFocused() {
  mapKeypadInputs("lineLengthFeet", "lineLengthInches", 0);
}

/**
 * Context bar's Line Length inches input event handler.
 *
 * @returns {void}
 */
function lineLengthInchesFocused() {
  mapKeypadInputs("lineLengthFeet", "lineLengthInches", 1);
}

/**
 * Context bar's Line Rise feet input event handler.
 *
 * @returns {void}
 */
function lineRiseFeetFocused() {
  mapKeypadInputs("lineRiseFeet", "lineRiseInches", 0);
}

/**
 * Context bar's Line Rise inches input event handler.
 *
 * @returns {void}
 */
function lineRiseInchesFocused() {
  mapKeypadInputs("lineRiseFeet", "lineRiseInches", 1);
}

/**
 * Context bar's Line Run feet input event handler.
 *
 * @returns {void}
 */
function lineRunFeetFocused() {
  mapKeypadInputs("lineRunFeet", "lineRunInches", 0);
}

/**
 * Context bar's Line Run inches input event handler.
 *
 * @returns {void}
 */
function lineRunInchesFocused() {
  mapKeypadInputs("lineRunFeet", "lineRunInches", 1);
}

/**
 * Stops the Text rotation.
 *
 * @returns {void}
 */
function rotateUp() {
  parent.sketchControl.isRotating = false;
}

/**
 * Text -> Larger event handler.
 *
 * @returns {void}
 */
function largeFontClicked() {
  if (parent.sketchControl.selected) {
    const textObject = parent.sketchControl.selected[0].textObject;
    if (textObject.worldUnits) textObject.fontSize += 1;
    else if (textObject.fontSize) textObject.fontSize += 4;
    else
      textObject.fontSize =
        parseInt(
          textObject.texts[0].fontSize.substr(
            0,
            textObject.texts[0].fontSize.length - 2
          )
        ) + 4;
  }
  parent.sketchControl.dismissTextInput();
  parent.sketchControl.draw();
}

/**
 * Text -> Smaller event handler.
 *
 * @returns {void}
 */
function smallFontClicked() {
  if (parent.sketchControl.selected) {
    const textObject = parent.sketchControl.selected[0].textObject;
    if (textObject.worldUnits) textObject.fontSize -= 1;
    else if (textObject.fontSize) textObject.fontSize -= 4;
    else
      textObject.fontSize =
        parseInt(
          textObject.texts[0].fontSize.substr(
            0,
            textObject.texts[0].fontSize.length - 2
          )
        ) - 4;
  }
  parent.sketchControl.dismissTextInput();
  parent.sketchControl.draw();
}

/**
 * Text -> Arrow event handler.
 *
 * @returns {void}
 */
function textArrowClicked() {
  const history = parent.sketchControl.createHistory();
  parent.sketchControl.addToUndoHistory(history);
  if (parent.sketchControl.selected) {
    const textObject = parent.sketchControl.selected[0].textObject;
    if (textObject.arrow) {
      for (let i = 0; i < parent.sketchControl.layers.length; i++) {
        if (parent.sketchControl.layers[i].objects) {
          const current = parent.sketchControl.layers[i].objects.find(
            object =>
              object.objects &&
              object.objects.length > 0 &&
              object.objects[0].uniqueIdentifier == textObject.arrow
          );
          if (current) {
            const selectedText = parent.sketchControl.selected;
            parent.sketchControl.selected = [
              {
                layer: parent.sketchControl.layers[i],
                path: current.objects[0]
              }
            ];
            parent.sketchControl.deleteSelection(
              parent.sketchControl,
              current.objects[0]
            );
            parent.sketchControl.selected = selectedText;
            break;
          }
        }
      }
    } else {
      const newObject = new SketchObject();
      const path = new SketchPath();
      path.uniqueIdentifier = SketchControl.uuidv4();
      path.arrow = true;
      path.start = { x: textObject.x, y: textObject.y };
      path.elements = [{ x: textObject.x + 5, y: textObject.y + 5 }];
      textObject.arrow = path.uniqueIdentifier;
      newObject.objects = [path];
      for (let i = 0; i < parent.sketchControl.layers.length; i++) {
        if (parent.sketchControl.layers[i].objects) {
          const current = parent.sketchControl.layers[i].objects.find(
            object =>
              object.objects[0].uniqueIdentifier == textObject.uniqueIdentifier
          );
          if (current) {
            parent.sketchControl.layers[i].objects.push(newObject);
            break;
          }
        }
      }
    }
  }
  parent.sketchControl.draw();
}

/**
 * Text -> Hide event handler.
 *
 * @returns {void}
 */
function hideDistanceClicked() {
  let hideDistance = document.getElementById("hideDistance");
  if (hideDistance.style.visibility == "collapse") {
    let fontColor = parent.sketchControl.selected[0].textObject.fontColor;
    let fontAlpha =
      fontColor && fontColor.length > 7
        ? Math.round(parseInt(fontColor.substr(fontColor.length - 2), 16) / 2)
        : Math.round(0xff / 2);
    parent.sketchControl.selected[0].textObject.fontColor =
      "#b70f0a" + fontAlpha.toString(16);
    parent.sketchControl.draw();
  }
}

/**
 * Text -> Show event handler.
 *
 * @returns {void}
 */
function showDistanceClicked() {
  let hideDistance = document.getElementById("hideDistance");
  if (hideDistance.style.visibility !== "collapse") {
    let fontColor = parent.sketchControl.selected[0].textObject.fontColor;
    let fontAlpha =
      fontColor && fontColor.length > 7
        ? parseInt(fontColor.substr(fontColor.length - 2), 16) * 2
        : 0xff;
    const style = getLayerStyle(parent.sketchControl.selected[0].layer);
    switch (style.substr(0, 4)) {
      case "teal":
        fontColor = "#007e8f";
        break;
      case "mage":
        fontColor = "#8f0081";
        break;
      case "gree":
        fontColor = "#007e8f";
        break;
      case "purp":
        fontColor = "#25008f";
        break;
      case "gray":
        fontColor = "#888888";
        break;
      default:
        fontColor = "#000000";
    }
    parent.sketchControl.selected[0].textObject.fontColor =
      fontColor + Math.min(fontAlpha, 0xff).toString(16);
  }
}

/**
 * Show -> Square Feet event handler on Read-Only mode.
 *
 * @returns {void}
 */
function showSFClicked() {
  parent.sketchControl.hideSF = parent.sketchControl.hideSF ? false : true;
  parent.sketchControl.draw();
}

/**
 * Toggles the view of the line distances on Read-Only mode.
 *
 * @returns {void}
 */
function showDistancesClicked() {
  parent.sketchControl.hideDistances = parent.sketchControl.hideDistances
    ? false
    : true;
  parent.sketchControl.draw();
}

/**
 * Info -> Versions sketch selection event handler.
 *
 * @param {event} event
 * @param {object} selectedVersion
 * @param {HTMLElement} versionsDiv
 * @returns {void}
 */
function selectVersion(event, selectedVersion, versionsDiv) {
  parent.sketchControl.selectedSketchVersion = selectedVersion;
  const sketchVersions = document.getElementsByClassName("sketchVersion");
  for (let i = 0; i < sketchVersions.length; i++) {
    sketchVersions[i].classList.remove("sketchVersionSelected");
  }

  event.target.classList.add("sketchVersionSelected");

  //Generate totalsInfo using area totals in relatedEntity (saved) and in sketch version file (new)
  const versionTabView = versionsDiv.querySelector(".versions-tab-view");
  while (versionTabView.firstChild) {
    versionTabView.removeChild(versionTabView.lastChild);
  }
  const areaTotalsTableTemplate = document.getElementById(
    "areaTotalsTableTemplate"
  );
  const areaTotalsTable = areaTotalsTableTemplate.content
    .cloneNode(true)
    .querySelector(".table-template");

  const sketchVersionTotals = parent.sketchControl.getSketchVersionTotals(
    selectedVersion.sketch
  );
  populateAreaTotalsTable(
    "versions",
    areaTotalsTable.querySelector("table"),
    sketchVersionTotals,
    areaTotalsTable.querySelector(".list-container")
  );
  versionTabView.appendChild(areaTotalsTable);
}
