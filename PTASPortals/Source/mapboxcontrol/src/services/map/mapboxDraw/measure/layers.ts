/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { pick } from 'lodash';
import { SymbolLayer, SymbolLayout } from 'mapbox-gl';

export const measureSourceId = 'measureLabel';

export type PointPosition = 'left' | 'right' | 'bottom' | 'top';

const distanceLayersLayout: SymbolLayout = {
  'icon-image': ['get', 'icon-image'],
  'icon-size': [
    'interpolate',
    ['exponential', 2],
    ['zoom'],
    1,
    0.1,
    18,
    0.5,
    21,
    0.7,
  ],
  'icon-allow-overlap': false,
  'text-field': ['get', 'text'],
  'text-allow-overlap': false,
  'text-size': [
    'interpolate',
    ['exponential', 2],
    ['zoom'],
    1,
    2,
    18,
    12,
    22,
    13,
  ],
  'text-font': ['Roboto Condensed'],
};

const distanceIconOffset = {
  min: 10,
  stop1: 29,
  max: 32,
};
/**
 * Icon and text translate
 */
const pointTranslate = {
  right: {
    ...['icon-translate', 'text-translate'].reduce((prev, curr) => {
      return {
        ...prev,
        [curr]: [
          'interpolate',
          ['exponential', 2],
          ['zoom'],
          1,
          ['literal', [distanceIconOffset.min, 0]],
          18,
          ['literal', [distanceIconOffset.stop1, 0]],
          22,
          ['literal', [distanceIconOffset.max, 0]],
        ],
      };
    }, {}),
  },
  left: {
    ...['icon-translate', 'text-translate'].reduce((prev, curr) => {
      return {
        ...prev,
        [curr]: [
          'interpolate',
          ['exponential', 2],
          ['zoom'],
          1,
          ['literal', [-1 * distanceIconOffset.min, 0]],
          18,
          ['literal', [-1 * distanceIconOffset.stop1, 0]],
          22,
          ['literal', [-1 * distanceIconOffset.max, 0]],
        ],
      };
    }, {}),
  },
  top: {
    ...['icon-translate', 'text-translate'].reduce((prev, curr) => {
      return {
        ...prev,
        [curr]: [
          'interpolate',
          ['exponential', 2],
          ['zoom'],
          1,
          ['literal', [0, distanceIconOffset.min * -1]],
          18,
          ['literal', [0, distanceIconOffset.stop1 * -1]],
          22,
          ['literal', [0, distanceIconOffset.max * -1]],
        ],
      };
    }, {}),
  },
  bottom: {
    ...['icon-translate', 'text-translate'].reduce((prev, curr) => {
      return {
        ...prev,
        [curr]: [
          'interpolate',
          ['exponential', 2],
          ['zoom'],
          1,
          ['literal', [0, distanceIconOffset.min]],
          18,
          ['literal', [0, distanceIconOffset.stop1]],
          22,
          ['literal', [0, distanceIconOffset.max]],
        ],
      };
    }, {}),
  },
};

export const distanceLayers: Record<PointPosition, SymbolLayer> = {
  right: {
    type: 'symbol',
    source: measureSourceId,
    id: measureSourceId + '-right',
    filter: ['==', ['get', 'layer'], 'right'],
    paint: {
      ...pointTranslate.right,
    },
    layout: distanceLayersLayout,
  },
  left: {
    type: 'symbol',
    source: measureSourceId,
    id: measureSourceId + '-left',
    filter: ['==', ['get', 'layer'], 'left'],
    paint: {
      ...pointTranslate.left,
    },
    layout: distanceLayersLayout,
  },
  top: {
    type: 'symbol',
    source: measureSourceId,
    id: measureSourceId + '-top',
    filter: ['==', ['get', 'layer'], 'top'],
    paint: {
      ...pointTranslate.top,
    },
    layout: distanceLayersLayout,
  },
  bottom: {
    type: 'symbol',
    source: measureSourceId,
    id: measureSourceId + '-bottom',
    filter: ['==', ['get', 'layer'], 'bottom'],
    paint: {
      ...pointTranslate.bottom,
    },
    layout: distanceLayersLayout,
  },
};

/**
 * Area text layer
 */
export const areaLayer: SymbolLayer = {
  type: 'symbol',
  source: measureSourceId,
  id: measureSourceId + '-area-layer',
  filter: ['==', ['get', 'layer'], 'area'],
  layout: {
    ...pick(distanceLayersLayout, ['icon-image', 'text-field', 'text-font']),
    'icon-allow-overlap': true,
    'text-allow-overlap': true,
    'text-size': [
      'interpolate',
      ['exponential', 2],
      ['zoom'],
      1,
      4,
      18,
      12,
      22,
      18,
    ],
    'icon-size': [
      'interpolate',
      ['exponential', 2],
      ['zoom'],
      1,
      1 / 12,
      18,
      0.25,
      22,
      0.4,
    ],
  },
};
