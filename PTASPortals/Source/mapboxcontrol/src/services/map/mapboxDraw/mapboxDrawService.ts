// mapboxDrawService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/* eslint-disable @typescript-eslint/no-explicit-any */

import mapboxgl from 'mapbox-gl';
import MapboxDraw from '@mapbox/mapbox-gl-draw';
import { isActiveFeature } from '@mapbox/mapbox-gl-draw/src/lib/common_selectors';
import { DragCircleMode, CircleMode } from 'mapbox-gl-draw-circle';
import { once, chain } from 'lodash';
import { Subject } from 'rxjs';

import { MapboxDrawEvents } from '../../../appConstants';
import { utilService } from 'services/common';
import {
  DirectModeOverwrite,
  SimpleSelectModeOverwrite,
  DrawMode,
  DrawLineStringOverwrite,
  simpleSelectEvt,
  directModeEvt,
  SelectEvent,
  DrawPolygonOverwrite,
} from './modes';
import drawUtilService from './drawUtilService';
import { mapUtilService } from '../mapUtilService';
import { drawStyles } from './drawStyles';
import { MeasureService } from './measure';
export const $drawAreaUpdated: Subject<MapboxDraw> = new Subject<MapboxDraw>();

/**
 * A service for drawing stuff with mapbox-gl-draw
 *
 * @remarks
 * See https://github.com/mapbox/mapbox-gl-draw/blob/master/docs/API.md
 */
class MapBoxDrawService {
  /**
   * mapboxgl Map instance
   */
  private _map: mapboxgl.Map | null = null;
  /**
   * mapbox-gl-draw MapboxDraw instance
   */
  public draw: MapboxDraw | null = null;
  /**
   * mapbox-gl-draw current mode
   */
  public currentMode: DrawMode = 'simple_select';

  measure?: MeasureService;

  constructor() {
    directModeEvt.clickNoTarget.subscribe(() => {
      if (this.currentMode !== 'simple_select') {
        this.setMode(this.currentMode);
      }
    });

    simpleSelectEvt.clickAnyWhere.subscribe(() => {
      if (this.currentMode !== 'simple_select') {
        this.setMode(this.currentMode);
      }
    });
  }

  /**
   * Initialize the drawing functionality
   */
  init = once((map: mapboxgl.Map) => {
    this._map = map;
    const FreehandMode = {
      onSetup: function (): Record<string, any> {
        console.log('FREE');
        return {};
      },
      toDisplayFeatures: function (
        _: any,
        geojson: Record<string, any>,
        display: Function
      ): void {
        console.log(geojson);
        display(geojson);
      },
      onMouseMove: function (state: any, e: any): void {
        console.log(e);
      },
    };

    /* eslint-disable @typescript-eslint/camelcase */
    this.draw = new MapboxDraw({
      styles: drawStyles,
      userProperties: true,
      displayControlsDefault: false,
      modes: {
        ...MapboxDraw.modes,
        draw_circle: CircleMode,
        drag_circle: DragCircleMode,
        direct_select: DirectModeOverwrite as any,
        simple_select: SimpleSelectModeOverwrite,
        draw_polygon: DrawPolygonOverwrite,
        draw_line_string: DrawLineStringOverwrite,
        freeHand: FreehandMode,
      },
    });
    (window as any).draw = this.draw;

    chain(this.simpleSelectMouseMove(map.getCanvas()))
      .tap((val) => {
        directModeEvt.onMouseMove.subscribe(val);
        simpleSelectEvt.mouseMove.subscribe(val);
      })
      .value();

    /* eslint-enable @typescript-eslint/camelcase */
    drawUtilService.init(this.draw);
    this.measure = new MeasureService(this._map, this.draw);
    this._map.addControl(this.draw);
    this._bindDrawEvents(this.draw);
    this._map.dragRotate.disable();
    (window as any).mapboxDraw = this;
  });

  /**
   * Set the draw mode
   * @remarks
   * Set a mode based on parcel selection selected option
   * @param mode -The draw mode to be set
   */
  setMode = (mode: DrawMode): void => {
    if (!this._map) return;
    if (!this.draw) return;
    this.currentMode = mode;
    this.draw.changeMode(mode, {
      initialRadiusInKm: 0.05,
    });

    if (
      mode === 'draw_circle' ||
      mode === 'draw_polygon' ||
      mode === 'draw_line_string'
    ) {
      this._map.getCanvas().style.cursor = 'crosshair';
      return;
    }
  };

  /**
   * Delete all mapbox-gl-draw features
   */
  deleteAll = (): void => {
    if (!this.draw) return;
    this.draw?.deleteAll();
    const prevMode = this.draw.getMode();
    this.draw.changeMode('simple_select');
    this.draw?.changeMode(prevMode);
    $drawAreaUpdated.next(this.draw);
  };

  /**
   * Bind map events related to drawing stuff
   */
  private _bindDrawEvents(draw: MapboxDraw): void {
    if (!this._map) return;
    const triggerAreaUpdate = (): void => {
      $drawAreaUpdated.next(draw);
    };

    const drawCreate = (e: any): void => {
      const id = e.features[0]?.id;
      const drawnFeatures = drawUtilService.getDrawnFeaturesIds();
      id && draw.setFeatureProperty(id.toString(), 'drawn', true);
      if (!mapUtilService.controlKeyPressed) {
        draw.delete(drawnFeatures);
      }

      $drawAreaUpdated.next(draw);
    };

    this._map.on(MapboxDrawEvents.DRAW_CREATE, drawCreate);
    this._map.on(MapboxDrawEvents.DRAW_UPDATE, triggerAreaUpdate);
    this._map.on(MapboxDrawEvents.DRAW_DELETE, triggerAreaUpdate);
  }

  /**
   * Mouse move on simple select mode
   */
  simpleSelectMouseMove = (canvas: HTMLCanvasElement) => ({
    e,
  }: {
    state: object;
    e?: SelectEvent;
  }): void => {
    if (!e) return;
    if (isActiveFeature(e)) {
      if (e?.featureTarget.properties.user_disableMove) {
        canvas.style.cursor = 'grab';
        return;
      }
      canvas.style.cursor = 'move';
      return;
    }
    canvas.style.cursor = 'unset';
  };

  /**
   * calculate the radius of a circle
   *
   * @param radiusInKm -The circle radius in kilometers
   */
  getCircleRadius = (radiusInKm: number): number =>
    utilService.metersToFeet(radiusInKm * 1000);
}

export const mapBoxDrawService = new MapBoxDrawService();
