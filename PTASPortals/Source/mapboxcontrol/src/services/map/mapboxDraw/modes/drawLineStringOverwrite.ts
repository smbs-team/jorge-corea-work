// drawLineString
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { modes } from '@mapbox/mapbox-gl-draw';
import { Feature, Point } from '@turf/turf';
import { Subject } from 'rxjs';
import { ModeFnEvent, DrawLineStringState, DrawLineString } from './types';

const { draw_line_string: originalMode } = modes;

export type DrawLineStringSubjectProps = {
  state: DrawLineStringState;
  e: ModeFnEvent;
  mode: DrawLineString;
};

export const drawLineStringEvt = {
  clickAnywhere: new Subject<DrawLineStringSubjectProps>(),
  clickOnVertex: new Subject<DrawLineStringSubjectProps>(),
};

export function createVertex(
  parentId: string,
  coordinates: [number, number],
  path: string,
  selected: boolean
): Feature<Point> {
  return {
    type: 'Feature',
    properties: {
      meta: 'vertex',
      parent: parentId,
      // eslint-disable-next-line @typescript-eslint/camelcase
      coord_path: path,
      active: selected ? 'true' : 'false',
    },
    geometry: {
      type: 'Point',
      coordinates,
    },
  };
}

const DrawLineStringOverwrite: DrawLineString = {
  ...originalMode,
  clickOnVertex: function (state, e) {
    originalMode.clickOnVertex.call(this, state, e);
    drawLineStringEvt.clickOnVertex.next({
      e,
      state,
      mode: this,
    });
  },
  clickAnywhere: function (state, e) {
    originalMode.clickAnywhere.call(this, state, e);
    drawLineStringEvt.clickAnywhere.next({
      state,
      e,
      mode: this,
    });
  },
  toDisplayFeatures: function (state, geojson, display): void {
    const isActiveLine = geojson?.properties?.id === state.line.id;
    if (!geojson.properties) {
      geojson.properties = {};
    }
    geojson.properties.active = isActiveLine ? 'true' : 'false';
    if (!isActiveLine) return display(geojson);
    // Only render the line if it has at least one real coordinate
    if ((geojson?.geometry?.coordinates?.length ?? 0) < 2) return;
    geojson.properties.meta = 'feature';
    const vertexIndex = 0;
    const vertex = createVertex(
      state.line.id,
      (geojson?.geometry?.coordinates[vertexIndex] ?? [0, 0]) as [
        number,
        number
      ],
      `${
        state.direction === 'forward'
          ? geojson?.geometry?.coordinates.length ?? 0 - 2
          : 1
      }`,
      false
    );
    if (!vertex.properties) {
      vertex.properties = {};
    }
    display(vertex);
    display(geojson);
  },
};

export default DrawLineStringOverwrite;
