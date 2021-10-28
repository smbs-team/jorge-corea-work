// rendererRule.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { DataSetColumnType } from './rendererDataset';

export type RuleType = 'Simple' | 'Unique' | 'Class';

type commonOperatorId = 'equals' | 'different';

export type NumericOperatorId = commonOperatorId | 'lt' | 'gt' | 'gte' | 'lte';

export type StringOperatorId = commonOperatorId | 'includes' | 'notIncludes';

export type OperatorRow = {
  name: string;
  mapboxOperator: string;
};

export interface RendererRuleBase {
  columnType: DataSetColumnType;
  ruleType: RuleType;
  datasetId?: string;
}
