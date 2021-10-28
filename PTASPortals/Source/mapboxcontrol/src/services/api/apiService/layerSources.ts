/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ApiServiceResult, handleReq } from 'services/common';
import { LayerSource, LayerSourceRes } from 'services/map';
import { axiosCsInstance } from '../axiosInstances';
import { layerSourcesResPipe } from '../dataPipes';

export class LayerSources {
  private _pipe: (arr: LayerSource[]) => LayerSource[] = (arr: LayerSource[]) =>
    arr;

  setPipe = (fn: (items: LayerSource[]) => LayerSource[]): void => {
    this._pipe = fn;
  };

  get = async (): Promise<ApiServiceResult<LayerSource[]>> =>
    handleReq(async () => {
      const layerSources = (
        await axiosCsInstance.get<LayerSourceRes[]>('/GIS/GetLayerSources', {
          transformResponse: (res) => {
            try {
              return layerSourcesResPipe(res);
            } catch (e) {
              return res;
            }
          },
          params: {
            cache: true,
          },
        })
      ).data;
      return new ApiServiceResult<LayerSource[]>({
        data: this._pipe(layerSources.map((item) => new LayerSource(item))),
      });
    });
}
