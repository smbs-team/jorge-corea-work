// drawUtilService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import MapboxDraw from '@mapbox/mapbox-gl-draw';
import { EventData } from 'mapbox-gl';
import { Feature, Geometry } from '@turf/turf';

/**
 *  mapbox-gl-draw utility
 */
export interface DrawUtilService {
  init: (_draw: MapboxDraw) => void;
  getFeaturesAtPoint: (draw: MapboxDraw, e: EventData) => string[];
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  isDrawnFeature: (f: any) => boolean;
  getDrawnFeatures: () => Feature<Geometry>[];
  getDrawnFeaturesIds: () => string[];
}

const drawUtilService = (): DrawUtilService => {
  let draw: MapboxDraw | null = null;

  /**
   *initialize the service
   * @param draw -MapboxDraw instance
   */
  const init = (_draw: MapboxDraw): void => {
    draw = _draw;
  };

  /**
   * Get mapbox-gl-draw features at point
   * @param draw -MapboxDraw instance
   * @param e -The map event
   * @returns The features that are inside the point
   */
  const getFeaturesAtPoint = (draw: MapboxDraw, e: EventData): string[] =>
    draw.getFeatureIdsAt(e.point).filter((f) => f !== undefined) || [];

  /**
   * Detects if the event feature is already drawn
   *
   * @param e -mapbox event
   * @returns boolean value that determines weather or not the feature is already drawn
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const isDrawnFeature = (f: any): boolean =>
    !!f?.properties?.user_drawn || !!f?.properties?.drawn;

  /**
   *Return all drawn features
   *
   *@returns - Features with property user_drawn=true
   */
  const getDrawnFeatures = (): Feature<Geometry>[] =>
    (draw?.getAll().features.filter(isDrawnFeature) || []) as Feature<
      Geometry
    >[];

  /**
   * Get drawn features ids
   *
   * @returns An string array of the features ids
   */
  const getDrawnFeaturesIds = (): string[] =>
    getDrawnFeatures().map((item) => item.id?.toString() ?? '');

  return {
    init,
    getDrawnFeatures,
    getDrawnFeaturesIds,
    getFeaturesAtPoint,
    isDrawnFeature,
  };
};

export default drawUtilService();
