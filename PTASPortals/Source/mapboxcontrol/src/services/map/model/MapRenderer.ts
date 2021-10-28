/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { RendererLabel } from '../renderer/textRenderer/textRenderersService';
import { LayerSource, PtasLayer } from './LayerSource';
import { RendererClassBreakRule } from './renderer/rendererClassBreakRule';
import { RuleType } from './renderer/ColorRenderer';
import { RendererSimpleRule } from './renderer/rendererSimpleRule';
import { RendererUniqueValuesRule } from './renderer/RendererUniqueValuesRule';
import { ColorRule } from './userMap';

const genRuleInstance = (
  mapRendererLogicType: RuleType,
  rule?: ColorRule
): ColorRule => {
  if (mapRendererLogicType === 'Simple') {
    return new RendererSimpleRule(rule as RendererSimpleRule);
  } else if (mapRendererLogicType === 'Class') {
    return new RendererClassBreakRule(rule as RendererClassBreakRule);
  } else if (mapRendererLogicType === 'Unique') {
    return new RendererUniqueValuesRule(rule as RendererUniqueValuesRule);
  }
  throw new Error('Invalid Rule type ' + mapRendererLogicType);
};

export type MapRendererRules = Pick<LayerSource, 'nativeMapboxLayers'> & {
  layer: PtasLayer;
  colorRule?: {
    rule: ColorRule;
    layer: PtasLayer;
  };

  labels?: RendererLabel[];
};

export class MapRenderer {
  mapRendererId: number;
  mapRendererName: string;
  createdBy: string;
  lastModifiedBy: string;
  rendererRules: MapRendererRules;
  createdTimestamp?: string;
  lastModifiedTimestamp?: string;
  userMapId: number;
  layerSourceId: number;

  constructor(fields: MapRenderer) {
    this.mapRendererId = fields.mapRendererId;
    this.mapRendererName = fields.mapRendererName;
    this.createdBy = fields.createdBy;
    this.lastModifiedBy = fields.lastModifiedBy;
    this.userMapId = fields.userMapId;
    this.layerSourceId = fields.layerSourceId;
    this.rendererRules = fields.rendererRules;
    if (fields.rendererRules) {
      if (fields.rendererRules.colorRule) {
        this.rendererRules.colorRule = fields.rendererRules.colorRule;
        if (fields.rendererRules.colorRule.rule) {
          this.rendererRules.colorRule.rule = genRuleInstance(
            fields.rendererRules.colorRule.rule.ruleType,
            fields.rendererRules.colorRule.rule
          );
        }
      }

      if (fields.rendererRules.labels) {
        this.rendererRules.labels = fields.rendererRules.labels;
      }
    }
  }

  /**
   * toCustomString
   */
  public toCustomString? = (rendererRules: MapRendererRules): string => {
    //Default layer string
    let layerStr =
      '\n' +
      '_layerId=' +
      rendererRules.layer.id +
      '_layerType=' +
      rendererRules.layer.type +
      // rendererRules.layer.source?.toString() +
      // rendererRules.layer['source-layer']?.toString() +
      '_minZoom=' +
      (rendererRules.layer.minzoom ?? '') +
      '_maxZoom=' +
      (rendererRules.layer.maxzoom ?? '');

    if (rendererRules.layer.type === 'fill') {
      if (
        rendererRules.layer.paint &&
        rendererRules.layer.paint['fill-color']
      ) {
        layerStr += '_fillColor=' + rendererRules.layer.paint['fill-color'];
      }
      if (
        rendererRules.layer.paint &&
        rendererRules.layer.paint['fill-outline-color']
      ) {
        layerStr +=
          '_outlineColor=' + rendererRules.layer.paint['fill-outline-color'];
      }
    } else if (rendererRules.layer.type === 'line') {
      if (
        rendererRules.layer.paint &&
        rendererRules.layer.paint['line-color']
      ) {
        layerStr += '_lineColor=' + rendererRules.layer.paint['line-color'];
      }
      if (
        rendererRules.layer.paint &&
        rendererRules.layer.paint['line-width']
      ) {
        layerStr += '_lineWidth=' + rendererRules.layer.paint['line-width'];
      }
    }

    //Color rule string
    let colorRuleStr = rendererRules.colorRule
      ? '\n_colorRule=' +
        (rendererRules.colorRule?.rule.columnName ?? '') +
        (rendererRules.colorRule?.rule.columnType ?? '') +
        (rendererRules.colorRule?.rule.ruleType ?? '')
      : '';
    if (rendererRules.colorRule?.rule instanceof RendererSimpleRule) {
      colorRuleStr +=
        rendererRules.colorRule?.rule.conditionOperator +
        rendererRules.colorRule?.rule.conditionValue +
        rendererRules.colorRule?.rule.fillColorTrue +
        rendererRules.colorRule?.rule.fillColorFalse +
        rendererRules.colorRule?.rule.fillColorNull +
        rendererRules.colorRule?.rule.outlineColorTrue +
        rendererRules.colorRule?.rule.outlineColorFalse +
        rendererRules.colorRule?.rule.outlineColorNull;
    } else if (
      rendererRules.colorRule?.rule instanceof RendererUniqueValuesRule
    ) {
      colorRuleStr +=
        rendererRules.colorRule?.rule.fillOpacity +
        rendererRules.colorRule?.rule.outlineOpacity +
        (rendererRules.colorRule?.rule.selectedFillRampId ?? '') +
        (rendererRules.colorRule?.rule.selectedOutlineRampId ?? '');
      //TODO: check if colors should be included. There can be changes
      //in color values, without the user having explicitly changed
      //anything. E.g. because a color ramp changed.
      rendererRules.colorRule?.rule.colors.forEach((color) => {
        colorRuleStr +=
          color.columnValue + color.fill + color.outline + color.tabIndex;
      });
    } else if (
      rendererRules.colorRule?.rule instanceof RendererClassBreakRule
    ) {
      colorRuleStr +=
        rendererRules.colorRule?.rule.breakOption +
        (rendererRules.colorRule?.rule.selectedFillRampId ?? '') +
        (rendererRules.colorRule?.rule.selectedOutlineRampId ?? '') +
        rendererRules.colorRule?.rule.fillOpacity +
        rendererRules.colorRule?.rule.outlineOpacity +
        (rendererRules.colorRule?.rule.stDeviationInterval ?? '');
      //TODO: check if colors should be included. There can be changes
      //in color values, without the user having explicitly changed
      //anything. E.g. because a color ramp changed.
      rendererRules.colorRule?.rule.colorRanges.forEach((range) => {
        colorRuleStr +=
          range.from + range.to + range.fill + range.outline + range.tabIndex;
      });
    }

    //Labels string
    let labelsStr = '';
    rendererRules.labels?.forEach((label) => {
      labelsStr +=
        '\n_label=' +
        // (label.labelConfig?.id ?? '') +
        (label.labelConfig?.labelName ?? '') +
        (label.labelConfig?.typeZoom ?? '') +
        (label.labelConfig?.minZoom ?? '') +
        (label.labelConfig?.maxZoom ?? '') +
        // (label.labelConfig?.queryFilter ?? '') +
        (label.labelConfig?.typeFilter ?? '') +
        (label.labelConfig?.content ?? '') +
        (label.labelConfig?.color ?? '') +
        (label.labelConfig?.backgroundColor ?? '') +
        (label.labelConfig?.fontSize ?? '') +
        (label.labelConfig?.padding ?? '') +
        (label.labelConfig?.isBoldText ?? '') +
        (label.labelConfig?.horizontalAlign ?? '') +
        (label.labelConfig?.verticalAlign ?? '');
    });

    const rendererRulesStr = layerStr + colorRuleStr + labelsStr;
    //Do not use property mapRendererId, because the backend changes it after saving
    return (
      '\n' +
      '_rendName=' +
      this.mapRendererName +
      '_sourceId=' +
      this.layerSourceId +
      +rendererRulesStr
    );
  };
}
