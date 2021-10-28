/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { RGBA_BLACK, RGBA_TRANSPARENT } from 'appConstants';
import { camelCase } from 'lodash';
import { FillPaint } from 'mapbox-gl';
import { LayerSource, LayerSourceRes, UserMap } from 'services/map';

export const userMapReqPipe = (req: Partial<UserMap>): Partial<UserMap> => {
  delete req.toCustomString;
  req.mapRenderers = (req as UserMap | undefined)?.mapRenderers?.map(
    (renderer) => ({
      ...renderer,
      //Just needed in backend
      mapRendererType: 'default',
      hasLabelRenderer: false,
      datasetId: '',
    })
  );
  return req;
};

export const layerSourcesResPipe = (res: string): LayerSourceRes[] => {
  const { layerSources } = JSON.parse(res) as {
    layerSources: LayerSourceRes[];
  };

  for (const item of layerSources) {
    if (item.metadata) {
      item.metadata = (Object.entries(item.metadata).reduce((prev, [k, v]) => {
        return {
          ...prev,
          [camelCase(k)]: v,
        };
      }, {}) as unknown) as LayerSource['metadata'];
    }

    /** Annotations layers */
    if (
      item.defaultMapboxLayer?.id &&
      /^ca[0-9]Layer/.test(item.defaultMapboxLayer.id)
    ) {
      (item.defaultMapboxLayer.paint as FillPaint)['fill-outline-color'] =
        process.env.REACT_APP_SHOW_ANNOTATIONS_SHAPE === 'true'
          ? RGBA_BLACK
          : RGBA_TRANSPARENT;
    }
  }

  return layerSources.sort((a, b) =>
    a.defaultMapboxLayer.id > b.defaultMapboxLayer.id ? 1 : -1
  );
};
