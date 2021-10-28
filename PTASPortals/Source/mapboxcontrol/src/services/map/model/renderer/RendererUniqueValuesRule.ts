// rendererUniqueValuesRule
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { v4 as uuidV4 } from 'uuid';
import { RendererRuleBase, RuleType } from './ColorRenderer';
import { DataSetColumnType } from './rendererDataset';
import { RangeBase } from './rendererClassBreakRule';
import { RGBA_BLACK, RGBA_TRANSPARENT } from 'appConstants';

export class RendererUniqueValuesRule implements RendererRuleBase {
  columnName = '';
  columnType: DataSetColumnType = 'unknown';
  ruleType: RuleType = 'Unique';
  colors: UniqueValuesRuleValueColor[] = [];
  selectedFillRampId?: string;
  selectedOutlineRampId?: string;
  fillOpacity = 1;
  outlineOpacity = 1;
  datasetId?: string;
  constructor(fields?: Omit<RendererUniqueValuesRule, 'ruleType'>) {
    if (fields) {
      this.columnName = fields.columnName;
      this.columnType = fields.columnType;
      this.colors = fields.colors;
      this.selectedFillRampId = fields.selectedFillRampId;
      this.selectedOutlineRampId = fields.selectedOutlineRampId;
      this.fillOpacity = fields.fillOpacity ?? 1;
      this.outlineOpacity = fields.outlineOpacity ?? 1;
      this.datasetId = fields.datasetId;
    }
  }
}

export class UniqueValuesRuleValueColor implements RangeBase {
  tabIndex: number;
  id: string;
  fill: string;
  outline: string;
  columnValue: number | string;
  columnDescription: string;

  constructor(
    fields: Omit<UniqueValuesRuleValueColor, 'id' | 'fill' | 'outline'> &
      Partial<Pick<UniqueValuesRuleValueColor, 'id' | 'fill' | 'outline'>>
  ) {
    this.columnValue = fields.columnValue;
    this.columnDescription = fields.columnDescription;
    this.id = fields.id ?? uuidV4();
    this.fill = fields.fill ?? RGBA_TRANSPARENT;
    this.outline = fields.outline ?? RGBA_BLACK;
    this.tabIndex = fields.tabIndex;
  }
}
