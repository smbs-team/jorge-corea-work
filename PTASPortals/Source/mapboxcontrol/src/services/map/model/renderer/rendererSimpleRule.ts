/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { RGBA_BLACK, RGBA_TRANSPARENT } from 'appConstants';
import { DataSetColumnType } from './rendererDataset';
import {
  NumericOperatorId,
  RendererRuleBase,
  RuleType,
  StringOperatorId,
} from './ColorRenderer';
import {
  isNumericOperator,
  isStringOperator,
  numericOperators,
  stringOperators,
} from 'services/map/renderer/utils';

export class RendererSimpleRule implements RendererRuleBase {
  columnType: DataSetColumnType = 'unknown';
  columnName = '';
  ruleType: RuleType = 'Simple';
  fillColorTrue = 'rgba(255,0,0,1)';
  fillColorFalse = 'rgba(0,0,255,1)';
  fillColorNull = RGBA_TRANSPARENT;
  outlineColorTrue = RGBA_BLACK;
  outlineColorFalse = RGBA_BLACK;
  outlineColorNull = RGBA_BLACK;
  conditionOperator: NumericOperatorId | StringOperatorId = 'equals';
  conditionDescription = '';
  conditionValue: number | string = '';
  datasetId?: string;
  constructor(
    fields?: Omit<RendererSimpleRule, 'ruleType' | 'conditionOperatorLabel'>
  ) {
    if (fields) {
      this.columnName = fields.columnName;
      this.columnType = fields.columnType;
      this.fillColorTrue = fields.fillColorTrue;
      this.fillColorFalse = fields.fillColorFalse;
      this.fillColorNull = fields.fillColorNull;
      this.outlineColorTrue = fields.outlineColorTrue;
      this.outlineColorFalse = fields.outlineColorFalse;
      this.outlineColorNull = fields.outlineColorNull;
      this.conditionOperator = fields.conditionOperator;
      this.conditionValue =
        fields.columnType === 'boolean' ? '1' : fields.conditionValue;
      this.datasetId = fields.datasetId;
      if (fields.columnType === 'boolean') {
        this.conditionDescription = fields.columnName + ' true';
      } else if (
        fields.columnType === 'number' ||
        fields.columnType === 'date'
      ) {
        if (isNumericOperator(fields.conditionOperator)) {
          if (fields.columnType === 'number') {
            this.conditionDescription = [
              fields.columnName,
              numericOperators[fields.conditionOperator as NumericOperatorId]
                .name,
              fields.conditionValue,
            ].join(' ');
          } else {
            this.conditionDescription = [
              fields.columnName,
              numericOperators[fields.conditionOperator as NumericOperatorId]
                .name,
              new Date(+fields.conditionValue).toDateString(),
            ].join(' ');
          }
        }
      } else if (fields.columnType === 'string') {
        if (isStringOperator(fields.conditionOperator)) {
          this.conditionDescription = [
            fields.columnName,
            stringOperators[fields.conditionOperator as StringOperatorId].name,
            fields.conditionValue,
          ].join(' ');
        }
      }
    }
  }
}
