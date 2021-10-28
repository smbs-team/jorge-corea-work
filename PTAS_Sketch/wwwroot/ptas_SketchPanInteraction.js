// ptas_SketchPanInteraction.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
* Interactions for the Pan mode
*/
class SketchPanInteraction {
  /**
    * Moves the canvas within the visible area
    *
    * @param sketch - Control instance
    * @param event - Input event
    * @returns void
    *
    */
  performMove(sketch, event) {
    if (this.startX !== event.offsetX || this.startY !== event.offsetY) {
      this.isMove = true;
      sketch.offsetX = this.sketchOffsetX + (event.offsetX - this.startX);
      sketch.offsetY = this.sketchOffsetY + (event.offsetY - this.startY);
      sketch.updateTopLevelTransform();
      sketch.draw();
    }
  }

  /**
    * Called on mouse release to stop panning
    *
    * @returns void
    *
    */
  performUp() {
    this.isDown = false;
    this.startX = undefined;
    this.startY = undefined;
    this.sketchOffsetX = undefined;
    this.sketchOffsetY = undefined;
  }

  /**
    * Mouse down event handler
    *
    * @param sketch - Control instance
    * @param event - Input event
    * @returns void
    *
    */
  down(sketch, event) {
    this.isDown = true;
    this.sketchOffsetX = sketch.offsetX;
    this.sketchOffsetY = sketch.offsetY;
    this.startX = event.offsetX;
    this.startY = event.offsetY;
  }

  /**
    * Mouse move event handler
    *
    * @param sketch - Control instance
    * @param event - Input event
    * @returns void
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
    * @param sketch - Control instance
    * @param event - Input event
    * @returns void
    *
    */
  up(sketch, event) {
    if (this.isDown) {
      this.performMove(sketch, event);
      this.performUp(sketch, event);
    }
  }
}
