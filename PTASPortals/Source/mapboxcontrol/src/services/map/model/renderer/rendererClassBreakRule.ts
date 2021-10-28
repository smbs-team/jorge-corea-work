// rendererClassBreakRule.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { RendererRuleBase } from './ColorRenderer';
import { DataSetColumnType } from './rendererDataset';
import { RGBA_BLACK, RGBA_TRANSPARENT } from 'appConstants';

export enum BreakOptionKey {
  equalInterval = 'Equal interval',
  custom = 'Custom',
  quantile = 'Quantile',
  stDeviation = 'St deviation',
}

export class RendererClassBreakRule implements RendererRuleBase {
  id: string;
  columnName = '';
  columnType: DataSetColumnType = 'unknown';
  ruleType: 'Class' = 'Class';
  breakOption = '';
  colorRanges: ClassBreakRuleColorRange[] = [];
  selectedFillRampId?: string;
  selectedOutlineRampId?: string;
  fillOpacity = 1;
  outlineOpacity = 1;
  stDeviationInterval = 1;
  datasetId?: string;
  constructor(
    fields: Omit<RendererClassBreakRule, 'colorRanges' | 'ruleType'> & {
      colorRanges?: ClassBreakRuleColorRange[];
    }
  ) {
    this.id = fields.id;
    this.columnName = fields.columnName;
    this.columnType = fields.columnType;
    this.breakOption = fields.breakOption;
    this.colorRanges = fields.colorRanges ?? [];
    this.selectedFillRampId = fields.selectedFillRampId;
    this.selectedOutlineRampId = fields.selectedOutlineRampId;
    this.fillOpacity = fields.fillOpacity ?? 1;
    this.outlineOpacity = fields.outlineOpacity ?? 1;
    this.stDeviationInterval = fields.stDeviationInterval ?? 0;
    this.datasetId = fields.datasetId;
  }
}

export interface RangeBase {
  id: string;
  fill: string;
  outline: string;
  tabIndex: number;
}

export class ClassBreakRuleColorRange implements RangeBase {
  id: string;
  from: number;
  to: number;
  fill = RGBA_TRANSPARENT;
  outline = RGBA_BLACK;
  tabIndex = 0;
  constructor(
    fields: Omit<ClassBreakRuleColorRange, 'fill' | 'outline' | 'tabIndex'> &
      Partial<Pick<ClassBreakRuleColorRange, 'fill' | 'outline' | 'tabIndex'>>
  ) {
    this.id = fields.id;
    if (fields.fill) {
      this.fill = fields.fill;
    }
    if (fields.outline) {
      this.outline = fields.outline;
    }
    if (fields.tabIndex !== undefined) {
      this.tabIndex = fields.tabIndex;
    }
    this.from = fields.from;
    this.to = fields.to;
  }
}
