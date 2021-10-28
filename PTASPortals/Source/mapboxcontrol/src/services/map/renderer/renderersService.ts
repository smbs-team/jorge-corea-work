// rendererService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl from 'mapbox-gl';
import { Subject } from 'rxjs';
import { MapRenderer, RendererDataset } from '../model';
import { apiService } from '../../api';
import { Folder } from '../model/folder';
import { DEFAULT_DATASET_ID, QUERY_PARAM } from 'appConstants';
import {
  RendererClassBreakRule,
  RendererSimpleRule,
  RendererUniqueValuesRule,
} from '../model';
import { layerService } from '../layerService';
import { utilService } from 'services/common';
import ExpressionService from './ExpressionService';
import { $onError } from '../mapServiceEvents';

interface RendererFoldersChangeEvent {
  rendererFolders?: Folder[];
}

class RendererService extends ExpressionService {
  /**
   * Default dataset
   */
  defaultDataSet: RendererDataset | undefined;

  /**
   * Mapboxgl map
   */
  map: mapboxgl.Map | undefined;
  $onGetDefaultDataSet = new Subject<RendererDataset>();
  $onRendererFoldersChange = new Subject<RendererFoldersChangeEvent>();

  /**
   * The main method to be called
   * @param _map - Mapboxgl map
   */
  init = async (_map: mapboxgl.Map): Promise<void> => {
    this.map = _map;

    //Default dataset
    const datasetRes = await apiService.getRendererDataSet(DEFAULT_DATASET_ID);
    if (datasetRes.data) {
      this.defaultDataSet = datasetRes.data;
      this.$onGetDefaultDataSet.next(this.defaultDataSet);
    }
  };

  urlDs = (): string | undefined =>
    utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID);

  getDatasetId = (): string | undefined => {
    const colorRule = layerService.getParcelRenderer()?.rendererRules.colorRule;
    if (colorRule?.layer?.layout?.visibility === 'visible') {
      return colorRule.rule.datasetId;
    }
  };

  /**
   *  Generates simple rule paint fill and outline expressions for Fill layers or Circle layers
   * @param renderer - The renderer to apply the rule
   * @returns - The same renderer with paint fill and outline properties is returned
   */
  private _writeSimpleRule = (
    renderer: MapRenderer
  ): MapRenderer | undefined => {
    if (!this.map) return;
    if (!renderer.rendererRules.colorRule) return;
    if (
      renderer.rendererRules.layer.type !== 'fill' &&
      renderer.rendererRules.layer.type !== 'circle'
    )
      return;
    const { rule, layer } = renderer.rendererRules.colorRule;
    if (!(rule instanceof RendererSimpleRule)) return;
    if (['date', 'number', 'boolean'].includes(rule.columnType)) {
      layer.paint = {
        ...layer.paint,
        ...this.genNumericSimpleRuleExpression(
          rule,
          renderer.rendererRules.layer.id,
          renderer.rendererRules.layer.type
        ),
      };
    } else if (rule.columnType === 'string') {
      layer.paint = {
        ...layer.paint,
        ...this.genStringSimpleRuleExpression(
          rule,
          renderer.rendererRules.layer.id,
          renderer.rendererRules.layer.type
        ),
      };
    } else {
      console.error('Unhandled column type ' + rule.columnType);
      console.log(renderer.rendererRules.colorRule.rule);
      return;
    }

    return renderer;
  };

  private _writeClassBreakRule = (
    renderer: MapRenderer
  ): MapRenderer | undefined => {
    if (!this.map) return;
    if (!renderer.rendererRules.colorRule) return;
    const { rule, layer } = renderer.rendererRules.colorRule;
    if (!(rule instanceof RendererClassBreakRule)) return;
    if (['date', 'number'].includes(rule.columnType)) {
      layer.paint = {
        ...layer.paint,
        ...this.genClassBreakRuleExpression(rule),
      };
    } else {
      throw new Error('Unhandled column type ' + rule.columnType);
    }
    return renderer;
  };

  private _writeUniqueValuesRule = (
    renderer: MapRenderer
  ): MapRenderer | undefined => {
    if (!this.map) return;
    if (!renderer.rendererRules.colorRule) return;
    const { rule, layer } = renderer.rendererRules.colorRule;
    if (!(rule instanceof RendererUniqueValuesRule)) return;
    if (['date', 'number', 'string'].includes(rule.columnType)) {
      layer.paint = {
        ...layer.paint,
        ...this.genUniqueValuesRuleExpression(rule),
      };
    } else {
      throw new Error('Unhandled column type ' + rule.columnType);
    }
    return renderer;
  };

  genColorRule = (renderer: MapRenderer): MapRenderer => {
    try {
      if (!renderer.rendererRules.layer.paint) return renderer;
      if (!renderer.rendererRules.colorRule) return renderer;
      if (!renderer.rendererRules.colorRule.layer.paint) {
        renderer.rendererRules.colorRule.layer.paint = {};
      }
      renderer.rendererRules.colorRule.layer.paint =
        renderer.rendererRules.layer.paint;
      if (
        renderer.rendererRules.colorRule?.rule instanceof RendererSimpleRule
      ) {
        return this._writeSimpleRule(renderer) || renderer;
      }
      if (
        renderer.rendererRules.colorRule?.rule instanceof RendererClassBreakRule
      ) {
        return this._writeClassBreakRule(renderer) || renderer;
      }
      if (
        renderer.rendererRules.colorRule?.rule instanceof
        RendererUniqueValuesRule
      ) {
        return this._writeUniqueValuesRule(renderer) || renderer;
      }
      return renderer;
    } catch (e) {
      console.error(e);
      $onError.next('Error generating color rule');
      return renderer;
    }
  };
}

export default new RendererService();
