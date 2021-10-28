// typing.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { DropDownItem } from '@ptas/react-ui-library';
import { PtasLayer } from 'services/map';
import { labelDisplayKeys } from 'services/map/renderer/textRenderer/textRenderersService';

export const FILTER_TYPES = {
  all: {
    value: 'all',
    label: 'All',
  },
  onlySelected: {
    value: 'only-selected',
    label: 'Only selected',
  },
  useQuery: {
    value: 'use-query',
    label: 'Use Query',
  },
};

export interface TagLabelsManageGridRow {
  labelId: string;
  org: string;
  alias: string;
  jurisdiction: string;
  description: string;
  layerId: string;
}

export type TagLabelsManageGridCol = {
  name: keyof TagLabelsManageGridRow;
  title: string;
};

export type PropertyItem = {
  name: string;
  type: string;
  customOperators?: string[];
};

export type LineQuery = {
  property: string;
  type: string;
  operator: string;
  value: string | boolean;
  lineOperator?: string;
  operators: DropDownItem[];
};

export type TagLabel = {
  typeZoom: string;
  minZoom: number;
  maxZoom: number;
  typeLabelShow: string;
  typeFilter: string;
  queryFilter: LineQuery[];
  content: string;
  color: string;
  backgroundColor: string;
  fontSize: string;
  padding: string;
  isBoldText: boolean;
  horizontalAlign: string;
  verticalAlign: string;
};

export type OperatorsByType = {
  label: string;
  value: string;
  type: string[];
};

export class ClassTagLabel {
  refLayerId: string;
  refLayerType: PtasLayer['type'];
  id: string;
  labelName: string;
  typeZoom = 'all';
  minZoom = 16.6091;
  maxZoom = 17.9311;
  typeLabelShow: typeof labelDisplayKeys[number] = 'one-label-per-shape';
  typeFilter = 'all';
  queryFilter?: LineQuery[];
  content?: string;
  color = 'rgba(0,0,0,1)';
  backgroundColor = 'rgba(255,255,255, 1)';
  fontSize = '12pt';
  padding = 0;
  isBoldText = false;
  horizontalAlign = 'center';
  verticalAlign = 'middle';
  group?: string;
  constructor({
    id,
    labelName,
    refLayerId,
    refLayerType,
  }: Pick<ClassTagLabel, 'id' | 'labelName' | 'refLayerId' | 'refLayerType'>) {
    this.id = id;
    this.labelName = labelName;
    this.refLayerId = refLayerId;
    this.refLayerType = refLayerType;
  }
}
