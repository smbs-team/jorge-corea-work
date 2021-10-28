// layerService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AppLayers } from 'appConstants';
import mapboxgl from 'mapbox-gl';
import { Subject } from 'rxjs';
import { $onError, $onSelectedLayersChange } from './mapServiceEvents';
import { LayerSource, MapRenderer } from './model';

class LayerService {
  selectedLayers: LayerSource['defaultMapboxLayer'][] = [];
  map?: mapboxgl.Map;
  layersConfiguration: Record<string, LayerSource> = {};
  readonly $onRemoveLayer: Subject<
    LayerSource['defaultMapboxLayer']
  > = new Subject();
  readonly $onLayerAdded: Subject<
    LayerSource['defaultMapboxLayer']
  > = new Subject();

  layersConfigurationList: LayerSource[] = [];

  readonly getSelectedLayersIds = (): string[] =>
    this.selectedLayers.map((item) => item.id);

  readonly hasParcelLayer = (): boolean =>
    this.getSelectedLayersIds().includes(AppLayers.PARCEL_LAYER);

  renderers: MapRenderer[] = [];

  readonly loadFillPatternImage = (imageUrl: string): void => {
    const url = new URL(process.env.REACT_APP_MAP_TILE_SERVICE_HOST + imageUrl);
    url.searchParams.set('cache', 'true');
    url.searchParams.set('is-static', 'true');
    this.map?.loadImage(url.href, (error: unknown, image: ImageBitmap) => {
      if (error) {
        console.error(error);
        return;
      }
      this.map?.addImage(imageUrl, image);
    });
  };

  readonly addMapSources = async (layers: LayerSource[]): Promise<void> => {
    if (!this.map) return;
    for (const layerConf of layers) {
      this.layersConfiguration[layerConf.defaultMapboxLayer.id] = layerConf;
      const sourceKey = layerConf.defaultMapboxLayer.source?.toString() ?? '';
      if (!this.map.getSource(sourceKey)) {
        this.map.addSource(sourceKey, layerConf.mapSource);
        for (const layer of layerConf.nativeMapboxLayers) {
          if (
            layer.type === 'fill' &&
            layer.paint?.['fill-pattern'] &&
            typeof layer.paint['fill-pattern'] === 'string' &&
            layer.paint['fill-pattern'].endsWith('.png')
          ) {
            this.loadFillPatternImage(layer.paint['fill-pattern']);
          }
        }
      } else {
        console.warn('NOTICE: source ' + sourceKey + ' already exists');
      }
    }
  };

  readonly hasLayer = (id: string): boolean =>
    !!this.selectedLayers.find((val) => val.id === id);

  getRenderer = (rootLayerId: string): MapRenderer | undefined =>
    this.renderers.find(
      (renderer) => renderer.rendererRules.layer.id === rootLayerId
    );

  getParcelRenderer = (): MapRenderer | undefined =>
    this.getRenderer(AppLayers.PARCEL_LAYER);

  readonly setMapLayers = (newRenderers: MapRenderer[] | null): void => {
    try {
      if (!this.map) return;
      if (JSON.stringify(newRenderers) === JSON.stringify(this.selectedLayers))
        return;

      this.renderers = newRenderers ?? [];
      if (!newRenderers?.length) {
        for (const layer of this.selectedLayers) {
          this.map.removeLayer(layer.id);
          this.$onRemoveLayer.next(layer);
        }
        this.selectedLayers = [];
        return;
      }

      const prevLayersIds = this.selectedLayers.map((val) => val.id);
      for (const _previousLayer of this.selectedLayers) {
        const layerId = _previousLayer.id;
        if (
          newRenderers.every(
            (renderer) =>
              renderer.rendererRules.layer.id !== layerId &&
              renderer.rendererRules.colorRule?.layer.id !== layerId &&
              renderer.rendererRules.nativeMapboxLayers.every(
                (val) => val.id !== layerId
              )
          )
        ) {
          this.$onRemoveLayer.next(_previousLayer);
        }
        this.map.removeLayer(layerId);
      }

      this.selectedLayers = [];
      /**
       * Add new Layers
       */
      let prev = 'country-label';
      for (const _newLayer of newRenderers) {
        //Add color rule layers
        if (_newLayer.rendererRules.colorRule) {
          //Color renderers go on top of the default layer
          const colorRule = _newLayer.rendererRules.colorRule;
          this.map.addLayer(colorRule.layer, prev);
          if (!prevLayersIds.includes(colorRule.layer.id)) {
            this.$onLayerAdded.next(colorRule.layer);
          }
          this.selectedLayers.push(colorRule.layer);
          prev = colorRule.layer.id;
        }

        //Add Default mapbox layer
        this.map.addLayer(_newLayer.rendererRules.layer, prev);
        prev = _newLayer.rendererRules.layer.id;
        if (!prevLayersIds.includes(_newLayer.rendererRules.layer.id)) {
          this.$onLayerAdded.next(_newLayer.rendererRules.layer);
        }
        this.selectedLayers.push(_newLayer.rendererRules.layer);

        //Add the other source layer
        for (const layer of _newLayer.rendererRules.nativeMapboxLayers) {
          this.map.addLayer(layer, prev);
          if (!prevLayersIds.includes(layer.id)) {
            this.$onLayerAdded.next(layer);
          }
          prev = layer.id;
          this.selectedLayers.push(layer);
        }
      }
      $onSelectedLayersChange.next(this.renderers);
    } catch (e) {
      console.error(e);
      $onError.next('Error applying map layers');
    }
  };
}

export const layerService = new LayerService();
